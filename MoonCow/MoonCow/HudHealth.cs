using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    public class HudHealth:HudComponent
    {
        Texture2D hudHealthB;
        Texture2D hudHealthF;

        Texture2D solidWhite;
        Texture2D shieldMask;
        Texture2D hpMask;

        Vector2 healthPos;

        String shieldValue;
        String hpValue;

        Vector2 shieldBarPos;
        Vector2 hpBarPos;

        ShipHealthSystem hpSys;

        RenderTarget2D targ1;
        RenderTarget2D targ2;
        RenderTarget2D targ3;
        SpriteBatch sb;
        Effect alphaMap;


        public HudHealth(Hud hud, SpriteFont font, Game1 game):base(hud, font, game)
        {
            healthPos = new Vector2(659, 45);
            shieldBarPos = new Vector2(960, 75);
            hpBarPos = new Vector2(960, 115);
            hpSys = game.ship.shipHealth;
            wakeThresh = 3;

            targ1 = new RenderTarget2D(game.GraphicsDevice, 603, 104);
            targ2 = new RenderTarget2D(game.GraphicsDevice, 603, 104);
            targ3 = new RenderTarget2D(game.GraphicsDevice, 603, 104);
            sb = new SpriteBatch(game.GraphicsDevice);

            solidWhite = new Texture2D(game.GraphicsDevice, 1, 1);
            solidWhite.SetData(new Color[] { Color.White });

            hudHealthF = game.Content.Load<Texture2D>(@"Hud/hudHealthF");
            hudHealthB = game.Content.Load<Texture2D>(@"Hud/hudHealthB");
            shieldMask = game.Content.Load<Texture2D>(@"Hud/Masks/hudHealthM1");
            hpMask = game.Content.Load<Texture2D>(@"Hud/Masks/hudHealthM2");
            alphaMap = TextureManager.alphaMap;
        }

        public override void Update()
        {
            shieldValue = "SHIELDS AT " + (int)hpSys.shieldVal + "%";
            hpValue = hpSys.hpVal + " HP";

            if (hpSys.hpVal < hpSys.hpMax || hpSys.shieldVal < hpSys.shieldMax)
                wakeTime = 0;
            else
            {
                if (wakeTime < wakeThresh)
                    wakeTime += Utilities.deltaTime;
            }

            makeBars();
        }

        void makeBars()
        {
            float sWidth = hpSys.shieldVal / hpSys.shieldMax;

            game.GraphicsDevice.SetRenderTarget(targ1);
            game.GraphicsDevice.Clear(Color.Transparent);
            sb.Begin();
            sb.Draw(TextureManager.pureWhite, new Rectangle(15, 8, (int)Math.Ceiling(573*sWidth), 80), null, hud.contSecondary, 0, Vector2.Zero, SpriteEffects.None, 1);
            sb.End();

            sWidth = hpSys.hpVal / hpSys.hpMax;
            game.GraphicsDevice.SetRenderTarget(targ2);
            game.GraphicsDevice.Clear(Color.Transparent);
            sb.Begin();
            sb.Draw(TextureManager.pureWhite, new Rectangle(91, 53, (int)Math.Ceiling(412 * sWidth), 35), null, hud.redBody, 0, Vector2.Zero, SpriteEffects.None, 1);
            sb.End();

            game.GraphicsDevice.SetRenderTarget(targ3);
            game.GraphicsDevice.Clear(Color.Transparent);
            alphaMap.Parameters["MaskTexture"].SetValue(shieldMask);
            // start a spritebatch for our effect
            sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend,
                null, null, null, alphaMap);
            //spB.Begin();
            sb.Draw((Texture2D)targ1, Vector2.Zero, Color.White);

            alphaMap.Parameters["MaskTexture"].SetValue(hpMask);
            sb.Draw((Texture2D)targ2, Vector2.Zero, Color.White);

            sb.End();
            game.GraphicsDevice.SetRenderTarget(null);
        }

        public override void Draw(SpriteBatch sb)
        {
            if (wakeTime < wakeThresh)
            {
                game.GraphicsDevice.BlendState = BlendState.Additive;
                sb.Draw(hudHealthB, hud.scaledRect(healthPos, 603, 104), Color.White);
                sb.Draw(hudHealthF, hud.scaledRect(healthPos, 603, 104), Color.White);
                sb.Draw((Texture2D)targ3, hud.scaledRect(healthPos, 603, 104), Color.White);

                /*sb.DrawString(font, shieldValue, hud.scaledCoords(shieldBarPos), hud.blueBody, 0,
                    new Vector2(font.MeasureString(shieldValue).X / 2, font.MeasureString(shieldValue).Y / 2), hud.scale * (28.0f / 40), SpriteEffects.None, 0);

                sb.DrawString(font, hpValue, hud.scaledCoords(hpBarPos), hud.redBody, 0,
                    new Vector2(font.MeasureString(hpValue).X / 2, font.MeasureString(hpValue).Y / 2), hud.scale * (20.0f / 40), SpriteEffects.None, 0);*/

            }
        }
    }
}
