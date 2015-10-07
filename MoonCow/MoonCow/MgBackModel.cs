using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    public class MgBackModel:MgModel
    {
        public MgBackModel():base()
        {
            model = TextureManager.square;
            pos = new Vector3(0, 0, 40);
            scale = new Vector3(4,3,1);
            rot = Vector3.Zero;
        }

        public override void Update()
        {
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
                    effect.Texture = TextureManager.mgBack;
                    effect.Alpha = 1;

                    effect.LightingEnabled = true;

                    effect.AmbientLightColor = new Vector3(0.9f);
                    effect.EmissiveColor = new Vector3(0.3f, 0.3f, 0.3f);
                    effect.PreferPerPixelLighting = true;

                }
                mesh.Draw();
            }
        }
    }
}
