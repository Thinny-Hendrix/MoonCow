using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    public class AmmoGib:MoneyGib
    {
        WeaponSystem weps;
        int type;
        public AmmoGib(MoneyManager mon, Ship ship, Vector3 pos, Game1 game, int type) : base()
        {
            this.value = value;
            this.model = ModelLibrary.bombProjectile;
            this.pos = pos;
            this.ship = ship;
            this.game = game;
            this.type = type;
            moneyManager = mon;
            weps = ship.weapons;
            scale = new Vector3(.3f, .3f, .3f);

            setColor(type);

            glow = new MoneyGibGlow(TextureManager.square, this, game);
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
        }

        void setColor(int i)
        {
            //red, aqua, pink, orange, gold
            switch (i)
            {
                default:
                    color = Color.Green;
                    break;
                case 1:
                    color = Color.Green;
                    break;
                case 2:
                    color = Color.Firebrick;
                    break;
                case 3:
                    color = Color.Orange;
                    break;
            }
        }

        public override void Update(GameTime gameTime)
        {
            //first shoot in initDirection, over time change to move towards ship pos

            //calculate 3D target direction normal

            if (!Utilities.paused && !Utilities.softPaused)
            {
                yAngle = (float)Math.Atan2(pos.X - ship.pos.X, pos.Z - ship.pos.Z);
                xAngle = (float)Math.Atan2(pos.Y - ship.pos.Y, pos.Z - ship.pos.Z);
                targetDirection.X = -(float)Math.Sin(yAngle);
                targetDirection.Z = -(float)Math.Cos(yAngle);
                targetDirection.Y = -(float)Math.Sin(xAngle);
                targetDirection.Normalize();



                currentDirection = Vector3.Lerp(currentDirection, targetDirection, Utilities.deltaTime * changeDirectionSpeed);
                frameDiff += currentDirection * speed * Utilities.deltaTime;

                speed += Utilities.deltaTime * 6;
                checkCollision();
                frameDiff = Vector3.Zero;

                rot += (currentDirection * -0.1f);
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
                    effect.TextureEnabled = true;
                    effect.Texture = TextureManager.bombTex1;
                    effect.Alpha = 1;

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

        void checkCollision()
        {
            pos += frameDiff;

            col.Update(pos);

            Vector2 nodePos = new Vector2((int)((pos.X / 30) + 0.5f), (int)((pos.Z / 30) + 0.5f));

            try
            {
                if (col.checkOOBB(ship.boundingBox))
                {
                    collectGib();
                }
            }
            catch (IndexOutOfRangeException) { }
        }

        void collectGib()
        {
            weps.addAmmo(type);
            game.modelManager.removeEffect(glow);
            glow.Dispose();
            ship.particles.addMoneyParticle(color);
            collected = true;
            moneyManager.toDelete.Add(this);
        }
    }
}
