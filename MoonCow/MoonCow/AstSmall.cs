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

            mass = 5; //go ahead and change this to something which makes sense for physics

            health = 20;
            col = new CircleCollider(pos, 2f);
            model = new AsteroidModel(this, new Vector3(0.2f), game, ModelLibrary.ast1);
            game.modelManager.addObject(model);
        }

        public override void drillDamage(float value, Vector3 pos, bool boosting)
        {
            if (boosting)
            {
                onDeath();
                game.camera.setYShake(0.1f);
            }
            else
            {
                base.drillDamage(value, pos, boosting);
            }
        }

        public override void onDeath()
        {
            for (int i = 0; i < 4; i++)
                game.ship.moneyManager.addOreGib(20, pos, 0);
            base.onDeath();
            game.modelManager.addEffect(new AstCloudParticle(game, pos));
            for(int i = 0; i < 10; i++)
            {
                game.modelManager.addEffect(new AstCloudParticle(game, pos, dir, 1));
            }
            for (int i = 0; i < 3; i++)
            {
                game.modelManager.addObject(new AstShrapnel(pos, 0.12f, dir, game));
            }
        }
    }
}
