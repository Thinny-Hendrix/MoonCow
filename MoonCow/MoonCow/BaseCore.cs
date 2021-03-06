﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    public class BaseCore:GameComponent
    {

        public float health;
        public float maxHealth;
        Game1 game;
        public Vector3 pos;
        public CircleCollider col;
        CoreSphereModel model;
        public Vector2 nodePos;
        public List<BaseCoreSpot> spots;
        public List<BaseCoreSpot> waitSpots;
        bool triggeredEnd;
        float time;
        bool splode;
        CoreRing ring;

        public BaseCore(Game1 game):base(game)
        {
            this.game = game;
            maxHealth = 500;
            health = maxHealth;
        }

        void setWaitSpots()
        {
            waitSpots = new List<BaseCoreSpot>();
            waitSpots.Add(new BaseCoreSpot(this, pos + new Vector3(-10, 0, -35), Vector3.Backward));
            waitSpots.Add(new BaseCoreSpot(this, pos + new Vector3(10, 0, -35), Vector3.Backward));
        }

        void setSpots()
        {
            spots = new List<BaseCoreSpot>();
            float angle = 0;
            for(int i = 0; i < 30; i++)
            {
                spots.Add(new BaseCoreSpot(this, angle));
                angle += MathHelper.Pi / 15;
            }
        }

        public void setPos(Vector3 pos)
        {
            this.pos = pos;
            this.pos.Y += 4.5f;
            nodePos = new Vector2((int)((pos.X / 30) + 0.5f), (int)((pos.Z / 30) + 0.5f));

            setSpots();
            setWaitSpots();
            col = new CircleCollider(pos, 8);
            model = new CoreSphereModel(pos, game);
            game.modelManager.addAdditive(model);
            ring = new CoreRing(pos, game);
            game.modelManager.addEffect(ring);

            game.camera.setEndData(pos);
        }

        public void damage(float amount)
        {
            health -= amount;

            if(health <= 0)
            {
                if (!triggeredEnd)
                {
                    game.hud.hudEnd.activate(false);
                    game.camera.triggerEnd();
                    triggeredEnd = true;
                }
            }
        }

        List<BaseCoreSpot> hevNeighbors(int i)
        {
            List<BaseCoreSpot> s = new List<BaseCoreSpot>();
            //does a magic thing to loop back to the start if reach the end of the list
            s.Add(spots.ElementAt((i - 2 + 30) % 30));
            s.Add(spots.ElementAt((i - 1 + 30) % 30));
            s.Add(spots.ElementAt((i + 1 + 30) % 30));
            s.Add(spots.ElementAt((i + 2 + 30) % 30));

            return s;
        }

        public BaseCoreSpot getWaitSpot(Vector3 pos)
        {
            foreach (BaseCoreSpot s in waitSpots)
            {
                if(!s.taken)
                {
                    s.taken = true;
                    return s;
                }
            }
            return null;
        }

        public BaseCoreSpot getHeavySpot(Vector3 pos)
        {
            for(int i = 0; i < spots.Count(); i++)
            {
                if(!spots.ElementAt(i).taken)
                {
                    //checks the two spots on either side to see if they are all free, if so they're now all taken
                    List<BaseCoreSpot>s = hevNeighbors(i);

                    bool used = false;
                    foreach(BaseCoreSpot spo in s)
                    {
                        if (spo.taken)
                            used = true;
                    }
                    if(!used)
                    {
                        foreach (BaseCoreSpot spo in s)
                            spo.taken = true;
                        spots.ElementAt(i).taken = true;
                        return spots.ElementAt(i);
                    }
                }
            }
            return null;
        }

        public void releaseHeavySpot(BaseCoreSpot spot)
        {
            List<BaseCoreSpot> s = hevNeighbors(spots.IndexOf(spot));
            foreach (BaseCoreSpot spo in s)
                spo.taken = false;
            spot.taken = false;
        }

        public BaseCoreSpot getSpot(Vector3 pos)
        {
            Vector3 dir = col.directionFrom(pos);
            float angle = (float)Math.Atan2(dir.X,dir.Z);
            BaseCoreSpot b = null;
            foreach(BaseCoreSpot s in spots)
            {
                if(s.rot < angle)
                {
                    if (!s.taken)
                    {
                        b = s;
                    }
                }
            }
            if(b == null)//if it couldn't get a good spot close to it,just get the next vacant spot
            {
                foreach(BaseCoreSpot s in spots)
                {
                    if(!s.taken)
                    {
                        b = s;
                    }
                    if (b != null)
                        break;                        
                }
            }
            b.taken = true;
            return b;
        }

        public List<Vector3> coordsToWait(BaseCoreSpot b, Vector3 currentPos, Vector3 corePos)
        {
            List<Vector3> temp = new List<Vector3>();

            if(corePos.X < pos.X)
            {
                if(currentPos.X < pos.X)
                {
                    temp.Add(pos+new Vector3(-30,0,-35));
                }
                else
                {
                    if(waitSpots.ElementAt(1).taken)
                    {
                        temp.Add(pos + new Vector3(30, 0, -15));
                        temp.Add(pos + new Vector3(0, 0, -15));
                    }
                    else
                    {
                        temp.Add(pos + new Vector3(30, 0, -35));
                    }
                }
            }
            else
            {
                if(currentPos.X > pos.X)
                {
                    temp.Add(pos + new Vector3(30, 0, -35));
                }
                else
                {
                    if (waitSpots.ElementAt(0).taken)
                    {
                        temp.Add(pos + new Vector3(-30, 0, -15));
                        temp.Add(pos + new Vector3(0, 0, -15));
                    }
                    else
                    {
                        temp.Add(pos + new Vector3(-30, 0, -35));
                    }
                }
            }

            temp.Add(corePos);

            return temp;
        }

        public List<Vector3> coordsToSpot(BaseCoreSpot b, Vector3 currentPos, Vector3 corePos)
        {
            List<Vector3> temp = new List<Vector3>();

            if(b.rot <= MathHelper.PiOver4*0.5f || b.rot >= MathHelper.PiOver4*7.5f)
            {
                if(currentPos.X > pos.X+15)
                {
                    temp.Add(pos + new Vector3(30, 0, -30));
                }
                else if(currentPos.X < pos.X-15)
                {
                    temp.Add(pos + new Vector3(-30, 0, -30));
                }
                temp.Add(pos + new Vector3(0, 0, -30));
                temp.Add(corePos);
            }
            else if (b.rot > MathHelper.PiOver4*0.5f && b.rot <= MathHelper.PiOver4*1.5f)
            {
                if (currentPos.X < pos.X)
                {
                    temp.Add(pos + new Vector3(-30, 0, -30));
                }
                else
                {
                    temp.Add(pos + new Vector3(30, 0, -30));
                    temp.Add(pos + new Vector3(0, 0, -30));
                    temp.Add(pos + new Vector3(-30, 0, -30));
                }
                temp.Add(corePos);
            }
            else if (b.rot > MathHelper.PiOver4 * 1.5f && b.rot <= MathHelper.PiOver4 * 2.5f)
            {
                if (currentPos.X < pos.X)
                {
                    temp.Add(pos + new Vector3(-30, 0, -30));
                }
                else
                {
                    temp.Add(pos + new Vector3(30, 0, -30));
                    temp.Add(pos + new Vector3(0, 0, -30));
                    temp.Add(pos + new Vector3(-30, 0, -30));
                }
                temp.Add(pos + new Vector3(-30, 0, 0));
                temp.Add(corePos);
            }
            else if (b.rot > MathHelper.PiOver4 * 2.5f && b.rot <= MathHelper.PiOver4 * 3.5f)
            {
                if (currentPos.X < pos.X)
                {
                    temp.Add(pos + new Vector3(-30, 0, -30));
                }
                else
                {
                    temp.Add(pos + new Vector3(30, 0, -30));
                    temp.Add(pos + new Vector3(0, 0, -30));
                    temp.Add(pos + new Vector3(-30, 0, -30));
                }
                temp.Add(pos + new Vector3(-30, 0, 0));
                temp.Add(pos + new Vector3(-30, 0, 30));
                temp.Add(corePos);
            }
            else if (b.rot > MathHelper.PiOver4 * 3.5f && b.rot <= MathHelper.PiOver4 * 4.5f)
            {
                if (currentPos.X < pos.X)
                {
                    temp.Add(pos + new Vector3(-30, 0, -30));
                    temp.Add(pos + new Vector3(-30, 0, 0));
                    temp.Add(pos + new Vector3(-30, 0, 30));
                }
                else
                {
                    temp.Add(pos + new Vector3(30, 0, -30));
                    temp.Add(pos + new Vector3(30, 0, 0));
                    temp.Add(pos + new Vector3(30, 0, 30));
                }
                temp.Add(pos + new Vector3(0, 0, 30));
                temp.Add(corePos);
            }
            else if (b.rot > MathHelper.PiOver4 * 4.5f && b.rot <= MathHelper.PiOver4 * 5.5f)
            {
                if (currentPos.X < pos.X)
                {
                    temp.Add(pos + new Vector3(-30, 0, -30));
                    temp.Add(pos + new Vector3(0, 0, -30));
                }
                else
                {
                }
                temp.Add(pos + new Vector3(30, 0, -30));
                temp.Add(pos + new Vector3(30, 0, 0));
                temp.Add(pos + new Vector3(30, 0, 30));
                temp.Add(corePos);
            }
            else if (b.rot > MathHelper.PiOver4 * 5.5f && b.rot <= MathHelper.PiOver4 * 6.5f)
            {
                if (currentPos.X < pos.X)
                {
                    temp.Add(pos + new Vector3(-30, 0, -30));
                    temp.Add(pos + new Vector3(0, 0, -30));
                    temp.Add(pos + new Vector3(30, 0, -30));
                }
                else
                {
                    temp.Add(pos + new Vector3(30, 0, -30));
                }
                temp.Add(pos + new Vector3(30, 0, 0));
                temp.Add(corePos);
            }
            else//above 4pi*6.5 and less than 4pi*7.5
            {
                if (currentPos.X < pos.X)
                {
                    temp.Add(pos + new Vector3(-30, 0, -30));
                    temp.Add(pos + new Vector3(0, 0, -30));
                    temp.Add(pos + new Vector3(30, 0, -30));
                }
                else
                {
                    temp.Add(pos + new Vector3(30, 0, -30));
                }
                temp.Add(corePos);
            }

            return temp;
        }

        public override void Update(GameTime gameTime)
        {
            if(triggeredEnd)
            {
                if(!splode)
                {
                    time += Utilities.deltaTime;

                    if(time > 4.5f)
                    {
                        splode = true;
                        model.visible = false;
                        for(int i = 0; i < 50; i++)
                        {
                            game.modelManager.addEffect(new ElectroDir(pos, Color.White, Color.Aqua, game));
                        }
                        game.modelManager.addEffect(new LaserHitEffect(game, pos, Color.White, 3, BlendState.Additive));
                        game.modelManager.addEffect(new LaserHitEffect(game, pos, Color.White, 5, BlendState.Additive));
                        //game.modelManager.addEffect(new ImpactParticleModel(game, pos, 3));
                        game.modelManager.removeEffect(ring);
                        game.camera.makeShake();
                    }
                }
            }
        }
    }
}
