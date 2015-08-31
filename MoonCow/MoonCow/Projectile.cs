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
        Game1 game;
        public bool delete;

        public Projectile(Vector3 direction, Game1 game)
        {
            this.direction = direction;
            this.game = game;

            //this.game.modelManager.addEffect(new BasicModel(game.Content.Load<Model>(@"Models/Effects/tempbullet")));
        }

        public void update()
        {
            pos += direction * speed * Utilities.deltaTime;
        }

    }
}
