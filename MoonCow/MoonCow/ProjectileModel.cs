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
        Texture2D tex2;
        Texture2D tex3;

        Matrix tipMatrix;
        Matrix trailMatrix;

        ModelBone tip;
        ModelBone tail;

        RenderTarget2D tipTarg;
        RenderTarget2D trailTarg;
        RenderTarget2D endTarg;
        SpriteBatch sb;

        Vector2 texPos1;
        Vector2 texPos2;
        Vector2 texPos3;
        Vector2 texPos4;

        Color c1;
        Color c2;

        float endRot;


        public ProjectileModel(Model model, Vector3 pos, Projectile projectile, Color c1, Color c2, Game1 game):base(model)
        {
            this.model = model;
            this.game = game;
            this.pos = pos;
            this.projectile = projectile;
            this.c1 = c1;
            this.c2 = c2;
            tex = TextureManager.particle1;
            tex2 = TextureManager.particle2;
            tex3 = TextureManager.particle3;

            texPos1 = new Vector2(0, 0);

            tip = model.Bones["tip1"];
            tipMatrix = Matrix.Identity;

            tipTarg = new RenderTarget2D(game.GraphicsDevice, 256, 256);
            trailTarg = new RenderTarget2D(game.GraphicsDevice, 256, 768);
            endTarg = new RenderTarget2D(game.GraphicsDevice, 256, 256);
            sb = new SpriteBatch(game.GraphicsDevice);
    
        }

        public override void Update(GameTime gameTime)
        {
            pos = projectile.pos;
            rot = projectile.rot;
            //rot.Y += MathHelper.Pi;
            rot.Z += MathHelper.PiOver2;
            scale = new Vector3(.1f, .1f, .1f);
            //scale = new Vector3(1, 1, -1);

            endRot += MathHelper.Pi*Utilities.deltaTime*8;
            if(endRot > MathHelper.Pi*2)
                endRot -= MathHelper.Pi*2;

            //rot.Y = -rot.Y + MathHelper.PiOver2;
            //rot = Vector3.Transform(ship.direction, Matrix.CreateFromAxisAngle(Vector3.Up, ship.rot.Y));

            setMatrices();

            texPos1.Y += (int)(Utilities.deltaTime * 1200);
            if (texPos1.Y > 256)
                texPos1.Y -= 256;
            texPos2.Y = texPos1.Y - 256;
            texPos3.Y = texPos1.Y + 512;
            texPos4.Y = texPos1.Y + 256;

            game.GraphicsDevice.SetRenderTarget(tipTarg);

            sb.Begin();
            sb.Draw(tex, Vector2.Zero, c1);
            sb.End();

            game.GraphicsDevice.SetRenderTarget(trailTarg);

            sb.Begin();
            sb.Draw(tex3, texPos1, c1);
            sb.Draw(tex3, texPos2, c1);
            sb.Draw(tex3, texPos3, c1);
            sb.Draw(tex3, texPos4, c1);

            sb.Draw(tex3, texPos1, c1);
            sb.Draw(tex3, texPos2, c1);
            sb.Draw(tex3, texPos3, c1);
            sb.Draw(tex3, texPos4, c1);

            sb.Draw(tex3, texPos1, c1);
            sb.Draw(tex3, texPos2, c1);
            sb.Draw(tex3, texPos3, c1);
            sb.Draw(tex3, texPos4, c1);
            sb.End();

            game.GraphicsDevice.SetRenderTarget(endTarg);

            sb.Begin();
            sb.Draw(tex2, new Rectangle(127, 127, 256, 256), null, c2, endRot, new Vector2(127, 127), SpriteEffects.None, 1);
            sb.Draw(tex2, new Rectangle(127, 127, 256, 256), null, c2, endRot, new Vector2(127, 127), SpriteEffects.None, 1);
            sb.Draw(tex2, new Rectangle(127, 127, 256, 256), null, c2, endRot, new Vector2(127, 127), SpriteEffects.None, 1);
            sb.Draw(tex2, new Rectangle(127, 127, 256, 256), null, c2, endRot, new Vector2(127, 127), SpriteEffects.None, 1);
            sb.End();

            game.GraphicsDevice.SetRenderTarget(null);

            /*
            tip.Transform = tipMatrix;
            model.Bones["tip2"].Transform = tipMatrix;
            model.Bones["tip3"].Transform = tipMatrix;
            model.Bones["tip4"].Transform = tipMatrix;*/

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
                            effect.World = Matrix.CreateScale(scale*2)*Matrix.CreateRotationY(MathHelper.PiOver2)*Matrix.CreateBillboard(pos, game.camera.cameraPosition, game.camera.tiltUp, null);
                            //effect.World = mesh.ParentBone.Transform * GetWorld();
                        }
                        else
                        {
                            effect.World = mesh.ParentBone.Transform * GetWorld();
                            //effect.World = Matrix.CreateScale(scale) * Matrix.CreateRotationY(MathHelper.PiOver2) * Matrix.CreateConstrainedBillboard(projectile.pos, game.camera.cameraPosition, Vector3.Cross(Vector3.Left, projectile.direction), null, null);
                        }

                        effect.View = camera.view;
                        effect.Projection = camera.projection;

                        
                        effect.TextureEnabled = true;

                        if (mesh.Name.StartsWith("tip"))
                            if (mesh.Name.EndsWith("3") || mesh.Name.EndsWith("4"))
                                effect.Texture = (Texture2D)tipTarg;
                            else
                                effect.Texture = tex;
                        else if (mesh.Name.StartsWith("end"))
                            effect.Texture = (Texture2D)endTarg;
                        else//tail
                            effect.Texture = (Texture2D)trailTarg;



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

            //base.Draw(device, camera);
        }

        public void setMatrices()
        {
            tipMatrix = Matrix.Identity * Matrix.CreateBillboard(Vector3.Zero, game.camera.cameraPosition, game.camera.tiltUp, game.camera.currentDirection);
            tipMatrix = Matrix.CreateConstrainedBillboard(projectile.pos, game.camera.cameraPosition, Vector3.Up, null, null);

            //System.Diagnostics.Debug.WriteLine("tipmatrix is " + tipMatrix);
        }
        

    }
}
