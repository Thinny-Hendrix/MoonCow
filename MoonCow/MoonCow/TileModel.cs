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

        Texture2D tex;

        public TileModel(Model model, Vector3 pos, float rotation, float scale) : base(model, pos, rotation, scale)
        {
            this.model = model;
            this.pos = pos;
            this.rot = rot;
            this.scale = new Vector3(scale, scale, scale);

            //tex = new Texture2D(, 1, 1);
            //tex = new Texture2D();
            //dummyTexture.SetData(new Color[] { myTransparentColor });

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
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

                    //effect.EnableDefaultLighting(); //did not work
                    effect.LightingEnabled = true;
                    effect.DirectionalLight0.DiffuseColor = new Vector3(0.3f, 0.3f, 0.3f); //RGB is treated as a vector3 with xyz being rgb - so vector3.one is white
                    effect.DirectionalLight0.Direction = new Vector3(0,-1,1);
                    effect.DirectionalLight0.SpecularColor = Vector3.One;
                    effect.AmbientLightColor = new Vector3(0.2f,.2f,.2f);
                    effect.EmissiveColor = Vector3.One;
                    effect.PreferPerPixelLighting = true;

                    
                }
                mesh.Draw();
            }
            //System.Diagnostics.Debug.WriteLine("Model Draw Called");
        }

    }
}
