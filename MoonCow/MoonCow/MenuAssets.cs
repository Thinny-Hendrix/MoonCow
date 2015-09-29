using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    public static class MenuAssets
    {
        public static SpriteFont font;
        public static void Initialize(Game1 game)
        {
            font = game.Content.Load<SpriteFont>(@"Hud/Venera40");
        }
    }
}
