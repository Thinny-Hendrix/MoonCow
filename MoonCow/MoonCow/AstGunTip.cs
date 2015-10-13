using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    public class AstGunTip:BasicModel
    {
        Projectile projectile;
        public Vector3 dir;
        Game1 game;
        float fScale;

        RenderTarget2D rTarg;
        SpriteBatch sb;
        bool drawn;
        Color c1;

        public AstGunTip(Projectile projectile, Game1 game, Color c1)
        {
            this.projectile = projectile;
            this.game = game;
            pos = projectile.pos;
            dir = projectile.direction;
            fScale = 0.04f;
            model = TextureManager.square;
            this.c1 = c1;

            rTarg = new RenderTarget2D(game.GraphicsDevice, 64, 64);
            sb = new SpriteBatch(game.GraphicsDevice);
        }

        public override void Update(GameTime gameTime)
        {
            if(!drawn)
            {
                game.GraphicsDevice.SetRenderTarget(rTarg);
                sb.Begin();
                sb.Draw(TextureManager.particle1small, Vector2.Zero, c1);
                sb.End();
                game.GraphicsDevice.SetRenderTarget(null);
                drawn = true;
            }
            pos = projectile.pos;
            rot.Z = Utilities.nextFloat() * MathHelper.Pi * 2;
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
                    effect.World = Matrix.CreateScale(1.5f) * mesh.ParentBone.Transform * GetWorld();
                    effect.View = camera.view;
                    effect.Projection = camera.projection;
                    effect.TextureEnabled = true;
                    effect.Texture = (Texture2D)rTarg;
                    effect.Alpha = 1;

                    //trying to get lighting to work, but so far the model just shows up as pure black - it was exported with a green blinn shader
                    //effect.EnableDefaultLighting(); //did not work
                    effect.LightingEnabled = true;


                }
                mesh.Draw();

                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = mesh.ParentBone.Transform * GetWorld();
                    effect.View = camera.view;
                    effect.Projection = camera.projection;
                    effect.TextureEnabled = true;
                    effect.Texture = TextureManager.particle1small;
                    effect.Alpha = 1;

                    //trying to get lighting to work, but so far the model just shows up as pure black - it was exported with a green blinn shader
                    //effect.EnableDefaultLighting(); //did not work
                    effect.LightingEnabled = true;


                }
                mesh.Draw();
            }
        }

        protected override Matrix GetWorld()
        {
            return Matrix.CreateScale(fScale) * Matrix.CreateRotationZ(rot.Z) * Matrix.CreateBillboard(pos, game.camera.cameraPosition, game.camera.tiltUp, null);
        }

        public override void Dispose()
        {
            rTarg.Dispose();
            sb.Dispose();
            game.modelManager.toDeleteModel(this);
        }
    }
}
