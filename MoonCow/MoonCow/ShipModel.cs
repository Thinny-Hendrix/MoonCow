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
        int currentModel;
        Game1 game;

        RenderTarget2D rTarg;
        SpriteBatch sb;
        Vector2 texPos;

        public ShipModel(Ship ship, Game1 game):base()
        {
            this.model = ModelLibrary.pewShip;
            this.ship = ship;
            this.game = game;
            scale = new Vector3(.06f,.06f,.06f);
            currentModel = 0;

            rTarg = new RenderTarget2D(game.GraphicsDevice, 512, 512);
            sb = new SpriteBatch(game.GraphicsDevice);
        }
        public override void Update(GameTime gameTime)
        {
            pos = ship.pos;
            rot = ship.rot;
            //rot.Y = -rot.Y + MathHelper.PiOver2;
                //rot = Vector3.Transform(ship.direction, Matrix.CreateFromAxisAngle(Vector3.Up, ship.rot.Y));

            texPos.Y += Utilities.deltaTime * 256;
            if (texPos.Y > 1024)
                texPos.Y -= 1024;

            game.GraphicsDevice.SetRenderTarget(rTarg);
            sb.Begin();
            sb.Draw(TextureManager.screenPulse, new Rectangle((int)texPos.X, (int)texPos.Y, 512, 1024), Color.White);
            sb.Draw(TextureManager.screenPulse, new Rectangle((int)texPos.X, (int)texPos.Y-1024, 512, 1024), Color.White);
            sb.End();
            game.GraphicsDevice.SetRenderTarget(null);
        }

        public void setShipModel(int i)
        {
            currentModel = i;
            switch(i)
            {
                default:
                    model = ModelLibrary.pewShip;
                    break;
                case 4:
                    model = ModelLibrary.drillShip;
                    break;
            }
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
                    if (mesh.Name.Contains("glass"))
                    {
                        effect.Texture = (Texture2D)rTarg;
                    }/*
                    else
                    {

                    }*/
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

    }
}
