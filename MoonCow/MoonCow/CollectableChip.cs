using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    class CollectableChip:Collectable
    {
        public CollectableChip(Vector3 pos, Game1 game):base()
        {
            this.pos = pos;
            this.pos.Y = 4.5f;
            this.game = game;
            scale = new Vector3(0.5f);
            col = new CircleCollider(pos, 1f);
            rot.X = MathHelper.PiOver4 / 3;
            model = ModelLibrary.chip;
            setEffect();
        }

        public override void Update(GameTime gameTime)
        {
            if (!Utilities.softPaused && !Utilities.paused)
            {
                rot.Y += Utilities.deltaTime * MathHelper.PiOver2;
                if (rot.Y > MathHelper.Pi * 2)
                    rot.Y -= MathHelper.Pi * 2;

                checkCollision();
            }
        }

        public override void setEffect()
        {
            Matrix[] transforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.TextureEnabled = true;
                    effect.Alpha = 1;
                    effect.Texture = TextureManager.chip;

                    //trying to get lighting to work, but so far the model just shows up as pure black - it was exported with a green blinn shader
                    //effect.EnableDefaultLighting(); //did not work
                    effect.LightingEnabled = true;

                    effect.DirectionalLight0.DiffuseColor = new Vector3(0.3f, 0.3f, 0.3f); //RGB is treated as a vector3 with xyz being rgb - so vector3.one is white
                    effect.DirectionalLight0.Direction = new Vector3(0, -1, 1);
                    effect.DirectionalLight0.SpecularColor = Vector3.One;
                    effect.AmbientLightColor = new Vector3(0.3f, 0.3f, 0.3f);
                    effect.EmissiveColor = new Vector3(0.3f, 0.3f, 0.3f);
                    effect.PreferPerPixelLighting = true;

                }
                mesh.Draw();
            }
        }
            

        public override void Draw(GraphicsDevice device, Camera camera)
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
                }
                mesh.Draw();
            }
        }

        public override void onCollect()
        {
            game.hud.hudCollectable.gotCard();

            game.modelManager.addEffect(new LaserHitEffect(game, pos, Color.LimeGreen));
            game.modelManager.addEffect(new LaserHitEffect(game, pos, Color.PaleGreen, 3,BlendState.Additive));
            game.modelManager.addEffect(new ImpactParticleModel(game, pos));
            for (int i = 0; i < 20; i++ )
            {
                game.modelManager.addEffect(new DirLineParticle(pos, game));
            }
            base.onCollect();
        }
    }
}
