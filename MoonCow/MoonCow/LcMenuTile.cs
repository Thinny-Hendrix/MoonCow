using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    public class LcMenuTile
    {
        Texture2D tex;
        Texture2D hiTex;

        public AABB bounds;

        Vector2 pos;
        public int type;
        bool highlighted;

        public LcMenuTile(Vector2 pos, int type)
        {
            this.pos = pos;
            this.type = type;
            setTex();
            bounds = new AABB(pos, tex.Bounds.Width,tex.Bounds.Height);
        }

        public void activate()
        {
            highlighted = true;
            setHiTex();
        }

        public void disable()
        {
            highlighted = false;
            setHiTex();
        }

        void setHiTex()
        {
            hiTex = LcAssets.bigHi;
        }

        void setTex()
        {
            switch (type)
            {
                default:
                    tex = LcAssets.back;
                    break;
                case 1:
                    tex = LcAssets.t1;
                    break;
                case 2:
                    tex = LcAssets.t2;
                    break;

                case 3:
                case 7:
                    tex = LcAssets.t3;
                    break;
                case 4:
                case 8:
                    tex = LcAssets.t4;
                    break;
                case 5:
                case 9:
                    tex = LcAssets.t5;
                    break;
                case 6:
                case 10:
                    tex = LcAssets.t6;
                    break;

                case 11:
                    tex = LcAssets.t11;
                    break;

                case 12:
                    tex = LcAssets.t12;
                    break;
                case 13:
                    tex = LcAssets.t13;
                    break;
                case 14:
                    tex = LcAssets.t14;
                    break;
                case 15:
                    tex = LcAssets.t15;
                    break;
                case 16:
                    tex = LcAssets.t16;
                    break;
                case 17:
                    tex = LcAssets.t17;
                    break;
                case 18:
                    tex = LcAssets.t18;
                    break;
                case 19:
                    tex = LcAssets.t19;
                    break;
                case 20:
                    tex = LcAssets.t20;
                    break;
                case 21:
                    tex = LcAssets.t21;
                    break;
                case 22:
                    tex = LcAssets.t22;
                    break;
                case 23:
                    tex = LcAssets.t23;
                    break;
                case 24:
                    tex = LcAssets.base1;
                    break;
                case 25:
                    tex = LcAssets.t25;
                    break;
                case 26:
                    tex = LcAssets.t26;
                    break;
                case 27:
                    tex = LcAssets.t27;
                    break;
                case 28:
                    tex = LcAssets.t28;
                    break;

                case 35:
                    tex = LcAssets.t35;
                    break;
                case 36:
                    tex = LcAssets.t36;
                    break;
                case 37:
                    tex = LcAssets.t37;
                    break;
                case 38:
                    tex = LcAssets.t38;
                    break;
                case 39:
                    tex = LcAssets.t39;
                    break;
                case 40:
                    tex = LcAssets.t40;
                    break;
                case 41:
                    tex = LcAssets.t41;
                    break;
                case 42:
                    tex = LcAssets.t42;
                    break;
                case 43:
                    tex = LcAssets.t43;
                    break;
                case 44:
                    tex = LcAssets.t44;
                    break;
                case 45:
                    tex = LcAssets.t45;
                    break;
                case 46:
                    tex = LcAssets.t46;
                    break;
                case 47:
                    tex = LcAssets.t47;
                    break;
                case 48:
                    tex = LcAssets.t48;
                    break;
                case 49:
                    tex = LcAssets.t49;
                    break;
                case 50:
                    tex = LcAssets.t50;
                    break;
                case 51:
                    tex = LcAssets.t51;
                    break;
                case 52:
                    tex = LcAssets.t52;
                    break;
                case 53:
                    tex = LcAssets.t53;
                    break;
                case 54:
                    tex = LcAssets.t54;
                    break;
                case 55:
                    tex = LcAssets.t55;
                    break;
                case 56:
                    tex = LcAssets.t56;
                    break;
                case 57:
                    tex = LcAssets.t57;
                    break;
                case 58:
                    tex = LcAssets.t58;
                    break;
                case 59:
                    tex = LcAssets.t59;
                    break;
            }
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(LcAssets.back, pos, Color.White);
            sb.Draw(tex, pos, Color.White);
            if (highlighted)
                sb.Draw(hiTex, new Rectangle((int)pos.X, (int)pos.Y, tex.Bounds.Width, tex.Bounds.Height), Color.White);
        }
    }
}
