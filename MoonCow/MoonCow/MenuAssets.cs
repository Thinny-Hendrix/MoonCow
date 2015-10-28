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
        public static Color contPrimary;
        public static Color contSecondary;
        public static Color outline;
        public static Color fill;
        public static Color redBody;
        public static Color blueBody;

        public static SpriteFont font;
        public static Texture2D load;
        public static Texture2D pureWhite;
        public static Vector2 lineTexPos;

        public static Texture2D logo;

        public static Texture2D bg;
        public static Texture2D bgHex;
        public static Texture2D bgLines;
        public static Texture2D border;

        public static Texture2D ringIn0;
        public static Texture2D ringIn1;
        public static Texture2D ringIn2;
        public static Texture2D ringIn3;
        public static Texture2D ringIn4;
        public static Texture2D ring1;
        public static Texture2D ring2;


        public static Texture2D lsHead;
        public static Texture2D lsScroll;
        public static Texture2D lsBody;
        public static Texture2D lsTab;


        public static void Initialize(Game1 game)
        {
            contPrimary = Color.White;
            contSecondary = new Color(174, 215, 255);
            outline = new Color(0, 64, 127);
            fill = new Color(0, 16, 73, 179);
            redBody = new Color(181, 77, 102);
            blueBody = new Color(86, 124, 193);       


            lineTexPos = Vector2.Zero;
            pureWhite = new Texture2D(game.GraphicsDevice, 1, 1);
            pureWhite.SetData(new Color[] { Color.White });

            font = game.Content.Load<SpriteFont>(@"Hud/Venera40");
            load = game.Content.Load<Texture2D>(@"Hud/Menu/loading");

            bg = game.Content.Load<Texture2D>(@"Hud/Menu/bg1");
            bgHex = game.Content.Load<Texture2D>(@"Hud/Menu/hexbg50");
            bgLines = game.Content.Load<Texture2D>(@"Hud/Menu/menuLines");
            border = game.Content.Load<Texture2D>(@"Hud/Menu/border");

            logo = game.Content.Load<Texture2D>(@"Hud/Menu/logo");

            ring1 = game.Content.Load<Texture2D>(@"Hud/Menu/ring0");
            ring2 = game.Content.Load<Texture2D>(@"Hud/Menu/ring2");
            ringIn0 = game.Content.Load<Texture2D>(@"Hud/Menu/ringIn0");
            ringIn1 = game.Content.Load<Texture2D>(@"Hud/Menu/ringIn1");
            ringIn2 = game.Content.Load<Texture2D>(@"Hud/Menu/ringIn2");
            ringIn3 = game.Content.Load<Texture2D>(@"Hud/Menu/ringIn3");
            ringIn4 = game.Content.Load<Texture2D>(@"Hud/Menu/ringIn4");

            lsHead = game.Content.Load<Texture2D>(@"Hud/Menu/lsText");
            lsScroll = game.Content.Load<Texture2D>(@"Hud/Menu/LsScroll");
            lsBody = game.Content.Load<Texture2D>(@"Hud/Menu/LsBody");
            lsTab = game.Content.Load<Texture2D>(@"Hud/Menu/LsTab");
        }

        public static void updateLinePos()
        {
            lineTexPos.Y -= Utilities.deltaTime * 32;
            if (lineTexPos.Y < -16)
            {
                lineTexPos.Y += 16;
            }
        }

        public static void drawMenuBackground(SpriteBatch sb)
        {
            sb.Draw(bg, Utilities.scaledRect(Vector2.Zero, 1920, 1080), Color.White);
            sb.Draw(bgLines, Utilities.scaledRect(lineTexPos, 1920, 1120), Color.White);
            sb.Draw(bgHex, Utilities.scaledRect(Vector2.Zero, 1920, 1080), Color.White * 0.5f);
        }
    }
}
