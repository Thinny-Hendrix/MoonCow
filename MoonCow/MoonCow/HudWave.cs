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
        Game1 game;

        public bool moving;
        float moveTime;
        bool flashing;

        SpriteFont font;
        RenderTarget2D targ1;
        RenderTarget2D targ2;
        SpriteBatch sb;
        Effect alphaMap;
        Texture2D barMask;

        Wave wave;

        public HudWave(Game1 game, Hud hud, HudAttackDisplayer displayer, Vector2 pos, Wave wave)
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
            moving = false;
            this.game = game;
            targ1 = new RenderTarget2D(game.GraphicsDevice, 170, 89);
            targ2 = new RenderTarget2D(game.GraphicsDevice, 170, 89);
            sb = new SpriteBatch(game.GraphicsDevice);
            alphaMap = TextureManager.alphaMap;
        }

        public void Dispose()
        {
            targ1.Dispose();
            targ2.Dispose();
            sb.Dispose();
        }

        public void nudge(Vector2 diff)
        {
            oldPos = pos;
            goalPos = pos + diff;
            moving = true;
            moveTime = 0;
        }
        void setPictures(int type)
        {
            switch(type)
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

        public void flash()
        {
            flashing = true;
            flashAlpha = 1;
            flashTime = 0;
        }

        public void Update()
        {

            if(moving)
            {
                pos = Vector2.SmoothStep(oldPos, goalPos, moveTime);
                moveTime += Utilities.deltaTime*3;
                if (moveTime >= 1)
                {
                    pos = goalPos;
                    moving = false;
                }
            }

            if(flashing)
            {
                flashAlpha = MathHelper.Lerp(1, 0, flashTime);
                flashTime += Utilities.deltaTime*4;
                if(flashTime >= 1)
                {
                    flashAlpha = 0;
                    flashing = false;
                }
            }

            drawBar();

        }

        void drawBar()
        {
            float scale = (displayer.activeAttack.maxWait-displayer.activeAttack.waitTime) / displayer.activeAttack.maxWait;
            game.GraphicsDevice.SetRenderTarget(targ1);
            game.GraphicsDevice.Clear(Color.Transparent);
            sb.Begin();
            sb.Draw(TextureManager.pureWhite, new Rectangle(23, 68, (int)Math.Ceiling(117 * scale), 15), null, hud.redBody, 0, Vector2.Zero, SpriteEffects.None, 1);
            sb.End();

            game.GraphicsDevice.SetRenderTarget(targ2);
            game.GraphicsDevice.Clear(Color.Transparent);
            alphaMap.Parameters["MaskTexture"].SetValue(displayer.barMask);
            // start a spritebatch for our effect
            sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend,
                null, null, null, alphaMap);
            //spB.Begin();
            sb.Draw((Texture2D)targ1, Vector2.Zero, Color.White);

            sb.End();
            game.GraphicsDevice.SetRenderTarget(null);
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(displayer.wavFill, hud.scaledRect(pos, displayer.wavFill.Bounds.Width, displayer.wavFill.Bounds.Height), Color.White);
            sb.Draw(displayer.wavOut, hud.scaledRect(pos, displayer.wavFill.Bounds.Width, displayer.wavFill.Bounds.Height), Color.White);

            if(flashAlpha != 0)
                sb.Draw(displayer.wavOutW, hud.scaledRect(pos, displayer.wavFill.Bounds.Width, displayer.wavFill.Bounds.Height), Color.White * flashAlpha);

            if (firstInList)
            {
                sb.Draw(smlIco, hud.scaledRect(pos, displayer.wavFill.Bounds.Width, displayer.wavFill.Bounds.Height), Color.White);
                sb.Draw((Texture2D)targ2, hud.scaledRect(pos, displayer.wavFill.Bounds.Width, displayer.wavFill.Bounds.Height), Color.White);

                /*sb.DrawString(font, ""+(int)displayer.activeAttack.waitTime, hud.scaledCoords(pos + new Vector2(90,78)), hud.redBody, 0,
                    new Vector2(font.MeasureString("" + (int)displayer.activeAttack.waitTime).X, font.MeasureString("" + (int)displayer.activeAttack.waitTime).Y / 2), hud.scale * (16.0f / 40), SpriteEffects.None, 0);
                */
                sb.DrawString(font, "" + (int)wave.waveMax, hud.scaledCoords(pos + new Vector2(140, 50)), Color.White, 0,
                    new Vector2(font.MeasureString("" + (int)wave.waveMax).X, font.MeasureString("" + (int)wave.waveMax).Y / 2), hud.scale * (20.0f / 40), SpriteEffects.None, 0);
            }
            else
            {
                sb.Draw(bigIco, hud.scaledRect(pos, displayer.wavFill.Bounds.Width, displayer.wavFill.Bounds.Height), Color.White);

                sb.DrawString(font, "" + (int)wave.waveMax, hud.scaledCoords(pos + new Vector2(130, 75)), Color.White, 0,
                    new Vector2(font.MeasureString("" + (int)wave.waveMax).X, font.MeasureString("" + (int)wave.waveMax).Y / 2), hud.scale * (20.0f / 40), SpriteEffects.None, 0);
            }

        }
    }
}
