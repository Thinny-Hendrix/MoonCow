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
        Game1 game;

        public Matrix transform;
        public Vector3 pos;
        public Vector3 rot;
        public Vector3 scale;
        public Vector3 direction;
        Vector3 frameDiff;//values are added to this in update before doing one collision check
        public Vector3 respawnPoint;


        public float moveSpeed;
        public float maxSpeed;
        public float boostSpeed;
        public float accel;
        float sideSpeed;
        float targetRoll;

        public float currentTurnSpeed;
        public float maxTurnSpeed;

        public float roll;

        bool turning;
        public bool boosting;
        bool inUTurn;
        float uTurnYaw;
        float totaluTurn;

        public bool finishingMove;

        Vector2 nodePos;

        float rollCooldown = 0;
        float tilt = 0;
        enum RollState {not, rolling, recovering};
        RollState rollState;

        public OOBB boundingBox;

        bool justHitWall = false;

        

        enum RollDir { left, right };
        RollDir rollDir = RollDir.left;
        float rollRecoverTime = MathHelper.PiOver2;

        ShipModel shipModel;
        SkyboxModel skyboxModel;
        SpeedCylModel speedCyl;
        RainbowTunnelModel rbowTun;

        public ShipParticleSystem particles;
        public WeaponSystem weapons;
        public MoneyManager moneyManager;
        public ShipHealthSystem shipHealth;

        public Ship(Game game) : base(game)
        {
            this.game = (Game1)game;

            pos = new Vector3(90, 4.5f, 0);
            moveSpeed = 0;
            accel = 0.5f;
            maxSpeed = 0.2f;
            boostSpeed = 0.6f;
            direction = new Vector3(0, 0, -1);
            currentTurnSpeed = 0;
            maxTurnSpeed = MathHelper.PiOver4 / 30;

            boundingBox = new OOBB(pos, direction, 1.5f, 1.5f); // Need to be changed to be actual ship dimentions

            shipModel = new ShipModel(game.Content.Load<Model>(@"Models/Ship/PewProto/shipPewfbx"), this);
            //shipModel = new ShipModel(game.Content.Load<Model>(@"Models/Enemies/Cubes/guncube"), this);
            skyboxModel = new SkyboxModel(game.Content.Load<Model>(@"Models/Misc/skybox1"), this);
            speedCyl = new SpeedCylModel(game.Content.Load<Model>(@"Models/Misc/speedCyl"), this, ((Game1)Game));
            rbowTun = new RainbowTunnelModel(game.Content.Load<Model>(@"Models/Misc/Rbow/rbowTun"), this, ((Game1)Game));

            ((Game1)Game).modelManager.addObject(shipModel);
            ((Game1)Game).modelManager.add(skyboxModel);
            ((Game1)Game).modelManager.addObject(rbowTun);
            ((Game1)Game).modelManager.addTransparent(speedCyl);

            weapons = new WeaponSystem(this, this.game);
            moneyManager = new MoneyManager(this.game);
            particles = new ShipParticleSystem(this.game, this);
            shipHealth = new ShipHealthSystem(this.game, this);

            game.Components.Add(weapons);
            game.Components.Add(moneyManager);
            game.Components.Add(particles);
            game.Components.Add(shipHealth);

        }

        public override void Initialize()
        {
            pos = respawnPoint;

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

                    if (Keyboard.GetState().IsKeyDown(Keys.S))
                    {
                        inUTurn = true;
                        currentTurnSpeed = 0;
                    }
                }

                //## BARREL ROLL ##
                updateBarrelRoll();

                checkFinishingMove();
                //########### Collision detection #########
                checkCollision();
            }

            if(Keyboard.GetState().IsKeyDown(Keys.I))
                moneyManager.addMoney(8);
            if (Keyboard.GetState().IsKeyDown(Keys.U))
                moneyManager.addMoney(1024);
            if (Keyboard.GetState().IsKeyDown(Keys.J))
                moneyManager.addMoney(-35);
            if (Keyboard.GetState().IsKeyDown(Keys.K))
                moneyManager.addMoney(-1329);

            if (Keyboard.GetState().IsKeyDown(Keys.L))
                shipHealth.onHit(3);

            if (Keyboard.GetState().IsKeyDown(Keys.T))
                respawn();


            //weapons.update();
            //moneyManager.update();
            //System.Diagnostics.Debug.WriteLine("Current angle is " + rot + ", direction is " + direction);

        }

        void respawn()
        {
            pos = respawnPoint;

            //stop moving
            moveSpeed = 0;
            sideSpeed = 0;

            //face the right direction
            direction = Vector3.Forward;
            rot.Y = 0;

            //reset turning
            currentTurnSpeed = 0;
            tilt = 0;

            //reset barrel roll
            rollState = RollState.not;
            rollCooldown = 0;
            roll = 0;

            //reset uturn
            inUTurn = false;
            totaluTurn = 0;
            pos.Y = 4.5f;
            uTurnYaw = 0;
            rot.X = 0;

            game.camera.reset();
        }

        void uTurn()
        {
            //rotate 180 degrees over an amount of time
            //if angle + rot less than 180, add angle else rot is 180
            //once hit 180, inuturn false
            //currentTurnSpeed = MathHelper.Lerp(currentTurnSpeed, maxTurnSpeed / 2, Utilities.deltaTime * 5);

            uTurnYaw += MathHelper.Pi * Utilities.deltaTime / (13);
            rot.Y += uTurnYaw;
            totaluTurn += uTurnYaw;

            direction.X = -(float)Math.Sin(rot.Y);
            direction.Z = -(float)Math.Cos(rot.Y);

            direction.Normalize();

            pos.Y = 4.5f - (float)(Math.Cos((totaluTurn*2))-1)/4;
            rot.X = (float)(Math.Sin((totaluTurn * 2))/2);
            rot.Z = tilt -(float)(Math.Cos((totaluTurn * 2)) - 1) / 2;

            //System.Diagnostics.Debug.WriteLine(uTurnYaw);

            moveSpeed = maxSpeed;// MathHelper.Lerp(moveSpeed, maxSpeed, Utilities.deltaTime * 5);


            //pos.X += direction.X * moveSpeed;
            //pos.Z += direction.Z * moveSpeed;

            frameDiff.X += direction.X * moveSpeed;
            frameDiff.Z += direction.Z * moveSpeed;

            //if (uTurnYaw > MathHelper.Pi / 20)
            if(totaluTurn > MathHelper.Pi)
            {
                rot.X = 0;
                totaluTurn = 0;
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
            if (Keyboard.GetState().IsKeyDown(Keys.Q) && rollCooldown <=0)
            {
                rollCooldown = 30;
                rollRecoverTime = MathHelper.PiOver2;

                if (roll < 0 && rollState == RollState.rolling)
                    targetRoll = 0;
                else
                    targetRoll = MathHelper.Pi * 2;

                rollState = RollState.rolling;
                rollDir = RollDir.left;
                
            }

            if (Keyboard.GetState().IsKeyDown(Keys.E) && rollCooldown <=0)
            {
                rollCooldown = 30;
                rollRecoverTime = MathHelper.PiOver2;

                if (roll > 0 && rollState == RollState.rolling)
                    targetRoll = 0;
                else
                    targetRoll = -MathHelper.Pi * 2;

                rollState = RollState.rolling;
                rollDir = RollDir.right;
                

            }

            if (rollState == RollState.rolling) //todo - treat sideways movement like forwards with lerping
            {
                if (rollCooldown > 0)
                    rollCooldown -= 60 * Utilities.deltaTime;

                if (rollDir == RollDir.left)
                {
                    sideSpeed = MathHelper.Lerp(sideSpeed, 15, Utilities.deltaTime*5);
                    roll += MathHelper.Pi * Utilities.deltaTime * 4 * (sideSpeed/15);
                    
                    if (roll > targetRoll)// && rollCooldown >= 10)
                    {
                        rollCooldown = 0;
                        roll = 0;
                        rollState = RollState.recovering;

                    }
                }
                else
                {
                    sideSpeed = MathHelper.Lerp(sideSpeed, -15, Utilities.deltaTime * 5);
                    roll += MathHelper.Pi * Utilities.deltaTime * 4 * (sideSpeed / 15);


                    if (roll < targetRoll)// && rollCooldown >= 10)
                    {
                        rollCooldown = 0;
                        roll = 0;
                        rollState = RollState.recovering;
                    }
                }
            }
            else
            {
                sideSpeed = MathHelper.Lerp(sideSpeed, 0, Utilities.deltaTime * 8);
            }
            if(rollState == RollState.recovering)
            {
                if(rollDir == RollDir.left)
                    roll = (float)((Math.Cos(rollRecoverTime) - 1) * 0.5f) * -MathHelper.Pi * Utilities.deltaTime * 8;
                if (rollDir == RollDir.right)
                    roll = (float)((Math.Cos(rollRecoverTime) - 1) * 0.5f) * MathHelper.Pi * Utilities.deltaTime * 8;

                //                    roll = (float)((Math.Cos(rollRecoverTime) - 1) * 0.5f) * MathHelper.PiOver4*0.65f;


                rollRecoverTime += MathHelper.Pi * 3.5f * Utilities.deltaTime;
                if(rollRecoverTime >= MathHelper.Pi*2)
                {
                    rollState = RollState.not;
                    roll = 0;
                    rollRecoverTime = MathHelper.PiOver2;
                }

            }

            frameDiff.X += Vector3.Cross(Vector3.Up, direction).X * sideSpeed * Utilities.deltaTime;
            frameDiff.Z += Vector3.Cross(Vector3.Up, direction).Z * sideSpeed * Utilities.deltaTime;
            rot.Z += roll;
        }

        void checkFinishingMove()
        {
            if(boosting && Keyboard.GetState().IsKeyDown(Keys.Space))//to change - if boosting, drill equipped and raycast detects enemy within range with low health
            {
                if(!finishingMove)
                {
                    ((Game1)Game).hud.flashTime = 0;
                    finishingMove = true;
                }
            }
            else
            {
                if (finishingMove)
                {
                    ((Game1)Game).hud.flashTime = 0;
                    finishingMove = false;
                }
            }
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

            bool colCheck = false;
            //For the current node check if your X component will make you collide with wall
            foreach (OOBB box in ((Game1)Game).map.map[(int)nodePos.X, (int)nodePos.Y].collisionBoxes)
            {
                if (boundingBox.intersects(box))
                {
                    //pos.X -= direction.X * moveSpeed;
                    pos.X -= frameDiff.X;
                    //pos.Z -= direction.Z * moveSpeed;
                    //currently just undoes the frames movement before drawing. effectively stopping the ship
                    
                    colCheck = true;
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
                    
                    colCheck = true;
                }
            }

            if(colCheck)
            {
                if (!justHitWall)           //plays the wallHit noise only if a collision is occurring
                {
                    justHitWall = true;
                    game.audioManager.wallHit();
                }
                if(moveSpeed > 0.1f)
                {
                    game.audioManager.wallScrape();
                }
            }
            else     //stops the scraping noise as soon as the ship is no longer moving against a wall
            {
                justHitWall = false;
                game.audioManager.wallScrapeStop();
            }
        }

        public void setRespawn(Vector3 respawn)
        {
            respawnPoint = respawn;
            pos = respawnPoint;
        }

        public void onDeath()
        {
            respawn();
        }

    }
}
