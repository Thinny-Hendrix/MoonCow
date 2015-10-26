using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    class Forcefield:BasicModel
    {
        protected RenderTarget2D rTarg;
        protected SpriteBatch sb;
        protected Game1 game;
        public List<SpriteParticle> particles;
        public List<SpriteParticle> pToDelete;
        OOBB col;
        protected Vector2 linePos;
        int type;

        public Forcefield(Game1 game, Vector3 pos, int type)
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
                col = new OOBB(pos, Vector3.Forward, 1, 30);
            }
            else
            {
                col = new OOBB(pos, Vector3.Forward, 30, 1);
            }
        }

        void addParticle()
        {
            float partPos = 0;
            if(type == 1)
            {
                partPos = pos.Z - game.ship.pos.Z;
            }
            else
            {
                partPos = pos.X - game.ship.pos.X;
            }
            particles.Add(new SpRing(new Vector2(partPos, 600), 1, pToDelete, 0));

        }

        public override void Update(GameTime gameTime)
        {
            if(game.ship.circleCol.checkOOBB(col))
            {
                //addParticle();
            }
            foreach(SpriteParticle p in particles)
            {
                p.Update();
            }
            foreach(SpriteParticle p in pToDelete)
            {
                particles.Remove(p);
            }
            pToDelete.Clear();

            linePos.Y -= Utilities.deltaTime * 16;
            if (linePos.Y < -16)
                linePos.Y += 16;


            game.GraphicsDevice.SetRenderTarget(rTarg);
            sb.Begin();
            sb.Draw(TextureManager.pureWhite, new Rectangle(0, 0, 1024, 1024), Color.Aqua * 0.2f);
            sb.Draw(TextureManager.forceLines, linePos, Color.Aqua * 0.2f);
            sb.Draw(TextureManager.forceLines, linePos + new Vector2(0,1024), Color.Aqua * 0.5f);
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
                    effect.Alpha = 1;

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
