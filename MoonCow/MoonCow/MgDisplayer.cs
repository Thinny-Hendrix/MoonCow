using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    class MgDisplayer:BasicModel
    {
        Game1 game;
        Minigame minigame;
        MgManager manager;
        MgScreen screen;
        Vector3 dir;
        Texture2D flash;
        Texture2D lines;
        bool visible;
        Vector3 goalScale;

        SpriteBatch sb;
        RenderTarget2D rTarg;

        Vector2 linePos;

        Color blue;

        public MgDisplayer(Minigame minigame, MgManager manager, Game1 game):base()
        {
            this.minigame = minigame;
            this.manager = manager;
            this.game = game;
            screen = minigame.screen;

            scale = new Vector3(0.25f);

            model = ModelLibrary.mgScreen;
            rot.X = MathHelper.Pi / 2.8f;

            sb = new SpriteBatch(game.GraphicsDevice);
            rTarg = new RenderTarget2D(game.GraphicsDevice, 64, 64);

            visible = false;

            linePos = Vector2.Zero;

            blue = new Color(179, 235, 255);
        }

        public void wake(Vector3 pos, Vector3 dir)
        {
            this.pos = pos;
            this.pos.Y += 2.6f;
            this.dir = dir;
            this.pos += dir * 5;
            rot.Y = (float)Math.Atan2(dir.X, dir.Z);
            visible = true;
        }

        public void shut()
        {
            visible = false;
        }

        public override void Update(GameTime gameTime)
        {
            linePos.Y += Utilities.deltaTime * manager.speed / 20;
            if (linePos.Y > 64)
                linePos.Y -= 64;
            game.GraphicsDevice.SetRenderTarget(rTarg);
            game.GraphicsDevice.Clear(blue * 0.5f);
            sb.Begin();
            //sb.Draw(TextureManager.mgLines, Vector2.Zero, Color.White);
            sb.Draw(TextureManager.mgLines, linePos, blue);
            sb.Draw(TextureManager.mgLines, new Vector2(0, linePos.Y-64), blue);
            sb.End();
            game.GraphicsDevice.SetRenderTarget(null);
        }

        public override void Draw(GraphicsDevice device, Camera camera)
        {
            if (visible)
            {
                Matrix[] transforms = new Matrix[model.Bones.Count];
                model.CopyAbsoluteBoneTransformsTo(transforms);

                foreach (ModelMesh mesh in model.Meshes)
                {
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        effect.World = mesh.ParentBone.Transform * GetWorld();
                        effect.View = camera.view;
                        effect.Projection = camera.projection;
                        effect.TextureEnabled = true;
                        effect.Alpha = 1;

                        if (mesh.Name.Equals("screen"))
                        {
                            effect.Texture = (Texture2D)screen.rTarg;
                            device.BlendState = BlendState.AlphaBlend;
                        }
                        else
                            effect.Texture = (Texture2D)rTarg;
                        effect.LightingEnabled = true;
                        effect.AmbientLightColor = Vector3.One;
                        effect.EmissiveColor = new Vector3(0.5f);
                        effect.PreferPerPixelLighting = true;

                    }
                    mesh.Draw();
                }
            }
        }
    }
}
