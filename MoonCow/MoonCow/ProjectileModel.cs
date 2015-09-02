using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    class ProjectileModel:BasicModel
    {
        Projectile projectile;
        Game1 game;
        Texture2D tex;
        Texture2D tex2;
        Texture2D tex3;

        Matrix tipMatrix;
        Matrix trailMatrix;

        ModelBone tip;
        ModelBone tail;


        public ProjectileModel(Model model, Vector3 pos, Projectile projectile, Game1 game, Texture2D tex):base(model)
        {
            this.model = model;
            this.game = game;
            this.pos = pos;
            this.projectile = projectile;
            this.tex = tex;

            tip = model.Bones["tip1"];
            tipMatrix = Matrix.Identity;
    
        }

        public override void Update(GameTime gameTime)
        {
            pos = projectile.pos;
            rot = projectile.rot;
            rot.Y += MathHelper.Pi;
            rot.Z -= MathHelper.PiOver2;
            scale = new Vector3(.1f, .1f, .1f);
            //scale = new Vector3(1, 1, -1);

            //rot.Y = -rot.Y + MathHelper.PiOver2;
            //rot = Vector3.Transform(ship.direction, Matrix.CreateFromAxisAngle(Vector3.Up, ship.rot.Y));

            setMatrices();

            /*
            tip.Transform = tipMatrix;
            model.Bones["tip2"].Transform = tipMatrix;
            model.Bones["tip3"].Transform = tipMatrix;
            model.Bones["tip4"].Transform = tipMatrix;*/

        }

        public override void Draw(GraphicsDevice device, Camera camera)
        {
            Matrix[] transforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transforms);
            game.GraphicsDevice.BlendState = BlendState.Additive;
            game.GraphicsDevice.DepthStencilState = DepthStencilState.None;
                foreach (ModelMesh mesh in model.Meshes)
                {
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        /*if (mesh.Name.StartsWith("tip"))
                        {
                            //effect.World = tipMatrix;
                            effect.World = mesh.ParentBone.Transform * GetWorld();
                        }
                        else*/
                        {
                            effect.World = mesh.ParentBone.Transform * GetWorld();
                            effect.View = camera.view;
                            effect.Projection = camera.projection;
                        }

                        
                        effect.TextureEnabled = true;

                        effect.Texture = tex;

                        effect.Alpha = 1;

                        //effect.EnableDefaultLighting(); //did not work
                        effect.LightingEnabled = true;

                        //effect.DirectionalLight0.DiffuseColor = new Vector3(0.6f, 0.5f, 0.6f); //RGB is treated as a vector3 with xyz being rgb - so vector3.one is white
                        effect.DirectionalLight0.Direction = Vector3.Down;
                        //effect.DirectionalLight0.SpecularColor = Vector3.One;
                        effect.AmbientLightColor = Vector3.One;
                        //effect.EmissiveColor = Vector3.One;
                        effect.PreferPerPixelLighting = true;

                    }
                    mesh.Draw();
                }

            //base.Draw(device, camera);
        }

        public void setMatrices()
        {
            tipMatrix = Matrix.Identity * Matrix.CreateBillboard(Vector3.Zero, game.camera.cameraPosition, game.camera.tiltUp, game.camera.currentDirection);
            tipMatrix = Matrix.CreateConstrainedBillboard(projectile.pos, game.camera.cameraPosition, Vector3.Up, null, null);

            //System.Diagnostics.Debug.WriteLine("tipmatrix is " + tipMatrix);
        }
        

    }
}
