using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    class SpinParticle:BasicModel
    {
        float time;
        float fScale;
        Texture2D tex;
        Game1 game;

        public SpinParticle(Game1 game, int type)
        {
            this.game = game;
            pos = game.ship.pos;
            tex = TextureManager.spinGlow_d;
            model = TextureManager.square;
        }

        public override void Update(GameTime gameTime)
        {
            pos = game.ship.pos;

            rot.Z += Utilities.deltaTime * MathHelper.Pi * 6;

            time += Utilities.deltaTime * MathHelper.Pi * 4;
            fScale = MathHelper.Lerp(0, 0.15f, (float)(Math.Sin(time) + 1) / 2);
            if (time > MathHelper.Pi * 1.5f)
                game.modelManager.toDeleteModel(this);
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
                    effect.Texture = tex;
                    effect.Alpha = 1;
                }
                mesh.Draw();
            }
        }

        protected override Matrix GetWorld()
        {
            return Matrix.CreateScale(fScale) * Matrix.CreateRotationZ(rot.Z) * Matrix.CreateBillboard(pos, game.camera.cameraPosition, game.camera.tiltUp, null);
        }
    }
}
