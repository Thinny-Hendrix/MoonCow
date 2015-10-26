using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    public class SpRing:SpriteParticle
    {
        float speed;
        List<SpriteParticle> toDelete;

        public SpRing(Vector2 pos, float speed, List<SpriteParticle> toDelete, float scale)
            : base(pos)
        {
            this.toDelete = toDelete;
            tex = TextureManager.particle2;
            this.speed = speed;
            this.scale = scale;
            rot = Utilities.nextFloat() * MathHelper.Pi * 2;
            alpha = 1;
        }
        public SpRing(Vector2 pos, float speed, List<SpriteParticle> toDelete):base(pos)
        {
            this.toDelete = toDelete;
            tex = TextureManager.particle2;
            this.speed = speed;
            scale = 0.4f;
            rot = Utilities.nextFloat() * MathHelper.Pi * 2;
            alpha = 1;
        }

        public SpRing(Vector2 pos, float speed, List<SpriteParticle> toDelete, int type)
            : base(pos)
        {
            this.toDelete = toDelete;
            if (type == 1)
                tex = TextureManager.rbowRing;
            else
                tex = TextureManager.particle2;
            this.speed = speed;
            scale = 0.4f;
            alpha = 1;
            rot = Utilities.nextFloat() * MathHelper.Pi * 2;
        }

        public override void Update()
        {
            //rot += Utilities.deltaTime * speed * MathHelper.PiOver4/4;
            scale += speed*Utilities.deltaTime;
            alpha -= Utilities.deltaTime;
            if (alpha <= 0)
                Dispose();
        }

        public override void Dispose()
        {
            toDelete.Add(this);
        }


    }
}
