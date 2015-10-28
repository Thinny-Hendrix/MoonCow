using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    public static class LcAssets
    {
        public static SpriteFont font;

        public static Texture2D border;
        public static Texture2D box;
        public static Texture2D save0;
        public static Texture2D save1;
        public static Texture2D save2;
        public static Texture2D saved;

        public static Texture2D t1;
        public static Texture2D t2;
        public static Texture2D t3;
        public static Texture2D t4;
        public static Texture2D t5;
        public static Texture2D t6;
        public static Texture2D t7;
        public static Texture2D t8;
        public static Texture2D t9;
        public static Texture2D t10;
        public static Texture2D t11;
        public static Texture2D t12;
        public static Texture2D t13;
        public static Texture2D t14;
        public static Texture2D t15;
        public static Texture2D t16;
        public static Texture2D t17;
        public static Texture2D t18;
        public static Texture2D t19;
        public static Texture2D t20;
        public static Texture2D t21;
        public static Texture2D t22;
        public static Texture2D t23;
        public static Texture2D t24;
        public static Texture2D t25;
        public static Texture2D t26;
        public static Texture2D t27;
        public static Texture2D t28;
        public static Texture2D t35;
        public static Texture2D t36;
        public static Texture2D t37;
        public static Texture2D t38;
        public static Texture2D t39;
        public static Texture2D t40;
        public static Texture2D t41;
        public static Texture2D t42;
        public static Texture2D t43;
        public static Texture2D t44;
        public static Texture2D t45;
        public static Texture2D t46;
        public static Texture2D t47;
        public static Texture2D t48;
        public static Texture2D t49;
        public static Texture2D t50;
        public static Texture2D t51;
        public static Texture2D t52;
        public static Texture2D t53;
        public static Texture2D t54;
        public static Texture2D t55;
        public static Texture2D t56;
        public static Texture2D t57;
        public static Texture2D t58;
        public static Texture2D t59;
        public static Texture2D back;
        public static Texture2D base1;
        public static Texture2D softHi;
        public static Texture2D bigHi;
        public static Texture2D plus;
        public static Texture2D minus;

        public static Texture2D pureWhite;


        public static void initialize(Game1 game)
        {
            pureWhite = new Texture2D(game.GraphicsDevice, 1, 1);
            pureWhite.SetData(new Color[] { Color.White });


            font = game.Content.Load<SpriteFont>(@"Hud/Venera40");

            border = game.Content.Load<Texture2D>(@"MapEditor/lcBorder");
            box = game.Content.Load<Texture2D>(@"MapEditor/lcBox");
            save0 = game.Content.Load<Texture2D>(@"MapEditor/lcSave0");
            save1 = game.Content.Load<Texture2D>(@"MapEditor/lcSave1");
            save2 = game.Content.Load<Texture2D>(@"MapEditor/lcSave2");
            saved = game.Content.Load<Texture2D>(@"MapEditor/lcSaved");

            t1 = game.Content.Load<Texture2D>(@"MapEditor/Tiles/1");
            t2 = game.Content.Load<Texture2D>(@"MapEditor/Tiles/2");
            t3 = game.Content.Load<Texture2D>(@"MapEditor/Tiles/3");
            t4 = game.Content.Load<Texture2D>(@"MapEditor/Tiles/4");
            t5 = game.Content.Load<Texture2D>(@"MapEditor/Tiles/5");
            t6 = game.Content.Load<Texture2D>(@"MapEditor/Tiles/6");
            t7 = game.Content.Load<Texture2D>(@"MapEditor/Tiles/7");
            t8 = game.Content.Load<Texture2D>(@"MapEditor/Tiles/8");
            t9 = game.Content.Load<Texture2D>(@"MapEditor/Tiles/9");
            t10 = game.Content.Load<Texture2D>(@"MapEditor/Tiles/10");
            t11 = game.Content.Load<Texture2D>(@"MapEditor/Tiles/11");
            t12 = game.Content.Load<Texture2D>(@"MapEditor/Tiles/12");
            t13 = game.Content.Load<Texture2D>(@"MapEditor/Tiles/13");
            t14 = game.Content.Load<Texture2D>(@"MapEditor/Tiles/14");
            t15 = game.Content.Load<Texture2D>(@"MapEditor/Tiles/15");
            t16 = game.Content.Load<Texture2D>(@"MapEditor/Tiles/16");
            t17 = game.Content.Load<Texture2D>(@"MapEditor/Tiles/17");
            t18 = game.Content.Load<Texture2D>(@"MapEditor/Tiles/18");
            t19 = game.Content.Load<Texture2D>(@"MapEditor/Tiles/19");
            t20 = game.Content.Load<Texture2D>(@"MapEditor/Tiles/20");
            t21 = game.Content.Load<Texture2D>(@"MapEditor/Tiles/21");
            t22 = game.Content.Load<Texture2D>(@"MapEditor/Tiles/22");
            t23 = game.Content.Load<Texture2D>(@"MapEditor/Tiles/23");
            t24 = game.Content.Load<Texture2D>(@"MapEditor/Tiles/24");
            t25 = game.Content.Load<Texture2D>(@"MapEditor/Tiles/25");
            t26 = game.Content.Load<Texture2D>(@"MapEditor/Tiles/26");
            t27 = game.Content.Load<Texture2D>(@"MapEditor/Tiles/27");
            t28 = game.Content.Load<Texture2D>(@"MapEditor/Tiles/28");
            t35 = game.Content.Load<Texture2D>(@"MapEditor/Tiles/35");
            t36 = game.Content.Load<Texture2D>(@"MapEditor/Tiles/36");
            t37 = game.Content.Load<Texture2D>(@"MapEditor/Tiles/37");
            t38 = game.Content.Load<Texture2D>(@"MapEditor/Tiles/38");
            t39 = game.Content.Load<Texture2D>(@"MapEditor/Tiles/39");
            t40 = game.Content.Load<Texture2D>(@"MapEditor/Tiles/40");
            t41 = game.Content.Load<Texture2D>(@"MapEditor/Tiles/41");
            t42 = game.Content.Load<Texture2D>(@"MapEditor/Tiles/42");
            t43 = game.Content.Load<Texture2D>(@"MapEditor/Tiles/43");
            t44 = game.Content.Load<Texture2D>(@"MapEditor/Tiles/44");
            t45 = game.Content.Load<Texture2D>(@"MapEditor/Tiles/45");
            t46 = game.Content.Load<Texture2D>(@"MapEditor/Tiles/46");
            t47 = game.Content.Load<Texture2D>(@"MapEditor/Tiles/47");
            t48 = game.Content.Load<Texture2D>(@"MapEditor/Tiles/48");
            t49 = game.Content.Load<Texture2D>(@"MapEditor/Tiles/49");
            t50 = game.Content.Load<Texture2D>(@"MapEditor/Tiles/50");
            t51 = game.Content.Load<Texture2D>(@"MapEditor/Tiles/51");
            t52 = game.Content.Load<Texture2D>(@"MapEditor/Tiles/52");
            t53 = game.Content.Load<Texture2D>(@"MapEditor/Tiles/53");
            t54 = game.Content.Load<Texture2D>(@"MapEditor/Tiles/54");
            t55 = game.Content.Load<Texture2D>(@"MapEditor/Tiles/55");
            t56 = game.Content.Load<Texture2D>(@"MapEditor/Tiles/56");
            t57 = game.Content.Load<Texture2D>(@"MapEditor/Tiles/57");
            t58 = game.Content.Load<Texture2D>(@"MapEditor/Tiles/58");
            t59 = game.Content.Load<Texture2D>(@"MapEditor/Tiles/59");
            back = game.Content.Load<Texture2D>(@"MapEditor/Tiles/back");
            base1 = game.Content.Load<Texture2D>(@"MapEditor/Tiles/base");
            bigHi = game.Content.Load<Texture2D>(@"MapEditor/Tiles/bigHi");
            softHi = game.Content.Load<Texture2D>(@"MapEditor/Tiles/softHi");
            plus = game.Content.Load<Texture2D>(@"MapEditor/Tiles/plus");
            minus = game.Content.Load<Texture2D>(@"MapEditor/Tiles/minus");
        }
    }
}
