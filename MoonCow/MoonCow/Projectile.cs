using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    //superclass all weapon projectiles will inherit
    class Projectile
    {
        Vector3 pos;
        Vector3 rot;
        Vector3 scale;
        Vector3 direction;
        float speed;
        float life;

        public Projectile()
        {

        }

        public void update()
        {

        }

    }
}
