using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    public class SpeedCylModel : BasicModel
    {
        Vector3 direction;
        Ship ship;
        float offset;
        Game1 game;
        float time;

        public SpeedCylModel(Model model, Ship ship, Game1 game):base(model)
        {
            this.model = model;
            this.ship = ship;
            this.game = game;
            scale = new Vector3(100, 100, 100);
            offset = -25;
        }

        public override void Update(GameTime gameTime)
        {
            if (!Utilities.paused && !Utilities.softPaused)
            {
                direction = ship.direction;
                pos = ship.pos;
                rot.Y = ship.rot.Y;

                time += Utilities.deltaTime;
                if (time > .06f)
                {
                    rot.Z = (float)Utilities.random.NextDouble() * MathHelper.Pi * 2;
                    time = 0;
                }
                if (ship.boosting)
                    offset = MathHelper.Lerp(offset, 0, Utilities.deltaTime * 5);
                else
                    offset = MathHelper.Lerp(offset, -25, Utilities.deltaTime * 3);
            }
        }

        public override void Draw(GraphicsDevice device, Camera camera)
        {

        }

        public void overrideDraw(GraphicsDevice device, Camera camera)
        {
            if (!game.minigame.active)
            {
                Matrix[] transforms = new Matrix[model.Bones.Count];
                model.CopyAbsoluteBoneTransformsTo(transforms);
                game.GraphicsDevice.BlendState = BlendState.Additive;
                game.GraphicsDevice.DepthStencilState = DepthStencilState.None;
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

        }

        protected override Matrix GetWorld()
        {
            return Matrix.Identity * Matrix.CreateFromYawPitchRoll(rot.Y, rot.X, rot.Z) * Matrix.CreateScale(scale) * Matrix.CreateTranslation(pos) * Matrix.CreateTranslation(direction * offset);
        }
    }
}
