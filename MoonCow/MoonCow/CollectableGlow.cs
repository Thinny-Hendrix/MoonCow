using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    class CollectableGlow:BasicModel
    {
        Game1 game;
        Collectable gib;
        Texture2D tex;
        RenderTarget2D rTarg;
        SpriteBatch sb;
        Color col;

        public CollectableGlow(Collectable gib, Game1 game, float scale, Color col):base()
        {
            this.model = TextureManager.square;
            this.gib = gib;
            this.game = game;
            this.scale = new Vector3(scale);
            tex = TextureManager.gibGlow;
            sb = new SpriteBatch(game.GraphicsDevice);
            rTarg = new RenderTarget2D(game.GraphicsDevice, 64, 64);
            rot.Z = Utilities.nextFloat() * MathHelper.PiOver2;

            this.col = col;

            game.GraphicsDevice.SetRenderTarget(rTarg);
            sb.Begin();
            sb.Draw(tex, Vector2.Zero, col);
            sb.End();
            game.GraphicsDevice.SetRenderTarget(null);
        }

        public override void Update(GameTime gameTime)
        {
            this.pos = gib.pos;
            base.Update(gameTime);
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
                    effect.Alpha = 1;                
                }
                mesh.Draw();
            }
        }

        protected override Matrix GetWorld()
        {
            return Matrix.CreateScale(scale) * Matrix.CreateRotationZ(rot.Z) * Matrix.CreateBillboard(pos, game.camera.cameraPosition, game.camera.tiltUp, null);
        }

        public override void Dispose()
        {
            sb.Dispose();
            rTarg.Dispose();
        }
    }
}
