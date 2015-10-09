using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace MoonCow
{
    public class GlowStreak:BasicModel
    {
        Game1 game;
        float time;
        float speed;
        //SpriteBatch sb;
        //RenderTarget2D rTarg;
        Texture2D tex;
        Color col;
        Vector2 maxScale;
        float alpha;
        Vector3 dir;

        public GlowStreak(Game1 game, Vector3 pos, Vector2 scale, float speed, Color col, int type)
        {
            this.game = game;
            this.model = TextureManager.dirSquare;
            this.pos = pos;
            this.col = col;
            this.scale = Vector3.One;
            this.maxScale = scale;
            this.speed = (Utilities.nextFloat() * 0.2f + 0.9f) * speed;
            this.speed = speed;
            alpha = 1;

            setDir(type);
        }

        void setDir(int type)
        {
            switch (type)
            {
                default:
                    tex = TextureManager.glowStreak1;
                    dir.X = Utilities.nextFloat() * 2 - 1;
                    dir.Z = Utilities.nextFloat() * 2 - 1;
                    dir.Y = Utilities.nextFloat() * 2 - 1;
                    break;
                case 1:
                    tex = TextureManager.glowStreak2;
                    dir.X = Utilities.nextFloat() * 2 - 1;
                    dir.Z = Utilities.nextFloat() * 2 - 1;
                    dir.Y = Utilities.nextFloat();
                    break;
            }
            dir.Normalize();
        }

        public override void Update(GameTime gameTime)
        {
            if (!Utilities.paused && !Utilities.softPaused)
            {
                scale.Y = MathHelper.Lerp(0, maxScale.Y, (float)(Math.Sin(time) + 1) / 2);
                scale.X = MathHelper.Lerp(0, maxScale.X, (float)(Math.Sin(time) + 1) / 2);

                time += Utilities.deltaTime * MathHelper.Pi * speed;

                if (time > MathHelper.Pi)
                {
                    alpha = MathHelper.Lerp(1, 0, (time - MathHelper.Pi) / MathHelper.PiOver2);
                }

                if (time > MathHelper.Pi * 1.5f)
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
                    effect.Texture = tex;

                    effect.Alpha = alpha;

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
            //rTarg.Dispose();
            //sb.Dispose();
            game.modelManager.toDeleteModel(this);
        }
    }
}
