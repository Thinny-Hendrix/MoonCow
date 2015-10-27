using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MoonCow
{
    public class Camera : Microsoft.Xna.Framework.GameComponent
    {
        Game1 game;
        Ship ship;
        public Matrix view { get; protected set; }
        public Matrix projection { get; protected set; }

        // Camera vectors
        public Vector3 cameraPosition;
        Vector3 lookAt;
        Vector3 cameraUp;
        Vector3 goalDirection;
        public Vector3 currentDirection;

        //these are so the camera can change position when ship is boosting
        float standardFov = MathHelper.Pi/3;
        float boostFov = MathHelper.PiOver2;
        float currentFov = MathHelper.Pi / 3;
        float boostDist = 7;
        float normDist = 15;
        float currentDist = 15;

        //this is to control camera tilt
        float tiltAngle;
        public Vector3 tiltUp = Vector3.Up; //the up vector the camera will use
        bool tilting = false;
        float tiltStrength;
        float tiltTime;
        Vector3 shakeOffset;
        float shakeTime;
        bool shaking;
        float shakeStrength;
        int shakeConstant;

        //to control changing camera lookat for minigame
        float transitionTime;
        Vector3 staticLook;
        Vector3 staticPos;
        Vector3 prevPos;
        Vector3 prevLook;
        public bool transitioning;
        bool useStatic;

        Vector3 currentPos;
        Vector3 currentLook;

        //endgame camera movement
        Vector3 endLook;
        Vector3 endPos1;
        Vector3 endPos2;
        public float endTime;
        public bool endGame;

        public Camera(Game1 game, Vector3 pos, Vector3 target, Vector3 up)
            : base(game)
        {
            // Build camera view matrix
            this.game = game;
            ship = game.ship;
            cameraPosition = pos;
            cameraUp = up;
            lookAt = target;
            CreateLookAt();
            projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.Pi/3, (float)Game.Window.ClientBounds.Width /
                (float)Game.Window.ClientBounds.Height, 1, 3000);

            shakeTime = 0;
            shakeConstant = 1;
        }

        public override void Initialize()
        {
            // Set mouse position
            Mouse.SetPosition(Game.Window.ClientBounds.Width / 2, Game.Window.ClientBounds.Height / 2);
            base.Initialize();
        }

        public void setEndData(Vector3 corePos)
        {
            endLook = corePos;
            endPos1 = corePos + new Vector3(-38, 15, -42);
            endPos2 = corePos + new Vector3(38, 15, -42);
        }

        public void triggerEnd()
        {
            endGame = true;
            endTime = 0;
        }

        public void setStaticTarg(Vector3 pos, Vector3 look)
        {
            staticPos = pos;
            staticLook = look;
            useStatic = true;
            transitioning = true;
            transitionTime = 0;

            prevPos = cameraPosition;
            prevLook = lookAt;
        }

        public void followShip()
        {
            useStatic = false;
            staticPos = currentPos;
            staticLook = currentLook;
            transitioning = true;
            transitionTime = 0;
        }

        void oldUpdateMethod()
        {
            // here for moving camera
            // Move forward/backward
            /*
            if (Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.W))
            {
                lookAt.X += (float)(System.Math.Sin(xTurnValue * MathHelper.Pi / 180)) * (float)-runSpeed;
                lookAt.Z += (float)(System.Math.Cos(xTurnValue * MathHelper.Pi / 180)) * (float)-runSpeed;
            }

            if (Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.A))
            {
                lookAt.X += (float)(System.Math.Sin((xTurnValue + 90) * MathHelper.Pi / 180)) * (float)-runSpeed;
                lookAt.Z += (float)(System.Math.Cos((xTurnValue + 90) * MathHelper.Pi / 180)) * (float)-runSpeed;
            }

            if (Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.S))
            {
                lookAt.X += (float)(System.Math.Sin((xTurnValue + 180) * MathHelper.Pi / 180)) * (float)-runSpeed;
                lookAt.Z += (float)(System.Math.Cos((xTurnValue + 180) * MathHelper.Pi / 180)) * (float)-runSpeed;
            }

            if (Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D))
            {
                lookAt.X += (float)(System.Math.Sin((xTurnValue + 270) * MathHelper.Pi / 180)) * (float)-runSpeed;
                lookAt.Z += (float)(System.Math.Cos((xTurnValue + 270) * MathHelper.Pi / 180)) * (float)-runSpeed;
            }

            if (Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Space) && lookAt.Y == height)
            {
                verticalSpeed = jumpSpeed;
            }

            lookAt.Y += (float)verticalSpeed;
            verticalSpeed -= gravity;
            if (lookAt.Y <= height)
            {
                verticalSpeed = 0;
                lookAt.Y = height;
            }
            try
            {
                xTurnValue += Game.Window.ClientBounds.Width / 2 - Mouse.GetState().X;
                yTurnValue += Game.Window.ClientBounds.Height / 2 - Mouse.GetState().Y;
                //Mouse.SetPosition(Game.Window.ClientBounds.Width / 2, Game.Window.ClientBounds.Height / 2);
            }
            catch (NullReferenceException) { }

            if (yTurnValue > 90)
                yTurnValue = 90;
            if (yTurnValue < -90)
                yTurnValue = -90;*/


            //cameraPosition.X = lookAt.X + (float)(System.Math.Sin(xTurnValue * MathHelper.Pi / 180)) * turnRadius;
            //cameraPosition.Y = lookAt.Y + (float)-(System.Math.Sin(yTurnValue * MathHelper.Pi / 180)) * turnRadius;
            //cameraPosition.Z = lookAt.Z + (float)(System.Math.Cos(xTurnValue * MathHelper.Pi / 180)) * turnRadius;

            //System.Diagnostics.Debug.WriteLine((float)Math.Tan(8));

            /*
            lookAt.X += currentDirection.X * 11;
            lookAt.Z += currentDirection.Z * 11;
            //lookAt.Y += 3;

            cameraPosition.X = lookAt.X - (currentDirection.X * 15);
            cameraPosition.Y = lookAt.Y + (2.1f);
            cameraPosition.Z = lookAt.Z - (currentDirection.Z * 15);*/


            ///###### Real camera code #####
            ///
            ///
        }

        void updateShipCam()
        {
            goalDirection = ship.direction;
            currentDirection = Vector3.Lerp(currentDirection, goalDirection, Utilities.deltaTime * 5);
            currentDirection.Normalize();

            if (ship.boosting)
            {
                if (ship.finishingMove)
                    currentDist = MathHelper.Lerp(currentDist, 3, Utilities.deltaTime * 2);
                else
                    currentDist = MathHelper.Lerp(currentDist, boostDist, Utilities.deltaTime * 4);
            }
            else
                currentDist = MathHelper.Lerp(currentDist, normDist, Utilities.deltaTime * 2);


            lookAt = ship.pos;

            lookAt.X += currentDirection.X * (currentDist * ((float)11.0 / (float)15.0));
            lookAt.Z += currentDirection.Z * (currentDist * ((float)11.0 / (float)15.0));
            if (ship.finishingMove)
                lookAt.Y = 4.8f;
            else
                lookAt.Y = 4.5f;
            lookAt += shakeOffset;

            cameraPosition.X = lookAt.X - (currentDirection.X * currentDist);
            if (ship.finishingMove)
                cameraPosition.Y = lookAt.Y + (currentDist * (float)Math.Tan(MathHelper.ToRadians(7)));
            else
                cameraPosition.Y = lookAt.Y + (currentDist * (float)Math.Tan(MathHelper.ToRadians(8)));

            cameraPosition.Z = lookAt.Z - (currentDirection.Z * currentDist);
        }

        public void updateEnd()
        {
            endTime += Utilities.deltaTime;
            currentLook = endLook;
            if(endTime < 2)
            {
                currentPos = endPos1;
            }
            else if(endTime < 8)
            {
                currentPos = Vector3.SmoothStep(endPos1, endPos2, (endTime - 2) / 6);
            }
            else
            {
                currentPos = endPos2;
            }

            if(endTime > 12)
            {
                game.exitMainGame();
            }
            cameraPosition = currentPos;
        }

        public override void Update(GameTime gameTime)
        {
            /// camera angle is 8 degrees, positions derived from tan(8) - eg 10tan(8) gives 1.4 height and a distance of 10 between camera and target

            if (!Utilities.paused && !Utilities.softPaused)
            {
                if(endGame)
                {
                    updateEnd();
                }
                else
                {
                    updateShipCam();

                    if (transitioning)
                    {
                        transitionTime += Utilities.deltaTime * MathHelper.Pi;
                        if (useStatic)
                        {
                            currentPos = Vector3.Lerp(staticPos, cameraPosition, (float)(Math.Cos(transitionTime) + 1) / 2);
                            currentLook = Vector3.Lerp(staticLook, lookAt, (float)(Math.Cos(transitionTime) + 1) / 2);
                        }
                        else
                        {
                            currentPos = Vector3.Lerp(cameraPosition, staticPos, (float)(Math.Cos(transitionTime) + 1) / 2);
                            currentLook = Vector3.Lerp(lookAt, staticLook, (float)(Math.Cos(transitionTime) + 1) / 2);
                        }
                        if (transitionTime >= MathHelper.Pi)
                            transitioning = false;
                    }
                    else
                    {
                        if (useStatic)
                        {
                            currentPos = staticPos;
                            currentLook = staticLook;
                        }
                        else
                        {
                            currentPos = cameraPosition;
                            currentLook = lookAt;
                        }
                    }

                    currentPos += shakeOffset;


                

                    if (ship.boosting)
                        currentFov = MathHelper.Lerp(currentFov, boostFov, Utilities.deltaTime * 2);
                    else
                        currentFov = MathHelper.Lerp(currentFov, standardFov, Utilities.deltaTime * 2);
                }

                try
                {
                    projection = Matrix.CreatePerspectiveFieldOfView(currentFov, (float)Game.Window.ClientBounds.Width / (float)Game.Window.ClientBounds.Height, 0.1f, 3000);
                }
                catch (NullReferenceException) { }
                /*
                if (Keyboard.GetState().IsKeyDown(Keys.O))
                    makeShake();*/

                updateTilt();
                updateYshake();

                CreateLookAt();
            }
        }

        public void setYShake(float amount)
        {
            if(amount > shakeStrength)
                shakeStrength = amount;
            shaking = true;
        }

        public void makeYShake()
        {
            shakeTime = 0;
            shaking = true;
            shakeStrength = 0.15f;

            if (Utilities.random.Next(0, 10) < 5) //this just chooses the starting direction for the tilt angle so it's not the same every single time
            {
                shakeConstant = -1;
            }
            else
                shakeConstant = 1;
        }
        public void makeShake()
        {
            tiltStrength = MathHelper.PiOver4 / 6;
            tiltTime = 0;
            tilting = true;
            shakeTime = 0;
            shaking = true;
            shakeStrength = 0.15f;

            if (Utilities.random.Next(0, 10) < 5) //this just chooses the starting direction for the tilt angle so it's not the same every single time
            {
                shakeConstant = -1;
            }
            else
                shakeConstant = 1;

        }

        private void updateYshake()
        {
            if (shaking)
            {
                shakeOffset.Y = shakeConstant * (float)Math.Sin(shakeTime) * shakeStrength * 1;
                shakeStrength = MathHelper.Lerp(shakeStrength, 0, Utilities.deltaTime * 3f);
                shakeTime += MathHelper.Pi * Utilities.deltaTime * 12;
                if (shakeTime > MathHelper.Pi * 2)
                    shakeTime -= MathHelper.Pi * 2;
                if (shakeStrength <= 0.001)
                {
                    shakeTime = 0;
                    shakeOffset.Y = 0;
                    shaking = false;
                }
            }
        }

        private void updateTilt()
        {
            if(tilting)
            {
                tiltTime += MathHelper.Pi * Utilities.deltaTime * 5;
                if(tiltTime >= MathHelper.Pi*4)
                {
                    tiltTime = 0;
                    tilting = false;
                }

                tiltAngle = shakeConstant * (float)Math.Sin(tiltTime) * tiltStrength;
                tiltStrength = MathHelper.Lerp(tiltStrength, 0, Utilities.deltaTime * 2.5f); //decreases tiltStrength to 0 in ~20 frames

                tiltUp = Vector3.Transform(Vector3.Up,Matrix.CreateFromAxisAngle(currentDirection,tiltAngle));
            }
        }

        public void reset()
        {
            currentDirection = ship.direction;
            currentFov = standardFov;
            useStatic = false;
        }

        private void CreateLookAt()
        {
            view = Matrix.CreateLookAt(currentPos, currentLook, tiltUp);
        }

    }
}
