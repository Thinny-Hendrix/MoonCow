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

        int xTurnValue;
        int yTurnValue;

        float height = 5;       //values can be changed to suit player proportions relative to environment
        double gravity = 0.1;
        double jumpSpeed = 2;
        double verticalSpeed = 0;
        double runSpeed = 0.4;
        int turnRadius = 10; //this is the distance from the camera to the player (lookAt)

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
        }

        public override void Initialize()
        {
            // Set mouse position
            Mouse.SetPosition(Game.Window.ClientBounds.Width / 2, Game.Window.ClientBounds.Height / 2);
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
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

            ///###### Real camera code #####
            ///
            /// camera angle is 8 degrees, positions derived from tan(8) - eg 10tan(8) gives 1.4 height and a distance of 10 between camera and target
            ///

            goalDirection = ship.direction;
            currentDirection = Vector3.Lerp(currentDirection, goalDirection, Utilities.deltaTime*5);
            currentDirection.Normalize();

            if (ship.boosting)
            {
                if(ship.finishingMove)
                    currentDist = MathHelper.Lerp(currentDist, 4, Utilities.deltaTime * 2);
                else
                    currentDist = MathHelper.Lerp(currentDist, boostDist, Utilities.deltaTime * 2);
            }
            else
                currentDist = MathHelper.Lerp(currentDist, normDist, Utilities.deltaTime * 2);


            lookAt = ship.pos;

            lookAt.X += currentDirection.X * (currentDist * ((float)11.0 / (float)15.0));
            lookAt.Z += currentDirection.Z * (currentDist * ((float)11.0 / (float)15.0));
            lookAt.Y = 4.5f;
            lookAt += shakeOffset;

            cameraPosition.X = lookAt.X - (currentDirection.X * currentDist);
            cameraPosition.Y = lookAt.Y + (currentDist * (float)Math.Tan(MathHelper.ToRadians(8)));
            cameraPosition.Z = lookAt.Z - (currentDirection.Z * currentDist);

            cameraPosition += shakeOffset;
             

            //System.Diagnostics.Debug.WriteLine((float)Math.Tan(8));

            /*
            lookAt.X += currentDirection.X * 11;
            lookAt.Z += currentDirection.Z * 11;
            //lookAt.Y += 3;

            cameraPosition.X = lookAt.X - (currentDirection.X * 15);
            cameraPosition.Y = lookAt.Y + (2.1f);
            cameraPosition.Z = lookAt.Z - (currentDirection.Z * 15);*/

            if (ship.boosting)
                currentFov = MathHelper.Lerp(currentFov, boostFov, Utilities.deltaTime * 2);
            else
                currentFov = MathHelper.Lerp(currentFov, standardFov, Utilities.deltaTime * 2);

            try
            {
                projection = Matrix.CreatePerspectiveFieldOfView(currentFov, (float)Game.Window.ClientBounds.Width / (float)Game.Window.ClientBounds.Height, 0.1f, 3000);
            }
            catch (NullReferenceException) { }


            if (Keyboard.GetState().IsKeyDown(Keys.O))
                makeShake();


            updateTilt();
            updateYshake();




            //cameraPosition.X = lookAt.X + (float)(System.Math.Sin(xTurnValue * MathHelper.Pi / 180)) * turnRadius;
            //cameraPosition.Y = lookAt.Y + (float)-(System.Math.Sin(yTurnValue * MathHelper.Pi / 180)) * turnRadius;
            //cameraPosition.Z = lookAt.Z + (float)(System.Math.Cos(xTurnValue * MathHelper.Pi / 180)) * turnRadius;

            CreateLookAt();


            /*
            // Roll rotation
            if (Mouse.GetState(  ).LeftButton == ButtonState.Pressed)
            {
                cameraUp = Vector3.Transform(cameraUp,
                     Matrix.CreateFromAxisAngle(cameraDirection,
                     MathHelper.PiOver4 / 45));
            }
            if (Mouse.GetState(  ).RightButton == ButtonState.Pressed)
            {
                cameraUp = Vector3.Transform(cameraUp,
                    Matrix.CreateFromAxisAngle(cameraDirection,
                    -MathHelper.PiOver4 / 45));
            }
            */

            base.Update(gameTime);
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
                if (shakeTime >= MathHelper.Pi * 15)
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


                /*#### This is the part that needs fixing - currently the up vector tilts itself based on the world axes, 
                 * so the strength of the tilt is relative to the angle of the ship (how close to pure x or z it's facing)
                 * What it needs to do is rotate on the camera's 'roll'/Z axis - relative to the direction the camera is facing - my guess is you'll have to use currentDirection in some form, but what I've tried so far doesn't work
                 * 
                 * Note that tiltUp only updates when this function is being called, so be careful of the camera suddenly turning sideways or upside down when you rotate far enough in either direction (I've had that happen when doing stuff with currentDirection)
                 * You'll have to press O every time you want this function to be called, there's no toggle so it'll reset every frame you have O held down but that's fine for this testing situation
                 */
                //tiltUp = currentDirection;
                //tiltUp = Vector3.Cross(currentDirection, Vector3.Forward);

                tiltUp.Z = (float)Math.Sin(tiltAngle);
                tiltUp.X = (float)Math.Sin(tiltAngle);
                //tiltUp.Y = (float)Math.Cos(tiltAngle);
                tiltUp.Normalize();
            }



        }

        public void reset()
        {
            currentDirection = ship.direction;
            currentFov = standardFov;
        }

        private void CreateLookAt()
        {
            view = Matrix.CreateLookAt(cameraPosition, lookAt, tiltUp);
        }

    }
}
