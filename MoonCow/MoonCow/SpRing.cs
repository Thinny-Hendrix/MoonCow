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
        float scaleSpeed;
        MgManager manager;
        public SpRing(Vector2 pos, float speed, MgManager manager):base(pos)
        {
            this.manager = manager;
            tex = TextureManager.particle2;
            scaleSpeed = speed;
            scale = 0.4f;
            alpha = 1;
        }

        public override void Update()
        {
            scale += scaleSpeed*Utilities.deltaTime;
            alpha -= Utilities.deltaTime;
            if (alpha <= 0)
                Dispose();
        }

        public override void Dispose()
        {
            manager.pToDelete.Add(this);
        }


    }
}
