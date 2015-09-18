using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    public class GattleTurretModel:TurretModel
    {
        Vector3 bodyMat;
        Vector3 barrelMat;
        float barrelRot;
        Vector3 barrelScale;

        public GattleTurretModel(Turret turret, Game1 game)
            : base(turret, game)
        {
            model = ModelLibrary.turretGatt;

            bodyMat = new Vector3(0, 1.45f, 0);

            barrelMat = new Vector3(0, 1.87f, 0);
        }

        public override void Update(GameTime gameTime)
        {
            if (turret.state == Turret.State.active)
            {
                barrelRot += MathHelper.PiOver4 / 10;
                if (barrelRot > MathHelper.Pi * 2)
                    barrelRot -= MathHelper.Pi * 2;
            }

            //TO DO - Z rotation
            barrelScale = Vector3.Lerp(barrelScale, Vector3.One, Utilities.deltaTime * 5);

            rot.Z = -Utilities.tdTan(turret.currentDir);

            //rot.Z += MathHelper.PiOver4 / 200;
            //rot.Z = -MathHelper.Clamp((float)Math.Atan2(turret.currentDir.Y, turret.currentDir.X), 0, (float)MathHelper.Pi*2);
            //if (rot.Z < 0)
            //  rot.Z += MathHelper.Pi;
            rot.Y = (float)Math.Atan2(turret.currentDir.X, turret.currentDir.Z) + MathHelper.PiOver2;
        }

        public override void fire()
        {
            barrelScale.X = 0.9f;
        }

        Matrix turretRot()
        {
            return Matrix.CreateRotationZ(rot.Z) * Matrix.CreateRotationY(rot.Y);
            //* Matrix.CreateRotationY(rot.Y);// *Matrix.CreateFromAxisAngle(turret.currentDir, rot.X);
        }

        public override void Draw(GraphicsDevice device, Camera camera)
        {
            Matrix[] transforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    if (mesh.Name.Contains("barrel"))
                        effect.World = Matrix.CreateTranslation(barrelMat * -1) * Matrix.CreateRotationX(barrelRot) *
                            Matrix.CreateTranslation(barrelMat) * Matrix.CreateTranslation(bodyMat * -1) * Matrix.CreateScale(barrelScale) *
                            turretRot() * Matrix.CreateTranslation(bodyMat) * Matrix.CreateTranslation(pos);

                    else if (mesh.Name.Contains("body"))
                        effect.World = Matrix.CreateTranslation(bodyMat * -1) * turretRot() *
                            Matrix.CreateTranslation(bodyMat) * Matrix.CreateScale(scale) * Matrix.CreateTranslation(pos);

                    else
                        effect.World = mesh.ParentBone.Transform * Matrix.CreateScale(scale) * Matrix.CreateTranslation(pos);

                    effect.View = camera.view;
                    effect.Projection = camera.projection;
                    effect.TextureEnabled = true;
                    effect.Alpha = 1;

                    //trying to get lighting to work, but so far the model just shows up as pure black - it was exported with a green blinn shader
                    //effect.EnableDefaultLighting(); //did not work
                    effect.LightingEnabled = true;

                    if (mesh.Name.Contains("Glow"))
                    {
                        effect.AmbientLightColor = Vector3.One;
                    }
                    else
                    {
                        effect.DirectionalLight0.DiffuseColor = new Vector3(0.3f, 0.3f, 0.3f); //RGB is treated as a vector3 with xyz being rgb - so vector3.one is white
                        effect.DirectionalLight0.Direction = new Vector3(0, -1, 1);
                        effect.DirectionalLight0.SpecularColor = Vector3.One;
                        effect.AmbientLightColor = new Vector3(0.3f, 0.3f, 0.3f);
                        effect.EmissiveColor = new Vector3(0.3f, 0.3f, 0.3f);
                    }

                    effect.PreferPerPixelLighting = true;

                }
                mesh.Draw();
            }
        }
    }
}
