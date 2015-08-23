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
        Vector3 cameraDirection;
        Vector3 cameraUp;

        float speed;

        MouseState prevMouseState;

        public Camera(Game game, Vector3 pos, Vector3 target, Vector3 up)
            : base(game)
        {
            // Build camera view matrix
            cameraPosition = pos;
            cameraDirection = target - pos;
            cameraDirection.Normalize();
            cameraUp = up;
            CreateLookAt();
            projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, (float)Game.Window.ClientBounds.Width /
                (float)Game.Window.ClientBounds.Height, 1, 3000);
            speed = 3.0f;
        }

        public override void Initialize()
        {
            // Set mouse position and do initial get state
            Mouse.SetPosition(Game.Window.ClientBounds.Width / 2, Game.Window.ClientBounds.Height / 2);
            prevMouseState = Mouse.GetState();
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            // here for moving camera
            // Move forward/backward
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                cameraPosition += cameraDirection * speed; // for flying cam
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                cameraPosition -= cameraDirection * speed; // for flying cam
            }
            // Move side to side
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                cameraPosition += Vector3.Cross(cameraUp, cameraDirection) * speed;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                cameraPosition -= Vector3.Cross(cameraUp, cameraDirection) * speed;
            }

            // Recreate the camera view matrix
            CreateLookAt();

            // Yaw rotation
            float yawAngle = (-MathHelper.PiOver4 / 150) * (Mouse.GetState().X - prevMouseState.X);
            cameraDirection = Vector3.Transform(cameraDirection, Matrix.CreateFromAxisAngle(cameraUp, yawAngle));


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

            // Pitch rotation
            float pitchAngle = (MathHelper.PiOver4 / 150) * (Mouse.GetState().Y - prevMouseState.Y);

            cameraDirection = Vector3.Transform(cameraDirection, Matrix.CreateFromAxisAngle(Vector3.Cross(cameraUp, cameraDirection), pitchAngle));

            // Reset prevMouseState
            prevMouseState = Mouse.GetState();

            base.Update(gameTime);
        }

        private void CreateLookAt()
        {
            view = Matrix.CreateLookAt(cameraPosition, cameraPosition + cameraDirection, cameraUp);
        }

    }
}
