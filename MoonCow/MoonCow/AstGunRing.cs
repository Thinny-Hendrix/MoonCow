using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    class AstGunRing:BasicModel
    {
        Projectile projectile;
        Game1 game;
        Vector3 dir;
        float time;
        float alpha;
        float dist;
        Vector3 basePos;
        float speed;
        RenderTarget2D rTarg;
        SpriteBatch sb;
        Color c1;
        Color c2;
        public AstGunRing(Projectile proj, Game1 game, Color c1, Color c2)
        {
            this.projectile = proj;
            this.game = game;
            this.dir = projectile.direction;
            rot.Y = (float)Math.Atan2(dir.X, dir.Z);
            model = TextureManager.square;
            scale = new Vector3(0.015f);
            speed = 50;
            pos = projectile.pos + dir * -1;
            basePos = this.pos;
            alpha = 1;
            rTarg = new RenderTarget2D(game.GraphicsDevice, 64, 64);
            sb = new SpriteBatch(game.GraphicsDevice);
            this.c1 = c1;
            this.c2 = c2;
        }

        public override void Update(GameTime gameTime)
        {
            time += Utilities.deltaTime;


            if (((AstGunProjectile)projectile).active)
            {
                basePos += dir * speed * Utilities.deltaTime;
                dist = time * 10;
                pos = basePos + dir * -dist;
            }
            else
            {
                basePos += dir * speed/3 * Utilities.deltaTime;
                dist = time * 10;
                pos = basePos + dir * -dist;
            }

            scale = new Vector3(0.015f+time/12);


            game.GraphicsDevice.SetRenderTarget(rTarg);
            sb.Begin();
            sb.Draw(TextureManager.particle3small, Vector2.Zero, Color.Lerp(c1, c2, time * 3));
            sb.End();
            game.GraphicsDevice.SetRenderTarget(null);


            if(time > 0.5f)
            {
                Dispose();
            }
            else
            {
                if (time > 0.3f)
                {
                    alpha = MathHelper.Lerp(1, 0, (time - 0.3f) * 5);
                }
            }
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
                    effect.Texture = (Texture2D)rTarg;
                    effect.Alpha = alpha;

                    //trying to get lighting to work, but so far the model just shows up as pure black - it was exported with a green blinn shader
                    //effect.EnableDefaultLighting(); //did not work
                    effect.LightingEnabled = true;
                    effect.AmbientLightColor = Vector3.One;

                }
                mesh.Draw();
            }
        }

        public override void Dispose()
        {
            game.modelManager.toDeleteModel(this);
            rTarg.Dispose();
            sb.Dispose();
        }


    }
}
