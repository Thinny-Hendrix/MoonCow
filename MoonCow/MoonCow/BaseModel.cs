using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    class BaseModel:BasicModel
    {
        float topRot;

        public BaseModel(Vector3 pos):base()
        {
            this.pos = pos;
            this.pos.Y -= 3;
            this.scale = Vector3.One;
            model = ModelLibrary.stationBase;
        }

        public override void Update(GameTime gameTime)
        {
            if (!Utilities.paused && !Utilities.softPaused)
            {
                topRot += Utilities.deltaTime * MathHelper.PiOver4 / 4;
                if (topRot > MathHelper.Pi * 2)
                    topRot -= MathHelper.Pi * 2;
            }
        }

        public override void Draw(GraphicsDevice device, Camera camera)
        {
            Matrix[] transforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in model.Meshes)
            {
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        if (mesh.Name.Contains("spin"))
                            effect.World = Matrix.CreateRotationY(topRot) * GetWorld();
                        else
                            effect.World = mesh.ParentBone.Transform * GetWorld();



                        effect.View = camera.view;
                        effect.Projection = camera.projection;
                        effect.TextureEnabled = true;
                        effect.Texture = TextureManager.station1;
                        effect.Alpha = 1;

                        //effect.EnableDefaultLighting(); //did not work
                        effect.LightingEnabled = true;
                        effect.DirectionalLight0.DiffuseColor = new Vector3(0.2f, 0.2f, 0.2f); //RGB is treated as a vector3 with xyz being rgb - so vector3.one is white
                        effect.DirectionalLight0.Direction = Vector3.Normalize(new Vector3(1, -0.2f, 1));
                        effect.DirectionalLight0.SpecularColor = new Vector3(0.3f);
                        effect.AmbientLightColor = new Vector3(0.6f);
                        effect.EmissiveColor = new Vector3(.2f, .2f, .2f);
                        effect.PreferPerPixelLighting = true;


                    }
                    mesh.Draw();
            }
        }
    }
}
