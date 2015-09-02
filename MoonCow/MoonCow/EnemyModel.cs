using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    public class EnemyModel:BasicModel
    {
        Enemy enemy;

        public EnemyModel(Model model, Enemy ship):base(model)
        {
            this.model = model;
            this.enemy = ship;
            scale = new Vector3(.07f, .07f, .07f);
        }

        public override void Update(GameTime gameTime)
        {
            pos = enemy.pos;
            pos.Y -= 0.7f;
            rot = enemy.rot;

            rot.Y -= MathHelper.Pi;
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

                    //trying to get lighting to work, but so far the model just shows up as pure black - it was exported with a green blinn shader
                    //effect.EnableDefaultLighting(); //did not work
                    effect.LightingEnabled = true;

                    effect.DirectionalLight0.DiffuseColor = new Vector3(0.3f, 0.3f, 0.3f); //RGB is treated as a vector3 with xyz being rgb - so vector3.one is white
                    effect.DirectionalLight0.Direction = new Vector3(0, -1, 1);
                    //effect.DirectionalLight0.SpecularColor = Vector3.One;
                    effect.AmbientLightColor = new Vector3(0.3f, 0.3f, 0.3f);
                    effect.SpecularColor = Vector3.One;
                    effect.EmissiveColor = new Vector3(.3f, .3f, .3f);
                    effect.PreferPerPixelLighting = true;

                }
                mesh.Draw();
            }
        }

    }
}
