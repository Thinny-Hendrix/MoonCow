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
        public static Texture2D load;
        public static Texture2D pureWhite;

        public static void Initialize(Game1 game)
        {
            pureWhite = new Texture2D(game.GraphicsDevice, 1, 1);
            pureWhite.SetData(new Color[] { Color.White });

            font = game.Content.Load<SpriteFont>(@"Hud/Venera40");
            load = game.Content.Load<Texture2D>(@"Hud/Menu/load");

        }
    }
}
