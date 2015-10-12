using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    public class DrillDome:BasicModel
    {
        Ship ship;
        WeaponDrill drill;
        RenderTarget2D rTarg;
        SpriteBatch sb;
        List<SpriteParticle> particles;
        List<SpriteParticle> toDelete;
        Game1 game;
        public bool active;
        float timer;

        public DrillDome(Game1 game, WeaponDrill drill)
        {
            this.game = game;
            model = ModelLibrary.drillDome;
            this.drill = drill;
            scale = new Vector3(0.05f, 0.08f, 0.06f);
            rot = Vector3.Zero;

            particles = new List<SpriteParticle>();
            toDelete = new List<SpriteParticle>();

            rTarg = new RenderTarget2D(game.GraphicsDevice, 512, 256);
            sb = new SpriteBatch(game.GraphicsDevice);
        }

        public void setShip(Ship ship)
        {
            this.ship = ship;
            pos = ship.pos;
        }

        public void activate()
        {
            active = true;
            timer = 0;
        }

        public void disable()
        {
            active = false;
        }

        public override void Update(GameTime gameTime)
        {
            pos = ship.pos;
            rot = ship.rot;

            if (!Utilities.paused && !Utilities.softPaused)
            {
                if (active)
                {
                    timer -= Utilities.deltaTime;
                    if (timer <= 0)
                    {
                        particles.Add(new DrillLineParticle(toDelete));
                        timer = 0.1f;
                    }
                }

                foreach (SpriteParticle p in particles)
                    p.Update();
                foreach (SpriteParticle p in toDelete)
                    particles.Remove(p);
                toDelete.Clear();

                drawTex();
            }
        }

        void drawTex()
        {
            game.GraphicsDevice.SetRenderTarget(rTarg);
            sb.Begin();
            sb.Draw(TextureManager.pureWhite, new Rectangle(0, 0, 512, 256), Color.Black);
            foreach (SpriteParticle p in particles)
            {
                p.Draw(sb);
            }
            sb.Draw(TextureManager.drillMask, Vector2.Zero, Color.White);
            sb.End();
            game.GraphicsDevice.SetRenderTarget(null);
        }

        public override void Draw(GraphicsDevice device, Camera camera)
        {
            Matrix[] transforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transforms);
            game.GraphicsDevice.BlendState = BlendState.Additive;
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

                    //effect.EnableDefaultLighting(); //did not work
                    effect.LightingEnabled = true;
                    effect.AmbientLightColor = new Vector3(1);
                    //effect.EmissiveColor = Vector3.One;
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
