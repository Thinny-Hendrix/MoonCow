using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    public class BombExplosion:BasicModel
    {
        Game1 game;
        Model ripple;
        SpriteBatch sb;
        RenderTarget2D rippleTex;
        Vector2 texPos;

        float ringRot;
        float ringRot2;
        float ringRot3;
        float ringRot4;
        float rippleScale;
        float time;
        float ringAlpha;
        bool triggeredShake;

        public BombExplosion(Vector3 pos, Game1 game):base()
        {
            this.pos = pos;
            this.game = game;
            scale = new Vector3(0.5f);
            model = ModelLibrary.bombRings;
            ripple = ModelLibrary.bombRipples;

            ringRot = Utilities.nextFloat() * MathHelper.Pi * 2;
            ringRot2 = Utilities.nextFloat() * MathHelper.Pi * 2;
            ringRot3 = Utilities.nextFloat() * MathHelper.Pi * 2;
            ringRot4 = Utilities.nextFloat() * MathHelper.Pi * 2;

            rippleScale = 0.4f;
            ringAlpha = 1;
            triggeredShake = false;

            sb = new SpriteBatch(game.GraphicsDevice);
            rippleTex = new RenderTarget2D(game.GraphicsDevice, 512, 128);
            texPos = new Vector2(0, -128);

            rot = new Vector3(Utilities.nextFloat() * MathHelper.PiOver4 - MathHelper.PiOver4/2, 0, Utilities.nextFloat() * MathHelper.PiOver4 - MathHelper.PiOver4/2);
        }

        void updateRingRots()
        {
            ringRot += Utilities.deltaTime * MathHelper.Pi * 5;
            if (ringRot > MathHelper.Pi * 2)
                ringRot -= MathHelper.Pi * 2;

            ringRot2 += Utilities.deltaTime * MathHelper.Pi * 5.4f;
            if (ringRot2 > MathHelper.Pi * 2)
                ringRot2 -= MathHelper.Pi * 2;

            ringRot3 += Utilities.deltaTime * MathHelper.Pi * 5.7f;
            if (ringRot3 > MathHelper.Pi * 2)
                ringRot3 -= MathHelper.Pi * 2;

            ringRot4 += Utilities.deltaTime * MathHelper.Pi * 6;
            if (ringRot4 > MathHelper.Pi * 2)
                ringRot4 -= MathHelper.Pi * 2;
        }

        void updateRipple()
        {
            texPos.Y = (float)((Math.Cos((time / 15) * MathHelper.PiOver2*3))+1) * 64-128;
            game.GraphicsDevice.SetRenderTarget(rippleTex);
            sb.Begin();
            sb.Draw(TextureManager.bombRipple, texPos, Color.White);
            sb.End();
            game.GraphicsDevice.SetRenderTarget(null);
        }

        public override void Update(GameTime gameTime)
        {
            float frameTime = Utilities.deltaTime*60;

            if(time < 15)
            {
                scale -= new Vector3(0.025f)*frameTime;
                updateRipple();
            }
            else
            {
                if(!triggeredShake)
                {
                    game.camera.makeShake();
                    game.hud.makeFlash();
                    triggeredShake = true;
                }
                scale += new Vector3(0.05f)*frameTime;
                if (ringAlpha != 0)
                {
                    ringAlpha -= Utilities.deltaTime * 1.3f;
                    if (ringAlpha < 0)
                        ringAlpha = 0;
                }
            }
            updateRingRots();
            time += frameTime;
            if (time > 120)
            {
                game.modelManager.toDeleteModel(this);
                sb.Dispose();
                rippleTex.Dispose();
            }
        }

        void drawRings(Texture2D tex, Camera camera)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    if (mesh.Name.Equals("ring1"))
                    {
                        effect.World = Matrix.CreateRotationY(ringRot2) * mesh.ParentBone.Transform * GetWorld();
                    }
                    else if (mesh.Name.Equals("ring2"))
                    {
                        effect.World = Matrix.CreateRotationY(ringRot4) * mesh.ParentBone.Transform * GetWorld();
                    }
                    else if (mesh.Name.Equals("ring3"))
                    {
                        effect.World = Matrix.CreateRotationY(ringRot3) * mesh.ParentBone.Transform * GetWorld();
                    }
                    else if (mesh.Name.Equals("ring8"))
                    {
                        effect.World = Matrix.CreateRotationY(ringRot2) * mesh.ParentBone.Transform * GetWorld();
                    }
                    else if (mesh.Name.Equals("ring5"))
                    {
                        effect.World = Matrix.CreateRotationY(ringRot) * mesh.ParentBone.Transform * GetWorld();
                    }
                    else if (mesh.Name.Equals("ring6"))
                    {
                        effect.World = Matrix.CreateRotationY(ringRot3) * mesh.ParentBone.Transform * GetWorld();
                    }
                    else
                    {
                        effect.World = Matrix.CreateRotationY(ringRot4) * mesh.ParentBone.Transform * GetWorld();
                    }

                    effect.View = camera.view;
                    effect.Projection = camera.projection;


                    effect.TextureEnabled = true;

                    effect.Texture = tex;



                    effect.Alpha = ringAlpha;

                    effect.LightingEnabled = true;
                    effect.AmbientLightColor = Vector3.One;
                    effect.PreferPerPixelLighting = true;

                }
                mesh.Draw();
                mesh.Draw();
            }
        }

        public override void Draw(GraphicsDevice device, Camera camera)
        {
            Matrix[] transforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transforms);
            device.BlendState = BlendState.Additive;
            device.DepthStencilState = DepthStencilState.DepthRead;

            drawRings(TextureManager.bombRing, camera);
            if (time < 15)
            {
                drawRings(TextureManager.bombBlip, camera);

                transforms = new Matrix[ripple.Bones.Count];
                ripple.CopyAbsoluteBoneTransformsTo(transforms);

                foreach (ModelMesh mesh in ripple.Meshes)
                {
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        {
                            effect.World = Matrix.CreateScale(rippleScale) * Matrix.CreateRotationX(MathHelper.PiOver2 * 3) * Matrix.CreateBillboard(pos, camera.cameraPosition, camera.tiltUp, null);
                            //effect.World = Matrix.CreateScale(scale) * Matrix.CreateRotationY(MathHelper.PiOver2) * Matrix.CreateConstrainedBillboard(projectile.pos, game.camera.cameraPosition, Vector3.Cross(Vector3.Left, projectile.direction), null, null);
                        }

                        effect.View = camera.view;
                        effect.Projection = camera.projection;


                        effect.TextureEnabled = true;

                        effect.Texture = (Texture2D)rippleTex;



                        effect.Alpha = 1;

                        effect.LightingEnabled = true;
                        effect.AmbientLightColor = Vector3.One;
                        effect.PreferPerPixelLighting = true;

                    }
                    mesh.Draw();
                }
            }

            //base.Draw(device, camera);
        }
    }
}
