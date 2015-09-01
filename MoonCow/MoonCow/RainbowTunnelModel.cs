using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MoonCow
{
    public class RainbowTunnelModel:BasicModel
    {
        Vector3 direction;
        Ship ship;
        float offset;
        Game game;
        float time;
        RenderTarget2D rTarg;
        Texture2D rBow;
        Vector2 texPos1;
        Vector2 texPos2;
        Vector2 texPos3;
        SpriteBatch sb;
        DepthStencilState depthStencilState;

        public RainbowTunnelModel(Model model, Ship ship, Game game):base(model)
        {
            this.model = model;
            this.ship = ship;
            this.game = game;
            scale = new Vector3(200, 200, 200);
            offset = -2;

            texPos1 = new Vector2(0, 0);
            rTarg = new RenderTarget2D(game.GraphicsDevice, 2048, 2048);
            sb = new SpriteBatch(game.GraphicsDevice);
            //rBow = game.Content.Load<Texture2D>(@"Hud/hudMapF");
            //rBow = game.Content.Load<Texture2D>(@"Hud/rainbowTunnel");
            rBow = game.Content.Load<Texture2D>(@"Models/Misc/Rbow/rbowTunnelt");

            depthStencilState = new DepthStencilState();
            depthStencilState.DepthBufferEnable = true;
            depthStencilState.DepthBufferWriteEnable = true;


        }

        public override void Update(GameTime gameTime)
        {
            direction = ship.direction;
            pos = ship.pos;

            //direction 1
            rot.Y = ship.rot.Y - MathHelper.Pi;

            texPos3.Y += (int)(Utilities.deltaTime * 5000);
            if (texPos3.Y > 2048)
                texPos3.Y -= 2048;
            texPos1.Y = texPos3.Y - 4096;
            texPos2.Y = texPos3.Y - 2048;

            //direction 2
            /*rot.Y = ship.rot.Y;

            texPos3.Y -= (int)(Utilities.deltaTime * 4096);
            if (texPos3.Y < 2048)
                texPos3.Y += 2048;
            texPos1.Y = texPos3.Y - 4096;
            texPos2.Y = texPos3.Y - 2048;*/


            rot.Z += Utilities.deltaTime * MathHelper.PiOver4;
            if (ship.boosting)
                offset = MathHelper.Lerp(offset, 0, Utilities.deltaTime*5);
            else
                offset = MathHelper.Lerp(offset, -20, Utilities.deltaTime * 3);

            if (ship.finishingMove)
            {
                game.GraphicsDevice.SetRenderTarget(rTarg);

                sb.Begin();
                sb.Draw(rBow, texPos1, Color.White);
                sb.Draw(rBow, texPos2, Color.White);
                sb.Draw(rBow, texPos3, Color.White);
                sb.End();

                game.GraphicsDevice.SetRenderTarget(null);
            }
        }

        public override void Draw(GraphicsDevice device, Camera camera)
        {
            

            game.GraphicsDevice.DepthStencilState = depthStencilState;

            if (ship.finishingMove)
            {
                


                Matrix[] transforms = new Matrix[model.Bones.Count];
                model.CopyAbsoluteBoneTransformsTo(transforms);
                //game.GraphicsDevice.BlendState = BlendState.Additive;
                foreach (ModelMesh mesh in model.Meshes)
                {
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        effect.World = mesh.ParentBone.Transform * GetWorld();
                        effect.View = camera.view;
                        effect.Projection = camera.projection;
                        effect.TextureEnabled = true;
                        effect.Texture = (Texture2D)rTarg;
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
