using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    class MoneyCollectParticle:BasicModel
    {
        Game1 game;
        Ship ship;
        Texture2D tex;
        Color col;
        RenderTarget2D rTarg;
        SpriteBatch sb;
        Vector3 offset;
        Vector3 direction;
        float distance;
        float speed;
        float life;
        float alpha;
        float yFall;
        float scalef;
        float zRot;

        public MoneyCollectParticle(Game1 game, Ship ship, Color col):base()
        {
            this.model = TextureManager.square;
            this.game = game;
            this.ship = ship;
            this.col = col;
            pos = ship.pos;
            scalef = Utilities.nextFloat()/100 + 0.005f;
            zRot = Utilities.nextFloat() * MathHelper.PiOver2;
            tex = TextureManager.spark1;
            sb = new SpriteBatch(game.GraphicsDevice);
            rTarg = new RenderTarget2D(game.GraphicsDevice, 64, 64);

            direction.X = (float)Utilities.random.NextDouble() * 2 - 1;
            direction.Y = (float)Utilities.random.NextDouble();
            direction.Z = (float)Utilities.random.NextDouble() * 2 - 1;
            direction.Normalize();
            speed = 4.5f + Utilities.nextFloat();
            alpha = 1;
        }

        public override void Update(GameTime gameTime)
        {
            distance += speed *Utilities.deltaTime;
            if (speed > 0)
            {
                speed *= Utilities.deltaTime * 60*0.9f;
                if (speed < 0)
                    speed = 0;
            }
            yFall -= Utilities.deltaTime/3;


            //pos = ship.pos+direction*distance;
            pos += direction * speed * Utilities.deltaTime;
            pos.Y -= Utilities.deltaTime/3;

            life += Utilities.deltaTime*MathHelper.Pi*11;

            if (life > MathHelper.Pi * 10)
            {
                if (life > MathHelper.Pi * 14)
                    alpha = (float)((Math.Cos(life)) + 1) / 2;
                else
                    alpha = (float)((Math.Cos(life)) + 1) *0.6f+0.4f;

            }

            if(life > MathHelper.Pi*15)
            {
                ship.particles.moneyToDelete.Add(this);
            }

            
            game.GraphicsDevice.SetRenderTarget(rTarg);
            sb.Begin();
            sb.Draw(tex, new Rectangle(0,0,64,64), col*alpha);
            sb.End();
            game.GraphicsDevice.SetRenderTarget(null);

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
                    effect.Alpha = 1;

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
            return Matrix.CreateScale(scalef) * Matrix.CreateRotationZ(zRot) * Matrix.CreateBillboard(pos, game.camera.cameraPosition, game.camera.tiltUp, null);
        }
    }
}
