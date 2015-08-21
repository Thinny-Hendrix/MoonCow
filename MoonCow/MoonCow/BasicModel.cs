using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    class BasicModel
    {
        public Vector3 pos;
        public Vector3 rotation;
        public Vector3 scale;
        public Vector3 skew; //might not need this

        public Model model { get; protected set; }

        public BasicModel(Model model, Vector3 pos, Vector3 rotation, Vector3 scale)
        {
            this.model = model;
            this.pos = pos;
            this.rotation = rotation;
            this.scale = scale;
        }

        public virtual void Update()
        {

        }

        public virtual void Draw(GraphicsDevice device, Camera camera)
        {

            Matrix[] transforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = mesh.ParentBone.Transform * getWorld()*Matrix.CreateTranslation(this.pos);
                    effect.View = camera.view;
                    effect.Projection = camera.projection;
                    effect.TextureEnabled = true;
                    effect.EnableDefaultLighting();
                }
                mesh.Draw();
            }
        }

        protected virtual Matrix getWorld()
        {
            return Matrix.Identity;
        }


    }
}