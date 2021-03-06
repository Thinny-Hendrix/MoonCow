﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    class AstBig:Asteroid
    {
        public AstBig(Vector3 pos, Game1 game):base(pos,game)
        {
            rot.X = Utilities.nextFloat() * MathHelper.Pi * 2;
            rot.Y = Utilities.nextFloat() * MathHelper.Pi * 2;
            rot.Z = Utilities.nextFloat() * MathHelper.Pi * 2;

            mass = 15; //go ahead and change this to something which makes sense for physics

            health = 50;
            col = new CircleCollider(pos, 10f);
            model = new AsteroidModel(this, new Vector3(1.2f), game, ModelLibrary.ast1);
            game.modelManager.addObject(model);
        }

        public override void onDeath()
        {
            int smallCount;
            if(Utilities.random.Next(2) == 1)
                smallCount = 3;
            else
                smallCount = 2;

            Vector3 dir = Vector3.Zero;
            if (smallCount == 2)
            {
                dir.X = Utilities.nextFloat() * 2 - 1;
                dir.Z = Utilities.nextFloat() * 2 - 1;
                dir.Normalize();
                manager.addAsteroid(new AstMid(pos + (dir * 6), game));
                manager.addAsteroid(new AstMid(pos + (dir * -6), game));
                if (Utilities.random.Next(6) == 0)
                {
                    game.ship.moneyManager.addOreGib(20, pos, 0);
                    game.ship.moneyManager.addOreGib(20, pos, 0);
                    game.ship.moneyManager.addOreGib(20, pos, 0);
                }
            }
            else
            {
                float angle = Utilities.nextFloat() * MathHelper.Pi*2 / 3;
                for (int i = 0; i < smallCount; i++)
                {
                    dir.X = -(float)Math.Sin(angle);
                    dir.Z = -(float)Math.Cos(angle);
                    dir.Normalize();
                    manager.addAsteroid(new AstMid(pos + (dir * 6), game));
                    angle += MathHelper.Pi*2 / 3;
                }
            }
            base.onDeath();

            game.modelManager.addEffect(new AstCloudParticle(game, pos, 1.7f));
            for (int i = 0; i < 20; i++)
            {
                game.modelManager.addEffect(new AstCloudParticle(game, pos, dir, 1.4f, 1));
            }

            game.modelManager.addObject(new AstShrapnel(pos + new Vector3(0, 5, 0), 0.6f, game));
            game.modelManager.addObject(new AstShrapnel(pos+new Vector3(0,-5,0), 0.6f, game));

            for (int i = 0; i < 6; i++)
            {
                game.modelManager.addObject(new AstShrapnel(pos, 0.5f, dir, game));
            }

            game.camera.setYShake(0.2f);
        }
    }
}
