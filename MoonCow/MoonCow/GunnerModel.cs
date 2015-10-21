using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SkinnedModel;

namespace MoonCow
{
    class GunnerModel : EnemyModel
    {
        Gunner gunner;
        AnimationPlayer animPlayer;
        AnimationClip activeClip;
        AnimationClip fly;
        AnimationClip trans;
        AnimationClip shoot;
        AnimationClip rel;
        AnimationClip idle;
        AnimationClip hit1;
        AnimationClip hit2;
        AnimationClip attack1;
        AnimationClip attack2;
        AnimationClip elec1;
        AnimationClip elec2;

        float knockSpin;


        public GunnerModel(Gunner enemy):base(enemy)
        {
            this.gunner = enemy;
            model = ModelLibrary.gunFly1;
            scale = new Vector3(.1f); 

            setAnims();

            activeClip = fly;
            animPlayer.StartClip(activeClip);

            SetupEffects();
        }

        protected void setAnims()
        {
            SkinningData skinningData = ModelLibrary.gunFly1.Tag as SkinningData;

            if (skinningData == null)
                throw new InvalidOperationException
                    ("This model does not contain a SkinningData tag.");

            // Create an animation player, and start decoding an animation clip.
            animPlayer = new AnimationPlayer(skinningData);

            fly = skinningData.AnimationClips["Take 001"];

            skinningData = ModelLibrary.gunTrans.Tag as SkinningData;
            trans = skinningData.AnimationClips["Take 001"];

            skinningData = ModelLibrary.gunShoot.Tag as SkinningData;
            shoot = skinningData.AnimationClips["Take 001"];

            skinningData = ModelLibrary.gunRel.Tag as SkinningData;
            rel = skinningData.AnimationClips["Take 001"];

            skinningData = ModelLibrary.gunIdle.Tag as SkinningData;
            idle = skinningData.AnimationClips["Take 001"];

            skinningData = ModelLibrary.gunHit1.Tag as SkinningData;
            hit1 = skinningData.AnimationClips["Take 001"];

            skinningData = ModelLibrary.gunHit2.Tag as SkinningData;
            hit2 = skinningData.AnimationClips["Take 001"];

            skinningData = ModelLibrary.gunAttack1.Tag as SkinningData;
            attack1 = skinningData.AnimationClips["Take 001"];

            skinningData = ModelLibrary.gunAttack2.Tag as SkinningData;
            attack2 = skinningData.AnimationClips["Take 001"];

            skinningData = ModelLibrary.gunElec1.Tag as SkinningData;
            elec1 = skinningData.AnimationClips["Take 001"];

            skinningData = ModelLibrary.gunElec2.Tag as SkinningData;
            elec2 = skinningData.AnimationClips["Take 001"];
        }

        public override void changeAnim(int i)
        {
            switch(i)
            {
                default:
                    activeClip = fly;
                    break;
                case 1:
                    activeClip = trans;
                    break;
                case 2:
                    activeClip = shoot;
                    break;
                case 3:
                    activeClip = rel;
                    break;
                case 4:
                    activeClip = idle;
                    break;
                case 5:
                    activeClip = attack1;
                    break;
                case 6:
                    activeClip = attack2;
                    break;
                case 7:
                    activeClip = hit1;
                    break;
                case 8:
                    activeClip = hit2;
                    break;
                case 9:
                    activeClip = elec1;
                    break;
                case 10:
                    activeClip = elec2;
                    break;
                case 11:
                    activeClip = trans;
                    break;
            }

            if(i != 11)
            {
                animSpeed = 1;
            }
            else
            {
                animSpeed = -1;
            }

            activeIndex = i;

            animPlayer.StartClip(activeClip);
        }

        public override void Update(GameTime gameTime)
        {
            pos = enemy.pos;
            pos.Y -= 0.6f;
            rot.Y = (float)Math.Atan2(gunner.modelDir.X, gunner.modelDir.Z);
            rot.Y -= MathHelper.PiOver2;

            /*if(swarmer.state == Swarmer.State.hitByDrill)
            {
                knockSpin -= Utilities.deltaTime * MathHelper.Pi * 3;
                if (knockSpin < -MathHelper.Pi * 2)
                    knockSpin += MathHelper.Pi * 2;
            }*/

            if(!Utilities.paused && !Utilities.softPaused)
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
                    effect.Texture = TextureManager.gunTex;
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
