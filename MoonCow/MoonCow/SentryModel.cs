using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace MoonCow
{
    class SentryModel:BasicModel
    {
        Sentry sentry;
        Game1 game;
        Texture2D bodyTex;
        Texture2D visorTex;
        ModelBone ant;
        ModelBone topCap;
        ModelBone visor;
        ModelBone midCap;
        ModelBone canRot;
        ModelBone cannon;
        ModelBone botCap;
        float visorRot;
        float capRot;
        float cannonRot;
        Vector3 topOffset;
        bool waking;
        Vector3 shakeOffset;
        float tiltRot;
        float spinRot;

        float shakeSize;
        float shakeAmount;
        Vector3 shakeDir;
        float shakeTime;
        bool shaking;

        public SentryModel(Sentry sentry, Game1 game):base()
        {
            this.sentry = sentry;
            this.game = game;

            pos = sentry.pos;
            scale = new Vector3(0.2f);

            model = ModelLibrary.sentry;
            bodyTex = TextureManager.sentryBod;
            visorTex = TextureManager.sEye3;

            ant = model.Bones["anten"];
            topCap = model.Bones["topCap"];
            visor = model.Bones["visor"];
            midCap = model.Bones["midCap"];
            canRot = model.Bones["cannonRound"];
            cannon = model.Bones["cannon"];
            botCap = model.Bones["botCap"];

            topOffset = Vector3.Zero;
            tiltRot = 0;
            spinRot = 0;
        }

        public override void Update(GameTime gameTime)
        {
            if (!Utilities.paused && !Utilities.softPaused)
            {
                if (sentry.state == Sentry.State.knockBack)
                {
                    pos = sentry.pos;
                    tiltRot = -MathHelper.PiOver4/2;
                    rot.Y += Utilities.deltaTime * MathHelper.Pi * 4;
                }
                visorRot = (float)Math.Atan2(sentry.eyeDir.X, sentry.eyeDir.Z);
                cannonRot = (float)Math.Atan2(sentry.cannonDir.X, sentry.cannonDir.Z);

                updateEyes();

                topOffset.Y = 0;

                if (waking)
                {
                    topOffset.Y = ((-(float)Math.Cos(sentry.shockTime * MathHelper.Pi * 2) - 1) + 2) * 1.5f;
                }

                if (shaking)
                {
                    updateShake();
                    shakeOffset = shakeDir * shakeAmount;
                }

            }
        }

        void updateShake()
        {
            shakeAmount = (float)Math.Sin(shakeTime) * shakeSize;
            shakeSize = MathHelper.Lerp(shakeSize, 0, Utilities.deltaTime * 8f);
            shakeTime += MathHelper.Pi * Utilities.deltaTime*4;
            if (shakeSize <= 0.01f)
            {
                shakeSize = 0;
                shaking = false;
            }
        }

        public void hit(Vector3 dir)
        {
            shaking = true;
            shakeTime = 0;
            shakeSize = 6;
            shakeDir = dir;
        }

        public void wake()
        {
            waking = true;
            topOffset.Y = 0;
        }

        void updateEyes()
        {
            //idle, wake, active, fail, success, agro, hit, knockback
            switch((int)sentry.state)
            {
                default:
                    visorTex = TextureManager.sEye0;
                    break;
                case 1:
                    visorTex = TextureManager.sEye1;
                    break;
                case 3:
                    visorTex = TextureManager.sEye3;
                    break;
                case 4:
                    visorTex = TextureManager.sEye2;
                    break;
                case 5:
                    visorTex = TextureManager.sEye5;
                    break;
                case 6:
                    if(sentry.shockTime < 0.4f)
                        visorTex = TextureManager.sEye1;
                    else
                        visorTex = TextureManager.sEye4;
                    break;
                case 7:
                    visorTex = TextureManager.sEye6;
                    break;


            }
        }

        protected override Matrix GetWorld()
        {
            if (sentry.state == Sentry.State.knockBack)
                return Matrix.CreateFromAxisAngle(Vector3.Cross(sentry.knockDir, Vector3.Up), tiltRot) * base.GetWorld();
            else
                return base.GetWorld();
        }

        public override void Draw(GraphicsDevice device, Camera camera)
        {
            Matrix[] transforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    if (mesh.Name.Equals("anten") || mesh.Name.Equals("glowBall"))
                        effect.World = Matrix.CreateRotationY(visorRot) * Matrix.CreateTranslation(-shakeOffset*0.8f) * Matrix.CreateTranslation(ant.Transform.Translation - topCap.Transform.Translation) * Matrix.CreateFromAxisAngle(Vector3.Cross(shakeDir, Vector3.Up), shakeAmount * 0.1f)
                            * Matrix.CreateTranslation(topOffset) * topCap.Transform * GetWorld();

                    else if (mesh.Name.Equals("topCap"))
                        effect.World = Matrix.CreateFromAxisAngle(Vector3.Cross(shakeDir, Vector3.Up), shakeAmount * 0.1f) * Matrix.CreateTranslation(-shakeOffset) * Matrix.CreateTranslation(topOffset) * topCap.Transform * GetWorld();

                    else if(mesh.Name.Equals("visor") || mesh.Name.Equals("eyeRound"))
                        effect.World = Matrix.CreateScale(1, 1 + topOffset.Y / 3, 1) * Matrix.CreateRotationY(visorRot) * Matrix.CreateFromAxisAngle(Vector3.Cross(shakeDir, Vector3.Up), shakeAmount * 0.05f) *
                            Matrix.CreateTranslation(topOffset * 0.5f) * Matrix.CreateTranslation(-shakeOffset * 0) * visor.Transform * GetWorld();

                    else if (mesh.Name.Equals("midCap"))
                        effect.World = Matrix.CreateTranslation(shakeOffset*0.2f) * midCap.Transform * GetWorld();

                    else if (mesh.Name.Equals("cannonRound"))
                        effect.World = Matrix.CreateScale(1, 1 + topOffset.Y / 6, 1) * Matrix.CreateRotationY(cannonRot) * Matrix.CreateFromAxisAngle(Vector3.Cross(shakeDir, Vector3.Up), shakeAmount * -0.05f) * Matrix.CreateTranslation(-topOffset * 0.25f) * canRot.Transform * GetWorld();

                    else if (mesh.Name.Equals("cannon") || mesh.Name.Equals("glowCannon"))
                        effect.World = Matrix.CreateRotationY(cannonRot) * Matrix.CreateFromAxisAngle(Vector3.Cross(shakeDir, Vector3.Up), shakeAmount * -0.05f) * Matrix.CreateTranslation(-topOffset * 0.25f) * cannon.Transform * GetWorld();

                    else//botcap
                        effect.World = Matrix.CreateFromAxisAngle(Vector3.Cross(shakeDir, Vector3.Up), shakeAmount * -0.1f) * Matrix.CreateTranslation(-shakeOffset) * Matrix.CreateTranslation(-topOffset* 0.5f) * botCap.Transform * GetWorld();

                    effect.View = camera.view;
                    effect.Projection = camera.projection;
                    effect.TextureEnabled = true;
                    effect.Alpha = 1;

                    effect.LightingEnabled = true;

                    if (mesh.Name.Contains("visor"))
                        effect.Texture = visorTex;
                    else
                        effect.Texture = bodyTex;

                    if(mesh.Name.Contains("glow") || mesh.Name.Contains("visor"))
                    {
                        effect.AmbientLightColor = Vector3.One;
                        effect.EmissiveColor = new Vector3(0.5f);
                    }
                    else
                    {
                        effect.DirectionalLight0.DiffuseColor = new Vector3(0.2f, 0.2f, 0.2f); //RGB is treated as a vector3 with xyz being rgb - so vector3.one is white
                        effect.DirectionalLight0.Direction = new Vector3(0, -1, 1);
                        effect.AmbientLightColor = new Vector3(0.7f, 0.7f, 0.7f);
                        effect.SpecularColor = new Vector3(0.3f);
                        effect.EmissiveColor = new Vector3(.4f, .4f, .4f);
                        /*
                        effect.DirectionalLight0.DiffuseColor = new Vector3(0.3f, 0.3f, 0.3f); //RGB is treated as a vector3 with xyz being rgb - so vector3.one is white
                        effect.DirectionalLight0.Direction = new Vector3(0, -1, 1);
                        effect.DirectionalLight0.SpecularColor = Vector3.One;
                        effect.AmbientLightColor = new Vector3(0.5f, 0.5f, 0.5f);
                        effect.EmissiveColor = new Vector3(0.3f, 0.3f, 0.3f);*/
                    }

                    effect.PreferPerPixelLighting = true;

                }
                mesh.Draw();
            }
        }
    }
}
