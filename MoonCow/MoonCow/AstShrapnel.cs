using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    public class AstShrapnel:BasicModel
    {
        Game1 game;
        Vector3 dir;
        float speed;
        float fScale;
        float initScale;
        float time;
        Vector3 rotDir;
        public AstShrapnel(Vector3 pos, float scale, Vector3 dir, Game1 game)
        {
            model = ModelLibrary.ast1;
            this.pos = pos;
            this.scale = new Vector3(scale);
            this.game = game;
            fScale = scale;
            initScale = scale;
            speed = 2;
            time = 0;

            Vector3 tempDir = new Vector3(Utilities.nextFloat() * 2 - 1, Utilities.nextFloat() * 2 - 1, Utilities.nextFloat() * 2 - 1);
            this.dir = tempDir + dir;
            this.dir.Normalize();

            this.pos += this.dir * scale * 8;
            rotDir = this.dir;
        }

        public AstShrapnel(Vector3 pos, float scale, Game1 game)
        {
            model = ModelLibrary.ast1;
            this.pos = pos;
            this.scale = new Vector3(scale);
            this.game = game;
            fScale = scale;
            initScale = scale;
            speed = 2;
            time = 0;

            rotDir = new Vector3(Utilities.nextFloat() * 2 - 1, Utilities.nextFloat() * 2 - 1, Utilities.nextFloat() * 2 - 1);
            rotDir.Normalize();

            this.dir = Vector3.Zero;
        }

        public override void Update(GameTime gameTime)
        {
            if (!Utilities.paused && !Utilities.softPaused)
            {
                pos += dir * speed * Utilities.deltaTime;
                rot += rotDir * 5 * Utilities.deltaTime;

                time += Utilities.deltaTime;

                fScale = MathHelper.Lerp(initScale, 0, time);

                if (fScale <= 0)
                {
                    game.modelManager.toDeleteObject(this);
                }
            }
        }

        protected override Matrix GetWorld()
        {
            return Matrix.CreateScale(fScale) * Matrix.CreateFromYawPitchRoll(rot.Y, rot.X, rot.Z) * Matrix.CreateTranslation(pos);
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
                    effect.TextureEnabled = true;
                    effect.Alpha = 1;

                    effect.Texture = TextureManager.ast1;

                    //trying to get lighting to work, but so far the model just shows up as pure black - it was exported with a green blinn shader
                    //effect.EnableDefaultLighting(); //did not work
                    effect.LightingEnabled = true;

                    effect.DirectionalLight0.DiffuseColor = new Vector3(0.5f, 0.3f, 0.5f); //RGB is treated as a vector3 with xyz being rgb - so vector3.one is white
                    effect.DirectionalLight0.Direction = new Vector3(0, -1, 1);
                    effect.DirectionalLight0.SpecularColor = Vector3.One;
                    effect.AmbientLightColor = new Vector3(0.4f, 0.4f, 0.4f);
                    effect.EmissiveColor = new Vector3(0.4f, 0.4f, 0.4f);
                    effect.PreferPerPixelLighting = true;

                }
                mesh.Draw();
            }
        }
    }
}
