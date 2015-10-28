using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    public class AsteroidModel:BasicModel
    {
        Asteroid ast;
        Game1 game;
        public AsteroidModel(Asteroid ast, Vector3 scale, Game1 game, Model model):base()
        {
            //note - small 0.2, mid 0.4, large 1.2
            this.ast = ast;
            this.pos = ast.pos;
            this.rot = ast.rot;
            this.scale = scale;
            this.game = game;
            this.model = model;
        }

        public override void Update(GameTime gameTime)
        {
            pos = ast.pos;
            rot = ast.rot;
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

                    effect.Texture = TextureManager.ast1;

                    //trying to get lighting to work, but so far the model just shows up as pure black - it was exported with a green blinn shader
                    //effect.EnableDefaultLighting(); //did not work
                    effect.LightingEnabled = true;

                    effect.DirectionalLight0.DiffuseColor = new Vector3(0.5f, 0.3f, 0.5f); //RGB is treated as a vector3 with xyz being rgb - so vector3.one is white
                    effect.DirectionalLight0.Direction = new Vector3(0, -1, 1);
                    effect.DirectionalLight0.SpecularColor = Vector3.One;
                    effect.AmbientLightColor = new Vector3(0.4f, 0.4f, 0.4f);
                    effect.EmissiveColor = new Vector3(0.4f, 0.4f, 0.4f);
                    effect.PreferPerPixelLighting = true;

                }
                mesh.Draw();
            }
        }
    }
}
