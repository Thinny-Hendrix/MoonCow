using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    class GlowStreakCenter:BasicModel
    {
        Texture2D tex;
        Game1 game;
        float fScale;
        float time;
        float maxScale;
        float alpha;
        float speed;
        public GlowStreakCenter(Game1 game, Vector3 pos, float maxScale, float speed, int type)
            : base()
        {
            this.game = game;
            this.pos = pos;
            fScale = 0.25f;
            this.maxScale = maxScale * 0.1f;
            this.speed = speed;

            rot.Z = (float)Utilities.random.NextDouble() * MathHelper.Pi * 2;
            model = TextureManager.square;
            alpha = 1;
            setTex(type);
        }

        void setTex(int type)
        {
            switch (type)
            {
                case -2:
                    tex = TextureManager.particle1;
                    break;
                case -1:
                    tex = TextureManager.particle1;;
                    break;
                default:
                    tex = TextureManager.glowC_0;
                    break;
                case 1:
                    tex = TextureManager.glowC_1;
                    break;
                case 2:
                    tex = TextureManager.glowC_2;
                    break;
                case 3:
                    tex = TextureManager.glowC_3;
                    break;

            }
        }

        public override void Update(GameTime gameTime)
        {
            if (!Utilities.paused && !Utilities.softPaused)
            {
                fScale = MathHelper.Lerp(0, maxScale, (float)(Math.Sin(time) + 1) / 2);

                time += Utilities.deltaTime * MathHelper.Pi * 0.9f * speed;

                if (time > MathHelper.Pi)
                {
                    alpha = MathHelper.Lerp(1, 0, (time - MathHelper.Pi) / MathHelper.PiOver2);
                }

                if (time > MathHelper.Pi * 1.5f)
                    game.modelManager.toDeleteModel(this);
            }

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
