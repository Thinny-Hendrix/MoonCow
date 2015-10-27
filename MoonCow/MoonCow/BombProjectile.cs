using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    public class BombProjectile:Projectile
    {
        WeaponBomb wep;
        CircleCollider col;
        public BombProjectile(Vector3 pos, Vector3 direction, Game1 game, WeaponBomb wep):base()
        {
            this.direction = direction;
            this.game = game;
            this.pos = pos;
            this.rot.Y = (float)Math.Atan2(direction.X, direction.Z);
            this.wep = wep;

            speed = 50;
            life = 300;
            delete = false;
            damage = 5;

            col = new CircleCollider(pos, 1);

            model = new BombProjectileModel(this, pos, direction);
            game.modelManager.addObject(model);
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
            if (life <= 0)
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
            bool collided = false;

            //## COLLISIONS WHOOO! ##
            // Move the bounding box to new pos
            col.Update(pos);
            // Get current node co-ordinates
            nodePos = new Vector2((int)((pos.X / 30) + 0.5f), (int)((pos.Z / 30) + 0.5f));

            //For the current node check if your X component will make you collide with wall
            try
            {
                foreach (OOBB box in game.map.map[(int)nodePos.X, (int)nodePos.Y].collisionBoxes)
                {
                    if (col.checkOOBB(box))
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

            col.Update(pos);
            nodePos = new Vector2((int)((pos.X / 30) + 0.5f), (int)((pos.Z / 30) + 0.5f));

            try
            {
                foreach (OOBB box in game.map.map[(int)nodePos.X, (int)nodePos.Y].collisionBoxes) // for each bounding box in current node
                {
                    if (col.checkOOBB(box))
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
                        foreach(CircleCollider c in enemy.cols)
                        if (col.checkCircle(c))
                        {
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

            foreach (Sentry s in game.enemyManager.sentries)
            {
                if(col.checkCircle(s.col))
                {
                    collided = true;
                    game.modelManager.addEffect(new ImpactParticleModel(game, pos));
                }
            }

            foreach(Asteroid a in game.asteroidManager.asteroids)
            {
                if(col.checkCircle(a.col))
                {
                    collided = true;
                }
            }

            foreach (JunkShip j in game.asteroidManager.junkShips)
            {
                bool checkCollide = false;
                foreach (OOBB o in j.cols)
                {
                    if (col.checkOOBB(o))
                    {
                        checkCollide = true;
                    }
                }
                if (checkCollide)
                    collided = true;
            }

            if (col.checkCircle(game.core.col))
                collided = true;

            if (collided)
            {
                //for (int i = 0; i < 10; i++)
                  //  game.modelManager.addEffect(new DotParticle(game, pos));

                wep.splos.Add(new BombExplosion(pos, game, wep.level, wep));
                deleteProjectile();
            }
        }

        protected override void deleteProjectile()
        {
            game.modelManager.removeObject(model);
            wep.toDelete.Add(this);
            delete = true;
            model.Dispose();
        }
    }
}
