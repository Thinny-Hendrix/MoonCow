using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    public class Enemy
    {
        public Matrix transform;
        public Vector3 pos;
        public Vector3 rot;
        public Vector3 scale;
        public Vector3 direction;

        public float moveSpeed;
        public float maxSpeed;

        //All to do with turning;
        public float currentTurnSpeed;
        public float maxTurnSpeed;
        float turnAmount = 0;
        Vector3 nodeDirection = new Vector3(0, 0, 1);
        bool turnDirection = false; //false = left, true = right
        char isFacing = 'S'; //N, S, E, W
        char wasFacing = 'S';


        public Vector2 nodePos;

        Game1 game;

        Pathfinder pathfinder;
        List<Vector2> path = new List<Vector2>();
        Point coreLocation;

        Vector2 prevPosition;
        Vector2 nextPosition;
        Vector2 futurePosition;

        int pathPosition = 0;

        public EnemyModel enemyModel;
        //WeaponSystem weapons;

        public Enemy(Game1 currentGame)
        {
            game = currentGame;
            moveSpeed = 0;
            maxSpeed = 0.08f;
            direction = new Vector3(0, 0, 1);
            rot = new Vector3(0, (float)Math.PI, 0);
            currentTurnSpeed = 0;
            maxTurnSpeed = MathHelper.PiOver4 / 110;

            enemyModel = new EnemyModel(game.Content.Load<Model>(@"Models/Ship/shipBlock"), this);
            //enemyModel = new ShipModel(game.Content.Load<Model>(@"Models/Enemies/Sneaker/sneakproto"), this);

            game.modelManager.add(enemyModel);

            Random rnd = new Random();
            List<Vector2> spawns = game.map.getEnemySpawn();
            int value = rnd.Next(spawns.Count);

            //essentially, if there's more than 1 spawn point, pick a random spawn point to spawn at.
            Vector2 spawn = new Vector2(spawns[value].X, spawns[value].Y);
            prevPosition = spawn;

            pos = new Vector3(makeCentreCoordinate(spawn.X), 4.5f, makeCentreCoordinate(spawn.Y));

            pathfinder = new Pathfinder(game.map);

            coreLocation = new Point((int)game.map.getCoreLocation().X, (int)game.map.getCoreLocation().Y);

            path = pathfinder.findPath(new Point((int)spawn.X, (int)spawn.Y), coreLocation);

            //System.Diagnostics.Debug.WriteLine(path);

            //weapons = new WeaponSystem(this);
        }

        private float makeCentreCoordinate(float c)
        {
            return (c) * 30f;
        }

        private float makePointCoordinate(float c)
        {
            return (c)/30f;
        }

        public void Update(GameTime gameTime)
        {
            //current plan is this - set the coordinate for the next node position, then face that direction
            //travel to the 'next position' - once next position is reached, set the next position to be the next vector - repeat.
            nextPosition = path[pathPosition];

            System.Diagnostics.Debug.WriteLine(pos.X + " in " + (makeCentreCoordinate(nextPosition.X) + 10) +  " or " + (makeCentreCoordinate(nextPosition.X) - 10));
            System.Diagnostics.Debug.WriteLine(pos.Z + " in " + (makeCentreCoordinate(nextPosition.Y) + 10) + " or " + (makeCentreCoordinate(nextPosition.Y) - 10));

            if ((pos.X < makeCentreCoordinate(nextPosition.X) + 15 && pos.X > makeCentreCoordinate(nextPosition.X) - 15) && (pos.Z < makeCentreCoordinate(nextPosition.Y) + 15 && pos.Z > makeCentreCoordinate(nextPosition.Y) - 15))
            {
                System.Diagnostics.Debug.WriteLine("Next Direction!");

                if ((path.Count - 1) > pathPosition)
                {
                    prevPosition = nextPosition;
                    pathPosition += 1;
                    wasFacing = isFacing;
                }
                else
                {

                }
            }
            else
            {
                if (nextPosition.X > prevPosition.X)
                {
                    System.Diagnostics.Debug.WriteLine("Face East");

                    nodeDirection.X = -(float)Math.Sin(0);
                    nodeDirection.Z = -(float)Math.Cos(-1.8);

                    isFacing = 'E';
                }
                else if (nextPosition.X < prevPosition.X)
                {
                    System.Diagnostics.Debug.WriteLine("Face West");

                    nodeDirection.X = -(float)Math.Sin(0);
                    nodeDirection.Z = -(float)Math.Cos(1.8);

                    isFacing = 'W';
                }
                else if (nextPosition.Y > prevPosition.Y)
                {
                    System.Diagnostics.Debug.WriteLine("Face South");

                    nodeDirection.X = -(float)Math.Sin(0);
                    nodeDirection.Z = -(float)Math.Cos(Math.PI);

                    isFacing = 'S';
                }
                else if (nextPosition.Y < prevPosition.Y)
                {
                    System.Diagnostics.Debug.WriteLine("Face North");

                    nodeDirection.X = -(float)Math.Sin(0);
                    nodeDirection.Z = -(float)Math.Cos(0);

                    isFacing = 'N';
                }

                //nodeDirection.Normalize();
            }

            //Turning Code
            if (!(direction.Z > (nodeDirection.Z - 0.001f) && direction.Z < (nodeDirection.Z + 0.001f)))
            {
                currentTurnSpeed = MathHelper.Lerp(currentTurnSpeed, maxTurnSpeed, Utilities.deltaTime * 10);

                //rot.Z = tilt;
                switch (wasFacing)
                {
                    case 'N': 
                        switch (isFacing)
                        {
                            case 'E': turnDirection = true; break;
                            case 'W': turnDirection = false; break;
                        }
                        break;
                    case 'S':
                        switch (isFacing)
                        {
                            case 'E': turnDirection = false; break;
                            case 'W': turnDirection = true; break;
                        }
                        break;
                    case 'E':
                        switch (isFacing)
                        {
                            case 'N': turnDirection = false; break;
                            case 'S': turnDirection = true; break;
                        }
                        break;
                    case 'W':
                        switch (isFacing)
                        {
                            case 'N': turnDirection = true; break;
                            case 'S': turnDirection = false; break;
                        }
                        break;
                }

                if (turnDirection)
                    rot.Y -= currentTurnSpeed;
                else
                    rot.Y += currentTurnSpeed;

                direction.X = -(float)Math.Sin(rot.Y);
                direction.Z = -(float)Math.Cos(rot.Y);
                direction.Normalize();
            }

            System.Diagnostics.Debug.WriteLine(nodeDirection.X + ", " + nodeDirection.Z);
            System.Diagnostics.Debug.WriteLine(direction.X + ",, " + direction.Z);

            //Movement Code
            moveSpeed = MathHelper.Lerp(moveSpeed, maxSpeed, Utilities.deltaTime * 3);

            //pos = new Vector3(makeCentreCoordinate(nextPosition.X), 4.5f, makeCentreCoordinate(nextPosition.Y));
            
            //(if not at core)
            pos.X += direction.X * moveSpeed;
            pos.Z += direction.Z * moveSpeed;
            //end if
        }

        public void updatePath()
        {
            path = pathfinder.findPath(new Point((int)makePointCoordinate(nextPosition.X), (int)makePointCoordinate(nextPosition.Y)), coreLocation);
            pathPosition = 0;
        }
    }
}