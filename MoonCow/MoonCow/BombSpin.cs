﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    class BombSpin : BasicModel
    {
        Texture2D tex;
        Game1 game;
        float fScale;
        SpriteBatch sb;
        RenderTarget2D rt;
        Color col;
        float alpha;
        float maxScale;
        float time;
        BlendState state;
        public BombSpin(Game1 game, Vector3 pos, Color col, float maxScale)
            : base()
        {
            this.game = game;
            this.pos = pos;
            this.col = col;
            this.maxScale = maxScale;
            fScale = 0.3f;

            state = BlendState.AlphaBlend;

            rot.Z = (float)Utilities.random.NextDouble() * MathHelper.Pi * 2;
            tex = TextureManager.spinM;
            model = TextureManager.square;

            sb = new SpriteBatch(game.GraphicsDevice);
            rt = new RenderTarget2D(game.GraphicsDevice, 512, 512);
            alpha = 1;
        }

        public override void Update(GameTime gameTime)
        {
            if (!Utilities.paused && !Utilities.softPaused)
            {
                time += Utilities.deltaTime * 1;
                if (time > 1)
                    time = 1;

                fScale = MathHelper.SmoothStep(0.3f, maxScale, time*4);

                rot.Z += Utilities.deltaTime * MathHelper.Pi;

                //fScale += Utilities.deltaTime * 2.5f;

                //if (time > 0.1f)
                    //alpha = 1 - (fScale - 0.1f) * 5;

                game.GraphicsDevice.SetRenderTarget(rt);
                game.GraphicsDevice.Clear(Color.Transparent);
                sb.Begin();
                sb.Draw(tex, Vector2.Zero, col);
                sb.End();
                game.GraphicsDevice.SetRenderTarget(null);



                if (time > 0.30f)
                {
                    sb.Dispose();
                    rt.Dispose();
                    game.modelManager.toDeleteModel(this);
                }
            }

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
                    effect.Texture = (Texture2D)rt;
                    effect.Alpha = alpha;

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
