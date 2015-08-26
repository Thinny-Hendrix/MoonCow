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

        public ShipModel shipModel;

        public bool paused;

        public Ship(Game game) : base(game)
        {
            pos = Vector3.Zero;
            moveSpeed = 0.3f;
            paused = false;
            direction = new Vector3(1, 0, 0);
            currentTurnSpeed = 0.01f;

            shipModel = new ShipModel(game.Content.Load<Model>(@"Models/TempRails/raildaetest"), this);
            ((Game1)Game).modelManager.add(shipModel);
        }

        public override void Initialize()
        {
            

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            
            if (Keyboard.GetState().IsKeyDown(Keys.W) && !paused)
            {
                pos.X += direction.X * moveSpeed;
                pos.Z += direction.Z * moveSpeed;
                System.Diagnostics.Debug.WriteLine("W is pressed - new pos is");


            }

            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                pos.X -= direction.X * moveSpeed;
                pos.Z -= direction.Z * moveSpeed;

            }

            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                rot.Y += currentTurnSpeed;
                direction = Vector3.Transform(direction, Matrix.CreateFromAxisAngle(Vector3.Up, rot.Y));

                //pos.X += Vector3.Cross(Vector3.Up, direction).X * moveSpeed;
                //pos.Z += Vector3.Cross(Vector3.Up, direction).Z * moveSpeed;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                rot.Y -= currentTurnSpeed;
                direction = Vector3.Transform(direction, Matrix.CreateFromAxisAngle(Vector3.Up, rot.Y));
                //pos.X -= Vector3.Cross(Vector3.Up, direction).X * moveSpeed;
                //pos.Z -= Vector3.Cross(Vector3.Up, direction).Z * moveSpeed;
            }
        }



    }
}
