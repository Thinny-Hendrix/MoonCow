using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    class CollectableDrill:Collectable
    {
        public CollectableDrill(Vector3 pos, Game1 game):base()
        {
            this.pos = pos;
            this.pos.Y = 4;
            this.game = game;
            scale = new Vector3(0.5f);
            col = new CircleCollider(pos, 1f);
            rot.X = MathHelper.PiOver4 / 3;
            model = ModelLibrary.drillItem;
            setEffect();
        }

        public override void Update(GameTime gameTime)
        {
            if (!Utilities.softPaused && !Utilities.paused)
            {
                rot.Y += Utilities.deltaTime * MathHelper.PiOver2;
                if (rot.Y > MathHelper.Pi * 2)
                    rot.Y -= MathHelper.Pi * 2;

                checkCollision();
            }
        }

        public override void onCollect()
        {
            game.ship.weapons.gotDrill();

            game.modelManager.addEffect(new LaserHitEffect(game, pos, Color.SeaGreen));
            game.modelManager.addEffect(new LaserHitEffect(game, pos, Color.White, 3, BlendState.Additive));
            game.modelManager.addEffect(new ImpactParticleModel(game, pos));
            for (int i = 0; i < 20; i++)
            {
                game.modelManager.addEffect(new DirLineParticle(pos, game));
            }

            for (int i = 0; i < 10; i++ )
            {
                game.modelManager.addEffect(new ElectroDir(pos, Color.White, Color.SeaGreen, game));
            }
                base.onCollect();
        }
    }
}
