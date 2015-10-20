using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    class Heavy : Enemy
    {
        protected Pathfinder pathfinder;
        protected List<Vector2> path = new List<Vector2>();
        protected Point coreLocation;
        protected Vector3 target;
        Vector3 frameDiff = new Vector3(0, 0, 0);
        CircleCollider attackCol;
        float attackTime;
        public enum State { goToBase, travelAttack, atBase, atCore, attackCore, strongHit, knockBack, hitByDrill }
        State state;

        public Heavy(Game1 game)
            : base(game)
        {
            this.game = game;
            maxSpeed = 5.5f;
            moveSpeed = maxSpeed;
            direction = new Vector3(0, (float)Math.PI, 0);
            rot = direction;
            currentTurnSpeed = 0;
            maxTurnSpeed = MathHelper.PiOver4 / (59f + Utilities.nextFloat() * 62f);

            //enemyModel = new EnemyModel(game.Content.Load<Model>(@"Models/Ship/shipBlock"), this);
            enemyModel = new HeavyModel(this);
            enemyType = 3;

            state = State.goToBase;

            game.modelManager.addEnemy(enemyModel);

            List<Vector2> spawns = game.map.getEnemySpawn();
            int value = Utilities.random.Next(spawns.Count);

            //essentially, if there's more than 1 spawn point, pick a random spawn point to spawn at.
            Vector2 spawn = new Vector2(spawns[value].X, spawns[value].Y);
            prevPosition = spawn;

            pos = new Vector3(makeCentreCoordinate(spawn.X), 4.5f, makeCentreCoordinate(spawn.Y));

            switch (EnemyBehaviour.heavyBehaviour)
            {
                case EnemyBehaviour.Behaviour.ShortestPathFirst:
                    pathfinder = new Pathfinder(game.map);
                    break;
                case EnemyBehaviour.Behaviour.AvoidTurretDamage:
                    pathfinder = new PathfinderTurretAvoid(game.map);
                    break;
                case EnemyBehaviour.Behaviour.AvoidPlayerDamage:
                    pathfinder = new PathfinderPlayerAvoid(game.map);
                    break;
                default:
                    pathfinder = new Pathfinder(game.map);
                    break;
            }

            coreLocation = new Point((int)game.map.getCoreLocation().X, (int)game.map.getCoreLocation().Y);

            path = pathfinder.findPath(new Point((int)spawn.X, (int)spawn.Y), coreLocation);

            target = pos;

            //System.Diagnostics.Debug.WriteLine(path);

            boundingBox = new OOBB(pos, direction, 1.5f, 1.5f);
            agroSphere = new CircleCollider(new Vector2(pos.X, pos.Z), 7f);
            attackCol = new CircleCollider(pos, 5f);

            health = 50;

            cols.Add(new CircleCollider(pos, 2.5f));
            cols.Add(new CircleCollider(pos + Vector3.Cross(direction, Vector3.Up)*3, 2.5f));
            cols.Add(new CircleCollider(pos + Vector3.Cross(direction, Vector3.Up) * -3, 2.5f));


            //weapons = new WeaponSystem(this);
        }

        public override void Update(GameTime gameTime)
        {
            //target = game.ship.pos;
            //target = new Vector3(makeCentreCoordinate(coreLocation.X), 4.5f, makeCentreCoordinate(coreLocation.Y));

            //Agro stuff and attacks
            if (!electroDamage.active)
            {
                if(state == State.goToBase)
                {
                    goToBase();
                    agroSphere.Update(pos);
                    if(agroSphere.checkCircle(game.ship.circleCol))
                    {
                        state = State.travelAttack;
                        attackTime = 0;
                    }
                }
                else if (state == State.travelAttack)
                {
                    goToBase();
                    agroSphere.Update(pos);
                    attackCol.Update(pos);
                    attackTime += Utilities.deltaTime;
                    if(attackTime > 0.5f)
                    {
                        if(attackCol.checkCircle(game.ship.circleCol))
                        {
                            game.ship.shipHealth.onHit(Utilities.deltaTime * 20);
                        }
                    }
                    if(attackTime > 2)
                    {
                        state = State.goToBase;
                    }
                }

                nodePos = new Vector2((int)((pos.X / 30) + 0.5f), (int)((pos.Z / 30) + 0.5f));
                boundingBox.Update(pos, direction);

                pos.Y = 4.5f + (float)Math.Sin(time) * 0.2f;

                time += Utilities.deltaTime * MathHelper.Pi * 2.4f;

                if (time > MathHelper.Pi * 2)
                    time -= MathHelper.Pi * 2;
            }

            base.Update(gameTime);

            if (health <= 0)
            {
                death();
            }
        }

        void goToBase()
        {
            float yAngle;
            Vector3 targetDirection = Vector3.Zero;
            agroSphere.Update(pos);
            if (agroSphere.checkPoint(new Vector2(game.ship.pos.X, game.ship.pos.Z)))
            {
                //Agro mode active, begin doing collision checks on player and chase
                //System.Diagnostics.Debug.WriteLine("Enemy now aggressive");
            }

            if ((pos.X < target.X + 1 && pos.X > target.X - 1) && (pos.Z < target.Z + 1 && pos.Z > target.Z - 1))
            {
                if (path.Count > pathPosition)
                {
                    prevPosition = nextPosition;
                    nextPosition = path[pathPosition];
                    pathPosition += 1;

                    if (path.Count > pathPosition)
                    {
                        target = new Vector3(makeCentreCoordinate(nextPosition.X), 4.5f, makeCentreCoordinate(nextPosition.Y));
                    }
                    else if (path.Count == pathPosition)
                    {
                        // How do I get the enemies to surround the core without going through it?
                        // Answering this question will be post alpha
                        if (prevPosition.X < nextPosition.X)
                        {
                            target = new Vector3(makeCentreCoordinate(nextPosition.X), 4.5f, makeCentreCoordinate(nextPosition.Y));
                        }
                        else if (prevPosition.X > nextPosition.X)
                        {
                            target = new Vector3(makeCentreCoordinate(nextPosition.X), 4.5f, makeCentreCoordinate(nextPosition.Y));
                        }
                        else if (prevPosition.Y < nextPosition.Y)
                        {
                            target = new Vector3(makeCentreCoordinate(nextPosition.X), 4.5f, makeCentreCoordinate(nextPosition.Y));
                        }
                        else if (prevPosition.Y > nextPosition.Y)
                        {
                            target = new Vector3(makeCentreCoordinate(nextPosition.X), 4.5f, makeCentreCoordinate(nextPosition.Y));
                        }
                    }
                }
                else
                {
                    atCore = true;
                    target = new Vector3(makeCentreCoordinate(nextPosition.X), 4.5f, makeCentreCoordinate(nextPosition.Y));
                }
            }

            //Turning Code
            yAngle = (float)Math.Atan2(pos.X - target.X, pos.Z - target.Z);
            targetDirection.X = -(float)Math.Sin(yAngle);
            targetDirection.Z = -(float)Math.Cos(yAngle);
            targetDirection.Normalize();

            direction = Vector3.Lerp(direction, targetDirection, Utilities.deltaTime * 4.5f);
            rot.Y = (float)Math.Atan2(direction.X, direction.Z) + MathHelper.Pi;

            //Movement Code
            if (!atCore)
            {
                frameDiff += direction * moveSpeed * Utilities.deltaTime;

                if (moveSpeed < maxSpeed)
                {
                    moveSpeed += Utilities.deltaTime * 6;
                }

                checkCollision();
                frameDiff = Vector3.Zero;
            }
            else
            {
                //need slowdown code
                moveSpeed = 0;

            }
        }

        protected override void death()
        {
            for (int i = 0; i < 10; i++)
                game.modelManager.addEffect(new GlowStreak(game, pos, new Vector2(2, 15), 2, Color.White, 0, 3));
            game.modelManager.addEffect(new GlowStreakCenter(game, pos, 10, 2, 3));

            game.ship.moneyManager.makeMoney(500, 3, pos);
            

            game.modelManager.removeEnemy(enemyModel);
            game.enemyManager.toDelete.Add(this);
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
            boundingBox.Update(pos, direction);

            cols.ElementAt(0).Update(pos);
            cols.ElementAt(1).Update(pos + Vector3.Cross(direction, Vector3.Up) * 3);
            cols.ElementAt(2).Update(pos + Vector3.Cross(direction, Vector3.Up) * -3);

            // Get current node co-ordinates
            Vector2 nodePos = new Vector2((int)((pos.X / 30) + 0.5f), (int)((pos.Z / 30) + 0.5f));

            //For the current node check if your X component will make you collide with wall

            pos.Z += frameDiff.Z;

            boundingBox.Update(pos, direction);
            cols.ElementAt(0).Update(pos);
            cols.ElementAt(1).Update(pos + Vector3.Cross(direction, Vector3.Up) * 3);
            cols.ElementAt(2).Update(pos + Vector3.Cross(direction, Vector3.Up) * -3);
            nodePos = new Vector2((int)((pos.X / 30) + 0.5f), (int)((pos.Z / 30) + 0.5f));
        }

        public override void updatePath()
        {
            path = pathfinder.findPath(new Point((int)makePointCoordinate(nextPosition.X), (int)makePointCoordinate(nextPosition.Y)), coreLocation);
            pathPosition = 0;
            atCore = false;
        }
    }
}