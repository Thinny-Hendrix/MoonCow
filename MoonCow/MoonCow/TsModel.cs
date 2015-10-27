using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace MoonCow
{
    class TsModel
    {
        bool active;
        Model model;
        Vector3 scale;
        Vector3 rot;
        Vector3 pos;
        Texture2D tex;
        int type;

        public TsModel(int type)
        {
            switch(type)
            {
                default:
                    model = ModelLibrary.turretGatt;
                    //tex = TextureManager.
                    break;
                case 1:
                    model = ModelLibrary.turretPyro;
                    break;
                case 2:
                    model = ModelLibrary.turretElec;
                    break;
            }
            this.type = type;
            pos = Vector3.Zero;
            scale = new Vector3(1f);
            active = false;
        }

        public void activate()
        {
            active = true;
        }

        public void disable()
        {
            active = false;
        }

        public void Update()
        {
            rot.Y += Utilities.deltaTime * MathHelper.PiOver4;
            if(rot.Y > MathHelper.Pi*2)
            {
                rot.Y -= MathHelper.Pi * 2;
            }
        }

        public virtual void Draw(GraphicsDevice device, TsCamera camera)
        {
            if (active)
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
                        if(type == 2)
                        {
                            effect.Texture = TextureManager.elecTurTex;
                        }
                        effect.Alpha = 1;

                        effect.LightingEnabled = true;

                        effect.DirectionalLight0.DiffuseColor = new Vector3(0.3f, 0.3f, 0.3f); //RGB is treated as a vector3 with xyz being rgb - so vector3.one is white
                        effect.DirectionalLight0.Direction = new Vector3(0, -1, 1);
                        effect.DirectionalLight0.SpecularColor = Vector3.One;
                        effect.AmbientLightColor = new Vector3(0.3f, 0.3f, 0.3f);
                        effect.EmissiveColor = new Vector3(0.3f, 0.3f, 0.3f);
                        effect.PreferPerPixelLighting = true;

                    }
                    mesh.Draw();
                }
            }
        }

        protected virtual Matrix GetWorld()
        {
            return Matrix.CreateScale(scale) * Matrix.CreateFromYawPitchRoll(rot.Y, rot.X, rot.Z) * Matrix.CreateTranslation(pos);
        }
    }
}
