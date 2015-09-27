using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    class SpawnVortex:BasicModel
    {
        Game1 game;
        Texture2D tex;
        WaveManager manager;
        bool isVisible;

        public SpawnVortex(Vector3 pos, Vector3 rot, Game1 game):base()
        {
            this.pos = pos;
            this.rot = rot;
            this.game = game;
            model = game.Content.Load<Model>(@"Models/Enemies/SpawnPoint/vortex1");
            tex = game.Content.Load<Texture2D>(@"Models/Enemies/SpawnPoint/spiraltest");
            scale = new Vector3(0.8f,0.8f,0.7f);

        }

        public override void Update(GameTime gameTime)
        {
            if (!Utilities.paused && !Utilities.softPaused)
            {
                try
                {
                    if (manager.spawnState == WaveManager.SpawnState.deploying)
                        isVisible = true;
                    else
                        isVisible = false;
                }
                catch (NullReferenceException)
                {
                    manager = game.waveManager;
                }

                if (isVisible)
                {
                    rot.Z -= Utilities.deltaTime * 2;
                    if (rot.Z < MathHelper.Pi * 2)
                        rot.Z += MathHelper.Pi * 2;
                }
            }
        }

        public override void Draw(GraphicsDevice device, Camera camera)
        {


            //game.GraphicsDevice.DepthStencilState = depthStencilState;
            if (isVisible)
            {
                Matrix[] transforms = new Matrix[model.Bones.Count];
                model.CopyAbsoluteBoneTransformsTo(transforms);
                game.GraphicsDevice.BlendState = BlendState.Additive;
                foreach (ModelMesh mesh in model.Meshes)
                {
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        effect.World = mesh.ParentBone.Transform * GetWorld();
                        effect.View = camera.view;
                        effect.Projection = camera.projection;
                        effect.TextureEnabled = true;

                        effect.Texture = tex;

                        effect.Alpha = 1;

                        //trying to get lighting to work, but so far the model just shows up as pure black - it was exported with a green blinn shader
                        //effect.EnableDefaultLighting(); //did not work
                        effect.LightingEnabled = true;

                        //effect.DirectionalLight0.DiffuseColor = new Vector3(0.6f, 0.5f, 0.6f); //RGB is treated as a vector3 with xyz being rgb - so vector3.one is white
                        effect.DirectionalLight0.Direction = Vector3.Forward;
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
            return Matrix.Identity * Matrix.CreateFromYawPitchRoll(rot.Y, rot.X, rot.Z) * Matrix.CreateScale(scale) * Matrix.CreateTranslation(pos);// *Matrix.CreateTranslation(direction * offset);
        }


    }
}
