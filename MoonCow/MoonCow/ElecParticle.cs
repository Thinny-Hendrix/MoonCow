using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    public class ElecParticle:BasicModel
    {
        float time;
        Vector3 dir;
        Texture2D tex;
        Game1 game;
        SpriteBatch sb;
        RenderTarget2D rTarg;
        Color c1;
        Color c2;
        public ElecParticle(Vector3 pos, Game1 game):base()
        {
            initialize(pos, game);
            c1 = Color.Aqua;
            c2 = Color.SeaGreen;
        }

        public ElecParticle(Vector3 pos, Game1 game, Color c1, Color c2):base()
        {
            initialize(pos, game);
            this.c1 = c1;
            this.c2 = c2;
        }

        void initialize(Vector3 pos, Game1 game)
        {
            this.pos = pos;
            this.scale = new Vector3(0.6f);
            this.game = game;
            dir.X = Utilities.nextFloat() * MathHelper.Pi * 2 - MathHelper.Pi;
            dir.Y = Utilities.nextFloat() * MathHelper.Pi * 2 - MathHelper.Pi;
            dir.Z = Utilities.nextFloat() * MathHelper.Pi * 2 - MathHelper.Pi;
            dir.Normalize();
            model = TextureManager.dirSquare;
            tex = TextureManager.elecL2;

            sb = new SpriteBatch(game.GraphicsDevice);
            rTarg = new RenderTarget2D(game.GraphicsDevice, 128, 512);
        }

        void changeTex()
        {
            switch(Utilities.random.Next(4))
            {
                default:
                    tex = TextureManager.elecL1;
                    break;
                case 1:
                    tex = TextureManager.elecL2;
                    break;
                case 2:
                    tex = TextureManager.elecL3;
                    break;
                case 3:
                    tex = TextureManager.elecL4;
                    break;
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (!Utilities.paused && !Utilities.softPaused)
            {
                time += MathHelper.Pi * 6 * Utilities.deltaTime;

                changeTex();

                game.GraphicsDevice.SetRenderTarget(rTarg);
                sb.Begin();
                sb.Draw(tex, Vector2.Zero, Color.Lerp(c1, c2, Utilities.nextFloat()));
                sb.End();
                game.GraphicsDevice.SetRenderTarget(null);

                scale.Y = (float)Math.Sin(time) * 2;
                if (time > MathHelper.Pi)
                    Dispose();
            }
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

        public override void Dispose()
        {
            rTarg.Dispose();
            sb.Dispose();
            game.modelManager.toDeleteModel(this);
        }
    }
}
