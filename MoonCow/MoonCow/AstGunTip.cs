using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    public class AstGunTip:BasicModel
    {
        Projectile projectile;
        public Vector3 dir;
        Game1 game;
        float fScale;

        public AstGunTip(Projectile projectile, Game1 game)
        {
            this.projectile = projectile;
            this.game = game;
            pos = projectile.pos;
            dir = projectile.direction;
            fScale = 0.05f;
            model = TextureManager.square;
        }

        public override void Update(GameTime gameTime)
        {
            pos = projectile.pos;
            rot.Z = Utilities.nextFloat() * MathHelper.Pi * 2;
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
                    effect.Texture = TextureManager.particle1small;
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

        public override void Dispose()
        {
            game.modelManager.toDeleteModel(this);
        }
    }
}
