using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    class FireParticle:BasicModel
    {
        Game1 game;
        Vector3 startPos;
        Vector3 goalPos;
        Vector3 direction;
        float speed;
        float life;
        float alpha;
        float yFall;
        float scalef;
        float time;

        RenderTarget2D rTarg;
        SpriteBatch sb;

        public FireParticle(Vector3 pos, Game1 game, float dist)
        {
            this.model = TextureManager.dirSquare;
            this.game = game;
            startPos = pos;
            scalef = Utilities.nextFloat() + .25f;

            direction.X = (float)Utilities.random.NextDouble() * 2 - 1;
            direction.Y = (float)Utilities.random.NextDouble() * 1.5f - .5f;
            direction.Z = (float)Utilities.random.NextDouble() * 2 - 1;
            direction.Normalize();

            startPos += direction * Utilities.nextFloat() / 2;

            goalPos = pos + direction * dist;
            speed = 4.5f + Utilities.nextFloat();
            alpha = 1;

            rTarg = new RenderTarget2D(game.GraphicsDevice, 128, 128);
            sb = new SpriteBatch(game.GraphicsDevice);

            game.GraphicsDevice.SetRenderTarget(rTarg);
            sb.Begin();
            sb.Draw(TextureManager.pyroFlame, Vector2.Zero, Color.Lerp(Color.Red, Color.White, Utilities.nextFloat()));
            sb.End();
            game.GraphicsDevice.SetRenderTarget(null);
        }
        public override void Update(GameTime gameTime)
        {
            if (!Utilities.paused && !Utilities.softPaused)
            {
                if (speed > 0)
                {
                    speed *= Utilities.deltaTime * 60 * 0.9f;
                    if (speed < 0)
                        speed = 0;
                }
                yFall -= Utilities.deltaTime / 3;


                //pos = ship.pos+direction*distance;
                if(time != MathHelper.PiOver2)
                {
                    time += Utilities.deltaTime * MathHelper.Pi;

                    if (time > MathHelper.PiOver2)
                        time = MathHelper.PiOver2;

                    pos = Vector3.Lerp(startPos, goalPos, (float)Math.Sin(time));
                }
                pos.Y += Utilities.deltaTime;

                life += Utilities.deltaTime * MathHelper.Pi * 11;

                if (life > MathHelper.Pi * 8)
                {
                    if (life > MathHelper.Pi * 14)
                        alpha = (float)((Math.Cos(life)) + 1) / 2;
                    else
                        alpha = (float)((Math.Cos(life)) + 1) * 0.6f + 0.4f;
                }

                if (life > MathHelper.Pi * 15)
                {
                    Dispose();
                    game.modelManager.toDeleteModel(this);
                }
            }
        }

        public override void Dispose()
        {
            sb.Dispose();
            rTarg.Dispose();
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
                    effect.World = mesh.ParentBone.Transform * Matrix.CreateScale(scalef) * Matrix.CreateRotationY(MathHelper.Pi) * Matrix.CreateConstrainedBillboard(pos, camera.cameraPosition, Vector3.Up, null, Vector3.Up);
;
                    effect.View = camera.view;
                    effect.Projection = camera.projection;
                    effect.TextureEnabled = true;
                    effect.Texture = (Texture2D)rTarg;
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
    }
}
