using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    class ForcefieldDrill:Forcefield
    {
        /*
        protected RenderTarget2D rTarg;
        protected SpriteBatch sb;
        protected Game1 game;
        public List<SpriteParticle> particles;
        public List<SpriteParticle> pToDelete;
        protected Vector2 linePos;
        int type;*/
        bool fading;
        float time;
        float alpha1;
        float alpha2;
        public ForcefieldDrill(Game1 game, Vector3 pos, int type)
        {
            this.pos = pos;
            this.pos.Y = -1;
            this.game = game;
            rTarg = new RenderTarget2D(game.GraphicsDevice, 1024, 1024);
            sb = new SpriteBatch(game.GraphicsDevice);
            model = ModelLibrary.forceField;
            particles = new List<SpriteParticle>();
            pToDelete = new List<SpriteParticle>();
            this.type = type;
            if(type == 1)
            {
                rot.Y += MathHelper.PiOver2;
            }
            alpha1 = 1;
            
        }
        public void disable()
        {
            fading = true;
            time = 0;
        }

        public override void Update(GameTime gameTime)
        {
            pToDelete.Clear();

            linePos.Y -= Utilities.deltaTime * 16;
            if (linePos.Y < -16)
                linePos.Y += 16;

            if(fading)
            {
                if(time < 1)
                {
                    time += Utilities.deltaTime;
                    if(time >= 1)
                    {
                        fading = false;
                        time = 1;
                    }
                }
                alpha1 = MathHelper.SmoothStep(1,0,time);
            }

            game.GraphicsDevice.SetRenderTarget(rTarg);
            sb.Begin();
            sb.Draw(TextureManager.pureWhite, new Rectangle(0, 0, 1024, 1024), Color.Aqua * 0.2f);
            sb.Draw(TextureManager.forceLines, linePos, Color.Aqua * 0.2f);
            sb.Draw(TextureManager.forceLines, linePos + new Vector2(0,1024), Color.Aqua * 0.5f);
            sb.Draw(TextureManager.drillHolo, new Vector2(256,64), Color.Aqua * 0.5f);
            sb.Draw(TextureManager.forceVing, Vector2.Zero, Color.Aqua * 0.5f);
            foreach(SpriteParticle p in particles)
            {
                p.Draw(sb);
            }
            sb.End();
            game.GraphicsDevice.SetRenderTarget(null);
        }



        public override void Dispose()
        {
            sb.Dispose();
            rTarg.Dispose();
        }

        public override void Draw(GraphicsDevice device, Camera camera)
        {
            Matrix[] transforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transforms);

            device.BlendState = BlendState.Additive;
            device.DepthStencilState = DepthStencilState.DepthRead;

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {

                    effect.World = mesh.ParentBone.Transform * GetWorld();



                    effect.View = camera.view;
                    effect.Projection = camera.projection;
                    effect.TextureEnabled = true;
                    effect.Texture = (Texture2D)rTarg;
                    effect.Alpha = alpha1;

                    //effect.EnableDefaultLighting(); //did not work
                    effect.LightingEnabled = true;
                    effect.AmbientLightColor = Vector3.One;
                    effect.EmissiveColor = new Vector3(.2f, .2f, .2f);
                    effect.PreferPerPixelLighting = true;

                }
                mesh.Draw();
            }
        }
    }
}
