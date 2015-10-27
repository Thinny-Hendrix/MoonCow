using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    class ElectroDir:BasicModel
    {
        Vector3 dir;
        Texture2D tex;
        Color c1;
        Color c2;
        float speed;
        RenderTarget2D rTarg;
        SpriteBatch sb;
        Game1 game;
        float time;
        float timeMax;

        public ElectroDir(Vector3 pos, Color c1, Color c2, Game1 game, Vector3 direction)
        {
            this.pos = pos;
            this.game = game;
            dir = new Vector3(Utilities.nextFloat() * 2 - 1, 0, Utilities.nextFloat() * 2 - 1);
            dir *= 0.4f;
            dir += direction;
            dir.Normalize();
            this.c1 = c1;
            this.c2 = c2;
            this.model = TextureManager.dirSquare;
            speed = 44 + Utilities.nextFloat()*4;
            scale = new Vector3(1, 4, 1);

            timeMax = 0.5f;

            rTarg = new RenderTarget2D(game.GraphicsDevice, 128, 512);
            sb = new SpriteBatch(game.GraphicsDevice);
        }

        public ElectroDir(Vector3 pos, Color c1, Color c2, Game1 game):base()
        {
            this.pos = pos;
            this.game = game;
            dir = new Vector3(Utilities.nextFloat() * 2 - 1, Utilities.nextFloat() * 2 - 1, Utilities.nextFloat() * 2 - 1);
            this.c1 = c1;
            this.c2 = c2;
            this.model = TextureManager.dirSquare;
            speed = 28 + Utilities.nextFloat()*4;
            scale = new Vector3(1, 4, 1);

            timeMax = 1f;

            rTarg = new RenderTarget2D(game.GraphicsDevice, 128, 512);
            sb = new SpriteBatch(game.GraphicsDevice);
        }

        void changeTex()
        {
            switch (Utilities.random.Next(4))
            {
                default:
                    tex = TextureManager.elecE1;
                    break;
                case 1:
                    tex = TextureManager.elecE2;
                    break;
                case 2:
                    tex = TextureManager.elecE3;
                    break;
                case 3:
                    tex = TextureManager.elecE4;
                    break;
            }
        }

        public override void Update(GameTime gameTime)
        {
            pos += dir * speed * Utilities.deltaTime;

            changeTex();

            game.GraphicsDevice.SetRenderTarget(rTarg);
            sb.Begin();
            sb.Draw(tex, Vector2.Zero, Color.Lerp(c1, c2, Utilities.nextFloat()));
            sb.End();
            game.GraphicsDevice.SetRenderTarget(null);

            time += Utilities.deltaTime;

            if(time > timeMax/2)
            {
                scale.X = MathHelper.Lerp(1, 0, (time - timeMax/2)/(timeMax/2));
            }
            if (time > timeMax)
                Dispose();
        }

        public override void Dispose()
        {
            rTarg.Dispose();
            sb.Dispose();
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
                    effect.Texture = (Texture2D)rTarg;

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
