using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    public class DrillLineParticle:SpriteParticle
    {
        List<SpriteParticle> toDeleteList;
        Color col;
        public DrillLineParticle(Color col, List<SpriteParticle> toDeleteList):base()
        {
            this.toDeleteList = toDeleteList;
            pos = new Vector2(256, -32);
            setTex();
            scale = 1;
            alpha = 1;
            this.col = col;
        }

        void setTex()
        {
            switch(Utilities.random.Next(5))
            {
                default:
                    tex = TextureManager.drawLine0;
                    break;
                case 1:
                    tex = TextureManager.drawLine1;
                    break;
                case 2:
                    tex = TextureManager.drawLine2;
                    break;
                case 3:
                    tex = TextureManager.drawLine3;
                    break;
                case 4:
                    tex = TextureManager.drawLine4;
                    break;
            }
        }

        public override void Update()
        {
            if (!Utilities.paused && !Utilities.softPaused)
            {
                setTex();
                pos.Y += Utilities.deltaTime * 512;
                if (pos.Y > 256)
                    Dispose();
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(tex, new Rectangle((int)pos.X, (int)pos.Y, (int)(tex.Bounds.Width * scale), (int)(tex.Bounds.Height * scale)), null, col * alpha, rot, new Vector2(tex.Bounds.Width / 2, tex.Bounds.Height / 2), SpriteEffects.None, 0);
        }

        public override void Dispose()
        {
            toDeleteList.Add(this);
        }
    }
}
