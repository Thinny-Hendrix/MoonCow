using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    public class ElectroShotModel:BasicModel
    {
        ElectroShot shot;
        Game1 game;
        Texture2D tex;
        float dirMult;

        public ElectroShotModel(ElectroShot shot, Game1 game):base()
        {
            this.shot = shot;
            this.game = game;
            pos = shot.pos;
            scale = new Vector3(1f);
            this.model = TextureManager.dirSquare;
            tex = TextureManager.elecL1;
            dirMult = -1;

        }

        public override void Update(GameTime gameTime)
        {
            scale.Y = shot.targetDist * shot.length;
            if(dirMult == -1 && shot.time > MathHelper.PiOver2)
            {
                pos = shot.target.pos;
                dirMult = 1;
            }
        }

        public override void Draw(GraphicsDevice device, Camera camera)
        {
            Matrix[] transforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transforms);
            game.GraphicsDevice.BlendState = BlendState.Additive;
            game.GraphicsDevice.DepthStencilState = DepthStencilState.DepthRead;
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    if (mesh.Name.StartsWith("tip"))
                    {
                        effect.World = Matrix.CreateScale(scale * 2) * Matrix.CreateRotationY(MathHelper.PiOver2) * Matrix.CreateBillboard(pos, game.camera.cameraPosition, game.camera.tiltUp, null);
                        //effect.World = mesh.ParentBone.Transform * GetWorld();
                    }
                    else
                    {
                        effect.World = mesh.ParentBone.Transform * GetWorld();
                        effect.World = Matrix.CreateScale(scale) * Matrix.CreateRotationY(MathHelper.Pi) * Matrix.CreateConstrainedBillboard(pos, game.camera.cameraPosition, shot.direction*dirMult, null, shot.direction*dirMult);
                        // effect.World = Matrix.CreateScale(scale) * Matrix.CreateFromYawPitchRoll(-MathHelper.Pi+0.5f,MathHelper.Pi,MathHelper.PiOver2) * Matrix.CreateConstrainedBillboard(projectile.pos, game.camera.cameraPosition, projectile.direction, null, projectile.direction);
                        //effect.World = Matrix.CreateScale(scale) * Matrix.CreateRotationY(MathHelper.Pi) * Matrix.CreateRotationX(-MathHelper.PiOver2) * Matrix.CreateRotationZ(MathHelper.PiOver2) * Matrix.CreateConstrainedBillboard(projectile.pos, game.camera.cameraPosition, projectile.direction, null, projectile.direction);
                    }

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
                mesh.Draw();
            }

            //base.Draw(device, camera);
        }

        public override void Dispose()
        {
            game.modelManager.removeEffect(this);
        }
    }
}
