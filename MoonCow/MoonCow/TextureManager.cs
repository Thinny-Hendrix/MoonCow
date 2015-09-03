using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    public static class TextureManager
    {
        public static Texture2D particle1;
        public static Texture2D particle2;
        public static Texture2D particle3;
        public static Texture2D whiteBurst;


        public static void initialize(Game game)
        {
            particle1 = game.Content.Load<Texture2D>(@"Models/Effects/tex1");
            particle2 = game.Content.Load<Texture2D>(@"Models/Effects/tex2");
            particle3 = game.Content.Load<Texture2D>(@"Models/Effects/tex3");
            whiteBurst = game.Content.Load<Texture2D>(@"Models/Effects/burstWhite");

        }

    }
}
