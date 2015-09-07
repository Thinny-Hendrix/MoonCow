using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    class StationModel: BasicModel
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
        float cogRot;

        ModelBone cog1;
        Vector3 cog1trans;
        ModelBone cog2;
        Vector3 cog2trans;
        

        public StationModel(Model model, Vector3 pos, float rotation, float scale) : base(model, pos, rotation, scale)
        {
            this.model = model;
            this.pos = pos;
            this.rot = rot;
            this.scale = new Vector3(scale, scale, scale);

            tex = TextureManager.station1;

            try
            {
                cog1 = this.model.Bones["cog1"];
                cog1trans = cog1.Transform.Translation;
            }
            catch (KeyNotFoundException) { }
            try
            {
                cog2 = this.model.Bones["cog2"];
                cog2trans = cog2.Transform.Translation;
            }
            catch (KeyNotFoundException) 
            {
                try
                {
                    cog1 = this.model.Bones["cog"];
                    cog1trans = cog1.Transform.Translation;
                }
                catch (KeyNotFoundException) { }
            }

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            cogRot += Utilities.deltaTime * MathHelper.PiOver4/2;
        }

        public override void Draw(GraphicsDevice device, Camera camera)
        {
            Matrix[] transforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in model.Meshes)
            {
                if (!mesh.Name.Contains("window"))
                {
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        if(mesh.Name.Contains("cog2"))
                            effect.World = Matrix.CreateRotationZ(cogRot) * Matrix.CreateTranslation(cog2trans) * GetWorld();
                        else if (mesh.Name.Contains("cog1"))
                            effect.World = Matrix.CreateRotationZ(-cogRot) * Matrix.CreateTranslation(cog1trans) * GetWorld();
                        else
                            effect.World = mesh.ParentBone.Transform * GetWorld();

                        

                        effect.View = camera.view;
                        effect.Projection = camera.projection;
                        effect.TextureEnabled = true;
                        effect.Texture = tex;
                        effect.Alpha = 1;

                        //effect.EnableDefaultLighting(); //did not work
                        effect.LightingEnabled = true;
                        effect.DirectionalLight0.DiffuseColor = new Vector3(0.8f, 0.8f, 0.8f); //RGB is treated as a vector3 with xyz being rgb - so vector3.one is white
                        effect.DirectionalLight0.Direction = Vector3.Normalize(new Vector3(1, -0.2f, 1));
                        effect.DirectionalLight0.SpecularColor = Vector3.One;
                        effect.AmbientLightColor = new Vector3(0.4f, .4f, .4f);
                        effect.EmissiveColor = new Vector3(.2f, .2f, .2f);
                        effect.PreferPerPixelLighting = true;


                    }
                    mesh.Draw();
                }
            }
            //System.Diagnostics.Debug.WriteLine("Model Draw Called");
        }

        public void DrawRails(GraphicsDevice device, Camera camera)
        {
            Matrix[] transforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in model.Meshes)
            {
                if (mesh.Name.Contains("plane"))
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
                        effect.DirectionalLight0.DiffuseColor = new Vector3(0.5f, 0.5f, 0.5f); //RGB is treated as a vector3 with xyz being rgb - so vector3.one is white
                        effect.DirectionalLight0.Direction = Vector3.Normalize(new Vector3(1, -0.2f, 1));
                        effect.DirectionalLight0.SpecularColor = Vector3.One;
                        effect.AmbientLightColor = new Vector3(0.2f, .2f, .2f);
                        effect.EmissiveColor = new Vector3(.3f, .3f, .3f);
                        effect.PreferPerPixelLighting = true;


                    }
                    mesh.Draw();
                }
            }
            //System.Diagnostics.Debug.WriteLine("Model Draw Called");
        }
    }
}
