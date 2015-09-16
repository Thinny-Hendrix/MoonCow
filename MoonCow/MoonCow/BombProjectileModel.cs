using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    public class BombProjectileModel:BasicModel
    {
        Vector3 dir;
        Projectile proj;
        Texture2D tex;
        public BombProjectileModel(Projectile proj, Vector3 pos, Vector3 dir):base()
        {
            this.proj = proj;
            this.pos = pos;
            this.dir = dir;

            model = ModelLibrary.bombProjectile;
            tex = TextureManager.bombTex1;

            scale = new Vector3(0.2f);

            rot.X = Utilities.nextFloat() * MathHelper.Pi * 2;
            rot.Y = Utilities.nextFloat() * MathHelper.Pi * 2;
            rot.Z = Utilities.nextFloat() * MathHelper.Pi * 2;
        }

        public override void Update(GameTime gameTime)
        {
            if (!Utilities.paused && !Utilities.softPaused)
            {
                pos = proj.pos;
                rot += dir * Utilities.deltaTime * 10;
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
                    effect.World = mesh.ParentBone.Transform * GetWorld();
                    effect.View = camera.view;
                    effect.Projection = camera.projection;
                    effect.TextureEnabled = true;
                    effect.Alpha = 1;
                    effect.Texture = tex;

                    //trying to get lighting to work, but so far the model just shows up as pure black - it was exported with a green blinn shader
                    //effect.EnableDefaultLighting(); //did not work
                    effect.LightingEnabled = true;

                    if (mesh.Name.Contains("glow"))
                        effect.EmissiveColor = Vector3.One;
                    else
                    {
                        effect.DirectionalLight0.DiffuseColor = new Vector3(0.3f, 0.3f, 0.3f); //RGB is treated as a vector3 with xyz being rgb - so vector3.one is white
                        effect.DirectionalLight0.Direction = new Vector3(0, -1, 1);
                        effect.DirectionalLight0.SpecularColor = Vector3.One;
                        effect.AmbientLightColor = new Vector3(0.3f, 0.3f, 0.3f);
                        effect.EmissiveColor = new Vector3(0.3f, 0.3f, 0.3f);
                        effect.PreferPerPixelLighting = true;
                    }

                }
                mesh.Draw();
            }
        }

        public override void Dispose()
        {
        }
    }
}
