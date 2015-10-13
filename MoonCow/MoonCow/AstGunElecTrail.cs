using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    public class AstGunElecTrail:BasicModel
    {
        Vector3 dir;
        float time;
        Game1 game;
        AstGunProjectile proj;
        SpriteBatch sb;
        RenderTarget2D rTarg;
        Texture2D tex;
        Color c1, c2;

        public AstGunElecTrail(AstGunProjectile proj, Game1 game, Color c1, Color c2)
        {
            this.game = game;
            this.proj = proj;
            pos = proj.pos;
            this.dir = proj.direction*-1;
            model = TextureManager.dirSquare;
            rTarg = new RenderTarget2D(game.GraphicsDevice, 32, 128);
            sb = new SpriteBatch(game.GraphicsDevice);
            scale = new Vector3(0.5f, 5, 0.5f);
            changeTex();
            this.c1 = c1;
            this.c2 = c2;
        }

        void changeTex()
        {
            switch (Utilities.random.Next(4))
            {
                default:
                    tex = TextureManager.elecL1;
                    break;
                case 1:
                    tex = TextureManager.elecL2;
                    break;
                case 2:
                    tex = TextureManager.elecL3;
                    break;
                case 3:
                    tex = TextureManager.elecL4;
                    break;
            }
        }

        public override void Update(GameTime gameTime)
        {
            pos = proj.pos;
            changeTex();
            game.GraphicsDevice.SetRenderTarget(rTarg);
            sb.Begin();
            sb.Draw(tex, new Rectangle(0, 0, 32, 128), Color.Lerp(c1, c2, Utilities.nextFloat()));
            sb.End();
            game.GraphicsDevice.SetRenderTarget(null);

            if(time < 1)
            {
                time += Utilities.deltaTime * 4;
                if (time > 1)
                    time = 1;
            }
            scale.Y = MathHelper.Lerp(1, 5, time);

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
                    effect.World = Matrix.CreateScale(scale) * Matrix.CreateRotationY(MathHelper.Pi) * Matrix.CreateConstrainedBillboard(pos, game.camera.cameraPosition, dir, null, dir);
                    effect.View = camera.view;
                    effect.Projection = camera.projection;


                    effect.TextureEnabled = true;
                    effect.Texture = (Texture2D)rTarg;

                    effect.Alpha = 1;

                    effect.LightingEnabled = true;
                    effect.AmbientLightColor = Vector3.One;
                    effect.PreferPerPixelLighting = true;

                }
                mesh.Draw();
                mesh.Draw();
            }
        }

        public override void Dispose()
        {
            rTarg.Dispose();
            sb.Dispose();
        }
    }
}
