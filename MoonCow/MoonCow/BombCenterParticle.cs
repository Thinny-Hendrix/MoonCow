using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    class BombCenterParticle:BasicModel
    {
        Texture2D tex;
        Game1 game;
        float fScale;
        float alpha;
        public BombCenterParticle(Game1 game, Vector3 pos, int type):base()
        {
            this.game = game;
            this.pos = pos;
            alpha = 1;

            rot.Z = (float)Utilities.random.NextDouble() * MathHelper.Pi * 1.5f;

            if(type == 1)
            {
                tex = TextureManager.bombCenter;
                fScale = 0.7f;
            }
            else if (type == 3)
            {
                tex = TextureManager.bombCenter2;
                fScale = 1.2f;
            }
            else
            {
                tex = TextureManager.bombCenter;
                fScale = 1.2f;
            }
            model = TextureManager.square;
        }

        public override void Update(GameTime gameTime)
        {
            fScale -= Utilities.deltaTime;
            alpha -= Utilities.deltaTime;
            rot.Z += Utilities.deltaTime * MathHelper.PiOver4;

            if (alpha < 0 || fScale < 0)
                game.modelManager.toDeleteModel(this);

        }

        void drawMesh(ModelMesh mesh, Camera camera, float rot)
        {
            foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = mesh.ParentBone.Transform * GetWorld(rot);
                    effect.View = camera.view;
                    effect.Projection = camera.projection;
                    effect.TextureEnabled = true;
                    effect.Texture = tex;
                    effect.Alpha = alpha*0.6f;

                    //trying to get lighting to work, but so far the model just shows up as pure black - it was exported with a green blinn shader
                    //effect.EnableDefaultLighting(); //did not work
                    effect.LightingEnabled = true;


                }
                mesh.Draw();
                mesh.Draw();
        }

        public override void Draw(GraphicsDevice device, Camera camera)
        {
            game.GraphicsDevice.BlendState = BlendState.Additive;

            Matrix[] transforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transforms);
            game.GraphicsDevice.DepthStencilState = DepthStencilState.DepthRead;

            foreach (ModelMesh mesh in model.Meshes)
            {
                drawMesh(mesh, camera, rot.Z);
                drawMesh(mesh, camera, -rot.Z);

            }
        }

        Matrix GetWorld(float rot)
        {
            return Matrix.CreateScale(fScale) * Matrix.CreateRotationZ(rot) * Matrix.CreateBillboard(pos, game.camera.cameraPosition, game.camera.tiltUp, null);
        }
    }
}
