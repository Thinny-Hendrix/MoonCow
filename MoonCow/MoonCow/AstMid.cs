﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    class AstMid:Asteroid
    {
        public AstMid(Vector3 pos, Game1 game):base(pos,game)
        {
            rot.X = Utilities.nextFloat() * MathHelper.Pi * 2;
            rot.Y = Utilities.nextFloat() * MathHelper.Pi * 2;
            rot.Z = Utilities.nextFloat() * MathHelper.Pi * 2;

            mass = 10; //go ahead and change this to something which makes sense for physics

            health = 50;
            col = new CircleCollider(pos, 3.5f);
            model = new AsteroidModel(this, new Vector3(0.4f), game, ModelLibrary.ast1);
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
                manager.addAsteroid(new AstSmall(pos + (dir * 2), game));
                manager.addAsteroid(new AstSmall(pos + (dir * -2), game));
                if (Utilities.random.Next(6) == 0)
                {
                    game.ship.moneyManager.addOreGib(20, pos, 0);
                    game.ship.moneyManager.addOreGib(20, pos, 0);
                }
            }
            else
            {
                float angle = Utilities.nextFloat() * MathHelper.Pi*2 / 3;
                for (int i = 0; i < smallCount; i++)
                {
                    dir.X = (float)Math.Sin(angle);
                    dir.Z = (float)Math.Cos(angle);
                    dir.Normalize();
                    Asteroid a = new AstSmall(pos + (dir * 2), game);
                    manager.addAsteroid(a);
                    //a.push(10, pos, mass);
                    angle += MathHelper.Pi*2 / 3;
                }
            }
            base.onDeath();

            game.modelManager.addEffect(new AstCloudParticle(game, pos, 0.5f));
            for (int i = 0; i < 15; i++)
            {
                game.modelManager.addEffect(new AstCloudParticle(game, pos, dir, 0.4f, 1));
            }

            game.modelManager.addObject(new AstShrapnel(pos + new Vector3(0, 2, 0), 0.3f, game));
            game.modelManager.addObject(new AstShrapnel(pos + new Vector3(0, -2, 0), 0.3f, game));

            for (int i = 0; i < 3; i++)
            {
                game.modelManager.addObject(new AstShrapnel(pos, 0.2f, dir, game));
            }

            game.camera.setYShake(0.1f);
        }
    }
}
