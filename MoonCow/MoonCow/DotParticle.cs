using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    class DotParticle:BasicModel
    {
        Vector3 direction;
        float distance;
        float fScale;
        Texture2D tex;
        Game1 game;

        float speed;

        public DotParticle(Game1 game, Vector3 pos):base()
        {
            this.game = game;
            this.pos = pos;
            model = TextureManager.square;
            tex = TextureManager.smallDot;
            direction.X = Utilities.nextFloat() * 2 - 1;
            direction.Y = Utilities.nextFloat() * 2 - 1;
            direction.Z = Utilities.nextFloat() * 2 - 1;
            direction.Normalize();
            speed = 10 + Utilities.nextFloat() * 15;

            fScale = 0.003f + Utilities.nextFloat()*0.004f;
        }

        public override void Update(GameTime gameTime)
        {
            if (!Utilities.paused && !Utilities.softPaused)
            {
                pos += direction * speed * Utilities.deltaTime;
                if (speed != 0)
                {
                    speed -= Utilities.deltaTime * 50;
                    if (speed < 0)
                        speed = 0;
                }
                if (speed < 10f)
                {
                    fScale *= 0.9f;
                }

                if (fScale < 0.0005f)
                    game.modelManager.toDeleteModel(this);
            }
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
            return Matrix.CreateScale(fScale) * Matrix.CreateBillboard(pos, game.camera.cameraPosition, game.camera.tiltUp, null);
        }

    }
}
