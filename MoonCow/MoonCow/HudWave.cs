using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    class HudWave
    {
        Vector2 pos;
        Hud hud;
        HudAttackDisplayer displayer;
        Texture2D bigIco;
        Texture2D smlIco;
        public bool firstInList;
        float flashAlpha;
        float flashTime;
        Vector2 goalPos;
        Vector2 oldPos;

        bool moving;
        float moveTime;
        bool flashing;

        SpriteFont font;

        Wave wave;

        public HudWave(Hud hud, HudAttackDisplayer displayer, Vector2 pos, Wave wave)
        {
            this.hud = hud;
            this.displayer = displayer;
            font = hud.font;
            this.wave = wave;
            setPictures(wave.enemyType);
            this.pos = pos;
            goalPos = pos;
            flashing = true;
            firstInList = false;
        }

        public void nudge(Vector2 diff)
        {
            oldPos = pos;
            goalPos = pos += diff;
            moving = true;
            moveTime = 0;
        }
        void setPictures(int type)
        {
            switch(type-1)
            {
                default:
                    bigIco = displayer.swaBig;
                    smlIco = displayer.swaSml;
                    break;
                case 1:
                    bigIco = displayer.sneBig;
                    smlIco = displayer.sneSml;
                    break;
                case 2:
                    bigIco = displayer.gunBig;
                    smlIco = displayer.gunSml;
                    break;
                case 3:
                    bigIco = displayer.hevBig;
                    smlIco = displayer.hevSml;
                    break;

            }
        }

        public void Update()
        {

            if(moving)
            {
                pos = Vector2.SmoothStep(oldPos, goalPos, moveTime);
                moveTime += Utilities.deltaTime;
            }

            if(flashing)
            {
                flashAlpha = MathHelper.Lerp(1, 0, flashTime);
                flashTime += Utilities.deltaTime;
                if(flashTime >= 1)
                {
                    flashAlpha = 0;
                    flashing = false;
                }
            }

        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(displayer.wavFill, hud.scaledRect(pos, displayer.wavFill.Bounds.Width, displayer.wavFill.Bounds.Height), Color.White);
            sb.Draw(displayer.wavOut, hud.scaledRect(pos, displayer.wavFill.Bounds.Width, displayer.wavFill.Bounds.Height), Color.White);

            if(flashAlpha != 0)
                sb.Draw(displayer.wavOutW, hud.scaledRect(pos, displayer.wavFill.Bounds.Width, displayer.wavFill.Bounds.Height), Color.White * flashAlpha);

            if(firstInList)
                sb.Draw(smlIco, hud.scaledRect(pos, displayer.wavFill.Bounds.Width, displayer.wavFill.Bounds.Height), Color.White);
            else
                sb.Draw(bigIco, hud.scaledRect(pos, displayer.wavFill.Bounds.Width, displayer.wavFill.Bounds.Height), Color.White);

        }
    }
}
