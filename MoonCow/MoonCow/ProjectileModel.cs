using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    public class ProjectileModel:BasicModel
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
        Color c3;

        float endRot;

        public float tipRot1;
        public float tipRot2;

        public ProjectileModel(Projectile projectile, Texture2D t1, Texture2D t2, Texture2D t3, Color c1, Color c2, Color c3, Game1 game)
            : base()
        {
            model = ModelLibrary.projectile;
            this.game = game;
            this.projectile = projectile;
            pos = projectile.pos;
            tex = t1;
            tex2 = t2;
            tex3 = t3;
            this.c1 = c1;
            this.c2 = c2;
            this.c3 = c3;

            scale = new Vector3(.1f, .1f, .1f);

            texPos1 = new Vector2(0, 0);

            tip = model.Bones["tip1"];
            tipMatrix = Matrix.Identity;

            tipTarg = new RenderTarget2D(game.GraphicsDevice, 64, 64);
            trailTarg = new RenderTarget2D(game.GraphicsDevice, 64, 172);
            endTarg = new RenderTarget2D(game.GraphicsDevice, 64, 64);
            sb = new SpriteBatch(game.GraphicsDevice);

            updateRots();
        }
        public ProjectileModel(Projectile projectile, Texture2D t1, Texture2D t2, Texture2D t3, Color c1, Color c2, Game1 game):base()
        {
            model = ModelLibrary.projectile;
            this.game = game;
            this.projectile = projectile;
            pos = projectile.pos;
            tex = t1;
            tex2 = t2;
            tex3 = t3;
            this.c1 = c1;
            this.c2 = c2;
            this.c3 = c1;

            scale = new Vector3(.1f, .1f, .1f);

            texPos1 = new Vector2(0, 0);

            tip = model.Bones["tip1"];
            tipMatrix = Matrix.Identity;

            tipTarg = new RenderTarget2D(game.GraphicsDevice, 64, 64);
            trailTarg = new RenderTarget2D(game.GraphicsDevice, 64, 172);
            endTarg = new RenderTarget2D(game.GraphicsDevice, 64, 64);
            sb = new SpriteBatch(game.GraphicsDevice);

            updateRots();
        }


        public ProjectileModel(Model model, Vector3 pos, Projectile projectile, Color c1, Color c2, Game1 game):base(model)
        {
            this.model = model;
            this.game = game;
            this.pos = pos;
            this.projectile = projectile;
            this.c1 = c1;
            this.c2 = c2;
            tex = TextureManager.particle1small;
            tex2 = TextureManager.particle2small;
            tex3 = TextureManager.particle3small;

            scale = new Vector3(.1f, .1f, .1f);

            texPos1 = new Vector2(0, 0);

            tip = model.Bones["tip1"];
            tipMatrix = Matrix.Identity;

            tipTarg = new RenderTarget2D(game.GraphicsDevice, 64, 64);
            trailTarg = new RenderTarget2D(game.GraphicsDevice, 64, 172);
            endTarg = new RenderTarget2D(game.GraphicsDevice, 64, 64);
            sb = new SpriteBatch(game.GraphicsDevice);

            updateRots();
        }

        void updateRots()
        {
            tipRot1 = Utilities.nextFloat() * MathHelper.Pi * 2;
            tipRot2 = Utilities.nextFloat() * MathHelper.Pi * 2;
        }

        public override void Update(GameTime gameTime)
        {
            pos = projectile.pos;
            rot = projectile.rot;
            updateRots();
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

            texPos1.Y += (int)(Utilities.deltaTime * 300);
            if (texPos1.Y > 64)
                texPos1.Y -= 64;
            texPos2.Y = texPos1.Y - 64;
            texPos3.Y = texPos1.Y + 128;
            texPos4.Y = texPos1.Y + 64;

            game.GraphicsDevice.SetRenderTarget(tipTarg);

            sb.Begin();
            sb.Draw(tex, Vector2.Zero, c1);
            sb.End();

            game.GraphicsDevice.SetRenderTarget(trailTarg);

            sb.Begin();
            sb.Draw(tex3, texPos1, c3);
            sb.Draw(tex3, texPos2, c3);
            sb.Draw(tex3, texPos3, c3);
            sb.Draw(tex3, texPos4, c3);

            sb.Draw(tex3, texPos1, c3);
            sb.Draw(tex3, texPos2, c3);
            sb.Draw(tex3, texPos3, c3);
            sb.Draw(tex3, texPos4, c3);

            sb.Draw(tex3, texPos1, c3);
            sb.Draw(tex3, texPos2, c3);
            sb.Draw(tex3, texPos3, c3);
            sb.Draw(tex3, texPos4, c3);

            sb.End();

            game.GraphicsDevice.SetRenderTarget(endTarg);

            sb.Begin();
            sb.Draw(tex2, new Rectangle(32, 32, 64, 64), null, c2, endRot, new Vector2(32, 32), SpriteEffects.None, 1);
            sb.Draw(tex2, new Rectangle(32, 32, 64, 64), null, c2, endRot, new Vector2(32, 32), SpriteEffects.None, 1);
            sb.Draw(tex2, new Rectangle(32, 32, 64, 64), null, c2, endRot, new Vector2(32, 32), SpriteEffects.None, 1);
            sb.Draw(tex2, new Rectangle(32, 32, 64, 64), null, c2, endRot, new Vector2(32, 32), SpriteEffects.None, 1);
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
                            if (mesh.Name.EndsWith("1") || mesh.Name.EndsWith("3"))
                                effect.World = Matrix.CreateRotationX(tipRot1) * Matrix.CreateScale(scale * 2) * Matrix.CreateRotationY(MathHelper.PiOver2) * Matrix.CreateBillboard(pos, game.camera.cameraPosition, game.camera.tiltUp, null);
                            else
                                effect.World = Matrix.CreateRotationX(tipRot2) * Matrix.CreateScale(scale * 2) * Matrix.CreateRotationY(MathHelper.PiOver2) * Matrix.CreateBillboard(pos, game.camera.cameraPosition, game.camera.tiltUp, null);
                            //effect.World = mesh.ParentBone.Transform * GetWorld();
                        }
                        else
                        {
                            effect.World = mesh.ParentBone.Transform * GetWorld();
                            effect.World = Matrix.CreateScale(scale) * Matrix.CreateFromYawPitchRoll(-MathHelper.Pi, -MathHelper.PiOver2, -MathHelper.PiOver2) * Matrix.CreateConstrainedBillboard(projectile.pos, game.camera.cameraPosition, projectile.direction, null, projectile.direction);
                           // effect.World = Matrix.CreateScale(scale) * Matrix.CreateFromYawPitchRoll(-MathHelper.Pi+0.5f,MathHelper.Pi,MathHelper.PiOver2) * Matrix.CreateConstrainedBillboard(projectile.pos, game.camera.cameraPosition, projectile.direction, null, projectile.direction);
                            //effect.World = Matrix.CreateScale(scale) * Matrix.CreateRotationY(MathHelper.Pi) * Matrix.CreateRotationX(-MathHelper.PiOver2) * Matrix.CreateRotationZ(MathHelper.PiOver2) * Matrix.CreateConstrainedBillboard(projectile.pos, game.camera.cameraPosition, projectile.direction, null, projectile.direction);
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
        
        public override void Dispose()
        {
            sb.Dispose();
            tipTarg.Dispose();
            trailTarg.Dispose();
            endTarg.Dispose();
        }

    }
}
