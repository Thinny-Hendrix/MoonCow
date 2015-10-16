using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    public class LcTextField
    {
        public string text;
        string desc;
        int charMax;
        LevelCreator lc;
        Game1 game;
        public AABB bounds;
        Vector2 pos;
        string blinkLine;
        float blinkTime;
        bool active;

        public LcTextField(Game1 game, LevelCreator lc, Vector2 pos, string desc)
        {
            this.game = game;
            this.lc = lc;
            this.desc = desc;
            this.pos = pos;
            bounds = new AABB(pos, 300, 30);
            text = "";
            charMax = 16;
            blinkTime = 0.5f;
            blinkLine = "";
        }

        public void activate()
        {
            active = true;
            blinkLine = "|";
            blinkTime = 0.5f;
        }

        public void disable()
        {
            active = false;
            blinkLine = "";
        }

        public void Update()
        {
            if (active)
            {
                blinkTime -= Utilities.deltaTime;
                if (blinkTime <= 0)
                {
                    if (blinkLine.Equals(""))
                    {
                        blinkLine = "|";
                    }
                    else
                    {
                        blinkLine = "";
                    }
                    blinkTime = 0.5f;
                }
            }
        }

        public void deleteChar()
        {
            if(text.Count()> 0)
            {
                text = text.Remove(text.Count() - 1);
            }
        }


        public void addChar(char c)
        {
            if (text.Count() < charMax)
            {
                text += c;
            }
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(LcAssets.pureWhite, new Rectangle((int)pos.X, (int)pos.Y, 300, 32), Color.White);

            sb.DrawString(LcAssets.font, text+blinkLine, new Vector2(pos.X+5, pos.Y+18), Color.Black, 0,
                        new Vector2(0, LcAssets.font.MeasureString(text + blinkLine).Y / 2), Utilities.windowScale * 24.0f / 40, SpriteEffects.None, 0);

            sb.DrawString(LcAssets.font, desc, new Vector2(pos.X - 5, pos.Y + 18), Color.White, 0,
                        new Vector2(LcAssets.font.MeasureString(desc).X, LcAssets.font.MeasureString(desc).Y / 2), Utilities.windowScale * 24.0f / 40, SpriteEffects.None, 0);

            if(active)
            {
                sb.Draw(LcAssets.bigHi, new Rectangle((int)pos.X, (int)pos.Y, 300, 32), Color.White);
            }


        }
    }
}
