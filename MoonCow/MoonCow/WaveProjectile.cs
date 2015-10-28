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

        List<Enemy> eHitList; //once enemy is hit, added to the list so damage is only applied once
        List<Sentry> sHitList;
        List<Asteroid> aHitList;
        bool hasHit;


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

            model = new WaveProjectileModel(this, game, type);
            boundBox = new OOBB(pos, this.direction, 0.1f, 10f);

            eHitList = new List<Enemy>();
            sHitList = new List<Sentry>();
            aHitList = new List<Asteroid>();

            game.modelManager.addEffect(model);

            switch(type)
            {
                default:
                    damage = 10;
                    break;
                case 2:
                    damage = 20;
                    break;
                case 3:
                    damage = 30;
                    break;
            }
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
            scale = MathHelper.SmoothStep(1, maxScale*2, time);

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

            // By moving each component of the vector one at a time and seeing what causes the collision we can eliminate only that component
            // this means the ship will slide along walls rather than stick. Doing two collision checks per frame for the player seems to
            // be within tolerable limits for CPU time. This will only need to be done with the player
            //## COLLISIONS WHOOO! ##
            // Move the bounding box to new pos
            //circleCol.Update(pos, direction);
            // Get current node co-ordinates
            nodePos = new Vector2((int)((pos.X / 30) + 0.5f), (int)((pos.Z / 30) + 0.5f));

            try
            {
                foreach (Enemy enemy in game.enemyManager.enemies)
                {
                    if (enemy.nodePos.X >= nodePos.X - 1 && enemy.nodePos.X <= nodePos.X + 1 &&
                        enemy.nodePos.Y >= nodePos.Y - 1 && enemy.nodePos.Y <= nodePos.Y + 1)
                    {
                        if (!eHitList.Contains(enemy))
                        {
                            bool hit = false;
                            foreach (CircleCollider c in enemy.cols)
                            {
                                if (c.checkOOBB(boundBox))
                                {
                                    hit = true;
                                }
                            }
                            if (hit)
                            {
                                enemy.damage(damage);
                                eHitList.Add(enemy);
                                collided = true;
                                wep.addExp(10);
                            }
                        }
                    }
                }
            }
            catch (IndexOutOfRangeException)
            {
            }

            foreach (Sentry s in game.enemyManager.sentries)
            {
                if (!sHitList.Contains(s))
                {
                    if (s.col.checkOOBB(boundBox))
                    {
                        sHitList.Add(s);
                        Vector3 dir = new Vector3();
                        dir.X = pos.X - s.pos.X;
                        dir.Z = pos.Z - s.pos.Z;
                        dir.Normalize();
                        if (type == 3)
                            s.drillDamage(2, dir * -1, true);
                        else
                            s.damage(damage, dir * -1);

                        wep.addExp(5);
                        collided = true;
                    }
                }
            }

            foreach (Asteroid a in game.asteroidManager.asteroids)
            {
                if (!aHitList.Contains(a))
                {
                    if (a.col.checkOOBB(boundBox))
                    {
                        aHitList.Add(a);
                        a.damage(damage, pos);
                        wep.addExp(damage);
                        collided = true;
                    }
                }
            }

            if (collided)
            {
                game.camera.setYShake(type * 0.07f);
                if (!hasHit)
                {
                    game.levelStats.wavesHit++;
                    hasHit = true;
                }
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
