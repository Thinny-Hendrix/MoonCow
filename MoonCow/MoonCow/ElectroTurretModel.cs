using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    public class ElectroTurretModel:BasicModel
    {
        ElectroTurret turret;
        Game1 game;
        float topRot;
        float rotSpeed;
        Texture2D tex;
        public ElectroTurretModel(ElectroTurret turret, Game1 game):base()
        {
            this.game = game;
            this.turret = turret;
            this.pos = turret.pos;
            this.scale = new Vector3(1);

            rot.Y = (float)Math.Atan2(turret.currentDir.X, turret.currentDir.Z) + MathHelper.PiOver2;

            model = ModelLibrary.turretElec;
            tex = TextureManager.elecTurTex;
            topRot = 0;
        }

        public override void Update(GameTime gameTime)
        {
            if(turret.state == Turret.State.idle)
                rotSpeed = MathHelper.Lerp(rotSpeed, MathHelper.PiOver4, Utilities.deltaTime*2);
            else
            {
                if(turret.chargeTime > 1)
                    rotSpeed = MathHelper.Lerp(rotSpeed, MathHelper.Pi * 4, Utilities.deltaTime * 2);
                else
                    rotSpeed = MathHelper.Lerp(rotSpeed, MathHelper.Pi, Utilities.deltaTime * 2);
            }

            topRot += rotSpeed * Utilities.deltaTime;

            if (topRot > MathHelper.Pi * 2)
                topRot -= MathHelper.Pi * 2;
        }

        public override void Draw(GraphicsDevice device, Camera camera)
        {
            Matrix[] transforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    if (mesh.Name.Contains("rot"))
                        effect.World = Matrix.CreateRotationY(topRot) * Matrix.CreateScale(scale) * Matrix.CreateTranslation(pos);

                    else
                        effect.World = mesh.ParentBone.Transform * Matrix.CreateScale(scale) * Matrix.CreateTranslation(pos);

                    effect.View = camera.view;
                    effect.Projection = camera.projection;
                    effect.TextureEnabled = true;
                    effect.Texture = tex;
                    effect.Alpha = 1;

                    //trying to get lighting to work, but so far the model just shows up as pure black - it was exported with a green blinn shader
                    //effect.EnableDefaultLighting(); //did not work
                    effect.LightingEnabled = true;

                    if (mesh.Name.Contains("glow"))
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
