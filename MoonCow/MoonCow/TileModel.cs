using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace MoonCow
{
    class TileModel : BasicModel
    {
        /*
         * Just in case the model names are confusing, here's the ID they correlate to
         * - Proto is just a suffix to denote it's temporary
         * 
         * 1-2      -- straightProto
         * 3-10     -- dendProto
         * 11       -- tInt4Proto
         * 12-15    -- tInt3Proto
         * 16-19    -- cornerProto
         * 
         * =======BASE NODES======
         * 20       -- corstProto
         * 21       -- straight1Proto (rotated 270)
         * 22       -- corstFlipProto
         * 23       -- straight1Proto
         * 24       -- [nothing] - power core will be placed here
         * 25       -- straight1Proto
         * 26       -- corner1bigProto (rotated 90)
         * 27       -- straight1Proto (rotated 90)
         * 28       -- corner1bigProto (rotated 180)
         * 29       -- corner1bigProto
         * 30       -- corner1bigProto (rotated 270)
         * 31       -- corner2smallProto
         * 32       -- corner2smallProto (rotated 180)
         * 33       -- corner2smallproto(rotated 270)
         * 34       -- corner2smallproto(rotated 90)
         * ======================
         * 
         * 35-38    -- corner1bigProto
         * 39-42    -- straight1Proto
         * 43-46    -- corner2smallProto
         * 47-50    -- corner1smallProto
         * 51-54    -- corstProto
         * 55-58    -- corstFlipProto
         * 59       -- [nothing] - will have asteroid objects spawned in it
         * 60+      -- [coming eventually]
         * 
         * */
        

        public TileModel(Model model, Vector3 pos, float rotation, float scale) : base(model, pos, rotation, scale)
        {
            this.model = model;
            this.pos = pos;
            this.rotation = rotation;
            this.scale = scale;

        }

        public override void Update()
        {
            base.Update();
        }

        public override void Draw(GraphicsDevice device, Camera camera)
        {
            Matrix[] transforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = mesh.ParentBone.Transform * Matrix.CreateScale(scale) * Matrix.CreateRotationY(rotation) * Matrix.CreateTranslation(pos);
                    effect.View = camera.view;
                    effect.Projection = camera.projection;
                    effect.TextureEnabled = true;

                    //trying to get lighting to work, but so far the model just shows up as pure black - it was exported with a green blinn shader
                    //effect.EnableDefaultLighting(); //did not work
                    effect.LightingEnabled = true;
                    effect.DirectionalLight0.DiffuseColor = new Vector3(1, 1, 1); //RGB is treated as a vector3 with xyz being rgb - so vector3.one is white
                    effect.DirectionalLight0.Direction = Vector3.Left;
                    effect.DirectionalLight0.SpecularColor = Vector3.One;
                    effect.AmbientLightColor = Vector3.One;
                    effect.EmissiveColor = Vector3.One;
                }
                mesh.Draw();
            }
            //System.Diagnostics.Debug.WriteLine("Model Draw Called");
        }

    }
}
