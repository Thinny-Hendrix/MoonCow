using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    public class TurretHolo:BasicModel
    {
        bool active;
        Game1 game;
        float alpha1;
        float alpha2;
        public TurretHolo(Vector3 pos, Game1 game):base()
        {
            this.game = game;
            this.pos = pos;
            this.pos.Y += 0.5f;
            this.rot = Vector3.Zero;
            this.scale = new Vector3(3);
            model = ModelLibrary.turretHolo;
            active = true;
        }

        public void wake()
        {
            active = true;
        }

        public void close()
        {
            active = false;
        }

        public override void Update(GameTime gameTime)
        {
            if (!Utilities.softPaused && !Utilities.paused)
            {
                alpha1 = Utilities.nextFloat() / 3 + 0.7f;
                alpha2 = Utilities.nextFloat() / 3 + 0.7f;

                rot.Y += Utilities.deltaTime * MathHelper.PiOver4;
                if (rot.Y > MathHelper.Pi * 2)
                    rot.Y -= MathHelper.Pi * 2;
            }
        }

        public override void Draw(GraphicsDevice device, Camera camera)
        {
            if (active)
            {
                game.GraphicsDevice.BlendState = BlendState.Additive;

                Matrix[] transforms = new Matrix[model.Bones.Count];
                model.CopyAbsoluteBoneTransformsTo(transforms);
                game.GraphicsDevice.DepthStencilState = DepthStencilState.DepthRead;

                foreach (ModelMesh mesh in model.Meshes)
                {
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        effect.World = mesh.ParentBone.Transform * Matrix.CreateTranslation(new Vector3(0, .5f, 0)) * GetWorld();
                        effect.View = camera.view;
                        effect.Projection = camera.projection;
                        effect.TextureEnabled = true;

                        if (mesh.Name.StartsWith("fill"))
                        {
                            effect.Texture = TextureManager.turLogoIn;
                            effect.Alpha = alpha1;
                        }
                        else
                        {
                            effect.Texture = TextureManager.turLogoOut;
                            effect.Alpha = alpha2;
                        }


                    }
                    mesh.Draw();
                    mesh.Draw();
                }
            }
        }
    }
}
