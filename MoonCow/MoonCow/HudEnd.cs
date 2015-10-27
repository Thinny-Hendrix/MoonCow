using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    public class HudEnd
    {
        string line1;
        string line2;
        Game1 game;
        Hud hud;
        SpriteFont font;
        bool active;

        public HudEnd(Hud hud, Game1 game)
        {
            this.game = game;
            this.hud = hud;
            this.font = hud.hudMg.font;
        }

        public void activate(bool win)
        {
            active = true;
            if(win)
            {
                line1 = "you";
                line2 = "win!";
            }
            else
            {
                line1 = "game";
                line2 = "over";
            }
        }

        public void Draw(SpriteBatch sb)
        {
            if (active)
            {
                sb.Draw(hud.endF, hud.scaledRect(new Vector2(960, 540), 719, 272), null, Color.White, 0, new Vector2(359, 136), SpriteEffects.None, 0);
                sb.Draw(hud.endO, hud.scaledRect(new Vector2(960, 540), 719, 272), null, Color.White, 0, new Vector2(359, 136), SpriteEffects.None, 0);

                sb.DrawString(font, line1, hud.scaledCoords(960, 515), Color.White, 0,
                    new Vector2(font.MeasureString(line1).X / 2, font.MeasureString(line1).Y / 2), 0.5f, SpriteEffects.None, 0);
                sb.DrawString(font, line2, hud.scaledCoords(960, 600), Color.White, 0,
                    new Vector2(font.MeasureString(line2).X / 2, font.MeasureString(line2).Y / 2), 0.5f, SpriteEffects.None, 0);

                if(game.camera.endGame && game.camera.endTime > 11)
                {
                    sb.Draw(TextureManager.pureWhite, hud.scaledRect(Vector2.Zero, 1920, 1080), Color.Black * MathHelper.SmoothStep(0, 1, game.camera.endTime - 11));
                }
            }
        }
    }
}
