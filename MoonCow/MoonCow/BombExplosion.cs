using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    public class BombExplosion
    {
        Vector3 pos;
        Game1 game;
        Vector2 nodePos;
        List<Enemy> eHitList; //once enemy is hit, added to the list so damage is only applied once
        List<Sentry> sHitList;
        List<Asteroid> aHitList;

        float damage;

        CircleCollider collider;
        bool colEnabled;


        float time;
        bool triggeredShake;
        bool triggeredParticles;
        int type;
        Color c1;
        Color c2;
        WeaponBomb wep;
        bool hasHit;

        public BombExplosion(Vector3 pos, Game1 game, int type, WeaponBomb wep):base()
        {
            this.wep = wep;
            this.pos = pos;
            this.game = game;
            this.type = type;

            collider = new CircleCollider(pos, 5);

            triggeredShake = false;

            switch(type)
            {
                default:
                    c1 = Color.Red;
                    c2 = Color.Orange;
                    damage = 10;
                    break;

                case 2:
                    c1 = Color.Red;
                    c2 = Color.Orange;
                    damage = 15;
                    break;

                case 3:
                    c1 = Color.Aqua;
                    c2 = Color.Purple;
                    damage = 20;
                    break;
            }

            eHitList = new List<Enemy>();
            sHitList = new List<Sentry>();
            aHitList = new List<Asteroid>();

            if (type != 1)
            {
                game.modelManager.addEffect(new BombSpin(game, pos, Color.Black, 0.1f));
                game.modelManager.addEffect(new BombRings(pos, game, this, type));
            }
            else
            {
                time = 15;
            }

        }

        public void Update()
        {
            float frameTime = Utilities.deltaTime*60;

            if(time >= 15)
            {
                if(!triggeredShake)
                {
                    colEnabled = true;

                    game.modelManager.addEffect(new BombCenterParticle(game, pos, type));

                    if(type == 1)
                    {
                        game.camera.setYShake(0.2f);
                        game.modelManager.addEffect(new LaserHitEffect(game, pos, Color.Orange, 1.5f, BlendState.Additive));

                        for (int i = 0; i < 30; i++)
                        {
                            game.modelManager.addEffect(new DirLineParticle(pos, game));
                        }
                        /*
                        for(int i = 0; i < 15; i++)
                        {
                            game.modelManager.addEffect(new BombCircleParticle(pos, game, Vector3.Normalize(new Vector3(Utilities.nextFloat() * 2 - 1, Utilities.nextFloat() * 2 - 1, Utilities.nextFloat() * 2 - 1)), 0));
                        }
                        for (int i = 0; i < 15; i++)
                        {
                            game.modelManager.addEffect(new BombCircleParticle(pos, game, Vector3.Normalize(new Vector3(Utilities.nextFloat() * 2 - 1, Utilities.nextFloat() * 2 - 1, Utilities.nextFloat() * 2 - 1)), 1));
                        }*/
                    }
                    else 
                    {
                        game.camera.makeShake();
                        game.hud.makeFlash();

                        for (int i = 0; i < 30; i++)
                        {
                            game.modelManager.addEffect(new DirLineParticle(pos, game));
                        }

                        for (int i = 0; i < 15; i++)
                        {
                            game.modelManager.addEffect(new ElectroDir(pos, c1, c2, game));
                        }

                        game.modelManager.addEffect(new LaserHitEffect(game, pos, Color.Black, 4, BlendState.AlphaBlend));


                        if(type == 2)
                        {

                        }
                        else
                        {
                            for (int i = 0; i < 30; i++)
                            {
                                game.modelManager.addEffect(new ElectroDir(pos, c1, c2, game));
                            }
                        }
                    }
                    
                    triggeredShake = true;


                    /*float angle = 0;
                    for (int i = 0; i < 50; i++ )
                    {
                        game.modelManager.addEffect(new BombCircleParticle(pos, game, angle));
                        angle += MathHelper.Pi / 25;
                    }*/
                }
                if(!triggeredParticles && time > 15)
                {
                    //for (int i = 0; i < 30; i++)
                      //  game.modelManager.addEffect(new DotParticle(game, pos, 30));
                    triggeredParticles = true;
                }

                if(collider.radius < 17)
                {
                    collider.radius += Utilities.deltaTime * 60;
                    if(collider.radius >= 17)
                        colEnabled = false;
                }
                //damage -= Utilities.deltaTime;
                if(colEnabled)
                    checkCollision();
            }
            time += frameTime;
            if (time > 120)
            {
                
            }
        }

        void checkCollision()
        {
            // By moving each component of the vector one at a time and seeing what causes the collision we can eliminate only that component
            // this means the ship will slide along walls rather than stick. Doing two collision checks per frame for the player seems to
            // be within tolerable limits for CPU time. This will only need to be done with the player
            bool collided = false;

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
                            foreach(CircleCollider c in enemy.cols)
                            {
                                if(c.checkCircle(collider))
                                {
                                    hit = true;
                                }
                            }
                            if(hit)
                            {
                                enemy.damage(damage);
                                eHitList.Add(enemy);
                                collided = true;
                                wep.addExp(damage);
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
                if(!sHitList.Contains(s))
                {
                    if(collider.checkCircle(s.col))
                    {
                        sHitList.Add(s);
                        Vector3 dir = new Vector3();
                        dir.X = pos.X - s.pos.X;
                        dir.Z = pos.Z - s.pos.Z;
                        dir.Normalize();
                        if(type == 3)
                            s.drillDamage(2, dir * -1, true);
                        else
                            s.damage(2, dir * -1);

                        wep.addExp(damage);

                    }
                }
            }

            foreach(Asteroid a in game.asteroidManager.asteroids)
            {
                if(!aHitList.Contains(a))
                {
                    if(collider.checkCircle(a.col))
                    {
                        aHitList.Add(a);
                        a.damage(damage, pos);
                        wep.addExp(damage);
                    }
                }
            }

            if (collided)
            {
                if(!hasHit)
                {
                    game.levelStats.bombsHit++;
                    hasHit = true;
                }
            }
        }
    }
}
