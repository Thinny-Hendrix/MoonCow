using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    class GunnerProjectile:Projectile
    {
        Color c1;
        Color c2;
        public GunnerProjectile(Vector3 pos, Vector3 direction, Game1 game):base()
        {
            this.direction = direction;
            this.game = game;
            this.pos = pos;
            this.rot.Y = (float)Math.Atan2(direction.X, direction.Z);

            speed = 50;
            life = 120;
            delete = false;
            damage = 20;

            c1 = new Color(255, 0, 162);
            c2 = Color.Purple;

            col = new CircleCollider(pos, 0.2f);

            model = new ProjectileModel(this, TextureManager.elecRound64, TextureManager.elecRound64, TextureManager.elecTrail64, c1, c2, game);


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
            col.Update(pos);
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
                collided = true;
            }

            try
            {
                foreach (OOBB box in game.map.map[(int)nodePos.X, (int)nodePos.Y].collisionBoxes)
                {
                    if (col.checkOOBB(box))
                    {
                        collided = true;
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
                        foreach(CircleCollider c in enemy.cols)
                        {
                            if(c.checkCircle(col))
                            {
                                enemy.health -= damage*0.1f;
                                game.modelManager.addEffect(new ImpactParticleModel(game, pos));
                                collided = true;
                            }
                        }
                    }
                }
            }
            catch (IndexOutOfRangeException)
            {
                deleteProjectile();
            }

            if (collided)
            {
                for (int i = 0; i < 10; i++)
                    game.modelManager.addEffect(new DotParticle(game, pos));
                for (int i = 0; i < 5; i++)
                    game.modelManager.addEffect(new ElecParticle(pos, game, c1, c2));

                game.modelManager.addEffect(new LaserHitEffect(game, pos, c1));

                deleteProjectile();
            }
        }

        protected override void deleteProjectile()
        {
            game.modelManager.removeEffect(model);
            game.enemyManager.pToDelete.Add(this);
            delete = true;
            model.Dispose();
        }
    }
}
