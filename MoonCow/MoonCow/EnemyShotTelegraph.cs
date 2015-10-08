using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    public class EnemyShotTelegraph:BasicModel
    {
        Vector3 dir;
        SpriteBatch sb;
        RenderTarget2D rTarg;
        Game1 game;
        Vector2 texPos;
        Color col;
        bool active;

        public EnemyShotTelegraph(Game1 game, Vector3 pos, Vector3 dir, int type)
        {
            this.game = game;
            this.pos = pos;
            this.dir = dir;
            setColor(type);
            model = ModelLibrary.bombRipples;

            sb = new SpriteBatch(game.GraphicsDevice);
            rTarg = new RenderTarget2D(game.GraphicsDevice, 128, 128);
            texPos = Vector2.Zero;
            rot.X = MathHelper.PiOver2;
            active = true;

            scale = new Vector3(0.1f);
        }

        void setColor(int type)
        {
            col = Color.Red;
        }

        public override void Update(GameTime gameTime)
        {
            if (active)
            {
                game.GraphicsDevice.SetRenderTarget(rTarg);
                game.GraphicsDevice.Clear(Color.Transparent);
                sb.Begin();
                sb.Draw(TextureManager.mgPulse, new Rectangle((int)texPos.X, (int)texPos.Y, 128, 64), col);
                sb.End();
                game.GraphicsDevice.SetRenderTarget(rTarg);

                texPos.Y -= 128 * Utilities.deltaTime * 6;
                if (texPos.Y < -200)
                    active = false;
            }
        }

        public void wake()
        {
            active = true;
            texPos.Y = 128;
        }

        public void setPos(Vector3 pos, Vector3 dir)
        {
            this.pos = pos;
            this.dir = dir;

            this.rot.Y = (float)Math.Atan2(dir.X, dir.Z);
        }

        public override void Draw(GraphicsDevice device, Camera camera)
        {
            Matrix[] transforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    {
                        effect.World = GetWorld();
                        effect.World = Matrix.CreateScale(scale) * Matrix.CreateRotationX(rot.X) * Matrix.CreateRotationY(rot.Y) * Matrix.CreateTranslation(pos);
                        //effect.World = Matrix.CreateScale(scale) * Matrix.CreateRotationY(MathHelper.PiOver2) * Matrix.CreateConstrainedBillboard(projectile.pos, game.camera.cameraPosition, Vector3.Cross(Vector3.Left, projectile.direction), null, null);
                    }

                    effect.View = camera.view;
                    effect.Projection = camera.projection;

                    effect.TextureEnabled = true;
                    effect.Texture = (Texture2D)rTarg;

                    effect.Alpha = 1;
                    effect.LightingEnabled = true;
                    effect.AmbientLightColor = Vector3.One;
                    effect.PreferPerPixelLighting = true;

                }
                mesh.Draw();
                foreach (BasicEffect effect in mesh.Effects)
                {
                    {
                        effect.World = Matrix.CreateRotationX(MathHelper.Pi) * Matrix.CreateScale(scale) * Matrix.CreateRotationX(rot.X) * Matrix.CreateRotationY(rot.Y) * Matrix.CreateTranslation(pos);
                        //effect.World = Matrix.CreateScale(scale) * Matrix.CreateRotationY(MathHelper.PiOver2) * Matrix.CreateConstrainedBillboard(projectile.pos, game.camera.cameraPosition, Vector3.Cross(Vector3.Left, projectile.direction), null, null);
                    }

                    effect.View = camera.view;
                    effect.Projection = camera.projection;

                    effect.TextureEnabled = true;
                    effect.Texture = (Texture2D)rTarg;

                    effect.Alpha = 1;
                    effect.LightingEnabled = true;
                    effect.AmbientLightColor = Vector3.One;
                    effect.PreferPerPixelLighting = true;

                }
                mesh.Draw();
            }
        }

        public override void Dispose()
        {
            rTarg.Dispose();
            sb.Dispose();
        }
    }
}
