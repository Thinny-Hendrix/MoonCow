using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    public class LcSaveButton
    {
        Texture2D tex;
        Texture2D hiTex;
        bool highlighted;
        Vector2 pos;
        LevelCreator lc;
        Game1 game;
        public AABB bounds;

        public LcSaveButton(Game1 game, Vector2 pos, LevelCreator lc)
        {
            this.pos = pos;
            this.game = game;
            this.lc = lc;
            bounds = new AABB(pos, 188, 71);
            checkTex();
        }

        public void activate()
        {
            highlighted = true;
            //setHiTex();
        }

        public void disable()
        {
            highlighted = false;
            //setHiTex();
        }

        public void checkTex()
        {
            if(lc.textFields.ElementAt(0).text.Length > 0 && lc.textFields.ElementAt(1).text.Length > 0)
            {
                if (highlighted)
                    tex = LcAssets.save2;
                else
                    tex = LcAssets.save1;
            }
            else
            {
                tex = LcAssets.save0;
            }
        }

        void setHiTex()
        {
            hiTex = LcAssets.bigHi;
        }

        void setTex()
        {
            tex = LcAssets.plus;
        }

        public void onClick()
        {
            lc.saveLevel();
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(tex, pos, Color.White);
            /*if (highlighted)
                sb.Draw(hiTex, new Rectangle((int)pos.X, (int)pos.Y, tex.Bounds.Width, tex.Bounds.Height), Color.White);*/
        }
    }
}
