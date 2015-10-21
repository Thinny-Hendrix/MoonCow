using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    public class WaveProjectile:Projectile
    {
        WeaponWave wep;
        int type;
        Color c1;
        Color c2;
        OOBB boundBox;
        public float scale;
        float maxScale;
        public float time;

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
            life = 30;
            damage = 5;
            scale = 1;
            maxScale = 10;
            time = 0;

            model = new WaveProjectileModel(this, game);
            boundBox = new OOBB(pos, this.direction, 0.1f, 10f);

            game.modelManager.addEffect(model);
        }

        public override void Update()
        {
            frameDiff = Vector3.Zero;

            frameDiff += direction * speed * Utilities.deltaTime;

            if (time < 1)
            {
                time += Utilities.deltaTime * 3;
                if (time > 1)
                    time = 1;
            }
            scale = MathHelper.SmoothStep(1, maxScale, time);

            boundBox.resize(1, 1f * scale);
            boundBox.Update(pos, direction);


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
                    if (boundBox.intersects(box))
                    {
                        //collided = true;
                    }
                }
            }
            catch (IndexOutOfRangeException)
            {
                //deleteProjectile();
            }

            try
            {
                foreach (Enemy enemy in game.enemyManager.enemies)
                {
                    if (nodePos.X == enemy.nodePos.X && nodePos.Y == enemy.nodePos.Y)
                    {
                        //System.Diagnostics.Debug.WriteLine("Bullet in same node as enemy");
                        foreach (CircleCollider c in enemy.cols)
                        {
                            if (c.checkOOBB(boundBox))
                            {
                                enemy.health -= damage;
                                game.levelStats.wavesHit++;
                                game.modelManager.addEffect(new ImpactParticleModel(game, pos));
                                wep.addExp(5);
                                //collided = true;
                            }
                        }
                    }
                }
            }
            catch (IndexOutOfRangeException)
            {
                //deleteProjectile();
            }

            try
            {
                foreach (Sentry s in game.enemyManager.sentries)
                {
                    if (nodePos.X == s.nodePos.X && nodePos.Y == s.nodePos.Y)
                    {
                        //System.Diagnostics.Debug.WriteLine("Bullet in same node as enemy");
                        if (s.col.checkOOBB(boundBox))
                        {
                            s.damage(damage, direction);
                            game.modelManager.addEffect(new ImpactParticleModel(game, pos));
                            //collided = true;
                        }
                    }
                }
            }
            catch (IndexOutOfRangeException)
            {
                //deleteProjectile();
            }

            try
            {
                foreach (Asteroid a in game.asteroidManager.asteroids)
                {
                    if (nodePos.X == a.nodePos.X && nodePos.Y == a.nodePos.Y)
                    {
                        //System.Diagnostics.Debug.WriteLine("Bullet in same node as enemy");
                        if (a.col.checkOOBB(boundBox))
                        {
                            a.damage(damage, pos);
                            game.modelManager.addEffect(new ImpactParticleModel(game, pos));
                            //collided = true;
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
            wep.toDelete.Add(this);
            model.Dispose();
            game.modelManager.removeEffect(model);
        }
    }
}
