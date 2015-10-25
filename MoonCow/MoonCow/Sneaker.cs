using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    class Sneaker : Enemy
    {
        protected Pathfinder pathfinder;
        protected List<Vector2> path = new List<Vector2>();
        protected Point coreLocation;
        protected Vector3 target;
        Vector3 frameDiff = new Vector3(0, 0, 0);
        float pathTimer;
        public enum State { goToBase, atBase, atCore, attackCore, noticedPlayer, chargePlayer, coolDown, strongHit, knockBack, hitByDrill }
        State state;
        float waitTime;
        Vector3 chargeDir;
        float initDist;
        float cooldown;
        float chargeTime;
        State prevState;
        CircleCollider meleeCol;
        float turnTime;
        Vector3 oldDir;

        public Sneaker(Game1 game)
            : base(game)
        {
            this.game = game;
            maxSpeed = 10 + Utilities.nextFloat();
            moveSpeed = maxSpeed;
            direction = new Vector3(0, (float)Math.PI, 0);
            rot = direction;
            currentTurnSpeed = 0;
            maxTurnSpeed = MathHelper.PiOver4 / (59f + Utilities.nextFloat() * 62f);

            //enemyModel = new EnemyModel(game.Content.Load<Model>(@"Models/Ship/shipBlock"), this);
            enemyModel = new SneakerModel(this);
            enemyType = 1;
            electroDamage = new ElectroDamage(this, game, enemyType);
            pyroDamage = new PyroDamage(this, game, enemyType);

            game.modelManager.addEnemy(enemyModel);

            List<Vector2> spawns = game.map.getEnemySpawn();
            int value = Utilities.random.Next(spawns.Count);

            //essentially, if there's more than 1 spawn point, pick a random spawn point to spawn at.
            Vector2 spawn = new Vector2(spawns[value].X, spawns[value].Y);
            prevPosition = spawn;

            pos = new Vector3(makeCentreCoordinate(spawn.X), 4.5f, makeCentreCoordinate(spawn.Y));

            state = State.goToBase;

            switch (EnemyBehaviour.sneakerBehaviour)
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
            agroSphere = new CircleCollider(new Vector2(pos.X, pos.Z), 15);
            meleeCol = new CircleCollider(pos, 2);

            health = 35;
            pathTimer = 10;

            cols.Add(new CircleCollider(pos, 0.7f));

            //weapons = new WeaponSystem(this);
        }

        public override void Update(GameTime gameTime)
        {
            //target = game.ship.pos;
            //target = new Vector3(makeCentreCoordinate(coreLocation.X), 4.5f, makeCentreCoordinate(coreLocation.Y));

            //Agro stuff and attacks
            if (!electroDamage.active)
            {
                if (state == State.strongHit)
                {
                    waitTime -= Utilities.deltaTime;
                    if (waitTime <= 0)
                    {
                        state = prevState;
                        enemyModel.changeAnim(animIndex);
                    }
                }
                if (state == State.goToBase)
                {
                    goToBase();
                    agroSphere.Update(pos+direction*15);
                    if (cooldown == 0 && agroSphere.checkCircle(game.ship.circleCol))
                    {
                        enemyModel.changeAnim(1);
                        state = State.noticedPlayer;
                        waitTime = 0;
                    }
                    //if player in agro range
                    //if current node is base node

                    if(game.map.getNodeType(nodePos) > 19 && game.map.getNodeType(nodePos) < 35)//base node
                    {
                        state = State.atBase;
                        getCoreSpot();
                    }
                }
                else if(state == State.coolDown)
                {
                    goToBase();
                    waitTime += Utilities.deltaTime;
                    if (waitTime > 1f)
                    {
                        enemyModel.changeAnim(0);
                        state = State.goToBase;
                        setChargeDir();
                        facingDir = chargeDir;
                        chargeTime = 0;
                    }
                }
                if (state == State.noticedPlayer)
                {
                    waitTime += Utilities.deltaTime;
                    if (waitTime > 2.1f)
                    {
                        enemyModel.changeAnim(2);
                        state = State.chargePlayer;
                        setChargeDir();
                        facingDir = chargeDir;
                        chargeTime = 0;
                    }
                }
                if (state == State.chargePlayer)
                {
                    frameDiff = Vector3.Zero;
                    frameDiff += chargeDir * Utilities.deltaTime * 30;
                    updateMovement();
                    agroSphere.Update(pos+direction*15);
                    meleeCol.Update(pos);

                    if(meleeCol.checkCircle(game.ship.circleCol))
                    {
                        game.ship.shipHealth.onHit(200 * Utilities.deltaTime);
                        game.camera.setYShake(0.2f);
                    }

                    chargeTime += Utilities.deltaTime;
                    //if passed the ship
                    if(chargeTime > 2 || (!agroSphere.checkCircle(game.ship.circleCol) && cols.ElementAt(0).distFrom(game.ship.pos) > initDist+8))
                    {
                        if (game.map.getNodeType(nodePos) > 19 && game.map.getNodeType(nodePos) < 35)//base node
                        {
                            state = State.atBase;
                            getCoreSpot();
                            enemyModel.changeAnim(0);
                        }
                        else
                        {
                            updatePath();
                            cooldown = 10;
                            updatePath();
                            state = State.coolDown;
                            enemyModel.changeAnim(3);

                        }
                    } 
                }
                else if(state == State.atBase)
                {
                    goToCore();
                    atCore = true;
                    //pos = target;
                }
                else if (state == State.atCore)
                {
                    if (turnTime != 1)
                    {
                        turnTime += Utilities.deltaTime;
                        if (turnTime >= 1)
                            turnTime = 1;

                        Vector3 targetDir = game.core.col.directionFrom(pos);
                        direction = Vector3.SmoothStep(oldDir, targetDir, turnTime);
                    }
                    else
                    {
                        direction = game.core.col.directionFrom(pos);
                    }
                    waitTime += Utilities.deltaTime;
                    if (waitTime > 2.1f)
                    {
                        state = State.attackCore;
                        enemyModel.changeAnim(2);
                    }
                    updateMovement();
                }
                else if(state == State.attackCore)
                {
                    game.core.damage(0.05f*Utilities.deltaTime);
                    updateMovement();
                }

                if (health <= 0)
                {
                    death();
                }

                pathTimer -= Utilities.deltaTime;
                if (pathTimer <= 0)
                {
                    updatePath();
                    pathTimer = 10;
                }

                if(cooldown != 0)
                {
                    cooldown -= Utilities.deltaTime;
                    if (cooldown < 0)
                        cooldown = 0;
                }
            }
            base.Update(gameTime);
        }
        void setChargeDir()
        {
            chargeDir = -1*cols.ElementAt(0).directionFrom(game.ship.pos);
            chargeDir.Y = 0;
            initDist = cols.ElementAt(0).distFrom(game.ship.pos);
        }

        void getCoreSpot()
        {
            coreSpot = game.core.getSpot(pos);
            if (coreSpot != null)
            {
                target = coreSpot.sneSpot;
                posToCore = game.core.coordsToSpot(coreSpot, pos, target);
                currentBaseIndex = 0;
                target = posToCore.ElementAt(currentBaseIndex);
                resetDist();
            }
            oldDir = direction;
        }

        void resetDist()
        {
            currentDist = 0;
            Vector3 dist = target - pos;
            distToCore = Utilities.hypotenuseOf(dist.X, dist.Z);
        }

        void goToCore()
        {
            if (turnTime != 1)
            {
                turnTime += Utilities.deltaTime;
                if (turnTime >= 1)
                {
                    turnTime = 1;
                    resetDist();
                }
            }
            frameDiff = Vector3.Zero;
            Vector3 targetDir = target - pos;
            targetDir.Normalize();
            direction = Vector3.SmoothStep(oldDir, targetDir, turnTime);
            //            direction = targetDir;
            frameDiff = direction * moveSpeed * Utilities.deltaTime;
            updateMovement();
            Vector3 dif = pos - frameDiff;
            float frameDist = Utilities.hypotenuseOf(dif.X, dif.Y);
            //currentDist += frameDist;
            currentDist += moveSpeed * Utilities.deltaTime;

            if (currentDist > distToCore)
            {
                if (currentBaseIndex >= posToCore.Count() - 1)
                {
                    pos = target;
                    state = State.atCore;
                    turnTime = 0;
                    frameDiff = Vector3.Zero;
                    enemyModel.changeAnim(1);
                    waitTime = 0;
                    oldDir = direction;
                }
                else
                {
                    currentBaseIndex++;
                    target = posToCore.ElementAt(currentBaseIndex);
                    resetDist();
                    turnTime = 0;
                    oldDir = targetDir;
                }

            }
        }

        void goToBase()
        {
            float yAngle;
            Vector3 targetDirection = Vector3.Zero;

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

                pos += frameDiff;
                frameDiff = Vector3.Zero;
            }
            else
            {
                //need slowdown code
                moveSpeed = 0;

            }

            nodePos = new Vector2((int)((pos.X / 30) + 0.5f), (int)((pos.Z / 30) + 0.5f));
            boundingBox.Update(pos, direction);
            foreach (CircleCollider c in cols)
            {
                c.Update(pos);
            }

            pos.Y = 4.5f + (float)Math.Sin(time) * 0.2f;

            time += Utilities.deltaTime * MathHelper.Pi * 2.4f;

            if (time > MathHelper.Pi * 2)
                time -= MathHelper.Pi * 2;
        }

        public override void startElectro()
        {
            if (state != State.chargePlayer && state != State.attackCore)
            {
                animIndex = enemyModel.activeIndex;
                prevState = state;
            }
            else
            {
                prevState = State.goToBase;
                animIndex = 0;
            }
            enemyModel.changeAnim(5);
        }

        public override void endElectro()
        {
            enemyModel.changeAnim(animIndex);
            if(state != prevState)
            {
                state = prevState;
            }
        }

        public override void damage(float damage)
        {
            if (state != State.chargePlayer && state != State.attackCore)
            {
                if (damage > 5)
                {
                    if (state == State.chargePlayer || state == State.noticedPlayer || state == State.coolDown)
                    {
                        prevState = State.goToBase;
                        animIndex = 0;
                    }
                    else
                    {
                        prevState = state;
                        animIndex = enemyModel.activeIndex;
                    }

                    state = State.strongHit;
                    waitTime = 0.875f;
                    enemyModel.changeAnim(4);
                }
            }
            base.damage(damage);
        }

        protected override void death()
        {
            for (int i = 0; i < 10; i++)
                game.modelManager.addEffect(new GlowStreak(game, pos, new Vector2(2, 10), 2, Color.White, 0, 1));
            game.modelManager.addEffect(new GlowStreakCenter(game, pos, 6, 2, 1));

            game.ship.moneyManager.makeMoney(175, 1, pos);

            if (coreSpot != null)
                coreSpot.taken = false;

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
            foreach (CircleCollider c in cols)
            {
                c.Update(pos);
            } 
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
                    if (node.position.X == e.nodePos.X && node.position.Y == e.nodePos.Y)
                    {
                        if (!(e.Equals(this)))
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

        public override void drillDamage(float damage, Vector3 dir, bool boosting)
        {
            health -= damage;
            if (health <= 0)
                death();

            if (boosting)
            {
                game.camera.setYShake(0.1f);
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