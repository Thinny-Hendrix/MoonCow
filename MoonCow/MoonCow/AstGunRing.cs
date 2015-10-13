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
        public AstGunRing(Projectile proj, Game1 game)
        {
            this.projectile = proj;
            this.game = game;
            this.dir = projectile.direction;
            rot.Y = (float)Math.Atan2(dir.X, dir.Z);
            model = TextureManager.square;
            scale = new Vector3(0.03f);
            speed = 50;
            pos = projectile.pos;
            basePos = pos;
        }

        public override void Update(GameTime gameTime)
        {
            basePos += dir * speed * Utilities.deltaTime;
            time += Utilities.deltaTime;
            dist = time * 10;
            pos = basePos + dir * -dist;

            if(time > 0.5f)
            {
                Dispose();
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
                    effect.Texture = TextureManager.particle3small;
                    effect.Alpha = 1;

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
        }


    }
}
