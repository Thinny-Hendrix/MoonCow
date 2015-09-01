using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    class ProjectileModel:BasicModel
    {
        Projectile projectile;
        Game1 game;
        Texture2D tex;

        public ProjectileModel(Model model, Vector3 pos, Projectile projectile, Game1 game, Texture2D tex):base(model)
        {
            this.model = model;
            this.game = game;
            this.pos = pos;
            this.projectile = projectile;
            this.tex = tex;
    
        }

        public override void Update(GameTime gameTime)
        {
            pos = projectile.pos;
            rot = projectile.rot;
            scale = new Vector3(3, 3, 3);

            //rot.Y = -rot.Y + MathHelper.PiOver2;
            //rot = Vector3.Transform(ship.direction, Matrix.CreateFromAxisAngle(Vector3.Up, ship.rot.Y));
        }

        public override void Draw(GraphicsDevice device, Camera camera)
        {
            Matrix[] transforms = new Matrix[model.Bones.Count];
                model.CopyAbsoluteBoneTransformsTo(transforms);
                game.GraphicsDevice.BlendState = BlendState.Opaque;
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

                        //effect.EnableDefaultLighting(); //did not work
                        effect.LightingEnabled = true;

                        //effect.DirectionalLight0.DiffuseColor = new Vector3(0.6f, 0.5f, 0.6f); //RGB is treated as a vector3 with xyz being rgb - so vector3.one is white
                        effect.DirectionalLight0.Direction = Vector3.Down;
                        //effect.DirectionalLight0.SpecularColor = Vector3.One;
                        effect.AmbientLightColor = Vector3.One;
                        //effect.EmissiveColor = Vector3.One;
                        effect.PreferPerPixelLighting = true;

                    }
                    mesh.Draw();
                }

            base.Draw(device, camera);
        }
        

    }
}
