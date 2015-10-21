using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SkinnedModel;

namespace MoonCow
{
    class SwarmerModel:EnemyModel
    {
        AnimationPlayer animPlayer;
        AnimationClip activeClip;
        AnimationClip fly1;
        AnimationClip fly2;
        AnimationClip attack;
        AnimationClip notice;
        AnimationClip idle;
        AnimationClip hit;
        AnimationClip elec;
        int activeIndex;

        Swarmer swarmer;
        float knockSpin;


        public SwarmerModel(Swarmer enemy):base(enemy)
        {
            this.swarmer = enemy;
            model = ModelLibrary.swaFly1;
            scale = new Vector3(.07f);

            setAnims();

            // Look up our custom skinning information.
            SkinningData skinningData = model.Tag as SkinningData;

            if (skinningData == null)
                throw new InvalidOperationException
                    ("This model does not contain a SkinningData tag.");

            // Create an animation player, and start decoding an animation clip.
            animPlayer = new AnimationPlayer(skinningData);

            AnimationClip clip = skinningData.AnimationClips["Take 001"];

            activeClip = notice;
            animPlayer.StartClip(activeClip);

            SetupEffects();
        }

        protected void setAnims()
        {
            SkinningData skinningData = ModelLibrary.swaFly1.Tag as SkinningData;

            if (skinningData == null)
                throw new InvalidOperationException
                    ("This model does not contain a SkinningData tag.");

            // Create an animation player, and start decoding an animation clip.
            animPlayer = new AnimationPlayer(skinningData);

            fly1 = skinningData.AnimationClips["Take 001"];

            skinningData = ModelLibrary.swaNotice.Tag as SkinningData;
            notice = skinningData.AnimationClips["Take 001"];

            skinningData = ModelLibrary.swaFly2.Tag as SkinningData;
            fly2 = skinningData.AnimationClips["Take 001"];

            skinningData = ModelLibrary.swaIdle.Tag as SkinningData;
            idle = skinningData.AnimationClips["Take 001"];

            skinningData = ModelLibrary.swaElec.Tag as SkinningData;
            elec = skinningData.AnimationClips["Take 001"];

            skinningData = ModelLibrary.swaAttack.Tag as SkinningData;
            attack = skinningData.AnimationClips["Take 001"];

            skinningData = ModelLibrary.swaHit.Tag as SkinningData;
            hit = skinningData.AnimationClips["Take 001"];

        }

        public override void changeAnim(int i)
        {
            switch(i)
            {
                default:
                    activeClip = fly1;
                    break;
                case 1:
                    activeClip = notice;
                    break;
                case 2:
                    activeClip = fly2;
                    break;
                case 3:
                    activeClip = attack;
                    break;
                case 4:
                    activeClip = hit;
                    break;
                case 5:
                    activeClip = elec;
                    break;
                case 6:
                    activeClip = idle;
                    break;
            }

            animPlayer.StartClip(activeClip);
        }

        public override void Update(GameTime gameTime)
        {
            pos = enemy.pos;
            //pos.Y -= 0.7f;
            rot = enemy.rot;

            if(swarmer.state == Swarmer.State.hitByDrill)
            {
                knockSpin -= Utilities.deltaTime * MathHelper.Pi * 3;
                if (knockSpin < -MathHelper.Pi * 2)
                    knockSpin += MathHelper.Pi * 2;
            }

            rot.Y -= MathHelper.Pi;

            animPlayer.Update(gameTime.ElapsedGameTime, true, GetWorld());
                //rot = Vector3.Transform(ship.direction, Matrix.CreateFromAxisAngle(Vector3.Up, ship.rot.Y));
        }

        protected override Matrix GetWorld()
        {
            if (swarmer.state == Swarmer.State.hitByDrill)
                return Matrix.CreateFromAxisAngle(Vector3.Cross(enemy.knockDir, Vector3.Up), knockSpin) * base.GetWorld();
            else
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
                    effect.Texture = TextureManager.swarmerTex;
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
