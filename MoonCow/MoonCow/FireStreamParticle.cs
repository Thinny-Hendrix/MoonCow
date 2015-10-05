using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    public class FireStreamParticle:BasicModel
    {
        Vector3 dir;
        float speed;
        float alpha;
        Game1 game;
        float time;
        float fScale;
        public FireStreamParticle(Vector3 pos, Vector3 dir, Game1 game):base()
        {
            this.pos = pos;
            this.dir = dir;
            this.game = game;
            model = TextureManager.dirSquare;
            speed = 30;
            alpha = 1;
            fScale = 2;
            time = 0;
        }

        public override void Update(GameTime gameTime)
        {
            pos += dir * speed * Utilities.deltaTime;

            //alpha = MathHelper.Lerp(0, 1, time / MathHelper.Pi*1.5f);


            fScale = (float)(Math.Cos(time) + 1) * 1f;

            time += Utilities.deltaTime * MathHelper.Pi * 1f;

            if (time > MathHelper.Pi * 0.5f)
                alpha -= Utilities.deltaTime * 4;
                
            if(time > MathHelper.Pi)
                game.modelManager.toDeleteModel(this);

        }

        public override void Draw(GraphicsDevice device, Camera camera)
        {
            Matrix[] transforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transforms);
            device.BlendState = BlendState.Additive;
            device.DepthStencilState = DepthStencilState.DepthRead;
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = Matrix.CreateScale(fScale) * Matrix.CreateRotationY(MathHelper.Pi) * Matrix.CreateConstrainedBillboard(pos, camera.cameraPosition, dir, null, dir);
                    effect.View = camera.view;
                    effect.Projection = camera.projection;


                    effect.TextureEnabled = true;
                    effect.Texture = TextureManager.bombCenter;

                    effect.Alpha = alpha;

                    effect.LightingEnabled = true;
                    effect.AmbientLightColor = Vector3.One;
                    effect.PreferPerPixelLighting = true;

                }
                mesh.Draw();
                mesh.Draw();
            }
        }
    }
}
