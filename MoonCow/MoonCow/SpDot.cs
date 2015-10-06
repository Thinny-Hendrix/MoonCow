using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    class SpDot:SpriteParticle
    {
        float speed;
        List<SpriteParticle> toDelete;

        public SpDot(Vector2 pos, float speed, List<SpriteParticle> toDelete):base(pos)
        {
            this.toDelete = toDelete;
            tex = TextureManager.mgMarkWhite;
            this.speed = speed;
            scale = 1;
            rot = Utilities.nextFloat() * MathHelper.Pi * 2;
            alpha = 1;
        }

        public override void Update()
        {
            scale -= speed*Utilities.deltaTime * 2;
            if (scale <= 0)
                Dispose();
        }

        public override void Dispose()
        {
            toDelete.Add(this);
        }
    }
}
