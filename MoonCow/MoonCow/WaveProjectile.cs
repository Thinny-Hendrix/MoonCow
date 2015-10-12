using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    class WaveProjectile:Projectile
    {
        WeaponWave wep;
        int type;
        Color c1;
        Color c2;
        WaveProjectileModel model;

        public WaveProjectile(Vector3 pos, Vector3 direction, Game1 game, WeaponWave wep, int type)
            : base()
        {
            this.direction = direction;
            this.game = game;
            this.pos = pos;
            this.rot.Y = (float)Math.Atan2(direction.X, direction.Z);
            this.wep = wep;
            this.type = type;

            speed = 50;
            life = 200;
            damage = 5;

            col = new CircleCollider(pos, 0.2f);
            if (type == 1)
            {
                c1 = Color.Green;
                c2 = Color.CornflowerBlue;
                //model = new ProjectileModel(ModelLibrary.projectile, pos, this, c1, c2, game);

            }
            else if (type == 2)
            {
                c1 = Color.Blue;
                c2 = Color.Aqua;
                //model = new ProjectileModel(ModelLibrary.projectile, pos, this, c1, c2, game);
            }
            else
            {
                c1 = Color.Red;
                c2 = Color.Purple;
                //model = new ProjectileModel(this, TextureManager.elecRound64, TextureManager.elecRound64, TextureManager.elecTrail64, c1, c2, new Color(1, 0.4f, 0), game);
            }
            //game.modelManager.addEffect(model);
        }

        public override void Update()
        {
            frameDiff = Vector3.Zero;

            frameDiff += direction * speed * Utilities.deltaTime;
            col.Update(pos);
            checkCollision();

            life -= Utilities.deltaTime * 60;
            if(life <=0)
            {
                deleteProjectile();
            }
        }

        void onImpact()
        {
            for (int i = 0; i < 10; i++)
                game.modelManager.addEffect(new DotParticle(game, pos));

            game.modelManager.addEffect(new LaserHitEffect(game, pos, c1));
            if (type == 3)
            {
                game.modelManager.addEffect(new LaserHitEffect(game, pos, Color.Orange));
                for (int i = 0; i < 5; i++)
                    game.modelManager.addEffect(new ElecParticle(pos, game, c1, c2));
            }
        }

        protected override void checkCollision()
        {
            // By moving each component of the vector one at a time and seeing what causes the collision we can eliminate only that component
            // this means the ship will slide along walls rather than stick. Doing two collision checks per frame for the player seems to
            // be within tolerable limits for CPU time. This will only need to be done with the player
            pos += frameDiff;
            bool collided = false;

            //## COLLISIONS WHOOO! ##
            // Move the bounding box to new pos
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
                    if (nodePos.X == enemy.nodePos.X && nodePos.Y == enemy.nodePos.Y)
                    {
                        //System.Diagnostics.Debug.WriteLine("Bullet in same node as enemy");
                        if (col.checkOOBB(enemy.boundingBox))
                        {
                            enemy.health -= damage;
                            game.modelManager.addEffect(new ImpactParticleModel(game, pos));
                            wep.addExp(5);
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
                onImpact();
                deleteProjectile();
            }
        }

        protected override void deleteProjectile()
        {
            //game.modelManager.removeEffect(model);
            wep.toDelete.Add(this);
            //model.Dispose();
        }
    }
}
