using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    public class FireStreamParticle:BasicModel
    {
        Vector3 dir;
        float speed;
        float alpha;
        Game1 game;
        float time;
        float fScale;
        RenderTarget2D rTarg;
        SpriteBatch sb;
        public FireStreamParticle(Vector3 pos, Vector3 dir, Game1 game):base()
        {
            this.pos = pos;
            this.dir = dir;
            this.game = game;
            model = TextureManager.dirSquare;
            speed = 30;
            alpha = 1;
            fScale = 2;
            time = 0;

            rTarg = new RenderTarget2D(game.GraphicsDevice, 128, 128);
            sb = new SpriteBatch(game.GraphicsDevice);

            game.GraphicsDevice.SetRenderTarget(rTarg);
            sb.Begin();
            sb.Draw(TextureManager.pyroFlame, Vector2.Zero, Color.Lerp(Color.Red, Color.White, Utilities.nextFloat()));
            sb.End();
            game.GraphicsDevice.SetRenderTarget(null);
        }

        public override void Update(GameTime gameTime)
        {
            pos += dir * speed * Utilities.deltaTime;

            //alpha = MathHelper.Lerp(0, 1, time / MathHelper.Pi*1.5f);


            fScale = (float)(Math.Cos(time) + 1) * 0.8f;

            time += Utilities.deltaTime * MathHelper.Pi * 1f;

            if (time > MathHelper.Pi * 0.5f)
                alpha -= Utilities.deltaTime * 4;

            if (time > MathHelper.Pi)
            {
                Dispose();
                game.modelManager.toDeleteModel(this);
            }

        }

        public override void Dispose()
        {
            sb.Dispose();
            rTarg.Dispose();
        }

        public override void Draw(GraphicsDevice device, Camera camera)
        {
            Matrix[] transforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transforms);
            device.BlendState = BlendState.Additive;
            device.DepthStencilState = DepthStencilState.DepthRead;
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = Matrix.CreateScale(fScale) * Matrix.CreateRotationY(MathHelper.Pi) * Matrix.CreateConstrainedBillboard(pos, camera.cameraPosition, dir, null, dir);
                    effect.View = camera.view;
                    effect.Projection = camera.projection;


                    effect.TextureEnabled = true;
                    effect.Texture = (Texture2D)rTarg;

                    effect.Alpha = alpha;

                    effect.LightingEnabled = true;
                    effect.AmbientLightColor = Vector3.One;
                    effect.PreferPerPixelLighting = true;

                }
                mesh.Draw();
                mesh.Draw();
            }
        }
    }
}
