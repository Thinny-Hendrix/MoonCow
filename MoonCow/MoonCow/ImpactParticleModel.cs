using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    class ImpactParticleModel:BasicModel
    {
        Texture2D tex;
        Game1 game;
        float fScale;
        public ImpactParticleModel(Game1 game, Vector3 pos):base()
        {
            this.game = game;
            this.pos = pos;
            fScale = 0.25f;

            rot.Z = (float)Utilities.random.NextDouble() * MathHelper.Pi * 2;
            tex = TextureManager.whiteBurst;
            model = game.Content.Load<Model>(@"Models/Misc/square");
        }

        public override void Update(GameTime gameTime)
        {
            fScale -= Utilities.deltaTime*2.5f;

            if (fScale < 0)
                game.modelManager.toDeleteModel(this);

        }

        public override void Draw(GraphicsDevice device, Camera camera)
        {
            game.GraphicsDevice.BlendState = BlendState.Additive;

            Matrix[] transforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transforms);
            game.GraphicsDevice.DepthStencilState = DepthStencilState.DepthRead;

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

                    effect.DirectionalLight0.DiffuseColor = new Vector3(0.3f, 0.3f, 0.3f); //RGB is treated as a vector3 with xyz being rgb - so vector3.one is white
                    effect.DirectionalLight0.Direction = new Vector3(0, -1, 1);
                    effect.DirectionalLight0.SpecularColor = Vector3.One;
                    effect.AmbientLightColor = Vector3.One;
                    effect.EmissiveColor = new Vector3(0.3f, 0.3f, 0.3f);
                    effect.PreferPerPixelLighting = true;

                }
                mesh.Draw();
            }
        }

        protected override Matrix GetWorld()
        {
            return Matrix.CreateScale(fScale) * Matrix.CreateRotationZ(rot.Z) * Matrix.CreateBillboard(pos, game.camera.cameraPosition, game.camera.tiltUp, null);
        }
    }
}
