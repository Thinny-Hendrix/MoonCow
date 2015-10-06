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
            scale.Y = 0.1f;
            rot = Vector3.Zero;
        }

        public override void Pulse(float size)
        {
            scale.Y = size/10;
        }


        public override void Update()
        {
            rot.Y += Utilities.deltaTime * MathHelper.Pi/6;

            scale.Y = MathHelper.Lerp(scale.Y, 0.1f, Utilities.deltaTime*4);

            if (rot.Y > MathHelper.Pi * 2)
                rot.Y -= MathHelper.Pi * 2;
        }

        public override void Draw(GraphicsDevice device, MgCamera camera)
        {
            Matrix[] transforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = mesh.ParentBone.Transform * GetWorld();
                    effect.View = camera.view;
                    effect.Projection = camera.projection;
                    effect.TextureEnabled = true;
                    effect.Alpha = 1;
                    effect.Texture = TextureManager.mgGrid;

                    effect.LightingEnabled = true;

                    effect.DirectionalLight0.DiffuseColor = new Vector3(0.1f, 0.1f, 0.1f); //RGB is treated as a vector3 with xyz being rgb - so vector3.one is white
                    effect.DirectionalLight0.Direction = new Vector3(0, -1, 1);
                    effect.DirectionalLight0.SpecularColor = Vector3.One;
                    effect.AmbientLightColor = new Vector3(0.9f);
                    effect.EmissiveColor = new Vector3(0.3f, 0.3f, 0.3f);
                    effect.PreferPerPixelLighting = true;

                }
                mesh.Draw();
            }
        }
    }
}
