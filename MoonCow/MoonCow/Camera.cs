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
        public Matrix view { get; protected set; }
        public Matrix projection { get; protected set; }

        // Camera vectors
        public Vector3 cameraPosition;
        Vector3 lookAt;
        Vector3 cameraUp;

        int xTurnValue;
        int yTurnValue;

        float height = 5;       //values can be changed to suit player proportions relative to environment
        double gravity = 0.1;
        double jumpSpeed = 2;
        double verticalSpeed = 0;
        double runSpeed = 0.4;
        int turnRadius = 10; //this is the distance from the camera to the player (lookAt)

        public Camera(Game game, Vector3 pos, Vector3 target, Vector3 up)
            : base(game)
        {
            // Build camera view matrix
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
                Mouse.SetPosition(Game.Window.ClientBounds.Width / 2, Game.Window.ClientBounds.Height / 2);
            }
            catch (NullReferenceException) { }

            if (yTurnValue > 90)
                yTurnValue = 90;
            if (yTurnValue < -90)
                yTurnValue = -90;

            ///######Cheap hack to make lookat the ship pos, overrides the rest of this function
            lookAt = ((Game1)Game).ship.pos;


            cameraPosition.X = lookAt.X + (float)(System.Math.Sin(xTurnValue * MathHelper.Pi / 180)) * turnRadius;
            cameraPosition.Y = lookAt.Y + (float)-(System.Math.Sin(yTurnValue * MathHelper.Pi / 180)) * turnRadius;
            cameraPosition.Z = lookAt.Z + (float)(System.Math.Cos(xTurnValue * MathHelper.Pi / 180)) * turnRadius;

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

        private void CreateLookAt()
        {
            view = Matrix.CreateLookAt(cameraPosition, lookAt, cameraUp);
        }

    }
}
