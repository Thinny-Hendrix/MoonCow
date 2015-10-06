using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    public class MgPolyBg:MgModel
    {
        float time;
        public MgPolyBg():base()
        {
            model = ModelLibrary.mgBgPoly;
            pos = new Vector3(0, -3, 10);
            scale = new Vector3(0.6f);
            scale.Y = 0.3f;
            rot = Vector3.Zero;
        }

        public override void Pulse(float size)
        {
            scale.Y = size/10;
        }


        public override void Update()
        {
            rot.Y += Utilities.deltaTime * MathHelper.Pi/6;

            scale.Y = MathHelper.Lerp(scale.Y, 0.3f, Utilities.deltaTime*4);

            if (rot.Y > MathHelper.Pi * 2)
                rot.Y -= MathHelper.Pi * 2;
        }

        void drawPoly(GraphicsDevice device, MgCamera camera, int type)
        {
            Matrix[] transforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    if(type == 0)
                        effect.World = mesh.ParentBone.Transform * GetWorld();
                    else
                        effect.World = mesh.ParentBone.Transform * Matrix.CreateScale(5) * GetWorld();

                    effect.View = camera.view;
                    effect.Projection = camera.projection;
                    effect.TextureEnabled = true;
                    if (type == 0)
                    {
                        effect.Texture = TextureManager.mgBlue;
                        effect.Alpha = 0.8f;
                    }
                    else
                    {
                        effect.Texture = TextureManager.mgPink;
                        effect.Alpha = 0.8f;
                    }

                    effect.LightingEnabled = true;

                    effect.AmbientLightColor = new Vector3(0.9f);
                    effect.EmissiveColor = new Vector3(0.3f, 0.3f, 0.3f);
                    effect.PreferPerPixelLighting = true;

                }
                mesh.Draw();
            }
        }

        public override void Draw(GraphicsDevice device, MgCamera camera)
        {
            drawPoly(device, camera, 1);
            drawPoly(device, camera, 0);
        }
    }
}
