using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    public class BaseCore:GameComponent
    {

        public float health;
        Game1 game;
        Vector3 pos;
        public CircleCollider col;
        CoreSphereModel model;
        public Vector2 nodePos;
        public BaseCore(Game1 game):base(game)
        {
            this.game = game;
            health = 1000;
        }

        public void setPos(Vector3 pos)
        {
            this.pos = pos;
            nodePos = new Vector2((int)((pos.X / 30) + 0.5f), (int)((pos.Z / 30) + 0.5f));

            col = new CircleCollider(pos, 8);
            model = new CoreSphereModel(pos, game);
            game.modelManager.addAdditive(model);
            game.modelManager.addEffect(new CoreRing(pos, game));
        }

        public void damage(float amount)
        {
            health -= amount;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
