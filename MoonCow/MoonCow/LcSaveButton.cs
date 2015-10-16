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
            bounds = new AABB(pos, 30, 30);
            setTex();
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
            tex = LcAssets.plus;
        }

        public void onClick()
        {
            lc.saveLevel();
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(tex, pos, Color.White);
            if (highlighted)
                sb.Draw(hiTex, new Rectangle((int)pos.X, (int)pos.Y, tex.Bounds.Width, tex.Bounds.Height), Color.White);
        }
    }
}
