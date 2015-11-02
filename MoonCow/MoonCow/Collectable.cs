using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    class Collectable:BasicModel
    {
        public CircleCollider col;
        public Game1 game;
        public CollectableGlow glow;

        public Collectable() { }
        public Collectable(Vector3 pos, Game1 game)
        {
            this.pos = pos;
            col = new CircleCollider(pos, 0.5f);
            rot.X = MathHelper.PiOver4 / 3;
        }
        public override void Update(GameTime gameTime)
        {
            rot.Y += Utilities.deltaTime * MathHelper.PiOver4;
            if (rot.Y > MathHelper.Pi * 2)
                rot.Y -= MathHelper.Pi * 2;
        }

        public virtual void checkCollision()
        {
            if(col.checkCircle(game.ship.circleCol))
            {
                onCollect();
            }
        }

        public virtual void onCollect()
        {
            game.audioManager.addSoundEffect(AudioLibrary.itemCollect, 1);
            Dispose();
        }

        public override void Dispose()
        {
            if (glow != null)
            {
                glow.Dispose();
                game.modelManager.removeEffect(glow);
            }
            game.modelManager.toDeleteObject(this);
        }

        public override void setEffect()
        {
            Matrix[] transforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.TextureEnabled = true;
                    effect.Alpha = 1;

                    //trying to get lighting to work, but so far the model just shows up as pure black - it was exported with a green blinn shader
                    //effect.EnableDefaultLighting(); //did not work
                    effect.LightingEnabled = true;

                    if (mesh.Name.Contains("glow"))
                    {
                        effect.AmbientLightColor = Vector3.One;
                    }
                    else
                    {
                        effect.DirectionalLight0.DiffuseColor = new Vector3(0.3f, 0.3f, 0.3f); //RGB is treated as a vector3 with xyz being rgb - so vector3.one is white
                        effect.DirectionalLight0.Direction = new Vector3(0, -1, 1);
                        effect.DirectionalLight0.SpecularColor = Vector3.One;
                        effect.AmbientLightColor = new Vector3(0.3f, 0.3f, 0.3f);
                        effect.EmissiveColor = new Vector3(0.3f, 0.3f, 0.3f);
                        effect.PreferPerPixelLighting = true;
                    }

                }
                mesh.Draw();
            }
        }

        protected virtual Matrix GetWorld()
        {
            return Matrix.CreateScale(scale) * Matrix.CreateRotationX(rot.X) * Matrix.CreateRotationY(rot.Y) * Matrix.CreateTranslation(pos);
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
                }
                mesh.Draw();
            }
        }
    }
}
