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
        Color col;
        float speed;

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
            rot.Z = Utilities.nextFloat() * MathHelper.Pi * 2;
            rot.Y = MathHelper.Pi;

            setTex(type);

            speed = 2 + (float)type / 2;

            time = MathHelper.Pi * 2 * ((float)type / 5);
        }

        void setTex(int type)
        {
            if (type == 0)
                tex = TextureManager.spinS128;
            else
                tex = TextureManager.spinM128;
        }

        public override void Update(GameTime gameTime)
        {
            if(game.ship.weapons.drill.active)
            {
                if(game.ship.moving)
                {
                    //alpha = MathHelper.Lerp(alpha, 1, Utilities.deltaTime*5);
                    scale = Vector3.Lerp(scale, new Vector3(maxScale), Utilities.deltaTime * 8);
                }
                else
                {
                    //alpha = MathHelper.Lerp(alpha, 0, Utilities.deltaTime*5);
                    scale = Vector3.Lerp(scale, Vector3.Zero, Utilities.deltaTime * 8);
                }
                time -= Utilities.deltaTime * MathHelper.Pi * 8;
                if (time < -MathHelper.Pi * 2)
                    time += MathHelper.Pi * 2;
                col = Color.Lerp(Color.White, Color.SeaGreen, (float)(Math.Sin(time) + 1) / 2);
                game.GraphicsDevice.SetRenderTarget(rTarg);
                game.GraphicsDevice.Clear(Color.Transparent);
                sb.Begin();
                sb.Draw(tex, Vector2.Zero, col);
                sb.End();
                game.GraphicsDevice.SetRenderTarget(null);
                rot.Z += Utilities.deltaTime * MathHelper.Pi * speed;
                if (rot.Z > MathHelper.Pi * 2)
                {
                    rot.Z -= MathHelper.Pi * 2;
                }
            }
            else
            {
                scale = Vector3.Zero;
            }            
        }

        protected override Matrix GetWorld()
        {
            return Matrix.CreateScale(scale) * Matrix.CreateFromYawPitchRoll(rot.Y, rot.X, rot.Z) * Matrix.CreateTranslation(absOffset) * 
                Matrix.CreateFromYawPitchRoll(game.ship.rot.Y, game.ship.rot.X, game.ship.rot.Z) * Matrix.CreateTranslation(game.ship.pos);
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
                    //effect.Texture = tex;
                    effect.Texture = (Texture2D)rTarg;
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
