using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    class DrillSpinEffect:BasicModel
    {
        RenderTarget2D rTarg;
        SpriteBatch sb;
        Texture2D tex;
        Game1 game;
        public bool active;
        float alpha;
        float time;
        float maxScale;
        Vector3 absOffset;

        public DrillSpinEffect(Game1 game, Vector3 absOffset, float scale, int type)
        {
            this.game = game;
            this.absOffset = absOffset;
            rTarg = new RenderTarget2D(game.GraphicsDevice, 128, 128);
            sb = new SpriteBatch(game.GraphicsDevice);
            this.scale = new Vector3(scale);
            maxScale = scale;
            model = TextureManager.square;
            alpha = 1;
        }

        void setTex(int type)
        {
            if (type == 1)
                tex = TextureManager.spinM128;
            else
                tex = TextureManager.spinS128;
        }

        public override void Update(GameTime gameTime)
        {
            Vector3 dir = game.ship.direction;
            pos = game.ship.pos + dir*absOffset.X + Vector3.Cross(dir, Vector3.Up)*absOffset.Z;
            pos.Y = 4.5f;
            rot.Y = game.ship.rot.Y+MathHelper.Pi;
        }

        public override void Draw(GraphicsDevice device, Camera camera)
        {
            game.GraphicsDevice.BlendState = BlendState.AlphaBlend;

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
                    effect.Texture = TextureManager.spinM128;
                    //effect.Texture = (Texture2D)rTarg;
                    effect.Alpha = alpha;

                    //trying to get lighting to work, but so far the model just shows up as pure black - it was exported with a green blinn shader
                    //effect.EnableDefaultLighting(); //did not work
                    effect.LightingEnabled = true;
                    effect.AmbientLightColor = Vector3.One;
                    effect.EmissiveColor = Vector3.One;
                    effect.PreferPerPixelLighting = true;

                }
                mesh.Draw();
            }
        }

    }
}
