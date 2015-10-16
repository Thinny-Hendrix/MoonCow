using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    class BombCircleParticle:BasicModel
    {
        Game1 game;
        Vector3 dir;
        float alpha;
        float speed;
        float time;
        float fScale;
        Texture2D tex;

        public BombCircleParticle(Vector3 pos, Game1 game, Vector3 dir, int type)
        {
            this.pos = pos;
            this.game = game;
            this.dir = dir;

            model = TextureManager.square;


            if (type == 0)
            {
                tex = TextureManager.astCloud1;
                fScale = 0.1f;
            }
            else
            {
                tex = TextureManager.bombCenter;
                fScale = 0.2f;
            }
            speed = 10;
            alpha = 1;

            pos += dir * 2;

 

        }

        public BombCircleParticle(Vector3 pos, Game1 game, float angle)
        {
            this.pos = pos;
            this.game = game;

            model = TextureManager.square;
            fScale = 0.1f;
            speed = 30;
            alpha = 1;
            tex = TextureManager.particle1;

            dir.X = (float)Math.Sin(angle);
            dir.Z = (float)Math.Cos(angle);
        }

        public override void Update(GameTime gameTime)
        {
            this.pos += dir * speed * Utilities.deltaTime;
            time += Utilities.deltaTime;

            rot.Z += Utilities.deltaTime * MathHelper.Pi;

            if(time > 0.5f)
            {
                alpha = MathHelper.Lerp(1, 0, (time - 0.5f) * 2);
            }

            if(time >= 1)
            {
                game.modelManager.toDeleteModel(this);
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
                    effect.Texture = tex;
                    effect.Alpha = alpha;

                    //trying to get lighting to work, but so far the model just shows up as pure black - it was exported with a green blinn shader
                    //effect.EnableDefaultLighting(); //did not work
                    effect.LightingEnabled = true;

                    effect.DirectionalLight0.DiffuseColor = new Vector3(0.3f, 0.3f, 0.3f); //RGB is treated as a vector3 with xyz being rgb - so vector3.one is white
                    effect.DirectionalLight0.Direction = new Vector3(0, -1, 1);
                    effect.DirectionalLight0.SpecularColor = Vector3.One;
                    effect.AmbientLightColor = Vector3.One;
                    effect.EmissiveColor = new Vector3(0.3f, 0.3f, 0.3f);
                    effect.PreferPerPixelLighting = true;

                }
                mesh.Draw();
            }
        }

        protected override Matrix GetWorld()
        {
            return Matrix.CreateScale(fScale) * Matrix.CreateRotationZ(rot.Z) * Matrix.CreateBillboard(pos, game.camera.cameraPosition, game.camera.tiltUp, null);
        }
    }
}
