using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    public class ShipModel:BasicModel
    {
        Ship ship;

        public ShipModel(Model model, Ship ship):base(model)
        {
            this.model = model;
            this.ship = ship;
            scale = new Vector3(0.1f, .1f, .1f);
        }
        public override void Update(GameTime gameTime)
        {
            pos = ship.pos;
            rot = ship.rot;
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
                    effect.EnableDefaultLighting();
                }
                mesh.Draw();
            }
        }

    }
}
