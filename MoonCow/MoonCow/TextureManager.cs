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
        //stores a whole bunch of loaded textures that many things use, prevents needing to load it individually each time

        public static Texture2D particle1;
        public static Texture2D particle2;
        public static Texture2D particle3;

        public static Texture2D particle1small;
        public static Texture2D particle2small;
        public static Texture2D particle3small;

        public static Texture2D whiteBurst;
        public static Texture2D gibGlow;
        public static Texture2D spark1;

        public static Texture2D smoke1;
        public static Texture2D boostFlame;
        public static Texture2D shotBlip;
        public static Texture2D smallDot;

        public static Texture2D station1;


        public static Model square;
        public static Model flameSquare;

        //station window
        public static RenderTarget2D baseWindow;
        public static Texture2D windowLines;
        public static Texture2D warnLines;
        public static Texture2D warnSign2;
        public static SpriteBatch spriteBatch;
        static Vector2 linespos;
        static Color windowIdle1;

        //weapon effects
        public static Texture2D bombRing;
        public static Texture2D bombBlip;
        public static Texture2D bombRipple;

        public static Texture2D pureWhite;

        public static void initialize(Game game)
        {
            //station window
            pureWhite = new Texture2D(game.GraphicsDevice, 1, 1);
            pureWhite.SetData(new Color[] { Color.White });
            baseWindow = new RenderTarget2D(game.GraphicsDevice, 384, 640);
            spriteBatch = new SpriteBatch(game.GraphicsDevice);
            linespos = new Vector2(0, -16);
            windowIdle1 = new Color(0, 200, 255);


            //loading textures
            particle1 = game.Content.Load<Texture2D>(@"Models/Effects/tex1");
            particle2 = game.Content.Load<Texture2D>(@"Models/Effects/tex2");
            particle3 = game.Content.Load<Texture2D>(@"Models/Effects/tex3");

            particle1small = game.Content.Load<Texture2D>(@"Models/Effects/tex1-64");
            particle2small = game.Content.Load<Texture2D>(@"Models/Effects/tex2-64");
            particle3small = game.Content.Load<Texture2D>(@"Models/Effects/tex3-64");

            gibGlow = game.Content.Load<Texture2D>(@"Models/Effects/gibglow");
            whiteBurst = game.Content.Load<Texture2D>(@"Models/Effects/burstWhite");
            spark1 = game.Content.Load<Texture2D>(@"Models/Effects/spark1");
            smoke1 = game.Content.Load<Texture2D>(@"Models/Effects/smoke1");
            boostFlame = game.Content.Load<Texture2D>(@"Models/Effects/boostFlame");
            smallDot = game.Content.Load<Texture2D>(@"Models/Effects/smallDot");
            shotBlip = game.Content.Load<Texture2D>(@"Models/Effects/shotBlip");

            station1 = game.Content.Load<Texture2D>(@"Models/StationTiles/station4");

            windowLines = game.Content.Load<Texture2D>(@"Models/StationTiles/Window/screenlines");
            warnLines = game.Content.Load<Texture2D>(@"Models/StationTiles/Window/windowWarnLines");
            warnSign2 = game.Content.Load<Texture2D>(@"Models/StationTiles/Window/windowWarnSign");


            //weapon effects
            bombRing = game.Content.Load<Texture2D>(@"Models/Weapons/sploring");
            bombBlip = game.Content.Load<Texture2D>(@"Models/Weapons/sploblip");
            bombRipple = game.Content.Load<Texture2D>(@"Models/Weapons/bombRipple");




            square = game.Content.Load<Model>(@"Models/Misc/square");
            flameSquare = game.Content.Load<Model>(@"Models/Effects/flameSquare");


        }

        public static void Update(Game1 game)
        {
            linespos.Y -= Utilities.deltaTime * 32;
            if (linespos.Y < -32)
                linespos.Y += 16;
            game.GraphicsDevice.SetRenderTarget(baseWindow);

            spriteBatch.Begin();
            if (game.enemyManager.enemies.Count() != 0)
            {
                spriteBatch.Draw(pureWhite, new Rectangle(0, 0, 384, 640), Color.Red * 0.6f);
                spriteBatch.Draw(windowLines, linespos, Color.Red*0.5f);
                spriteBatch.Draw(warnLines, Vector2.Zero, Color.Red);
                spriteBatch.Draw(warnSign2, Vector2.Zero, Color.White);
                spriteBatch.Draw(warnSign2, Vector2.Zero, Color.White);

            }
            else
            {
                spriteBatch.Draw(pureWhite, new Rectangle(0, 0, 384, 640), Color.Aqua * 0.4f);
                spriteBatch.Draw(windowLines, linespos, windowIdle1);
            }
            spriteBatch.End();

            game.GraphicsDevice.SetRenderTarget(null);


        }

    }
}
