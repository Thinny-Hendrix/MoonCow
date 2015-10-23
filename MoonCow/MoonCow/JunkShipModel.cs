using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    class JunkShipModel:BasicModel
    {
        JunkShip junkShip;
        Game1 game;
        float swayTime;
        RenderTarget2D rTarg;
        SpriteBatch sb;
        Vector2 texPos;
        float initRot;
        public JunkShipModel(JunkShip junkShip, Game1 game):base()
        {
            this.junkShip = junkShip;
            this.game = game;
            pos = junkShip.pos;
            //pos.Y -= 8;
            model = ModelLibrary.artefact;
            scale = new Vector3(1f);
            rot.Y = (float)Math.Atan2(junkShip.dir.X, junkShip.dir.Z);
            rot.Y -= MathHelper.PiOver2;
            initRot = rot.Y;

            rTarg = new RenderTarget2D(game.GraphicsDevice, 512, 512);
            sb = new SpriteBatch(game.GraphicsDevice);
            texPos = Vector2.Zero;
        }

        public override void Update(GameTime gameTime)
        {
            //z rot sways on sin wave

            if(junkShip.destroying)
            {
                scale = Vector3.SmoothStep(new Vector3(1f), new Vector3(.2f), junkShip.time);
                rot.Y += Utilities.deltaTime * MathHelper.Pi * 6 * junkShip.time;
            }

            texPos.Y -= Utilities.deltaTime * 128;
            if (texPos.Y < -512)
                texPos.Y += 512;

            game.GraphicsDevice.SetRenderTarget(rTarg);
            sb.Begin();
            sb.Draw(TextureManager.bpCloud, texPos, Color.White);
            sb.Draw(TextureManager.bpCloud, texPos + new Vector2(0,512), Color.White);
            sb.End();
            game.GraphicsDevice.SetRenderTarget(null);

        }

        public override void Dispose()
        {
            rTarg.Dispose();
            sb.Dispose();
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

                    //trying to get lighting to work, but so far the model just shows up as pure black - it was exported with a green blinn shader
                    //effect.EnableDefaultLighting(); //did not work
                    effect.LightingEnabled = true;
                    if (junkShip.destroying)
                    {
                        effect.Texture = TextureManager.pureWhite;
                        effect.AmbientLightColor = Vector3.One;
                        effect.EmissiveColor = Vector3.One;
                    }
                    else
                    {
                        if (mesh.Name.Contains("glow"))
                        {
                                effect.Texture = (Texture2D)rTarg;
                            effect.AmbientLightColor = Vector3.One;
                        }
                        else
                        {
                                effect.Texture = TextureManager.artefact;

                            effect.DirectionalLight0.DiffuseColor = new Vector3(0.3f, 0.3f, 0.3f); //RGB is treated as a vector3 with xyz being rgb - so vector3.one is white
                            effect.DirectionalLight0.Direction = new Vector3(0, -1, 1);
                            effect.DirectionalLight0.SpecularColor = Vector3.One;
                            effect.AmbientLightColor = new Vector3(0.3f, 0.3f, 0.3f);
                            effect.EmissiveColor = new Vector3(0.3f, 0.3f, 0.3f);
                        }
                    }

                    
                    effect.PreferPerPixelLighting = true;

                }
                mesh.Draw();
            }
        }
    }
}
