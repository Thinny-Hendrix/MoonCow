using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    class GunnerModel : EnemyModel
    {
        //AnimationPlayer animPlayer;
        ModelBone bulletPos;
        Gunner gun;

        public GunnerModel(Gunner enemy)
            : base(enemy)
        {
            model = ModelLibrary.gunner;
            scale = new Vector3(.1f);
            this.gun = enemy;

            bulletPos = model.Bones["bulletPos"];
        }

        public override void Update(GameTime gameTime)
        {
            pos = enemy.pos;
            pos.Y -= 0.6f;
            rot.Y = (float)Math.Atan2(gun.modelDir.X, gun.modelDir.Z);
            rot.Y -= MathHelper.PiOver2;
            //rot = enemy.rot;

            //rot.Y -= MathHelper.Pi;
            //rot = Vector3.Transform(ship.direction, Matrix.CreateFromAxisAngle(Vector3.Up, ship.rot.Y));
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

                    effect.LightingEnabled = true;

                    if (mesh.Name.Contains("glow"))
                    {
                        effect.AmbientLightColor = new Vector3(0.8f);
                    }
                    else
                    {
                        effect.DirectionalLight0.DiffuseColor = new Vector3(0.2f); //RGB is treated as a vector3 with xyz being rgb - so vector3.one is white
                        effect.DirectionalLight0.Direction = new Vector3(0, -1, 1);
                        effect.AmbientLightColor = new Vector3(0.7f);
                        effect.SpecularColor = new Vector3(0.3f);
                        effect.EmissiveColor = new Vector3(.4f, .4f, .4f);
                    }
                    effect.PreferPerPixelLighting = true;

                }
                mesh.Draw();
            }
        }
    }
}
