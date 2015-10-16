using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    class DirLineParticle : BasicModel
    {
        Vector3 dir;
        float speed;
        Game1 game;
        float time;

        public DirLineParticle(Vector3 pos, Game1 game)
            : base()
        {
            this.pos = pos;
            this.game = game;
            dir = new Vector3(Utilities.nextFloat() * 2 - 1, Utilities.nextFloat() * 2 - 1, Utilities.nextFloat() * 2 - 1);
            this.model = TextureManager.dirSquare;
            speed = 50;
            scale = new Vector3(0.2f, 2, 0.2f);
        }

        public override void Update(GameTime gameTime)
        {
            pos += dir * speed * Utilities.deltaTime;

            time += Utilities.deltaTime*2;

            if (time > 0.5f)
            {
                scale.X = MathHelper.Lerp(0.2f, 0, (time - 0.5f) * 2);
            }
            if (time > 1)
                Dispose();
        }

        public override void Dispose()
        {
            game.modelManager.toDeleteModel(this);
        }

        public override void Draw(GraphicsDevice device, Camera camera)
        {
            Matrix[] transforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transforms);
            game.GraphicsDevice.BlendState = BlendState.Additive;
            game.GraphicsDevice.DepthStencilState = DepthStencilState.DepthRead;
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = Matrix.CreateScale(scale) * Matrix.CreateRotationY(MathHelper.Pi) * Matrix.CreateConstrainedBillboard(pos, game.camera.cameraPosition, dir, null, dir);
                    effect.View = camera.view;
                    effect.Projection = camera.projection;


                    effect.TextureEnabled = true;
                    effect.Texture = TextureManager.smallDot;

                    effect.Alpha = 1;

                    effect.LightingEnabled = true;
                    effect.DirectionalLight0.Direction = Vector3.Down;
                    effect.AmbientLightColor = Vector3.One;
                    effect.PreferPerPixelLighting = true;

                }
                mesh.Draw();
                mesh.Draw();
            }

            //base.Draw(device, camera);
        }
    }
}
