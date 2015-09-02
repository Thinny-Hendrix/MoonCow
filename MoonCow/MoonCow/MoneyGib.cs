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
        Model model;
        OOBB boundingBox;
        Ship ship;
        MoneyManager moneyManager;
        Vector3 initDirection;
        Vector3 currentDirection;
        Vector3 targetDirection;
        Vector3 frameDiff;
        float speed;
        float speedMax;
        float value;
        float yAngle;
        float xAngle;
        bool collected;

        public MoneyGib(float value, Model model, MoneyManager moneyManager, Ship ship, Vector3 pos) : base(model)
        {
            this.value = value;
            this.model = model;
            this.pos = pos;
            this.moneyManager = moneyManager;
            this.ship = ship;
            scale = new Vector3(1, 1, 1);

            initDirection.X = (float)Utilities.random.NextDouble()*2-1;
            initDirection.Y = (float)Utilities.random.NextDouble()*2-1;
            initDirection.Z = (float)Utilities.random.NextDouble()*2-1;
            initDirection.Normalize();

            currentDirection = initDirection;

            speed = 10;

            boundingBox = new OOBB(pos, currentDirection, .1f, .1f);
            collected = false;
        }

        public override void Update(GameTime gameTime)
        {
            //first shoot in direction, over time change to move towards ship pos

            //calculate target direction
            //targetDirection.X = pos.X - ship.pos.X;
            //targetDirection.Y = pos.Y - ship.pos.Y;
            //targetDirection.Z = pos.Z - ship.pos.Z;
            //targetDirection.Normalize();

            yAngle = (float)Math.Atan2(pos.X - ship.pos.X, pos.Z - ship.pos.Z);
            xAngle = (float)Math.Atan2(pos.Y - ship.pos.Y, pos.Z - ship.pos.Z);
            targetDirection.X = -(float)Math.Sin(yAngle);
            targetDirection.Z = -(float)Math.Cos(yAngle);
            targetDirection.Y = -(float)Math.Sin(xAngle);
            targetDirection.Normalize();



            //currentDirection = targetDirection;
            currentDirection = Vector3.Lerp(currentDirection, targetDirection, Utilities.deltaTime * 4);
            //currentDirection.Normalize();
            frameDiff += currentDirection * speed * Utilities.deltaTime;
            //pos = Vector3.Lerp(pos, ship.pos, Utilities.deltaTime*3);

            speed += Utilities.deltaTime*4;
            checkCollision();
            frameDiff = Vector3.Zero;

            base.Update(gameTime);
        }

        public override void Draw(GraphicsDevice device, Camera camera)
        {
            base.Draw(device, camera);
        }

        void checkCollision()
        {
            // By moving each component of the vector one at a time and seeing what causes the collision we can eliminate only that component
            // this means the ship will slide along walls rather than stick. Doing two collision checks per frame for the player seems to
            // be within tolerable limits for CPU time. This will only need to be done with the player
            pos.Y += frameDiff.Y;

            pos.X += frameDiff.X;

            //## COLLISIONS WHOOO! ##
            // Move the bounding box to new pos
            boundingBox.Update(pos, currentDirection);
            // Get current node co-ordinates
            Vector2 nodePos = new Vector2((int)((pos.X / 30) + 0.5f), (int)((pos.Z / 30) + 0.5f));

            //For the current node check if your X component will make you collide with wall
            try
            {
                if (boundingBox.intersects(ship.boundingBox))
                {
                    collectGib();
                }
            }
            catch (IndexOutOfRangeException){}

            pos.Z += frameDiff.Z;
            if (!collected)
            {
                boundingBox.Update(pos, currentDirection);
                nodePos = new Vector2((int)((pos.X / 30) + 0.5f), (int)((pos.Z / 30) + 0.5f));

                try
                {
                    if (boundingBox.intersects(ship.boundingBox))
                    {
                        collectGib();
                    }
                }
                catch (IndexOutOfRangeException){}
            }
        }

        void collectGib()
        {
            moneyManager.addMoney(value);
            moneyManager.toDelete.Add(this);
            collected = true;
        }


    }
}
