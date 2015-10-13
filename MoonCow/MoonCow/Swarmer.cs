using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    class Swarmer : Enemy
    {
        protected Pathfinder pathfinder;
        protected List<Vector2> path = new List<Vector2>();
        protected Point coreLocation;
        protected Vector3 target;
        Vector3 frameDiff = new Vector3(0, 0, 0);
        public enum State { goToBase, atBase, atCore, attackCore, noticedPlayer, goToPlayer, atPlayer, attackPlayer, strongHit, knockBack, hitByDrill}
        public State state;
        public State nextState;

        public Swarmer(Game1 game)
            : base(game)
        {
            this.game = game;
            moveSpeed = 0;
            maxSpeed = 10 + Utilities.nextFloat();
            direction = new Vector3(0, (float)Math.PI, 0);
            rot = direction;
            currentTurnSpeed = 0;
            maxTurnSpeed = MathHelper.PiOver4 / (59f + Utilities.nextFloat() * 62f);
            state = State.goToBase;

            //enemyModel = new EnemyModel(game.Content.Load<Model>(@"Models/Ship/shipBlock"), this);
            enemyModel = new SwarmerModel(this);
            enemyType = 1;

            game.modelManager.addEnemy(enemyModel);

            List<Vector2> spawns = game.map.getEnemySpawn();
            int value = Utilities.random.Next(spawns.Count);

            //essentially, if there's more than 1 spawn point, pick a random spawn point to spawn at.
            Vector2 spawn = new Vector2(spawns[value].X, spawns[value].Y);
            prevPosition = spawn;

            pos = new Vector3(makeCentreCoordinate(spawn.X), 3.5f, makeCentreCoordinate(spawn.Y));

            switch (EnemyBehaviour.swarmerBehaviour)
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
            agroSphere = new CircleCollider(pos, 10f);
            cols.Add(new CircleCollider(pos, 0.6f));

            health = 15;

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
                }

                if(state == State.atCore)
                {
                    game.core.damage(0.1f);
                }

                agroSphere.Update(pos);


                if(state == State.hitByDrill)
                {
                    pos += knockDir * Utilities.deltaTime * 40;
                    checkCollisions();
                }


                nodePos = new Vector2((int)((pos.X / 30) + 0.5f), (int)((pos.Z / 30) + 0.5f));
                boundingBox.Update(pos, direction);
                foreach (CircleCollider c in cols)
                    c.Update(pos);

                /*
                pos.Y = 4.5f + (float)Math.Sin(time) * 0.2f;

                time += Utilities.deltaTime * MathHelper.Pi * 2.4f;

                if (time > MathHelper.Pi * 2)
                    time -= MathHelper.Pi * 2;*/
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
                        target = new Vector3(makeCentreCoordinate(nextPosition.X) + (-5 + Utilities.nextFloat() * 10), 4.5f, makeCentreCoordinate(nextPosition.Y) + (-5 + Utilities.nextFloat() * 10));
                    }
                    else if (path.Count == pathPosition)
                    {
                        // How do I get the enemies to surround the core without going through it?
                        // Answering this question will be post alpha
                        if (prevPosition.X < nextPosition.X)
                        {
                            target = new Vector3(makeCentreCoordinate(nextPosition.X) + (-14 + Utilities.nextFloat() * 3), 4.5f, makeCentreCoordinate(nextPosition.Y) + (-10 + Utilities.nextFloat() * 20));
                        }
                        else if (prevPosition.X > nextPosition.X)
                        {
                            target = new Vector3(makeCentreCoordinate(nextPosition.X) + (14 + Utilities.nextFloat() * 3), 4.5f, makeCentreCoordinate(nextPosition.Y) + (-10 + Utilities.nextFloat() * 20));
                        }
                        else if (prevPosition.Y < nextPosition.Y)
                        {
                            target = new Vector3(makeCentreCoordinate(nextPosition.X) + (-10 + Utilities.nextFloat() * 20), 4.5f, makeCentreCoordinate(nextPosition.Y) + (-14 + Utilities.nextFloat() * 3));
                        }
                        else if (prevPosition.Y > nextPosition.Y)
                        {
                            target = new Vector3(makeCentreCoordinate(nextPosition.X) + (-10 + Utilities.nextFloat() * 20), 4.5f, makeCentreCoordinate(nextPosition.Y) + (14 + Utilities.nextFloat() * 3));
                        }
                    }
                }
                else
                {
                    atCore = true;
                    state = State.atCore;
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

        public override void drillDamage(float damage, Vector3 dir, bool boosting)
        {
            health -= damage;
            if (health <= 0)
                death();

            if (boosting)
            {
                game.camera.setYShake(0.1f);
                state = State.hitByDrill;
                knockDir = dir + cols.ElementAt(0).directionFrom(game.ship.pos);
                knockDir.Normalize();
            }
            else
            {
                game.camera.setYShake(0.03f);
                //state = State.hit;
            }

            /*if (timeSinceLastDrill <= 0)
            {
                timeSinceLastDrill = 0.2f;
                model.hit(dir);
                shockTime = 0;
                model.wake();
            }
            triggeredTele = false;
            cooldownTime = 2;*/
        }

        protected override void death()
        {
            for (int i = 0; i < 4; i++)
                game.ship.moneyManager.addGib(5, pos);

            if (Utilities.random.Next(5) == 0)
                game.ship.moneyManager.addAmmoGib(pos);

            enemyModel.Dispose();
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
            // Get current node co-ordinates
            Vector2 nodePos = new Vector2((int)((pos.X / 30) + 0.5f), (int)((pos.Z / 30) + 0.5f));

            //For the current node check if your X component will make you collide with wall

            pos.Z += frameDiff.Z;

            boundingBox.Update(pos, direction);
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