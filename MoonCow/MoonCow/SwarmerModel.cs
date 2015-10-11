﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    class SwarmerModel:EnemyModel
    {
        //AnimationPlayer animPlayer;
        Swarmer swarmer;
        float knockSpin;

        public SwarmerModel(Swarmer enemy):base(enemy)
        {
            this.swarmer = enemy;
            model = ModelLibrary.swarmer;
            scale = new Vector3(.07f);
        }

        public override void Update(GameTime gameTime)
        {
            pos = enemy.pos;
            //pos.Y -= 0.7f;
            rot = enemy.rot;

            if(swarmer.state == Swarmer.State.hitByDrill)
            {
                knockSpin -= Utilities.deltaTime * MathHelper.Pi * 3;
                if (knockSpin < -MathHelper.Pi * 2)
                    knockSpin += MathHelper.Pi * 2;
            }

            rot.Y -= MathHelper.Pi;
                //rot = Vector3.Transform(ship.direction, Matrix.CreateFromAxisAngle(Vector3.Up, ship.rot.Y));
        }

        protected override Matrix GetWorld()
        {
            if (swarmer.state == Swarmer.State.hitByDrill)
                return Matrix.CreateFromAxisAngle(Vector3.Cross(enemy.knockDir, Vector3.Up), knockSpin) * base.GetWorld();
            else
                return base.GetWorld();
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
                        effect.DirectionalLight0.DiffuseColor = new Vector3(0.2f, 0.2f, 0.2f); //RGB is treated as a vector3 with xyz being rgb - so vector3.one is white
                        effect.DirectionalLight0.Direction = new Vector3(0, -1, 1);
                        effect.AmbientLightColor = new Vector3(0.7f, 0.7f, 0.7f);
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
