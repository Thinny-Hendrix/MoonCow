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
        public Vector3 direction;
        public float speed { get; protected set; }
        public float life { get; protected set; }
        public Game1 game { get; protected set; }
        public bool delete { get; protected set; }
        public OOBB boundingBox { get; protected set; }
        public Vector3 frameDiff;
        public Vector2 nodePos { get; protected set; }
        Weapon weapon;
        public BasicModel model;
        public float damage { get; protected set; }
        int type;//for level ups
        public CircleCollider col;


        public Projectile(){}//empty constructor for children to use if necessary
        public Projectile(Vector3 pos, Vector3 direction, Game1 game, Weapon weapon, int type)
        {
            this.direction = direction;
            this.game = game;
            this.pos = pos;
            this.rot.Y = (float)Math.Atan2(direction.X, direction.Z);
            this.weapon = weapon;
            this.type = type;

            speed = 50;
            life = 300;
            delete = false;
            damage = 5;

            col = new CircleCollider(pos, 0.2f);

            boundingBox = new OOBB(pos, direction, 0.3f, 1); // Need to be changed to be actual projectile dimensions

            if(type == 1)
    //            model = new ProjectileModel(this, TextureManager.elecRound64, TextureManager.elecRound64, TextureManager.elecTrail64, Color.Magenta, Color.Purple, game);
                model = new ProjectileModel(ModelLibrary.projectile, pos, this, Color.Green, Color.CornflowerBlue, game);
            else if(type == 0)
                model = new ProjectileModel(ModelLibrary.projectile, pos, this, Color.Orange, Color.Purple, game);
            else
                model = new ProjectileModel(ModelLibrary.projectile, pos, this, new Color(255,0,255), Color.Green, game);

            game.modelManager.addEffect(model);
        }

        public virtual void Update()
        {
            frameDiff = Vector3.Zero;

            if (!delete)
            {
                frameDiff += direction * speed * Utilities.deltaTime;
                if(type != 2)
                    checkCollision();
            }
            life -= Utilities.deltaTime * 60;
            if(life <=0)
            {
                deleteProjectile();
            }
            boundingBox.Update(pos, direction);
            col.Update(pos);
        }

        protected virtual void checkCollision()
        {
            // By moving each component of the vector one at a time and seeing what causes the collision we can eliminate only that component
            // this means the ship will slide along walls rather than stick. Doing two collision checks per frame for the player seems to
            // be within tolerable limits for CPU time. This will only need to be done with the player
            pos += frameDiff;
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
                foreach (Sentry s in game.enemyManager.sentries)
                {
                    if (nodePos.X == s.nodePos.X && nodePos.Y == s.nodePos.Y)
                    {
                        //System.Diagnostics.Debug.WriteLine("Bullet in same node as enemy");
                        if (s.col.checkPoint(pos))
                        {
                            s.damage(damage, direction);
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

            try
            {
                foreach (Asteroid a in game.asteroidManager.asteroids)
                {
                    if (nodePos.X == a.nodePos.X && nodePos.Y == a.nodePos.Y)
                    {
                        //System.Diagnostics.Debug.WriteLine("Bullet in same node as enemy");
                        if (a.col.checkPoint(pos))
                        {
                            a.damage(damage, pos);
                            game.modelManager.addEffect(new ImpactParticleModel(game, pos));
                            collided = true;
                        }
                    }
                }
            }
            catch (IndexOutOfRangeException) { }

            if (collided)
            {
                for (int i = 0; i < 10; i++)
                    game.modelManager.addEffect(new DotParticle(game, pos));
                if(type == 1)
                    game.modelManager.addEffect(new LaserHitEffect(game, pos, Color.Green));
                else
                    game.modelManager.addEffect(new LaserHitEffect(game, pos, Color.Orange));

                if (type != 1)
                {
                    //game.modelManager.addEffect(new BombExplosion(pos, game));
                    //game.ship.moneyManager.addGib(73, pos);
                }
                deleteProjectile();
            }
        }

        protected virtual void deleteProjectile()
        {
            game.modelManager.removeEffect(model);
            weapon.toDelete.Add(this);
            delete = true;
            model.Dispose();
        }

    }
}
