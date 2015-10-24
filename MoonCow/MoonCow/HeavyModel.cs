using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SkinnedModel;

namespace MoonCow
{
    class HeavyModel:EnemyModel
    {
        Heavy heavy;
        
        AnimationPlayer animPlayer;
        AnimationClip activeClip;
        AnimationClip fly;
        AnimationClip attack;
        AnimationClip hit;
        AnimationClip elec;

        float knockSpin;


        public HeavyModel(Heavy enemy):base(enemy)
        {
            this.heavy = enemy;
            model = ModelLibrary.hevFly;
            scale = new Vector3(.2f);

            setAnims();

            activeClip = fly;
            animPlayer.StartClip(activeClip);

            SetupEffects();
        }

        protected void setAnims()
        {
            SkinningData skinningData = ModelLibrary.hevFly.Tag as SkinningData;

            if (skinningData == null)
                throw new InvalidOperationException
                    ("This model does not contain a SkinningData tag.");

            // Create an animation player, and start decoding an animation clip.
            animPlayer = new AnimationPlayer(skinningData);

            fly = skinningData.AnimationClips["Take 001"];

            skinningData = ModelLibrary.hevAttack.Tag as SkinningData;
            attack = skinningData.AnimationClips["Take 001"];

            skinningData = ModelLibrary.hevHit.Tag as SkinningData;
            hit = skinningData.AnimationClips["Take 001"];

            skinningData = ModelLibrary.hevElec.Tag as SkinningData;
            elec = skinningData.AnimationClips["Take 001"];
        }

        public override void changeAnim(int i)
        {
            switch(i)
            {
                default:
                    activeClip = fly;
                    break;
                case 1:
                    activeClip = attack;
                    break;
                case 2:
                    activeClip = hit;
                    break;
                case 3:
                    activeClip = elec;
                    break;
            }
            activeIndex = i;
            animPlayer.StartClip(activeClip);
        }

        public override void Update(GameTime gameTime)
        {
            pos = enemy.pos;
            pos.Y -= 0.7f;
            //rot = enemy.rot;
            rot.Y = (float)Math.Atan2(enemy.direction.X, enemy.direction.Z);
            //rot.Y -= MathHelper.Pi;

            /*if(swarmer.state == Swarmer.State.hitByDrill)
            {
                knockSpin -= Utilities.deltaTime * MathHelper.Pi * 3;
                if (knockSpin < -MathHelper.Pi * 2)
                    knockSpin += MathHelper.Pi * 2;
            }*/

            if (!Utilities.paused && !Utilities.softPaused)
                animPlayer.Update(gameTime.ElapsedGameTime, true, GetWorld());
                //rot = Vector3.Transform(ship.direction, Matrix.CreateFromAxisAngle(Vector3.Up, ship.rot.Y));
        }

        protected override Matrix GetWorld()
        {
            return base.GetWorld();
        }

        public override void Dispose()
        {
        }

        private void SetupEffects()
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (SkinnedEffect effect in mesh.Effects)
                {
                    //effect.World = mesh.ParentBone.Transform * GetWorld();
                    effect.Texture = TextureManager.hevTex;
                    effect.Alpha = 1;
                    effect.WeightsPerVertex = 1;
                    //effect.LightingEnabled = true;

                    if (mesh.Name.Contains("glow"))
                    {
                        effect.AmbientLightColor = new Vector3(0.8f);
                    }
                    else
                    {
                        effect.DirectionalLight0.DiffuseColor = new Vector3(0.2f, 0.2f, 0.2f); //RGB is treated as a vector3 with xyz being rgb - so vector3.one is white
                        effect.DirectionalLight0.Direction = new Vector3(0, -1, 1);
                        effect.AmbientLightColor = new Vector3(0.7f, 0.7f, 0.7f);
                        effect.SpecularColor = new Vector3(0.3f);
                        effect.EmissiveColor = new Vector3(.4f, .4f, .4f);
                    }
                    effect.PreferPerPixelLighting = true;
                }
            }
        }

        public override void Draw(GraphicsDevice device, Camera camera)
        {

            Matrix[] transforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transforms);

            Matrix[] bones = animPlayer.GetSkinTransforms();


            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (SkinnedEffect effect in mesh.Effects)
                {
                    effect.SetBoneTransforms(bones);

                    //effect.World = mesh.ParentBone.Transform * GetWorld();
                    effect.View = camera.view;
                    effect.Projection = camera.projection;
                }
                mesh.Draw();
            }
        }
    }
}
