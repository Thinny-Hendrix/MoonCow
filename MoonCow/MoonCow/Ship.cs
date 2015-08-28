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

        bool rolling;
        float rollCooldown = 10;
        float tilt = 0;


        enum RollDir { left, right };
        RollDir rollDir = RollDir.left;

        public ShipModel shipModel;
        WeaponSystem weapons;

        public Ship(Game game) : base(game)
        {
            pos = new Vector3(90, 4.5f, 90);
            moveSpeed = 0;
            accel = 0.5f;
            maxSpeed = 0.2f;
            boostSpeed = 0.6f;
            direction = new Vector3(0, 0, -1);
            currentTurnSpeed = 0;
            maxTurnSpeed = MathHelper.PiOver4 / 30;

            shipModel = new ShipModel(game.Content.Load<Model>(@"Models/Ship/shipBlock"), this);
            //shipModel = new ShipModel(game.Content.Load<Model>(@"Models/Enemies/Sneaker/sneakproto"), this);

            ((Game1)Game).modelManager.add(shipModel);

            weapons = new WeaponSystem(this);
        }

        public override void Initialize()
        {
            

            base.Initialize();
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


            moveSpeed = MathHelper.Lerp(moveSpeed, maxSpeed, Utilities.deltaTime * 5);
                      

            pos.X += direction.X * moveSpeed;
            pos.Z += direction.Z * moveSpeed;
            
            if (uTurnYaw > MathHelper.Pi/20)
            {
                uTurnYaw = 0;
                inUTurn = false;
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (!Utilities.paused)
            {
                if (inUTurn)
                {
                    uTurn();
                }
                else
                {
                    //## SHIP TURNING CODE ##
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



                    //## SHIP MOVEMENT CODE ##
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

                    pos.X += direction.X * moveSpeed;
                    pos.Z += direction.Z * moveSpeed;

                    //## BARREL ROLL ##
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

                    if(rolling) //todo - treat sideways movement like forwards with lerping
                    {
                        if (rollCooldown < 10)
                            rollCooldown += 60 * Utilities.deltaTime;

                        if(rollDir == RollDir.left)
                        {
                            roll += MathHelper.Pi * Utilities.deltaTime * 3;
                            pos.X += Vector3.Cross(Vector3.Up, direction).X * 15 * Utilities.deltaTime;
                            pos.Z += Vector3.Cross(Vector3.Up, direction).Z * 15 * Utilities.deltaTime;
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
                            pos.X -= Vector3.Cross(Vector3.Up, direction).X * 15 * Utilities.deltaTime;
                            pos.Z -= Vector3.Cross(Vector3.Up, direction).Z * 15 * Utilities.deltaTime;
                            if (roll < -MathHelper.Pi * 2)// && rollCooldown >= 10)
                            {
                                rollCooldown = 10;
                                roll = 0;
                                rolling = false;
                            }
                        }

                    }

                    rot.Z += roll;


                    if (!boosting)
                        weapons.update();

                    if (Keyboard.GetState().IsKeyDown(Keys.S))
                    {
                        inUTurn = true;
                        currentTurnSpeed = 0;
                        //pos.X -= direction.X * moveSpeed;
                        //pos.Z -= direction.Z * moveSpeed;

                    }



                }

                
            }


            System.Diagnostics.Debug.WriteLine("Current angle is " + rot + ", direction is " + direction);

        }



    }
}
