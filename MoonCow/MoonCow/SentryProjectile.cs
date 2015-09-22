using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    public class SentryProjectile:Projectile
    {
        Sentry enemy;
        bool sentFail;
        float distFromSource;
        public SentryProjectile(Vector3 pos, Vector3 direction, Game1 game, Sentry enemy):base()
        {
            this.direction = direction;
            this.game = game;
            this.pos = pos;
            this.rot.Y = (float)Math.Atan2(direction.X, direction.Z);
            this.enemy = enemy;

            speed = 50;
            life = 120;
            delete = false;
            damage = 20;

            boundingBox = new OOBB(pos, direction, 0.3f, 1); // Need to be changed to be actual projectile dimensions
            col = new CircleCollider(pos, 0.2f);

            model = new ProjectileModel(ModelLibrary.projectile, pos, this, Color.Red, Color.Orange, game);

            game.modelManager.addEffect(model);
        }

        public override void Update()
        {
            frameDiff = Vector3.Zero;

            if (!delete)
            {
                frameDiff += direction * speed * Utilities.deltaTime;
                checkCollision();
            }
            life -= Utilities.deltaTime * 60;
            if(life <=0)
            {
                deleteProjectile();
            }
            boundingBox.Update(pos, direction);
            col.Update(pos);

            distFromSource = Utilities.hypotenuseOf(pos.X - enemy.pos.X, pos.Z - enemy.pos.Z);
            if (!sentFail && distFromSource > enemy.distFromShip + 2)
            {
                sentFail = true;
                enemy.missedShip();
            }
        }

        protected override void checkCollision()
        {
            // By moving each component of the vector one at a time and seeing what causes the collision we can eliminate only that component
            // this means the ship will slide along walls rather than stick. Doing two collision checks per frame for the player seems to
            // be within tolerable limits for CPU time. This will only need to be done with the player
            pos.Y += frameDiff.Y;
            pos.X += frameDiff.X;
            pos.Z += frameDiff.Z;
            bool collided = false;

            //## COLLISIONS WHOOO! ##
            // Move the bounding box to new pos
            col.Update(pos);
            // Get current node co-ordinates
            nodePos = new Vector2((int)((pos.X / 30) + 0.5f), (int)((pos.Z / 30) + 0.5f));

            //For the current node check if your X component will make you collide with wall
            if(col.checkOOBB(game.ship.boundingBox))
            {
                game.ship.shipHealth.onHit(damage);
                enemy.hitShip();
                collided = true;
            }

            try
            {
                foreach (OOBB box in game.map.map[(int)nodePos.X, (int)nodePos.Y].collisionBoxes)
                {
                    if (col.checkOOBB(box))
                    {
                        collided = true;
                        if(!sentFail)
                           enemy.missedShip();
                    }
                }
            }
            catch (IndexOutOfRangeException)
            {
                deleteProjectile();
            }
            try 
            {
                foreach (Enemy enemy in game.enemyManager.enemies)
                {
                    if(nodePos.X == enemy.nodePos.X && nodePos.Y == enemy.nodePos.Y)
                    {
                        //System.Diagnostics.Debug.WriteLine("Bullet in same node as enemy");
                        if(col.checkOOBB(enemy.boundingBox))
                        {
                            enemy.health -= damage;
                            game.modelManager.addEffect(new ImpactParticleModel(game, pos));
                            collided = true;

                            if (!sentFail)
                                this.enemy.missedShip();
                        }
                    }
                }
            }
            catch (IndexOutOfRangeException)
            {
                deleteProjectile();
            }

            try
            {
                foreach (Asteroid a in game.asteroidManager.asteroids)
                {
                    if (nodePos.X == a.nodePos.X && nodePos.Y == a.nodePos.Y)
                    {
                        if (a.col.checkPoint(pos))
                        {
                            a.damage(damage, pos);
                            game.modelManager.addEffect(new ImpactParticleModel(game, pos));
                            collided = true;
                            if (!sentFail)
                                enemy.missedShip();
                        }
                    }
                }
            }
            catch (IndexOutOfRangeException) { }

            if (collided)
            {
                for (int i = 0; i < 10; i++)
                    game.modelManager.addEffect(new DotParticle(game, pos));
                    game.modelManager.addEffect(new LaserHitEffect(game, pos, Color.Red));

                deleteProjectile();
            }
        }

        protected override void deleteProjectile()
        {
            game.modelManager.removeEffect(model);
            enemy.toDelete.Add(this);
            delete = true;
            model.Dispose();
        }
    }
}
