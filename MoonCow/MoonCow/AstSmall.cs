using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace MoonCow
{
    public class AstSmall:Asteroid
    {
        public AstSmall(Vector3 pos, Game1 game):base(pos,game)
        {
            rot.X = Utilities.nextFloat() * MathHelper.Pi * 2;
            rot.Y = Utilities.nextFloat() * MathHelper.Pi * 2;
            rot.Z = Utilities.nextFloat() * MathHelper.Pi * 2;

            mass = 10; //go ahead and change this to something which makes sense for physics

            health = 20;
            col = new CircleCollider(pos, 2.5f);
            model = new AsteroidModel(this, new Vector3(0.2f), game, ModelLibrary.ast1);
            game.modelManager.addObject(model);
        }

        public override void onDeath()
        {
            for (int i = 0; i < 4; i++)
                game.ship.moneyManager.addGib(20, pos);
            base.onDeath();
        }
    }
}
