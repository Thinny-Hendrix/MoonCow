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

        Vector3 direction;

        public ShipThrustParticle(Game1 game, Ship ship, ShipParticleSystem particles, int type):base()
        {
            this.model = TextureManager.square;
            this.game = game;
            this.ship = ship;
            this.particles = particles;
            this.type = type;

            switch(type)
            { 
                default:
                    break;
                case 0:
                    tex = TextureManager.particle1small;
                    fScale = 0.015f;
                    distance = -0.35f;
                    break;

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
            }
        }

        public override void Update(GameTime gameTime)
        {
            direction = ship.direction;
            direction.Y = (float)Math.Sin(ship.rot.X);

            switch(type)
            { 
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
                case 3:
                    distance -= Utilities.deltaTime * 2;
                    pos = ship.pos + direction * distance;
                    fScale += Utilities.deltaTime * 0.06f;
                    if (fScale < 0.15f)
                        alpha -= Utilities.deltaTime * 3;

                    if(type == 2)
                        col = Color.Lerp(col, Color.Red, Utilities.deltaTime * 8);
                    else
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
            return Matrix.CreateScale(fScale) * Matrix.CreateRotationZ(rot.Z) * Matrix.CreateBillboard(pos, game.camera.cameraPosition, game.camera.tiltUp, null);
        }
    }
}
