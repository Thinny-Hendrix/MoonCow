using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    public class HudCollectable:HudComponent
    {
        public int spawnedChips;
        public int count;
        public int displayCount;
        Vector2 pos;
        Texture2D boxO;
        Texture2D boxF;

        public HudCollectable(Hud hud, SpriteFont font, Game1 game):base(hud, font, game)
        {
            boxO = game.Content.Load<Texture2D>(@"Hud/cardOut");
            boxF = game.Content.Load<Texture2D>(@"Hud/cardFill");
            wakeThresh = 3;
            //wakeTime = wakeThresh;
            count = 0;

            pos = new Vector2(1840, 300);
        }

        public void gotCard()
        {
            Wake();
            count++;
            displayCount++;

            if (count == 4)
                hud.hudMessage.drillDoorUnlock();
        }

        public void releaseCards()
        {
            displayCount = 0;
            Wake();
        }

        public override void Draw(SpriteBatch sb)
        {
            if (wakeTime < wakeThresh && displayCount > 0)
            {
                sb.Draw(boxF, hud.scaledRect(pos, boxF.Bounds.Width, boxF.Bounds.Height), null, Color.White, 0, new Vector2(boxF.Bounds.Width, boxF.Bounds.Height / 2), SpriteEffects.None, 0);
                sb.Draw(boxO, hud.scaledRect(pos, boxF.Bounds.Width, boxF.Bounds.Height), null, Color.White, 0, new Vector2(boxF.Bounds.Width, boxF.Bounds.Height / 2), SpriteEffects.None, 0);

                sb.DrawString(font, "" + displayCount, hud.scaledCoords(pos + new Vector2(-45, 20)), Color.White, 0,
                        new Vector2(font.MeasureString("" + displayCount).X / 2, font.MeasureString("" + displayCount).Y / 2), hud.scale * (20.0f / 40), SpriteEffects.None, 0);
            }
        }
    }
}
