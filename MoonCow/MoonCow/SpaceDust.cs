﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    class SpaceDust:BasicModel
    {
        Ship ship;
        float range = 50;
        Model glowModel;

        public SpaceDust(Model model, Ship ship, int yRange):base(model)
        {
            this.model = model;
            this.ship = ship;
            scale = new Vector3(.015f,.015f,.015f);
            rot.X = (float)Utilities.random.NextDouble();
            rot.Y = (float)Utilities.random.NextDouble();
            rot.Z = (float)Utilities.random.NextDouble();


            //pos.X = ship.pos.X + Utilities.random.NextDouble((float)0, range * 2) - range;

            pos.X = (ship.pos.X - range + ((float)Utilities.random.NextDouble() * range*2));
            pos.Z = (ship.pos.Z  - range + ((float)Utilities.random.NextDouble() * range*2));

            if (yRange == 0)
                pos.Y = (ship.pos.Y - 10 + ((float)Utilities.random.NextDouble() * 9.5f));
            else
                pos.Y = (ship.pos.Y + 10 - ((float)Utilities.random.NextDouble() * 7.5f));



            //System.Diagnostics.Debug.WriteLine(pos);



        }
        public override void Update(GameTime gameTime)
        {
            //scale.X += 0.1f;
            //scale.Y += 0.1f;
            //scale.Z += 0.1f;

            if (pos.X - ship.pos.X < -range)
                pos.X += range*2;

            if (pos.X - ship.pos.X > range )
                pos.X -= range*2;

            if (pos.Z - ship.pos.Z < -range)
                pos.Z += range*2;

            if (pos.Z - ship.pos.Z > range )
                pos.Z -= range*2;


            //pos = ship.pos;
            //rot = ship.rot;
            //rot.Y = -rot.Y + MathHelper.PiOver2;
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

                    effect.DirectionalLight0.Direction = ship.direction;
                    effect.DirectionalLight0.DiffuseColor = new Vector3(0.3f, 1f, 0.7f); //RGB is treated as a vector3 with xyz being rgb - so vector3.one is white
                    effect.DirectionalLight0.SpecularColor = Vector3.One;
                    effect.AmbientLightColor = new Vector3(.6f,.6f,.6f);
                    effect.PreferPerPixelLighting = true;
                    //effect.EmissiveColor = Vector3.One;
                }
                mesh.Draw();
            }
        }

    }
}
