using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    class MoneyGibGlow:BasicModel
    {
        Game1 game;
        MoneyGib gib;
        Texture2D tex;
        RenderTarget2D rTarg;
        SpriteBatch sb;

        public MoneyGibGlow(Model model, MoneyGib gib, Game1 game):base(model)
        {
            this.model = model;
            this.gib = gib;
            this.game = game;
            scale = new Vector3(.06f, .06f, .06f);
            tex = game.Content.Load<Texture2D>(@"Models/Effects/tex1");
            sb = new SpriteBatch(game.GraphicsDevice);
            rTarg = new RenderTarget2D(game.GraphicsDevice, 256, 256);
        }

        public override void Update(GameTime gameTime)
        {
            pos = gib.pos;
            base.Update(gameTime);

            game.GraphicsDevice.SetRenderTarget(rTarg);
            sb.Begin();
            sb.Draw(tex, Vector2.Zero, Color.Red);
            sb.End();
            game.GraphicsDevice.SetRenderTarget(null);

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
                    effect.Texture = (Texture2D)rTarg;
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
            return Matrix.CreateScale(scale) * Matrix.CreateBillboard(pos, game.camera.cameraPosition, game.camera.tiltUp, null);
        }
    }
}
