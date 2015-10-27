using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SkinnedModel;

namespace MoonCow
{
    class SpawnVortex:BasicModel
    {
        Game1 game;
        Texture2D tex;
        WaveManager manager;
        bool isVisible;
        AnimationPlayer animPlayer;
        AnimationClip animClip;
        RenderTarget2D targ1;
        RenderTarget2D targ2;
        SpriteBatch sb;
        Vector2 texPos;
        float spinRot;

        public SpawnVortex(Vector3 pos, Vector3 rot, Game1 game):base()
        {
            this.pos = pos;
            this.rot = rot;
            this.game = game;
            model = ModelLibrary.vortex;
            tex = game.Content.Load<Texture2D>(@"Models/Enemies/SpawnPoint/spiraltest");
            scale = new Vector3(0.8f,0.8f,0.7f);

            SkinningData skinningData = ModelLibrary.vortex.Tag as SkinningData;

            if (skinningData == null)
                throw new InvalidOperationException
                    ("This model does not contain a SkinningData tag.");

            // Create an animation player, and start decoding an animation clip.
            animPlayer = new AnimationPlayer(skinningData);

            animClip = skinningData.AnimationClips["Take 001"];

            animPlayer.StartClip(animClip);

            SetupEffects();

            sb = new SpriteBatch(game.GraphicsDevice);
            targ1 = new RenderTarget2D(game.GraphicsDevice, 1024, 1024);
            targ2 = new RenderTarget2D(game.GraphicsDevice, 1024, 1024);
            texPos = Vector2.Zero;
        }

        public override void Update(GameTime gameTime)
        {
            if (!Utilities.paused && !Utilities.softPaused)
            {
                try
                {
                    if (manager.spawnState == Utilities.SpawnState.deploying)
                    {
                        if (!isVisible)
                        {
                            isVisible = true;
                            game.modelManager.addEffect(new ImpactParticleModel(game, pos, 1));
                            game.modelManager.addEffect(new LaserHitEffect(game, pos, Color.Navy, 2.5f, BlendState.AlphaBlend));
                            game.modelManager.addEffect(new LaserHitEffect(game, pos, Color.Purple, 2, BlendState.Additive));
                            for(int i = 0; i < 20; i++)
                            {
                                game.modelManager.addEffect(new DirLineParticle(pos, game));
                            }
                        }
                    }
                    else
                    {
                        if (isVisible)
                        {
                            isVisible = false;
                            game.modelManager.addEffect(new LaserHitEffect(game, pos, Color.Navy, 2.5f, BlendState.AlphaBlend));
                            game.modelManager.addEffect(new LaserHitEffect(game, pos, Color.Purple, 2, BlendState.Additive));
                            game.modelManager.addEffect(new ImpactParticleModel(game, pos, 1));
                            for (int i = 0; i < 20; i++)
                            {
                                game.modelManager.addEffect(new DirLineParticle(pos, game));
                            }
                        }
                    }
                }
                catch (NullReferenceException)
                {
                    manager = game.waveManager;
                }

                if (isVisible)
                {
                    rot.Z -= Utilities.deltaTime;
                    if (rot.Z < MathHelper.Pi * 2)
                        rot.Z += MathHelper.Pi * 2;

                    texPos.Y += Utilities.deltaTime * 512;
                    if (texPos.Y > 1024)
                        texPos.Y -= 1024;

                    spinRot += Utilities.deltaTime * MathHelper.PiOver2;
                    if(spinRot > MathHelper.Pi*2)
                    {
                        spinRot -= MathHelper.Pi * 2;
                    }

                    game.GraphicsDevice.SetRenderTarget(targ1);
                    game.GraphicsDevice.Clear(Color.Transparent);
                    sb.Begin();
                    sb.Draw(TextureManager.vortLines, texPos, Color.White);
                    sb.Draw(TextureManager.vortLines, texPos + new Vector2(0, -1024), Color.White);
                    sb.End();
                    game.GraphicsDevice.SetRenderTarget(targ2);
                    game.GraphicsDevice.Clear(Color.Transparent);
                    sb.Begin();
                    sb.Draw(TextureManager.vortBack, new Rectangle(512,512,1024,1024), null, Color.White, spinRot, new Vector2(512,512), SpriteEffects.None, 0);
                    sb.End();
                    game.GraphicsDevice.SetRenderTarget(null);

                    animPlayer.Update(gameTime.ElapsedGameTime, true, GetWorld());
                }
            }
        }

        private void SetupEffects()
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (SkinnedEffect effect in mesh.Effects)
                {
                    //effect.World = mesh.ParentBone.Transform * GetWorld();
                    //effect.Texture = tex;
                    effect.Alpha = 1;
                    effect.WeightsPerVertex = 1;
                    //effect.LightingEnabled = true;
                    
                    if (mesh.Name.Equals("round"))
                    {
                        effect.Texture = tex;
                    }
                    else if(mesh.Name.Contains("round2"))
                    {
                        effect.Texture = (Texture2D)targ2;
                    }
                    else
                    {
                        effect.Texture = (Texture2D)targ1;
                    }
                    {
                        //effect.DirectionalLight0.DiffuseColor = new Vector3(0.2f, 0.2f, 0.2f); //RGB is treated as a vector3 with xyz being rgb - so vector3.one is white
                        //effect.DirectionalLight0.Direction = new Vector3(0, -1, 1);
                        effect.AmbientLightColor = new Vector3(0.9f, 0.9f, 0.9f);
                        //effect.SpecularColor = new Vector3(0.3f);
                        //effect.EmissiveColor = new Vector3(.4f, .4f, .4f);
                    }
                    effect.PreferPerPixelLighting = true;
                }
            }
        }

        public override void Draw(GraphicsDevice device, Camera camera)
        {


            //game.GraphicsDevice.DepthStencilState = depthStencilState;
            game.GraphicsDevice.BlendState = BlendState.Additive;
            if (isVisible)
            {
                Matrix[] transforms = new Matrix[model.Bones.Count];
                model.CopyAbsoluteBoneTransformsTo(transforms);

                Matrix[] bones = animPlayer.GetSkinTransforms();


                foreach (ModelMesh mesh in model.Meshes)
                {
                    foreach (SkinnedEffect effect in mesh.Effects)
                    {
                        effect.SetBoneTransforms(bones);

                        //effect.Texture = (Texture2D)targ1;

                        //effect.Texture = TextureManager.gib1_0;
                        if (mesh.Name.Equals("round"))
                        {
                            game.GraphicsDevice.BlendState = BlendState.AlphaBlend;
                            effect.Texture = (Texture2D)targ2;
                            effect.Alpha = 0.8f;
                            
                        }
                        else if (mesh.Name.Equals("round2"))
                        {
                            game.GraphicsDevice.BlendState = BlendState.Additive;
                            effect.Texture = TextureManager.vortSpir;
                            effect.Alpha = 1f;
                        }
                        else
                        {
                            game.GraphicsDevice.BlendState = BlendState.Additive;
                            effect.Texture = (Texture2D)targ1;
                            effect.Alpha = 1f;
                        }

                        //effect.World = mesh.ParentBone.Transform * GetWorld();
                        effect.View = camera.view;
                        effect.Projection = camera.projection;
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
