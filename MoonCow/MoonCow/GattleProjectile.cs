using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    public class GattleProjectile:Projectile
    {
        Turret turret;
        bool colEnabled;
        public GattleProjectile(Vector3 pos, Vector3 direction, Game1 game, Turret turret):base()
        {
            this.direction = direction;
            this.game = game;
            this.pos = pos;
            this.rot.Y = (float)Math.Atan2(direction.X, direction.Z);
            this.turret = turret;

            speed = 50;
            life = 300;
            delete = false;
            damage = 4f;

            boundingBox = new OOBB(pos, direction, 0.3f, 1); // Need to be changed to be actual projectile dimensions

            model = new ProjectileModel(ModelLibrary.projectile, pos, this, Color.Orange, Color.Red, game);
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
        }

        protected override void checkCollision()
        {
            // By moving each component of the vector one at a time and seeing what causes the collision we can eliminate only that component
            // this means the ship will slide along walls rather than stick. Doing two collision checks per frame for the player seems to
            // be within tolerable limits for CPU time. This will only need to be done with the player
            pos.X += frameDiff.X;
            pos.Y += frameDiff.Y;
            bool collided = false;

            //## COLLISIONS WHOOO! ##
            // Move the bounding box to new pos
            boundingBox.Update(pos, direction);
            // Get current node co-ordinates
            nodePos = new Vector2((int)((pos.X / 30) + 0.5f), (int)((pos.Z / 30) + 0.5f));

            //For the current node check if your X component will make you collide with wall
            try
            {
                foreach (OOBB box in game.map.map[(int)nodePos.X, (int)nodePos.Y].collisionBoxes)
                {
                    if (boundingBox.intersects(box))
                    {
                        pos.X -= frameDiff.X;
                        collided = true;
                    }
                }
            }
            catch (IndexOutOfRangeException)
            {
                deleteProjectile();
            }

            // Now add the Z component of the movement
            pos.Z += frameDiff.Z;

            boundingBox.Update(pos, direction);
            nodePos = new Vector2((int)((pos.X / 30) + 0.5f), (int)((pos.Z / 30) + 0.5f));

            try
            {
                foreach (OOBB box in game.map.map[(int)nodePos.X, (int)nodePos.Y].collisionBoxes) // for each bounding box in current node
                {
                    if (boundingBox.intersects(box))
                    {
                        collided = true;
                        pos.Z -= frameDiff.Z;
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
                    if (nodePos.X == enemy.nodePos.X && nodePos.Y == enemy.nodePos.Y)
                    {
                        //System.Diagnostics.Debug.WriteLine("Bullet in same node as enemy");
                        if (boundingBox.intersects(enemy.boundingBox))
                        {
                            enemy.health -= damage;
                            game.modelManager.addEffect(new ImpactParticleModel(game, pos));
                            collided = true;
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
                if (colEnabled)
                {
                    for (int i = 0; i < 10; i++)
                        game.modelManager.addEffect(new DotParticle(game, pos));
                    game.modelManager.addEffect(new LaserHitEffect(game, pos, Color.Orange));

                    deleteProjectile();
                }
            }
            else
            {
                colEnabled = true;
            }
        }

        protected override void deleteProjectile()
        {
            game.modelManager.removeEffect(model);
            turret.toDelete.Add(this);
            delete = true;
            model.Dispose();
        }
    }
}
