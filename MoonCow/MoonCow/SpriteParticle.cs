using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    public class SpriteParticle
    {
        public Texture2D tex;
        public Vector2 pos;
        public float scale;
        public float alpha;
        public float rot;

        public SpriteParticle() { }
        public SpriteParticle(Vector2 pos)
        {
            this.pos = pos;
        }

        public virtual void Update()
        {
        }

        public virtual void Draw(SpriteBatch sb)
        {
            sb.Draw(tex, new Rectangle((int)pos.X, (int)pos.Y, (int)(tex.Bounds.Width*scale), (int)(tex.Bounds.Height*scale)), null, Color.White * alpha, rot, new Vector2(tex.Bounds.Width / 2, tex.Bounds.Height / 2), SpriteEffects.None, 0);
        }

        public virtual void Dispose()
        {

        }
    }
}
