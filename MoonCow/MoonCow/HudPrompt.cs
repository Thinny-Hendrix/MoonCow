using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    public class HudPrompt
    {
        Texture2D promptFill;
        Texture2D promptOut;
        string text;
        Vector2 pos;
        Hud hud;
        Game1 game;
        SpriteFont font;
        bool active;

        public HudPrompt(Hud hud, Game1 game, SpriteFont font)
        {
            this.hud = hud;
            this.game = game;
            this.font = font;
            pos = new Vector2(960, 350);

            promptFill = game.Content.Load<Texture2D>(@"Hud/promptFill");
            promptOut = game.Content.Load<Texture2D>(@"Hud/promptOut");
        }

        public void activate(string s)
        {
            active = true;
            text = s;
        }

        public void close()
        {
            active = false;
        }

        public void Update()
        {

        }

        public void Draw(SpriteBatch sb)
        {
            if (active && !hud.quickSelect.active && !hud.turSelect.active && !game.minigame.active && !hud.expSelect.active)
            {
                sb.Draw(promptFill, hud.scaledRect(hud.scaledCoords(pos), promptFill.Bounds.Width, promptFill.Bounds.Height), Color.White);
                sb.Draw(promptOut, hud.scaledRect(hud.scaledCoords(pos), promptFill.Bounds.Width, promptFill.Bounds.Height), Color.White);

                sb.DrawString(font, text, hud.scaledCoords(pos + new Vector2(20, 0)), hud.contSecondary, 0,
                            new Vector2(font.MeasureString(text).X/2, font.MeasureString(text).Y / 2), hud.scale * (28.0f / 40), SpriteEffects.None, 0);
            }

        }
    }
}
