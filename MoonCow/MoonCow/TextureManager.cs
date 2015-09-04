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

        public static Texture2D particle1small;
        public static Texture2D particle2small;
        public static Texture2D particle3small;

        public static Texture2D whiteBurst;
        public static Texture2D gibGlow;
        public static Texture2D spark1;

        public static Model square;


        public static void initialize(Game game)
        {
            particle1 = game.Content.Load<Texture2D>(@"Models/Effects/tex1");
            particle2 = game.Content.Load<Texture2D>(@"Models/Effects/tex2");
            particle3 = game.Content.Load<Texture2D>(@"Models/Effects/tex3");

            particle1small = game.Content.Load<Texture2D>(@"Models/Effects/tex1-64");
            particle2small = game.Content.Load<Texture2D>(@"Models/Effects/tex2-64");
            particle3small = game.Content.Load<Texture2D>(@"Models/Effects/tex3-64");

            gibGlow = game.Content.Load<Texture2D>(@"Models/Effects/gibglow");
            whiteBurst = game.Content.Load<Texture2D>(@"Models/Effects/burstWhite");
            spark1 = game.Content.Load<Texture2D>(@"Models/Effects/spark1");

            square = game.Content.Load<Model>(@"Models/Misc/square");

        }

    }
}
