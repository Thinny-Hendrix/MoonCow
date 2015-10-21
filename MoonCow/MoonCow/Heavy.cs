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
        float waitTime;
        public enum State { goToBase, travelAttack, atBase, atCore, attackCore, strongHit, hitByDrill }
        State state;
        State prevState;

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
            electroDamage = new ElectroDamage(this, game, enemyType);

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

            health = 100;

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
                if(state == State.strongHit)
                {
                    waitTime -= Utilities.deltaTime;
                    if(waitTime <= 0)
                    {
                        state = prevState;
                        enemyModel.changeAnim(animIndex);
                    }
                }
                if(state == State.goToBase)
                {
                    goToBase();
                    agroSphere.Update(pos);
                    if(agroSphere.checkCircle(game.ship.circleCol))
                    {
                        enemyModel.changeAnim(1);
                        state = State.travelAttack;
                        attackTime = 0;
                    }

                    if (game.map.getNodeType(nodePos) > 19 && game.map.getNodeType(nodePos) < 35)//base node
                    {
                        state = State.atBase;
                        getCoreSpot();
                    }
                }
                else if (state == State.travelAttack)
                {
                    goToBase();
                    agroSphere.Update(pos);
                    attackCol.Update(pos);
                    attackTime += Utilities.deltaTime;
                    if(attackTime > 1.375f && attackTime < 3)
                    {
                        if(attackCol.checkCircle(game.ship.circleCol))
                        {
                            game.camera.setYShake(0.2f);
                            game.ship.shipHealth.onHit(Utilities.deltaTime * 20);
                        }
                    }
                    if(attackTime > 3.77)
                    {
                        if (agroSphere.checkCircle(game.ship.circleCol))
                        {
                            attackTime = 0;
                        }
                        else
                        {
                            enemyModel.changeAnim(0);
                            state = State.goToBase;
                        }
                    }
                }
                if (state == State.atBase)
                {
                    goToCore();

                    atCore = true;
                    //pos = target;
                }
                else if (state == State.atCore)
                {
                    enemyModel.changeAnim(1);
                    state = State.attackCore;
                    updateMovement();
                }
                else if (state == State.attackCore)
                {
                    game.core.damage(0.05f * Utilities.deltaTime);
                    updateMovement();
                }

                nodePos = new Vector2((int)((pos.X / 30) + 0.5f), (int)((pos.Z / 30) + 0.5f));
                pos.Y = 4.5f;
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

        void getCoreSpot()
        {
            coreSpot = game.core.getSpot(pos);
            if (coreSpot != null)
            {
                target = coreSpot.pos;
                posToCore = game.core.coordsToSpot(coreSpot, pos);
                currentBaseIndex = 0;
                target = posToCore.ElementAt(currentBaseIndex);
                resetDist();
            }
        }

        void resetDist()
        {
            currentDist = 0;
            Vector3 dist = target - pos;
            distToCore = Utilities.hypotenuseOf(dist.X, dist.Z);
        }

        void goToCore()
        {
            frameDiff = Vector3.Zero;
            Vector3 targetDir = target - pos;
            targetDir.Normalize();
            direction = targetDir;
            frameDiff = targetDir * moveSpeed * Utilities.deltaTime;
            currentDist += moveSpeed * Utilities.deltaTime;

            if (currentDist > distToCore)
            {
                if (currentBaseIndex >= posToCore.Count() - 1)
                {
                    pos = target;
                    state = State.atCore;
                }
                else
                {
                    currentBaseIndex++;
                    target = posToCore.ElementAt(currentBaseIndex);
                    resetDist();
                }

            }


            updateMovement();
            frameDiff = Vector3.Zero;
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

        public override void startElectro()
        {
            {
                prevState = State.goToBase;
                animIndex = 0;
            }
            enemyModel.changeAnim(3);
        }

        public override void endElectro()
        {
            enemyModel.changeAnim(animIndex);
            if (state != prevState)
            {
                state = prevState;
            }
        }
        public override void damage(float damage)
        {
            if(damage > 10)
            {
                animIndex = enemyModel.activeIndex;
                prevState = state;
                if(prevState == State.travelAttack)
                {
                    prevState = State.goToBase;
                    animIndex = 0;
                }
                state = State.strongHit;
                waitTime = 0.875f;
                enemyModel.changeAnim(2);
            }
            base.damage(damage);
        }

        protected override void death()
        {
            for (int i = 0; i < 10; i++)
                game.modelManager.addEffect(new GlowStreak(game, pos, new Vector2(2, 15), 2, Color.White, 0, 3));
            game.modelManager.addEffect(new GlowStreakCenter(game, pos, 10, 2, 3));

            game.ship.moneyManager.makeMoney(500, 3, pos);

            if (Utilities.random.Next(5) == 0)
                game.ship.moneyManager.addAmmoGib(pos);
            

            game.modelManager.removeEnemy(enemyModel);
            game.enemyManager.toDelete.Add(this);
        }

        void updateMovement()
        {
            pos += frameDiff;
            List<Vector3> normals = checkNormalCollision();
            if (normals.Count > 0)
            {
                for (int i = 0; i < normals.Count(); i++)
                {
                    Vector3 normalForce = Vector3.Zero;
                    // calculate how much to take off from movement here

                    normalForce += normals[i] * Math.Abs(Vector3.Dot(frameDiff, normals[i]));

                    pos += normalForce;

                    List<Vector3> test = checkNormalCollision();
                    if (test.Count() == 0)
                    {
                        break;
                    }

                }
            }
        }

        List<Vector3> checkNormalCollision()
        {
            // Move the bounding circle to new position
            cols.ElementAt(0).Update(pos);
            cols.ElementAt(1).Update(pos + Vector3.Cross(direction, Vector3.Up) * 3);
            cols.ElementAt(2).Update(pos + Vector3.Cross(direction, Vector3.Up) * -3);

            List<Vector3> normals = new List<Vector3>();

            // Get current node co-ordinates
            nodePos = new Vector2((int)((pos.X / 30) + 0.5f), (int)((pos.Z / 30) + 0.5f));

            // Make a list containing current node and all neighbor nodes
            List<MapNode> currentNodes = new List<MapNode>();
            currentNodes.Add(game.map.map[(int)nodePos.X, (int)nodePos.Y]);
            for (int i = 0; i < 4; i++)
            {
                if (game.map.map[(int)nodePos.X, (int)nodePos.Y].neighbors[i] != null)
                {
                    currentNodes.Add(game.map.map[(int)nodePos.X, (int)nodePos.Y].neighbors[i]);
                }
            }

            //For the current node check if your X component will make you collide with wall
            foreach (MapNode node in currentNodes)
            {
                // Check map collision boxes
                foreach (OOBB box in node.collisionBoxes)
                {
                    Vector3 normal = cols.ElementAt(0).wallCollide(box);
                    if (!(normal.Equals(Vector3.Zero)))
                    {
                        normals.Add(normal);
                    }
                }

                // Check enemies within spatial partitioning
                foreach (Enemy e in game.enemyManager.enemies)
                {
                    if (!(e.Equals(this)))
                    {
                        if (node.position.X == e.nodePos.X && node.position.Y == e.nodePos.Y)
                        {
                            foreach (CircleCollider c in e.cols)
                            {
                                Vector3 normal = cols.ElementAt(0).circleCollide(c);
                                if (!(normal.Equals(Vector3.Zero)))
                                {
                                    normals.Add(normal);
                                    //c.push(moveSpeed, pos, 4.0f);
                                    //game.modelManager.addEffect(new ImpactParticleModel(game, pos));
                                }
                            }
                        }
                    }
                }
            }

            // Check collision with core
            Vector3 coreNormal = cols.ElementAt(0).circleCollide(game.core.col);
            if (!(coreNormal.Equals(Vector3.Zero)))
            {
                normals.Add(coreNormal);
            }

            return normals;
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

        public override void drillDamage(float damage, Vector3 dir, bool boosting)
        {
            health -= damage;
            if (health <= 0)
                death();

            if (boosting)
            {
                game.camera.setYShake(0.06f);
                //state = State.hitByDrill;
                knockDir = dir + cols.ElementAt(0).directionFrom(game.ship.pos);
                knockDir.Normalize();
            }
            else
            {
                game.camera.setYShake(0.03f);
            }
        }

        public override void updatePath()
        {
            if (atCore == false)
            {
                path = pathfinder.findPath(new Point((int)((pos.X / 30) + 0.5f), (int)((pos.Z / 30) + 0.5f)), coreLocation);
                pathPosition = 0;

                nextPosition = path[pathPosition];
                target = new Vector3(makeCentreCoordinate(nextPosition.X) + (-5 + Utilities.nextFloat() * 10), 4.5f, makeCentreCoordinate(nextPosition.Y) + (-5 + Utilities.nextFloat() * 10));
            }
        }
    }
}