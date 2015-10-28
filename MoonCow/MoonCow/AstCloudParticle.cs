using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    class AstCloudParticle:BasicModel
    {
        Texture2D tex;
        Game1 game;
        float fScale;
        float alpha;
        float time;
        float speed;
        Vector3 dir;
        int type;

        public AstCloudParticle(Game1 game, Vector3 pos, Vector3 dir, float size, int type)
            : base()
        {
            this.game = game;
            this.pos = pos;
            this.type = type;
            alpha = 1;
            fScale = size;
            speed = Utilities.nextFloat() * 2 + 1;

            Vector3 tempDir = new Vector3(Utilities.nextFloat() * 2 - 1, Utilities.nextFloat() * 2 - 1, Utilities.nextFloat() * 2 - 1);
            this.dir = tempDir + dir;
            this.dir.Normalize();

            this.pos += this.dir * size/0.2f;

            rot.Z = (float)Utilities.random.NextDouble() * MathHelper.Pi * 2;
            tex = TextureManager.astCloud2;
            model = TextureManager.square;
        }
        public AstCloudParticle(Game1 game, Vector3 pos, float size):base()
        {
            this.game = game;
            this.pos = pos;
            alpha = 1;
            fScale = size;

            rot.Z = (float)Utilities.random.NextDouble() * MathHelper.Pi * 2;
            tex = TextureManager.astCloud2;
            model = TextureManager.square;
        }

        public override void Update(GameTime gameTime)
        {
            if (!Utilities.softPaused && !Utilities.paused)
            {
                if (type == 1)
                {
                    pos += dir * speed * Utilities.deltaTime;
                    speed *= 59 * Utilities.deltaTime;
                    fScale *= 59 * Utilities.deltaTime;
                }
                else
                {
                    fScale -= Utilities.deltaTime / 10;
                }

                time += Utilities.deltaTime;

                if (time > 0.5f)
                {
                    alpha -= Utilities.deltaTime;
                }

                if (alpha <= 0)
                    game.modelManager.toDeleteModel(this);
            }

        }

        public override void Draw(GraphicsDevice device, Camera camera)
        {
            game.GraphicsDevice.BlendState = BlendState.AlphaBlend;

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
                    effect.Alpha = alpha;

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
    }
}
