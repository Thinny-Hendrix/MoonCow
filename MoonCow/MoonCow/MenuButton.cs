using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    public class MenuButton
    {
        public bool active;
        public string name;
        Vector2 pos;
        public MainMenu menu;
        Color col;

        public MenuButton(string name, Vector2 pos)
        {
            active = false;
            this.name = name;
            this.pos = pos;
            col = Color.Gray;
        }

        public void activate()
        {
            active = true;
            col = Color.White;
        }

        public void disable()
        {
            active = false;
            col = Color.Gray;
        }

        public void Update()
        {

        }

        public void Draw(SpriteBatch sb)
        {
            sb.DrawString(MenuAssets.font, name, Utilities.scaledCoords(pos), col, 0,
                        new Vector2(0, MenuAssets.font.MeasureString(name).Y / 2), Utilities.windowScale * 28f / 40, SpriteEffects.None, 0);
        }
    }
}
