using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    //superclass all weapon projectiles will inherit
    public class Projectile
    {
        public Vector3 pos;
        public Vector3 rot;
        public Vector3 scale;
        public Vector3 direction;
        float speed;
        float life;
        Game1 game;
        public bool delete;
        OOBB boundingBox;
        Vector3 frameDiff;
        Vector2 nodePos;
        WeaponSystem weapons;
        ProjectileModel model;
        int damage;

        public Projectile(Vector3 pos, Vector3 direction, Game1 game, Texture2D tex, Texture2D tex2, Texture2D tex3, WeaponSystem weapons)
        {
            this.direction = direction;
            this.game = game;
            this.pos = pos;
            this.rot.Y = (float)Math.Atan2(direction.X, direction.Z);
            this.weapons = weapons;

            speed = 50;
            life = 120;
            delete = false;
            damage = 5;

            boundingBox = new OOBB(pos, direction, 0.3f, 1); // Need to be changed to be actual projectile dimensions

            model = new ProjectileModel(game.Content.Load<Model>(@"Models/Effects/shotEffectNew"), pos, this, game, tex, tex2, tex3);
            game.modelManager.addEffect(model);
        }

        public void update()
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

        void checkCollision()
        {
            // By moving each component of the vector one at a time and seeing what causes the collision we can eliminate only that component
            // this means the ship will slide along walls rather than stick. Doing two collision checks per frame for the player seems to
            // be within tolerable limits for CPU time. This will only need to be done with the player
            pos.X += frameDiff.X;

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
                        //game.ship.moneyManager.addGib(100, pos);
                        deleteProjectile();
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
                        deleteProjectile();
                        //game.ship.moneyManager.addGib(100, pos);
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
                    if(nodePos.X == enemy.nodePos.X && nodePos.Y == enemy.nodePos.Y)
                    {
                        //System.Diagnostics.Debug.WriteLine("Bullet in same node as enemy");
                        if(boundingBox.intersects(enemy.boundingBox))
                        {
                            enemy.health -= damage;
                            deleteProjectile();
                        }
                    }
                }
            }
            catch (IndexOutOfRangeException)
            {
                deleteProjectile();
            }
        }

        void deleteProjectile()
        {
            game.modelManager.removeEffect(model);
            weapons.toDelete.Add(this);
            delete = true;
        }

    }
}
