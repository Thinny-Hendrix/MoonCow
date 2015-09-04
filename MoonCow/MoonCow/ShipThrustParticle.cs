using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace MoonCow
{
    class ShipThrustParticle:BasicModel
    {
        Game1 game;
        Texture2D tex;
        RenderTarget2D rTarg;
        SpriteBatch sb;
        Ship ship;
        float distance;
        ShipParticleSystem particles;
        float fScale;
        float alpha;
        Color col;
        int type;
        float time;
        Vector3 rotAxis;
        Vector3 direction;

        public ShipThrustParticle(Game1 game, Ship ship, ShipParticleSystem particles, int type):base()
        {
            if(type != 4)
                this.model = TextureManager.square;
            else
                this.model = TextureManager.flameSquare;

            this.game = game;
            this.ship = ship;
            this.particles = particles;
            this.type = type;
            time = 0;

            switch(type)
            { 
                default:
                    break;
                case 0:
                    tex = TextureManager.particle1small;
                    fScale = 0.015f;
                    distance = -0.35f;
                    break;

                case -1:
                case 1:
                    tex = TextureManager.particle1small;
                    sb = new SpriteBatch(game.GraphicsDevice);
                    rTarg = new RenderTarget2D(game.GraphicsDevice, 64, 64);
                    fScale = 0.02f;
                    rot.Z = Utilities.nextFloat() * MathHelper.PiOver2;
                    distance = -0.35f;
                    alpha = 1;
                    col = Color.White;
                    pos = ship.pos+ship.direction*distance;
                    break;
                case 2:
                case 3:
                    tex = TextureManager.smoke1;
                    sb = new SpriteBatch(game.GraphicsDevice);
                    rTarg = new RenderTarget2D(game.GraphicsDevice, 64, 64);
                    fScale = 0.02f;
                    rot.Z = Utilities.nextFloat() * MathHelper.PiOver2;
                    distance = -0.35f;
                    alpha = 1;
                    if (type == 2)
                        col = Color.Orange;
                    else
                        col = Color.Cyan;
                    pos = ship.pos+ship.direction*distance;
                    break;
                case 4:
                    tex = TextureManager.boostFlame;
                    rTarg = new RenderTarget2D(game.GraphicsDevice, 64, 128);
                    sb = new SpriteBatch(game.GraphicsDevice);
                    alpha = 1;
                    fScale = 0;
                    distance = -0.35f;
                    time = 0;
                    rot = new Vector3(-.3f, MathHelper.Pi, Utilities.nextFloat()*MathHelper.Pi*2);

                    rotAxis.X = (float)Math.Sin(rot.Y);
                    rotAxis.Z = (float)Math.Cos(rot.Y);
                    rotAxis.Y = (float)Math.Sin(rot.X);
                    rotAxis.Normalize();
                    break;
                case 5:
                    fScale = 0;
                    distance = -0.35f;
                    pos = ship.pos + ship.direction * distance;
                    rot.Z = (float)Utilities.random.NextDouble() * MathHelper.Pi * 2;
                    tex = TextureManager.particle3small;
                    model = TextureManager.square;
                    col = Color.Cyan;
                    sb = new SpriteBatch(game.GraphicsDevice);
                    rTarg = new RenderTarget2D(game.GraphicsDevice, 64, 64);
                    alpha = 1;
                    break;
            }
        }

        public override void Update(GameTime gameTime)
        {
            direction = ship.direction;
            direction.Y = (float)Math.Sin(ship.rot.X);

            switch(type)
            { 
                case -1:
                    distance -= Utilities.deltaTime * 2;
                    pos = ship.pos + direction * distance;
                    fScale -= Utilities.deltaTime * 0.1f;
                    if (fScale < 0.15f)
                        alpha -= Utilities.deltaTime * 3;

                    game.GraphicsDevice.SetRenderTarget(rTarg);
                    sb.Begin();
                    sb.Draw(tex, new Rectangle(0, 0, 64, 64), col * alpha);
                    sb.End();
                    game.GraphicsDevice.SetRenderTarget(null);

                    if (fScale <= 0)
                    {
                        particles.thrustToDelete.Add(this);
                    }
                    break;
                case 0:
                    pos = ship.pos+direction*distance;
                    rot.Z = Utilities.nextFloat();
                    break;
                case 1:
                    distance -= Utilities.deltaTime*2;
                    pos = ship.pos+direction*distance;
                    fScale -= Utilities.deltaTime*0.15f;
                    if (fScale < 0.15f)
                        alpha -= Utilities.deltaTime * 3;

                    game.GraphicsDevice.SetRenderTarget(rTarg);
                    sb.Begin();
                    sb.Draw(tex, new Rectangle(0,0,64,64), col*alpha);
                    sb.End();
                    game.GraphicsDevice.SetRenderTarget(null);

                    if(fScale <= 0)
                    {
                        particles.thrustToDelete.Add(this);
                    }
                    break;
                case 2:
                    distance -= Utilities.deltaTime * 1.5f;
                    pos = ship.pos + direction * distance;


                    fScale = (float)(Math.Cos(time) + 1) * 0.01f;

                    time += Utilities.deltaTime * MathHelper.Pi * 2.5f;

                    fScale += Utilities.deltaTime * 0.06f;
                    if (time > MathHelper.Pi*0.5f)
                        alpha -= Utilities.deltaTime * 4;

                    col = Color.Lerp(col, Color.Red, Utilities.deltaTime * 8);
                   


                    game.GraphicsDevice.SetRenderTarget(rTarg);
                    sb.Begin();
                    sb.Draw(tex, new Rectangle(0, 0, 64, 64), col * alpha);
                    sb.End();
                    game.GraphicsDevice.SetRenderTarget(null);

                    if (time > MathHelper.Pi*1)
                    {
                        particles.thrustToDelete.Add(this);
                    }
                    break;
                case 3:
                    distance -= Utilities.deltaTime * 2;
                    pos = ship.pos + direction * distance;
                    fScale += Utilities.deltaTime * 0.06f;
                    if (fScale < 0.15f)
                        alpha -= Utilities.deltaTime * 3;

                    col = Color.Lerp(col, Color.Blue, Utilities.deltaTime * 8);


                    game.GraphicsDevice.SetRenderTarget(rTarg);
                    sb.Begin();
                    sb.Draw(tex, new Rectangle(0, 0, 64, 64), col * alpha);
                    sb.End();
                    game.GraphicsDevice.SetRenderTarget(null);

                    if (alpha <= 0)
                    {
                        particles.thrustToDelete.Add(this);
                    }
                    break;
                case 4:
                    pos = ship.pos+direction*distance;
                    game.GraphicsDevice.SetRenderTarget(rTarg);
                    sb.Begin();
                    sb.Draw(tex, new Rectangle(0, 0, 64, 128), Color.White * alpha);
                    sb.End();
                    game.GraphicsDevice.SetRenderTarget(null);

                    fScale = ((float)Math.Cos(time) + 1) * .03f;

                    time  += Utilities.deltaTime*MathHelper.Pi*6;
                    if(time > MathHelper.Pi)
                    {
                        particles.thrustToDelete.Add(this);
                    }
                    break;
                case 5:
                    pos = ship.pos + ship.direction * distance;
                    fScale += Utilities.deltaTime*0.4f;

                    //if (fScale > 0.05f)
                    alpha -= Utilities.deltaTime*3;

                    game.GraphicsDevice.SetRenderTarget(rTarg);

                    col = Color.Lerp(col, Color.Blue, Utilities.deltaTime * 8);

                    sb.Begin();
                    sb.Draw(tex, Vector2.Zero, col*alpha);
                    sb.End();
                    game.GraphicsDevice.SetRenderTarget(null);



                    if (fScale > 0.06f)
                        game.modelManager.toDeleteModel(this);
                    break;
                default:
                    break;
            }

        }

        public override void Draw(GraphicsDevice device, Camera camera)
        {
            if (type == 0 && !Keyboard.GetState().IsKeyDown(Keys.W))
            { }
            else
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
                        if(type == 0)
                            effect.Texture = tex;
                        else
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
        }

        protected override Matrix GetWorld()
        {
            if (type != 4)
                return Matrix.CreateScale(fScale) * Matrix.CreateRotationZ(rot.Z) * Matrix.CreateBillboard(pos, game.camera.cameraPosition, game.camera.tiltUp, null);
            else
                return Matrix.CreateScale(fScale) * Matrix.CreateRotationY(rot.Y) * Matrix.CreateRotationZ(rot.Z) *  Matrix.CreateConstrainedBillboard(pos, game.camera.cameraPosition, Vector3.Up, null, null);

        }
    }
}
