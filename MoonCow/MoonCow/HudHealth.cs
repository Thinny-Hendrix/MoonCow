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

        RenderTarget2D rTarg;
        SpriteBatch sb;


        public HudHealth(Hud hud, SpriteFont font, Game1 game):base(hud, font, game)
        {
            healthPos = new Vector2(659, 45);
            shieldBarPos = new Vector2(960, 75);
            hpBarPos = new Vector2(960, 115);
            hpSys = game.ship.shipHealth;
            wakeThresh = 3;

            rTarg = new RenderTarget2D(game.GraphicsDevice, 603, 104);
            sb = new SpriteBatch(game.GraphicsDevice);

            solidWhite = new Texture2D(game.GraphicsDevice, 1, 1);
            solidWhite.SetData(new Color[] { Color.White });

            hudHealthF = game.Content.Load<Texture2D>(@"Hud/hudHealthF");
            hudHealthB = game.Content.Load<Texture2D>(@"Hud/hudHealthB");
            shieldMask = game.Content.Load<Texture2D>(@"Hud/hudHealthM1");
            hpMask = game.Content.Load<Texture2D>(@"Hud/hudHealthM2");
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
        }

        void makeShieldBar()
        {
            game.GraphicsDevice.SetRenderTarget(rTarg);

            sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
            game.GraphicsDevice.Clear(Color.Transparent);
            sb.Draw(shieldMask, Vector2.Zero, Color.White); //The mask                                   
            //sb.End();

            //sb.Begin(SpriteSortMode.Immediate, null, null, s2, null, a);
            sb.Draw(hudHealthB, hud.scaledRect(Vector2.Zero, 603, 104), null, hud.blueBody, 0, Vector2.Zero, SpriteEffects.None, 1); //The background
            sb.End();

            sb.Begin();
        }

        public override void Draw(SpriteBatch sb)
        {
            if (wakeTime < wakeThresh)
            {
                game.GraphicsDevice.BlendState = BlendState.Additive;
                sb.Draw(hudHealthB, hud.scaledRect(healthPos, 603, 104), Color.White);
                sb.Draw(hudHealthF, hud.scaledRect(healthPos, 603, 104), Color.White);

                sb.DrawString(font, shieldValue, hud.scaledCoords(shieldBarPos), hud.blueBody, 0,
                    new Vector2(font.MeasureString(shieldValue).X / 2, font.MeasureString(shieldValue).Y / 2), hud.scale * (28.0f / 40), SpriteEffects.None, 0);

                sb.DrawString(font, hpValue, hud.scaledCoords(hpBarPos), hud.redBody, 0,
                    new Vector2(font.MeasureString(hpValue).X / 2, font.MeasureString(hpValue).Y / 2), hud.scale * (20.0f / 40), SpriteEffects.None, 0);

            }
        }
    }
}
