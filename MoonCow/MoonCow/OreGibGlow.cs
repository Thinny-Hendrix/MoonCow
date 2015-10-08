using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    class OreGibGlow:MoneyGibGlow
    {
        Game1 game;
        MoneyGib gib;
        Texture2D tex;
        RenderTarget2D rTarg;
        SpriteBatch sb;
        Color col;

        public List<Vector3> orePos;

        public OreGibGlow(MoneyGib gib, Game1 game):base()
        {
            this.model = TextureManager.square;
            this.gib = gib;
            this.game = game;
            scale = new Vector3(.05f, .05f, .05f);
            tex = TextureManager.gibGlow;
            sb = new SpriteBatch(game.GraphicsDevice);
            rTarg = new RenderTarget2D(game.GraphicsDevice, 64, 64);
            rot.Z = Utilities.nextFloat() * MathHelper.PiOver2;

            col = gib.color;

            orePos = new List<Vector3>();

            game.GraphicsDevice.SetRenderTarget(rTarg);
            sb.Begin();
            sb.Draw(tex, Vector2.Zero, col);
            sb.End();
            game.GraphicsDevice.SetRenderTarget(null);
        }

        public void setElementPos()
        {
            orePos.Clear();
            int i = 0;
            foreach (ModelMesh mesh in gib.model.Meshes)
            {
                Matrix m = (mesh.ParentBone.Transform) * Matrix.CreateScale(scale) * Matrix.CreateFromYawPitchRoll(gib.rot.Y, gib.rot.X, gib.rot.Z) * Matrix.CreateTranslation(pos);
;
                orePos.Add(m.Translation);
                i++;
            }
        }

        public override void Update(GameTime gameTime)
        {
            pos = gib.pos;
        }

        public override void Draw(GraphicsDevice device, Camera camera)
        {
            game.GraphicsDevice.BlendState = BlendState.Additive;

            Matrix[] transforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transforms);
            game.GraphicsDevice.DepthStencilState = DepthStencilState.DepthRead;

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (Vector3 pos in orePos)
                {
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        effect.World = mesh.ParentBone.Transform * Matrix.CreateScale(scale) * Matrix.CreateRotationZ(rot.Z) * Matrix.CreateBillboard(pos, game.camera.cameraPosition, game.camera.tiltUp, null); ;
                        effect.View = camera.view;
                        effect.Projection = camera.projection;
                        effect.TextureEnabled = true;
                        effect.Texture = (Texture2D)rTarg;
                        effect.Alpha = 1;
                    }
                    mesh.Draw();
                }
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
