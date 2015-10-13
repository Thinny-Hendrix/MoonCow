using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    public class WaveProjectileModel:BasicModel
    {
        RenderTarget2D rTarg;
        SpriteBatch sb;
        WaveProjectile projectile;
        Game1 game;
        float ripplePos;
        float pulsePos;
        float alpha;

        public WaveProjectileModel(WaveProjectile projectile, Game1 game):base()
        {
            this.projectile = projectile;
            this.game = game;
            rTarg = new RenderTarget2D(game.GraphicsDevice, 256, 64);
            sb = new SpriteBatch(game.GraphicsDevice);
            model = ModelLibrary.shockWave;

            pos = projectile.pos;
            scale = new Vector3(0.2f);
            pulsePos = -64;
            ripplePos = 64;
            alpha = 1;
        }

        public override void Update(GameTime gameTime)
        {
            pos = projectile.pos;
            rot = projectile.rot;
            rot.Y += MathHelper.Pi;
            scale = new Vector3(projectile.scale * 0.1f, 0.2f, projectile.scale*0.3f);

            ripplePos -= Utilities.deltaTime * 64 * 6;
            if (ripplePos < 0)
            {
                if(projectile.time < 1)
                    ripplePos += 64;
            }

            if(projectile.time > 0.5f)
            {
                if (pulsePos < 64)
                    pulsePos += Utilities.deltaTime * 172 * 3;
            }

            if (projectile.time >= 1)
            {
                if (alpha != 0)
                {
                    alpha -= Utilities.deltaTime * 3;
                    if (alpha < 0)
                        alpha = 0;
                }
            }

            game.GraphicsDevice.SetRenderTarget(rTarg);
            sb.Begin();
            sb.Draw(TextureManager.mgPulse, new Rectangle(0, (int)pulsePos, 256, 128), Color.White * 0.5f);
            sb.Draw(TextureManager.bombRipple, new Rectangle(0, (int)ripplePos, 256, 64), Color.White);
            sb.Draw(TextureManager.bombRipple, new Rectangle(0, (int)ripplePos-64, 256, 64), Color.White);
            sb.End();
            game.GraphicsDevice.SetRenderTarget(null);
        }

        public override void Draw(GraphicsDevice device, Camera camera)
        {
            game.GraphicsDevice.BlendState = BlendState.Additive;

            Matrix[] transforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transforms);
            game.GraphicsDevice.DepthStencilState = DepthStencilState.DepthRead;

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = mesh.ParentBone.Transform * GetWorld();
                    effect.View = camera.view;
                    effect.Projection = camera.projection;
                    effect.TextureEnabled = true;
                    effect.Texture = (Texture2D)rTarg;
                    effect.Alpha = alpha;

                    //trying to get lighting to work, but so far the model just shows up as pure black - it was exported with a green blinn shader
                    //effect.EnableDefaultLighting(); //did not work
                    effect.LightingEnabled = true;
                    effect.AmbientLightColor = Vector3.One;

                }
                mesh.Draw();
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
