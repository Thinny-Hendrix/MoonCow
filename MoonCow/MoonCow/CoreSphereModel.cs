using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    class CoreSphereModel:BasicModel
    {
        Vector3 direction;
        float offset;
        Game game;
        float time;
        RenderTarget2D rTarg;
        RenderTarget2D rTarg2;
        Texture2D rBow;
        Vector2 texPos1;
        Vector2 texPos2;
        Vector2 texPos3;
        Vector2 texPos4;
        Vector2 texPos5;
        SpriteBatch sb;
        DepthStencilState depthStencilState;
        float yRot;

        public CoreSphereModel(Model model, Vector3 pos, Game game):base(model)
        {
            this.model = model;
            this.game = game;
            this.pos = pos;
            scale = new Vector3(100, 100, 100);
            offset = -2;

            texPos1 = new Vector2(0, 0);
            rTarg = new RenderTarget2D(game.GraphicsDevice, 1024, 1024);
            rTarg2 = new RenderTarget2D(game.GraphicsDevice, 1024, 1024);

            sb = new SpriteBatch(game.GraphicsDevice);
            //rBow = game.Content.Load<Texture2D>(@"Hud/hudMapF");
            rBow = game.Content.Load<Texture2D>(@"Models/Base/electrosphere_0");
            //rBow = game.Content.Load<Texture2D>(@"Models/Misc/Rbow/rbowTunnelt");

            depthStencilState = new DepthStencilState();
            depthStencilState.DepthBufferEnable = true;
            depthStencilState.DepthBufferWriteEnable = true;


        }

        public override void Update(GameTime gameTime)
        {
            //game.GraphicsDevice.PresentationParameters.RenderTargetUsage = RenderTargetUsage.PreserveContents;

            yRot += Utilities.deltaTime * MathHelper.PiOver2/4;
            if (yRot > MathHelper.Pi * 2)
                yRot -= MathHelper.Pi * 2;

            //direction 1
            texPos3.Y += (int)(Utilities.deltaTime * 200);
            if (texPos3.Y > 1024)
                texPos3.Y -= 1024;
            texPos1.Y = texPos3.Y - 2048;
            texPos2.Y = texPos3.Y - 1024;


            texPos5.Y -= (int)(Utilities.deltaTime * 200);
            if (texPos5.Y < 0)
                texPos5.Y += 1024;
            texPos4.Y = texPos5.Y - 1024;

            //direction 2
            /*rot.Y = ship.rot.Y;

            texPos3.Y -= (int)(Utilities.deltaTime * 4096);
            if (texPos3.Y < 2048)
                texPos3.Y += 2048;
            texPos1.Y = texPos3.Y - 4096;
            texPos2.Y = texPos3.Y - 2048;*/


            //rot.Z += Utilities.deltaTime * MathHelper.PiOver4;


            game.GraphicsDevice.SetRenderTarget(rTarg);

            sb.Begin();
            sb.Draw(rBow, texPos1, Color.White);
            sb.Draw(rBow, texPos2, Color.White);
            sb.Draw(rBow, texPos3, Color.White);
            sb.End();

            game.GraphicsDevice.SetRenderTarget(rTarg2);

            sb.Begin();
            sb.Draw(rBow, texPos4, Color.White);
            sb.Draw(rBow, texPos5, Color.White);
            sb.End();

            //Adding Params.RenderTargetUsage = RenderTargetUsage.PreserveContents; into the PresentationParameter structure solves the problem.


            

            game.GraphicsDevice.SetRenderTarget(null);
        }

        public override void Draw(GraphicsDevice device, Camera camera)
        {
            

            game.GraphicsDevice.DepthStencilState = depthStencilState;
            game.GraphicsDevice.DepthStencilState = DepthStencilState.DepthRead;
               


                Matrix[] transforms = new Matrix[model.Bones.Count];
                model.CopyAbsoluteBoneTransformsTo(transforms);
                game.GraphicsDevice.BlendState = BlendState.Additive;
                foreach (ModelMesh mesh in model.Meshes)
                {
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        effect.View = camera.view;
                        effect.Projection = camera.projection;
                        effect.TextureEnabled = true;

                        if (mesh.Name.Equals("inSphere"))
                        {
                            effect.World = Matrix.CreateRotationY(yRot) * mesh.ParentBone.Transform * GetWorld();
                            effect.Texture = (Texture2D)rTarg2;
                        }
                        else
                        {
                            effect.World = Matrix.CreateRotationY(-yRot) * mesh.ParentBone.Transform * GetWorld();
                            effect.Texture = (Texture2D)rTarg;
                        }

                        effect.Alpha = 1;

                        //trying to get lighting to work, but so far the model just shows up as pure black - it was exported with a green blinn shader
                        //effect.EnableDefaultLighting(); //did not work
                        effect.LightingEnabled = true;

                        //effect.DirectionalLight0.DiffuseColor = new Vector3(0.6f, 0.5f, 0.6f); //RGB is treated as a vector3 with xyz being rgb - so vector3.one is white
                        effect.DirectionalLight0.Direction = direction;
                        //effect.DirectionalLight0.SpecularColor = Vector3.One;
                        effect.AmbientLightColor = new Vector3(2f, 2f, 2f);
                        //effect.EmissiveColor = Vector3.One;
                        effect.PreferPerPixelLighting = true;

                    }
                    mesh.Draw();
                }
            game.GraphicsDevice.BlendState = BlendState.Opaque;
        }

        public void overrideDraw(GraphicsDevice device, Camera camera)
        {

            

        }

        protected override Matrix GetWorld()
        {
            return Matrix.Identity * Matrix.CreateFromYawPitchRoll(rot.Y, rot.X, rot.Z) * Matrix.CreateScale(scale) * Matrix.CreateTranslation(pos);// *Matrix.CreateTranslation(direction * offset);
        }
    }

}
