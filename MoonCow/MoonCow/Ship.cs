using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    public class Ship : Microsoft.Xna.Framework.GameComponent
    {
        public Matrix transform;
        public Vector3 pos;
        public Vector3 rot;
        public Vector3 scale;
        public Vector3 direction;
        Vector3 frameDiff;//values are added to this in update before doing one collision check

        public float moveSpeed;
        public float maxSpeed;
        public float boostSpeed;
        public float accel;

        public float currentTurnSpeed;
        public float maxTurnSpeed;

        public float roll;

        bool turning;
        public bool boosting;
        bool inUTurn;
        float uTurnYaw;

        Vector2 nodePos;

        bool rolling;
        float rollCooldown = 10;
        float tilt = 0;

        OOBB boundingBox;

        //money and health
        public float shieldVal;
        public float shieldMax;
        public float hpVal;
        public float hpMax;

        enum RollDir { left, right };
        RollDir rollDir = RollDir.left;

        ShipModel shipModel;
        SkyboxModel skyboxModel;
        SpeedCylModel speedCyl;
        RainbowTunnelModel rbowTun;

        public WeaponSystem weapons;
        public MoneyManager moneyManager;

        public Ship(Game game) : base(game)
        {
            pos = new Vector3(120, 4.5f, 90);
            moveSpeed = 0;
            accel = 0.5f;
            maxSpeed = 0.2f;
            boostSpeed = 0.6f;
            direction = new Vector3(0, 0, -1);
            currentTurnSpeed = 0;
            maxTurnSpeed = MathHelper.PiOver4 / 30;

            boundingBox = new OOBB(pos, direction, 1.5f, 1.5f); // Need to be changed to be actual ship dimentions

            shipModel = new ShipModel(game.Content.Load<Model>(@"Models/Ship/shipBlock"), this);
            //shipModel = new ShipModel(game.Content.Load<Model>(@"Models/Enemies/Cubes/sneakcube"), this);
            skyboxModel = new SkyboxModel(game.Content.Load<Model>(@"Models/Misc/skybox1"), this);
            speedCyl = new SpeedCylModel(game.Content.Load<Model>(@"Models/Misc/speedCyl"), this, ((Game1)Game));
            rbowTun = new RainbowTunnelModel(game.Content.Load<Model>(@"Models/Misc/Rbow/rbowTun"), this, ((Game1)Game));


            ((Game1)Game).modelManager.add(shipModel);
            ((Game1)Game).modelManager.add(skyboxModel);
            ((Game1)Game).modelManager.add(rbowTun);
            ((Game1)Game).modelManager.addTransparent(speedCyl);


            weapons = new WeaponSystem(this, game);
            moneyManager = new MoneyManager();
        }

        public override void Initialize()
        {
            shieldMax = 100;
            shieldVal = 100;
            hpVal = 100;
            hpMax = 100;

            base.Initialize();
        }


        public override void Update(GameTime gameTime)
        {
            frameDiff = Vector3.Zero;
            if (!Utilities.paused)
            {
                if (inUTurn)
                {
                    uTurn();
                }
                else
                {
                    //## SHIP TURNING CODE ##
                    updateTurn();

                    //## SHIP MOVEMENT CODE ##
                    updateSpeed();

                    //## BARREL ROLL ##
                    updateBarrelRoll();

                    //########### Collision detection #########
                    checkCollision();

                    if (Keyboard.GetState().IsKeyDown(Keys.S))
                    {
                        inUTurn = true;
                        currentTurnSpeed = 0;
                    }
                }                
            }

            if(Keyboard.GetState().IsKeyDown(Keys.I))
                moneyManager.addMoney(8);
            if (Keyboard.GetState().IsKeyDown(Keys.U))
                moneyManager.addMoney(1024);
            if (Keyboard.GetState().IsKeyDown(Keys.J))
                moneyManager.addMoney(-35);
            if (Keyboard.GetState().IsKeyDown(Keys.K))
                moneyManager.addMoney(-1329);

            weapons.update();
            moneyManager.update();
            //System.Diagnostics.Debug.WriteLine("Current angle is " + rot + ", direction is " + direction);

        }

        void uTurn()
        {
            //rotate 180 degrees over an amount of time
            //if angle + rot less than 180, add angle else rot is 180
            //once hit 180, inuturn false
            //currentTurnSpeed = MathHelper.Lerp(currentTurnSpeed, maxTurnSpeed / 2, Utilities.deltaTime * 5);

            uTurnYaw += MathHelper.Pi * Utilities.deltaTime / (13);
            rot.Y += uTurnYaw;

            direction.X = -(float)Math.Sin(rot.Y);
            direction.Z = -(float)Math.Cos(rot.Y);

            direction.Normalize();

            //pos.Y = 4.5f - (float)(Math.Cos((uTurnYaw)));

            //System.Diagnostics.Debug.WriteLine(uTurnYaw);

            moveSpeed = maxSpeed*3;// MathHelper.Lerp(moveSpeed, maxSpeed, Utilities.deltaTime * 5);


            //pos.X += direction.X * moveSpeed;
            //pos.Z += direction.Z * moveSpeed;

            frameDiff.X += direction.X * moveSpeed;
            frameDiff.Z += direction.Z * moveSpeed;

            if (uTurnYaw > MathHelper.Pi / 20)
            {
                pos.Y = 4.5f;
                uTurnYaw = 0;
                inUTurn = false;
            }
        }

        void updateTurn()
        {
            turning = false;
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                turning = true;
                if (boosting)
                {
                    currentTurnSpeed = MathHelper.Lerp(currentTurnSpeed, maxTurnSpeed / 2, Utilities.deltaTime * 5);
                    tilt = MathHelper.Lerp(tilt, 10 * maxTurnSpeed, Utilities.deltaTime * 2); //note -20*maxTurnSpeed is pi/6
                }
                else
                {
                    currentTurnSpeed = MathHelper.Lerp(currentTurnSpeed, maxTurnSpeed, Utilities.deltaTime * 5);
                    tilt = MathHelper.Lerp(tilt, 20 * maxTurnSpeed, Utilities.deltaTime * 2);
                }
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                turning = true;
                if (boosting)
                {
                    currentTurnSpeed = MathHelper.Lerp(currentTurnSpeed, -maxTurnSpeed / 2, Utilities.deltaTime * 5);
                    tilt = MathHelper.Lerp(tilt, -10 * maxTurnSpeed, Utilities.deltaTime * 2); //note -20*maxTurnSpeed is pi/6
                }
                else
                {
                    currentTurnSpeed = MathHelper.Lerp(currentTurnSpeed, -maxTurnSpeed, Utilities.deltaTime * 5);
                    tilt = MathHelper.Lerp(tilt, -20 * maxTurnSpeed, Utilities.deltaTime * 2);
                }

            }

            if (!turning)
            {
                currentTurnSpeed = MathHelper.Lerp(currentTurnSpeed, 0, Utilities.deltaTime * 5);
                tilt = MathHelper.Lerp(tilt, 0, Utilities.deltaTime * 2);

            }

            rot.Z = tilt;

            rot.Y += currentTurnSpeed;
            direction.X = -(float)Math.Sin(rot.Y);
            direction.Z = -(float)Math.Cos(rot.Y);
            direction.Normalize();
        }

        void updateSpeed()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                if (Keyboard.GetState().IsKeyDown(Keys.LeftShift))
                {
                    boosting = true;
                    moveSpeed = MathHelper.Lerp(moveSpeed, boostSpeed, Utilities.deltaTime * 7);
                }
                else
                {
                    boosting = false;
                    moveSpeed = MathHelper.Lerp(moveSpeed, maxSpeed, Utilities.deltaTime * 3);
                }
            }
            else
            {
                boosting = false;
                if (moveSpeed != 0)
                {
                    moveSpeed = MathHelper.Lerp(moveSpeed, 0, Utilities.deltaTime * 1);
                }
            }

            frameDiff.X += direction.X * moveSpeed;
            frameDiff.Z += direction.Z * moveSpeed;
        }

        void updateBarrelRoll()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Q) && rollCooldown >= 10)
            {
                rollCooldown = 0;
                rolling = true;
                rollDir = RollDir.left;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.E) && rollCooldown >= 10)
            {
                rollCooldown = 0;
                rolling = true;
                rollDir = RollDir.right;
            }

            if (rolling) //todo - treat sideways movement like forwards with lerping
            {
                if (rollCooldown < 10)
                    rollCooldown += 60 * Utilities.deltaTime;

                if (rollDir == RollDir.left)
                {
                    roll += MathHelper.Pi * Utilities.deltaTime * 3;
                    frameDiff.X += Vector3.Cross(Vector3.Up, direction).X * 15 * Utilities.deltaTime;
                    frameDiff.Z += Vector3.Cross(Vector3.Up, direction).Z * 15 * Utilities.deltaTime;
                    if (roll > MathHelper.Pi * 2)// && rollCooldown >= 10)
                    {
                        rollCooldown = 10;
                        roll = 0;
                        rolling = false;
                    }
                }
                else
                {
                    roll -= MathHelper.Pi * Utilities.deltaTime * 3;
                    frameDiff.X -= Vector3.Cross(Vector3.Up, direction).X * 15 * Utilities.deltaTime;
                    frameDiff.Z -= Vector3.Cross(Vector3.Up, direction).Z * 15 * Utilities.deltaTime;
                    if (roll < -MathHelper.Pi * 2)// && rollCooldown >= 10)
                    {
                        rollCooldown = 10;
                        roll = 0;
                        rolling = false;
                    }
                }

            }

            rot.Z += roll;
        }

        void checkCollision()
        {
            // By moving each component of the vector one at a time and seeing what causes the collision we can eliminate only that component
            // this means the ship will slide along walls rather than stick. Doing two collision checks per frame for the player seems to
            // be within tolerable limits for CPU time. This will only need to be done with the player

            //pos.X += direction.X * moveSpeed;
            pos.X += frameDiff.X;

            //## COLLISIONS WHOOO! ##
            // Move the bounding box to new pos
            boundingBox.Update(pos, direction);
            // Get current node co-ordinates
            nodePos = new Vector2((int)((pos.X / 30) + 0.5f), (int)((pos.Z / 30) + 0.5f));

            //For the current node check if your X component will make you collide with wall
            foreach (OOBB box in ((Game1)Game).map.map[(int)nodePos.X, (int)nodePos.Y].collisionBoxes)
            {
                if (boundingBox.intersects(box))
                {
                    //pos.X -= direction.X * moveSpeed;
                    pos.X -= frameDiff.X;
                    //pos.Z -= direction.Z * moveSpeed;
                    //currently just undoes the frames movement before drawing. effectively stopping the ship
                }
            }

            // Now add the Z component of the movement
            //pos.Z += direction.Z * moveSpeed;
            pos.Z += frameDiff.Z;

            boundingBox.Update(pos, direction);
            nodePos = new Vector2((int)((pos.X / 30) + 0.5f), (int)((pos.Z / 30) + 0.5f));

            foreach (OOBB box in ((Game1)Game).map.map[(int)nodePos.X, (int)nodePos.Y].collisionBoxes) // for each bounding box in current node
            {
                if (boundingBox.intersects(box))
                {
                    //pos.X -= direction.X * moveSpeed;
                    //pos.Z -= direction.Z * moveSpeed;
                    pos.Z -= frameDiff.Z;
                    //currently just undoes the frames movement before drawing. effectively stopping the ship
                }
            }
        }

    }
}
