﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MoonCow
{
    public class MgManager
    {
        Game1 game;
        Minigame minigame;
        MgInstance instance;

        List<MgMarker> upMark;
        List<MgMarker> downMark;
        List<MgMarker> leftMark;
        List<MgMarker> rightMark;
        public List<MgMarker> mToDelete;

        public List<SpriteParticle> particles;
        public List<SpriteParticle> pToDelete;

        public float speed;

        public CircleCollider upGoal;
        public CircleCollider downGoal;
        public CircleCollider leftGoal;
        public CircleCollider rightGoal;

        public Texture2D bg;
        public Texture2D markerSprite;
        public SpriteFont font;

        public RenderTarget2D rTarg;
        SpriteBatch sb;

        float time;
        float nextMarker;
        int markerMax;
        int markerCount;
        int missCount;

        MgKey upKey;
        MgKey downKey;
        MgKey leftKey;
        MgKey rightKey;
        List<MgKey> keys;


        public MgManager(Minigame minigame, Game1 game)
        {
            this.game = game;
            this.minigame = minigame;

            upGoal = new CircleCollider(new Vector2(414,254), 64);
            downGoal = new CircleCollider(new Vector2(604,800), 64);
            leftGoal = new CircleCollider(new Vector2(240,618), 64);
            rightGoal = new CircleCollider(new Vector2(784,430), 64);

            upMark = new List<MgMarker>();
            downMark = new List<MgMarker>();
            leftMark = new List<MgMarker>();
            rightMark = new List<MgMarker>();
            mToDelete = new List<MgMarker>();

            particles = new List<SpriteParticle>();
            pToDelete = new List<SpriteParticle>();

            bg = game.Content.Load<Texture2D>(@"Minigame/mgbg");
            markerSprite = game.Content.Load<Texture2D>(@"Minigame/mgMark");
            font = game.Content.Load<SpriteFont>(@"Hud/Venera40");

            rTarg = new RenderTarget2D(game.GraphicsDevice, 1024, 1024);
            sb = new SpriteBatch(game.GraphicsDevice);

            upKey = new MgKey(Buttons.DPadUp, Keys.W, Keys.Up, GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y, 0.3f);
            downKey = new MgKey(Buttons.DPadDown, Keys.S, Keys.Down, GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y, -0.3f);
            leftKey = new MgKey(Buttons.DPadLeft, Keys.A, Keys.Left, GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X, -0.3f);
            rightKey = new MgKey(Buttons.DPadRight, Keys.D, Keys.Right, GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X, 0.3f);
            keys = new List<MgKey>();
            keys.Add(upKey);
            keys.Add(downKey);
            keys.Add(leftKey);
            keys.Add(rightKey);

            markerMax = 8;
            markerCount = 0;
            nextMarker = 1;
            speed = 650;
        }

        void checkInputs()
        {
            foreach (MgKey k in keys)
                k.update();

            if (upKey.pressed)
            {
                sendHit(upMark);
            }
            if (downKey.pressed)
                sendHit(downMark);
            if (leftKey.pressed)
                sendHit(leftMark);
            if (rightKey.pressed)
                sendHit(rightMark);
        }

        void sendHit(List<MgMarker> marks)
        {
            MgMarker m = closestToGoal(marks);
            if (m != null)
                m.hit();
        }

        MgMarker closestToGoal(List<MgMarker> marks)
        {
            MgMarker closest = null;
            float closestDist = 2000;
            foreach(MgMarker m in marks)
            {
                if (!m.passedGoal && m.distFromGoal < closestDist)
                {
                    closestDist = m.distFromGoal;
                    closest = m;
                }
            }
            return closest;
        }

        public void Update()
        {
            if(markerCount < markerMax)
            {
                time += Utilities.deltaTime * speed/650;
                if (time > nextMarker)
                    addMarker();
            }

            checkInputs();

            foreach (MgMarker m in upMark)
                m.Update();
            foreach (MgMarker m in downMark)
                m.Update();
            foreach (MgMarker m in leftMark)
                m.Update();
            foreach (MgMarker m in rightMark)
                m.Update();

            foreach (MgMarker m in mToDelete)
            {
                upMark.Remove(m);
                rightMark.Remove(m);
                leftMark.Remove(m);
                downMark.Remove(m);
            }
            mToDelete.Clear();

            foreach (SpriteParticle p in particles)
                p.Update();
            foreach (SpriteParticle p in pToDelete)
                particles.Remove(p);
            pToDelete.Clear();

            Draw();

            if(markerCount == markerMax && upMark.Count()+downMark.Count()+leftMark.Count()+rightMark.Count() == 0)
            {
                success();
            }
        }

        void addMarker()
        {
            switch(instance.markTypes.ElementAt(markerCount))
            {
                default:
                    upMark.Add(new MgMarker(this, new Vector2(414, 1224), 0));
                    break;
                case 1:
                    downMark.Add(new MgMarker(this, new Vector2(604, -200), 1));
                    break;
                case 2:
                    leftMark.Add(new MgMarker(this, new Vector2(1224, 618), 2));
                    break;
                case 3:
                    rightMark.Add(new MgMarker(this, new Vector2(-200, 430), 3));
                    break;
            }
            nextMarker = instance.nextTimes.ElementAt(markerCount);
            //nextMarker = 1;
            markerCount++;
            time = 0;
        }

        public void Draw()
        {
            game.GraphicsDevice.SetRenderTarget(rTarg);
            game.GraphicsDevice.Clear(Color.Transparent);
            sb.Begin();
            sb.Draw(bg, Vector2.Zero, Color.White);

            //sb.Draw(markerSprite, new Rectangle((int)upGoal.centre.X, (int)upGoal.centre.Y, markerSprite.Bounds.Width, markerSprite.Bounds.Height), null, Color.White, 0, new Vector2(77), SpriteEffects.None, 0);
            //sb.Draw(markerSprite, new Rectangle((int)downGoal.centre.X, (int)downGoal.centre.Y, markerSprite.Bounds.Width, markerSprite.Bounds.Height), null, Color.White, 0, new Vector2(77), SpriteEffects.None, 0);
            //sb.Draw(markerSprite, new Rectangle((int)leftGoal.centre.X, (int)leftGoal.centre.Y, markerSprite.Bounds.Width, markerSprite.Bounds.Height), null, Color.White, 0, new Vector2(77), SpriteEffects.None, 0);
            //sb.Draw(markerSprite, new Rectangle((int)rightGoal.centre.X, (int)rightGoal.centre.Y, markerSprite.Bounds.Width, markerSprite.Bounds.Height), null, Color.White, 0, new Vector2(77), SpriteEffects.None, 0);

            foreach (SpriteParticle p in particles)
                p.Draw(sb);

            foreach (MgMarker m in upMark)
                m.Draw(sb);
            foreach (MgMarker m in downMark)
                m.Draw(sb);
            foreach (MgMarker m in leftMark)
                m.Draw(sb);
            foreach (MgMarker m in rightMark)
                m.Draw(sb);

            sb.End();
            game.GraphicsDevice.SetRenderTarget(null);
        }

        public void miss()
        {
            missCount++;
            if (missCount >= 3)
                minigame.abort();
        }

        public void success()
        {
            minigame.successCount++;
            minigame.attempts++;
            minigame.abort();
            minigame.updateStats(true);
        }

        public void loadInstance(MgInstance instance)
        {
            this.instance = instance;
        }

        public void reset()
        {
            upMark.Clear();
            downMark.Clear();
            leftMark.Clear();
            rightMark.Clear();
            mToDelete.Clear();
            particles.Clear();
            pToDelete.Clear();

            markerCount = 0;
            nextMarker = 1;
            markerMax = instance.markTypes.Count();
            missCount = 0;
        }
    }
}
