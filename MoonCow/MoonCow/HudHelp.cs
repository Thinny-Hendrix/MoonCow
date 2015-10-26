using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MoonCow
{
    public class HudHelp
    {
        bool active;
        Game1 game;
        SpriteFont font;
        Hud hud;
        string line1;
        string line2;
        string line3;
        string line4;
        string line5;
        string line6;
        string line7;
        float time;

        public HudHelp(Hud hud, Game1 game, SpriteFont font)
        {
            this.hud = hud;
            this.game = game;
            this.font = font;
            line1 = "To unlock the energy cache:";
            line2 = "press the corresponding dPad button";
            line3 = "as the markers reach the target.";
            line4 = "reach the end without missing 3";
            line5 = "to earn money and weapon exp!";
        }

        public void activate()
        {
            active = true;
            time = 0;
            Utilities.softPaused = true;
        }

        public void Update()
        {
            if (active)
            {
                time += Utilities.deltaTime;
                if (time > 1)
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter) || GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.A))
                    {
                        Utilities.softPaused = false;
                        active = false;
                    }
                }
            }
        }

        public void Draw(SpriteBatch sb)
        {
            if (active)
            {
                sb.Draw(hud.endF, hud.scaledRect(new Vector2(960, 540), 719, 272), null, Color.White, 0, new Vector2(359, 136), SpriteEffects.None, 0);
                
                sb.DrawString(font, line1, hud.scaledCoords(960, 430), Color.White, 0,
                    new Vector2(font.MeasureString(line1).X / 2, 0), hud.scale * 20f / 40, SpriteEffects.None, 0);
                sb.DrawString(font, line2, hud.scaledCoords(960, 500), Color.White, 0,
                    new Vector2(font.MeasureString(line2).X / 2, 0), hud.scale * 15.0f / 40, SpriteEffects.None, 0);
                sb.DrawString(font, line3, hud.scaledCoords(960, 525), Color.White, 0,
                    new Vector2(font.MeasureString(line3).X / 2, 0), hud.scale * 15.0f / 40, SpriteEffects.None, 0);
                sb.DrawString(font, line4, hud.scaledCoords(960, 570), Color.White, 0,
                    new Vector2(font.MeasureString(line4).X / 2, 0), hud.scale * 15.0f / 40, SpriteEffects.None, 0);
                sb.DrawString(font, line5, hud.scaledCoords(960, 595), Color.White, 0,
                    new Vector2(font.MeasureString(line5).X / 2, 0), hud.scale * 15.0f / 40, SpriteEffects.None, 0);

                sb.Draw(hud.endO, hud.scaledRect(new Vector2(960, 540), 719, 272), null, Color.White, 0, new Vector2(359, 136), SpriteEffects.None, 0);

                if (time >= 1)
                {
                    sb.Draw(hud.butA, hud.scaledRect(new Vector2(1180, 730), 65, 65), null, Color.White, 0, new Vector2(30, 30), SpriteEffects.None, 0);
                    sb.DrawString(font, "continue", hud.scaledCoords(1140, 730), Color.White, 0,
                    new Vector2(font.MeasureString("continue").X, font.MeasureString("continue").Y / 2), hud.scale * 22.0f / 40, SpriteEffects.None, 0);
                }
            }
        }

    }
}
