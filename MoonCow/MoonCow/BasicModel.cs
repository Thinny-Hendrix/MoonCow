using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    public class BasicModel
    {
        public Vector3 pos;
        public Vector3 rot;
        public Vector3 scale;
        //public Vector3 skew; //might not need this

        public Model model { get; protected set; }

        public BasicModel()
        {
            this.pos = Vector3.Zero;
            this.rot = Vector3.Zero;
            this.scale = Vector3.One;
        }
        public BasicModel(Model model)
        {
            this.model = model;
            this.pos = Vector3.Zero;
            this.rot = Vector3.Zero;
            this.scale = Vector3.One;
        }
        
        public BasicModel(Model model, Vector3 pos, Vector3 rot, Vector3 scale)
        {
            this.model = model;
            this.pos = pos;
            this.rot = rot;
            this.scale = scale;
        }
        public BasicModel(Model model, Vector3 pos, float rot, float scale)
        {
            this.model = model;
            this.pos = pos;
            this.rot.Y = rot;
            this.scale = new Vector3(scale, scale, scale);
        }

        public virtual void Update(GameTime gameTime)
        {

        }

        public virtual void setEffect()
        { 
            //this is where to declare stuff about the effects that won't change
        }


        public virtual void Draw(GraphicsDevice device, Camera camera)
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
                    effect.DirectionalLight0.SpecularColor = Vector3.One;
                    effect.AmbientLightColor = new Vector3(0.3f, 0.3f, 0.3f);
                    effect.EmissiveColor = new Vector3(0.3f,0.3f,0.3f);
                    effect.PreferPerPixelLighting = true;

                }
                mesh.Draw();
            }
        }

        public virtual void DrawAlt(GraphicsDevice device, Camera camera)
        { }

        protected virtual Matrix GetWorld()
        {
            return Matrix.CreateScale(scale) * Matrix.CreateFromYawPitchRoll(rot.Y, rot.X, rot.Z) * Matrix.CreateTranslation(pos);
        }

        public virtual void Dispose()
        {

        }

    }
}