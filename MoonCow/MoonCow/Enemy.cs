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
        float time;

        //All to do with turning;
        public float currentTurnSpeed;
        public float maxTurnSpeed;
        bool atCore = false;
        Vector3 nodeDirection = new Vector3(0, 0, 1);
        bool turnDirection = false; //false = left, true = right
        char isFacing = 'S'; //North, South, East, West
        char wasFacing = 'S';

        public OOBB boundingBox;
        public int health;

        public Vector2 nodePos;

        Game1 game;

        Pathfinder pathfinder;
        List<Vector2> path = new List<Vector2>();
        Point coreLocation;

        Vector2 prevPosition;
        Vector2 nextPosition;
        //Vector2 futurePosition; - May need in future

        int pathPosition = 0;

        public EnemyModel enemyModel;
        //WeaponSystem weapons;

        public Enemy(Game1 game)
        {
            this.game = game;
            moveSpeed = 0;
            maxSpeed = 0.11f;
            direction = new Vector3(0, 0, 1);
            rot = new Vector3(0, (float)Math.PI, 0);
            currentTurnSpeed = 0;
            maxTurnSpeed = MathHelper.PiOver4 / 90;

            //enemyModel = new EnemyModel(game.Content.Load<Model>(@"Models/Ship/shipBlock"), this);
            enemyModel = new EnemyModel(game.Content.Load<Model>(@"Models/Enemies/Swarmer/swarmerAlpha"), this);

            game.modelManager.addEnemy(enemyModel);

            List<Vector2> spawns = game.map.getEnemySpawn();
            int value = Utilities.random.Next(spawns.Count);

            //essentially, if there's more than 1 spawn point, pick a random spawn point to spawn at.
            Vector2 spawn = new Vector2(spawns[value].X, spawns[value].Y);
            prevPosition = spawn;

            pos = new Vector3(makeCentreCoordinate(spawn.X), 4.5f, makeCentreCoordinate(spawn.Y));

            pathfinder = new Pathfinder(game.map);

            coreLocation = new Point((int)game.map.getCoreLocation().X, (int)game.map.getCoreLocation().Y);

            path = pathfinder.findPath(new Point((int)spawn.X, (int)spawn.Y), coreLocation);

            //System.Diagnostics.Debug.WriteLine(path);

            boundingBox = new OOBB(pos, direction, 1.5f, 1.5f);

            health = 15;

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
            nextPosition = path[pathPosition];

            if ((pos.X < makeCentreCoordinate(nextPosition.X) + 13 && pos.X > makeCentreCoordinate(nextPosition.X) - 13) && (pos.Z < makeCentreCoordinate(nextPosition.Y) + 13 && pos.Z > makeCentreCoordinate(nextPosition.Y) - 13))
            {
                //System.Diagnostics.Debug.WriteLine("Next Direction!");

                if ((path.Count - 1) > pathPosition)
                {
                    prevPosition = nextPosition;
                    pathPosition += 1;
                    wasFacing = isFacing;
                }
                else
                {
                    atCore = true;
                }
            }
            else
            {
                if (nextPosition.X > prevPosition.X)
                {
                    //System.Diagnostics.Debug.WriteLine("Face East");

                    nodeDirection.X = -(float)Math.Sin(0);
                    nodeDirection.Z = -(float)Math.Cos(-1.8);

                    isFacing = 'E';
                }
                else if (nextPosition.X < prevPosition.X)
                {
                    //System.Diagnostics.Debug.WriteLine("Face West");

                    nodeDirection.X = -(float)Math.Sin(0);
                    nodeDirection.Z = -(float)Math.Cos(1.8);

                    isFacing = 'W';
                }
                else if (nextPosition.Y > prevPosition.Y)
                {
                    //System.Diagnostics.Debug.WriteLine("Face South");

                    nodeDirection.X = -(float)Math.Sin(0);
                    nodeDirection.Z = -(float)Math.Cos(Math.PI);

                    isFacing = 'S';
                }
                else if (nextPosition.Y < prevPosition.Y)
                {
                    //System.Diagnostics.Debug.WriteLine("Face North");

                    nodeDirection.X = -(float)Math.Sin(0);
                    nodeDirection.Z = -(float)Math.Cos(0);

                    isFacing = 'N';
                }

                //nodeDirection.Normalize();
            }

            //Turning Code
            if ((wasFacing == 'N' || wasFacing == 'S') && (pos.X <= makeCentreCoordinate(nextPosition.X) + 13 && pos.X >= makeCentreCoordinate(nextPosition.X) + 7.5))
                evenOut(false);
            else if ((wasFacing == 'E' || wasFacing == 'W') && (pos.Z <= makeCentreCoordinate(nextPosition.Y) + 13 && pos.Z >= makeCentreCoordinate(nextPosition.Y) + 7.5))
                evenOut(false);
            else if ((wasFacing == 'N' || wasFacing == 'S') && (pos.X <= makeCentreCoordinate(nextPosition.X) - 7.5 && pos.X >= makeCentreCoordinate(nextPosition.X) - 13))
                evenOut(true);
            else if ((wasFacing == 'E' || wasFacing == 'W') && (pos.Z <= makeCentreCoordinate(nextPosition.Y) - 7.5 && pos.Z >= makeCentreCoordinate(nextPosition.Y) - 13))
                evenOut(true);

            if (!(direction.Z > (nodeDirection.Z - 0.005f) && direction.Z < (nodeDirection.Z + 0.005f)))
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

            //System.Diagnostics.Debug.WriteLine(nodeDirection.X + ", " + nodeDirection.Z);
            //System.Diagnostics.Debug.WriteLine(direction.X + ",, " + direction.Z);

            //Movement Code

            if (!atCore)
            {
                moveSpeed = MathHelper.Lerp(moveSpeed, maxSpeed, Utilities.deltaTime * 3);
            }
            else
            {
                //need slowdown code
                moveSpeed = 0;
            }
            pos.X += direction.X * moveSpeed;
            pos.Z += direction.Z * moveSpeed;
            nodePos = new Vector2((int)((pos.X / 30) + 0.5f), (int)((pos.Z / 30) + 0.5f));
            boundingBox.Update(pos, direction);

            pos.Y = 4.5f + (float)Math.Sin(time)*0.2f;
            time += Utilities.deltaTime * MathHelper.Pi*2.4f;
            if (time > MathHelper.Pi * 2)
                time -= MathHelper.Pi * 2;

            if(health <= 0)
            {
                death();
            }
        }

        private void death()
        {
            for (int i = 0; i < 4; i++)
                game.ship.moneyManager.addGib(5, pos);

            game.modelManager.removeEnemy(enemyModel);
            game.enemyManager.toDelete.Add(this);
        }

        private void evenOut(bool invert)
        {
            if (!atCore)
            {
                if (invert)
                {
                    if (turnDirection)
                        rot.Y += currentTurnSpeed;
                    else
                        rot.Y -= currentTurnSpeed;
                }
                else
                {
                    if (turnDirection)
                        rot.Y -= currentTurnSpeed;
                    else
                        rot.Y += currentTurnSpeed;
                }

                direction.X = -(float)Math.Sin(rot.Y);
                direction.Z = -(float)Math.Cos(rot.Y);
                direction.Normalize();
            }
        }

        public void updatePath()
        {
            path = pathfinder.findPath(new Point((int)makePointCoordinate(nextPosition.X), (int)makePointCoordinate(nextPosition.Y)), coreLocation);
            pathPosition = 0;
            atCore = false;
        }
    }
}