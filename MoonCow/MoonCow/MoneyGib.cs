using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    public class MoneyGib : BasicModel
    {
        public CircleCollider col { get; protected set; }
        public Ship ship { get; protected set; }
        public Game1 game { get; protected set; }
        public MoneyManager moneyManager;
        public MoneyGibGlow glow { get; protected set; }
        public Vector3 initDirection;
        public Vector3 currentDirection;
        public Vector3 targetDirection;
        public Vector3 frameDiff;
        Texture2D tex;
        public float changeDirectionSpeed { get; protected set; }
        public float speed { get; protected set; }
        public float speedMax { get; protected set; }
        public float value { get; protected set; }
        public float yAngle { get; protected set; }
        public float xAngle { get; protected set; }
        public bool collected { get; protected set; }
        public Color color { get; protected set; }

        public MoneyGib() : base() { }
        public MoneyGib(float value, Model model, MoneyManager moneyManager, Ship ship, Vector3 pos, Game1 game, int type) : base()
        {
            this.value = value;

            if (type == 4)
            {
                this.model = ModelLibrary.polyGib;
                scale = new Vector3(.15f, .15f, .15f);
            }
            else
            {
                this.model = model;
                scale = new Vector3(.03f, .03f, .03f);
            }

            this.pos = pos;
            this.moneyManager = moneyManager;
            this.ship = ship;
            this.game = game;

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
            //red, pink-red, aqua, pink, orange, gold
            switch(i)
            {
                case -1: //sentry
                    color = Color.Red;
                    tex = TextureManager.gib1_s;
                    break;
                default:
                    color = new Color(255,0,55);
                    tex = TextureManager.gib1_0;
                    break;
                case 1:
                    color = new Color(0,255,255);
                    tex = TextureManager.gib1_1;
                    break;
                case 2:
                    color = new Color(255,0,162);
                    tex = TextureManager.gib1_2;
                    break;
                case 3:
                    color = new Color(255,185,0);
                    tex = TextureManager.gib1_3;
                    break;
                case 4:
                    if(Utilities.random.Next(2) == 0)
                        color = new Color(0, 255, 255);
                    else
                        color = new Color(255,0,162);
                    tex = TextureManager.polyGib;
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
                    effect.Texture = tex;
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

        void checkCollision()
        {
            // By moving each component of the vector one at a time and seeing what causes the collision we can eliminate only that component
            // this means the ship will slide along walls rather than stick. Doing two collision checks per frame for the player seems to
            // be within tolerable limits for CPU time. This will only need to be done with the player
            pos += frameDiff;

            //## COLLISIONS WHOOO! ##
            // Move the bounding box to new pos
            col.Update(pos);
            // Get current node co-ordinates
            Vector2 nodePos = new Vector2((int)((pos.X / 30) + 0.5f), (int)((pos.Z / 30) + 0.5f));

            //For the current node check if your X component will make you collide with wall
            try
            {
                if (col.checkCircle(ship.circleCol))
                {
                    collectGib();
                }
            }
            catch (IndexOutOfRangeException){}
        }

        protected virtual void collectGib()
        {
            moneyManager.addMoney(value);
            moneyManager.toDelete.Add(this);
            float pitch = 0;
            if (moneyManager.collected <= 1)
            {
                game.audioManager.shipCollectMoney.Pitch = moneyManager.collected;
                pitch = moneyManager.collected;
            }
            else
            {
                game.audioManager.shipCollectMoney.Pitch = 1f;
                pitch = 1;
            }/*
            game.audioManager.shipCollectMoney.Stop();
            game.audioManager.shipCollectMoney.Play();*/
            game.audioManager.addSoundEffect(AudioLibrary.shipCollectMoney, 0.5f, pitch);
            moneyManager.collected += 0.05f;
            game.modelManager.removeEffect(glow);
            glow.Dispose();
            ship.particles.addMoneyParticle(color);
            collected = true;
        }
    }
}
