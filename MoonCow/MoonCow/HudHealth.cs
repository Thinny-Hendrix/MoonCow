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
        Vector2 healthPos;

        String shieldValue;
        String hpValue;

        Vector2 shieldBarPos;
        Vector2 hpBarPos;

        ShipHealthSystem hpSys;

        public HudHealth(Hud hud, SpriteFont font, Game1 game):base(hud, font, game)
        {
            healthPos = new Vector2(659, 45);
            shieldBarPos = new Vector2(960, 75);
            hpBarPos = new Vector2(960, 115);
            hpSys = game.ship.shipHealth;
            wakeThresh = 3;

            hudHealthF = game.Content.Load<Texture2D>(@"Hud/hudHealthF");
            hudHealthB = game.Content.Load<Texture2D>(@"Hud/hudHealthB");
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
