using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    class OreGib:MoneyGib
    {
        float initDist;
        float coreDist;
        Vector3 initScale;
        Vector3 smallScale;
        public OreGib(float value, MoneyManager moneyManager, Ship ship, Vector3 pos, Game1 game, int type) : base()
        {
            this.value = value;
            this.model = ModelLibrary.oreGib1;
            this.pos = pos;
            this.moneyManager = moneyManager;
            this.ship = ship;
            this.game = game;
            scale = new Vector3(.05f);
            initScale = scale;
            smallScale = new Vector3(.01f);

            setColor(type);

            glow = new MoneyGibGlow(this, game, 0.05f);
            game.modelManager.addEffect(glow);


            initDirection.X = (float)Utilities.random.NextDouble()*2-1;
            initDirection.Y = (float)Utilities.random.NextDouble()*2-1;
            initDirection.Z = (float)Utilities.random.NextDouble()*2-1;
            initDirection.Normalize();

            currentDirection = initDirection;

            rot = currentDirection;

            speed = Utilities.nextFloat()*5+17;
            changeDirectionSpeed = Utilities.nextFloat() + 4.5f;

            col = new CircleCollider(pos, 0.05f);
            collected = false;
            coreDist = 1;
            initDist = col.distFrom(ship.pos);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            coreDist = ((float)-Math.Cos(1 - (col.distFrom(ship.pos) / initDist)))+1.2f;
            if (col.distFrom(ship.pos) < initDist)
                scale = Vector3.Lerp(smallScale, initScale, col.distFrom(ship.pos) / initDist);
            else
                scale = initScale;
        }

        void setColor(int i)
        {
            color = Color.Gold;
        }

        public override void Draw(GraphicsDevice device, Camera camera)
        {
            Matrix[] transforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = Matrix.CreateFromYawPitchRoll(rot.Y, rot.X, rot.Z) * (mesh.ParentBone.Transform * coreDist) * GetWorld();
                    effect.View = camera.view;
                    effect.Projection = camera.projection;
                    effect.TextureEnabled = true;
                    effect.Texture = TextureManager.gibOre1;
                    effect.Alpha = 1;

                    //trying to get lighting to work, but so far the model just shows up as pure black - it was exported with a green blinn shader
                    //effect.EnableDefaultLighting(); //did not work
                    effect.LightingEnabled = true;

                    effect.DirectionalLight0.DiffuseColor = new Vector3(0.3f, 0.3f, 0.3f); //RGB is treated as a vector3 with xyz being rgb - so vector3.one is white
                    effect.DirectionalLight0.Direction = new Vector3(0, -1, 1);
                    effect.DirectionalLight0.SpecularColor = Vector3.One;
                    effect.AmbientLightColor = Vector3.One;
                    effect.EmissiveColor = new Vector3(0.3f, 0.3f, 0.3f);
                    effect.PreferPerPixelLighting = true;

                }
                mesh.Draw();
            }
        }

        protected override void collectGib()
        {
            moneyManager.addMoney(value);
            moneyManager.toDelete.Add(this);
            game.modelManager.removeEffect(glow);
            glow.Dispose();
            for (int i = 0; i < 4; i++ )
                ship.particles.addMoneyParticle(color);
            collected = true;
        }
    }
}
