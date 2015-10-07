using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    public class MgGridModel:MgModel
    {
        float speed;

        public MgGridModel():base()
        {
            model = TextureManager.dirSquare;
            pos = new Vector3(0.8f,-.6f,16);
            scale = new Vector3(10, 16, 16);
            rot = Vector3.Zero;
            rot.X = -MathHelper.PiOver2;
        }

        public override void Update()
        {
            pos.Z -= speed * Utilities.deltaTime;
            if (pos.Z < 0)
                pos.Z += 16;
        }

        public override void setSpeed(float speed)
        {
            this.speed = speed;
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
                    effect.Texture = TextureManager.mgGrid;
                    effect.Alpha = 1;

                    effect.LightingEnabled = true;

                    effect.AmbientLightColor = new Vector3(0.9f);
                    effect.EmissiveColor = new Vector3(0.3f, 0.3f, 0.3f);
                    effect.PreferPerPixelLighting = true;

                }
                mesh.Draw();
            }

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = mesh.ParentBone.Transform * GetWorld() * Matrix.CreateTranslation(new Vector3(0,0,16));

                    effect.View = camera.view;
                    effect.Projection = camera.projection;
                    effect.TextureEnabled = true;
                    effect.Texture = TextureManager.mgGrid;
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
