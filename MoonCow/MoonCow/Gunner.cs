﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    class Gunner : Enemy
    {
        protected Pathfinder pathfinder;
        protected List<Vector2> path = new List<Vector2>();
        protected Point coreLocation;
        protected Vector3 target;
        Vector3 frameDiff = new Vector3(0, 0, 0);
        public enum State { goToBase, playerInRange, shooting, reload, melee, playerOutRange, atBase, atCore, shootCore, reloadCore, strongHit, knockBack, hitByDrill }
        State state;
        float waitTime;
        float coolDown;
        int shotCount;
        float transitionTime;
        public Vector3 modelDir;
        CircleCollider meleeCol;
        CircleCollider meleeRange;
        bool successHit;
        State prevState;
        float turnTime;
        Vector3 oldDir;
        Vector3 oldFacing;

        public Gunner(Game1 game)
            : base(game)
        {
            this.game = game;
            moveSpeed = 0;
            maxSpeed = 10 + Utilities.nextFloat();
            direction = new Vector3(0, (float)Math.PI, 0);
            rot = direction;
            currentTurnSpeed = 0;
            maxTurnSpeed = MathHelper.PiOver4 / (59f + Utilities.nextFloat() * 62f);

            //enemyModel = new EnemyModel(game.Content.Load<Model>(@"Models/Ship/shipBlock"), this);
            enemyModel = new GunnerModel(this);
            enemyType = 2;
            electroDamage = new ElectroDamage(this, game, enemyType);
            pyroDamage = new PyroDamage(this, game, enemyType);

            game.modelManager.addEnemy(enemyModel);

            List<Vector2> spawns = game.map.getEnemySpawn();
            int value = Utilities.random.Next(spawns.Count);

            //essentially, if there's more than 1 spawn point, pick a random spawn point to spawn at.
            Vector2 spawn = new Vector2(spawns[value].X, spawns[value].Y);
            prevPosition = spawn;
            state = State.goToBase;

            pos = new Vector3(makeCentreCoordinate(spawn.X), 4.5f, makeCentreCoordinate(spawn.Y));

            switch(EnemyBehaviour.gunnerBehaviour)
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
            agroSphere = new CircleCollider(new Vector2(pos.X, pos.Z), 30f);
            meleeCol = new CircleCollider(pos, 2);
            meleeRange = new CircleCollider(pos, 3);

            health = 50;

            cols.Add(new CircleCollider(pos, 0.7f));

            frozen = false;

            //weapons = new WeaponSystem(this);
            spawnEffect();
        }

        public override void Update(GameTime gameTime)
        {
            

            //Agro stuff and attacks
            if (!frozen)
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
                if(state == State.goToBase)
                {
                    if (EnemyBehaviour.gunnerFollowPath)
                    {
                        goToBase();
                    }
                    agroSphere.Update(pos);
                    modelDir = direction;

                    meleeRange.Update(pos+modelDir*2);
                    if (game.ship.alive && EnemyBehaviour.gunnerPlayerAttack)
                    {
                        if (meleeRange.checkCircle(game.ship.circleCol) && EnemyBehaviour.gunnerMelee)
                        {
                            state = State.melee;
                            enemyModel.changeAnim(5);
                            waitTime = 0;
                        }
                        if (agroSphere.checkCircle(game.ship.circleCol) && EnemyBehaviour.gunnerRanged)
                        {
                            state = State.playerInRange;
                            enemyModel.changeAnim(1);
                            waitTime = 0.86f;
                        }
                    }
                    if (game.map.getNodeType(nodePos) > 19 && game.map.getNodeType(nodePos) < 35)//base node
                    {
                        state = State.atBase;
                        enemyModel.changeAnim(1);
                        waitTime = 0;
                        getCoreSpot();
                        oldDir = direction;
                        oldFacing = facingDir;
                    }
                }
                else if (state == State.playerInRange)
                {
                    goToBase();
                    agroSphere.Update(pos);
                    setDir();
                    modelDir = Vector3.Lerp(direction, facingDir, 1 - (waitTime / 0.875f));
                    waitTime -= Utilities.deltaTime;
                    if (waitTime <= 0)
                    {
                        enemyModel.changeAnim(2);
                        state = State.shooting;
                        shotCount = 0;
                        coolDown = 0;
                    }
                        //play animation 
                }
                else if(state == State.shooting)
                {
                    if (!atCore)
                    {
                        goToBase();
                        setDir();
                        meleeRange.Update(pos + facingDir * 2);
                        modelDir = facingDir;
                        if (meleeRange.checkCircle(game.ship.circleCol))
                        {
                            state = State.melee;
                            enemyModel.changeAnim(6);
                            waitTime = 0;
                        }
                        if (game.map.getNodeType(nodePos) > 19 && game.map.getNodeType(nodePos) < 35)//base node
                        {
                            state = State.atBase;
                            enemyModel.changeAnim(4);
                            getCoreSpot();
                            oldDir = direction;
                            oldFacing = facingDir;
                        }
                    }
                    else
                    {
                        if(turnTime != 1)
                        {
                            turnTime += Utilities.deltaTime;
                            if (turnTime > 1)
                                turnTime = 1;
                        }
                        modelDir = Vector3.Lerp(oldDir, game.core.col.directionFrom(pos), turnTime);
                        facingDir = modelDir;
                    }
                    agroSphere.Update(pos);

                    if (coolDown <= 0)
                    {
                        if (shotCount < 3)
                        {
                            shoot();
                            coolDown = 0.86f;
                            shotCount++;
                        }
                        else
                        {
                            state = State.reload;
                            enemyModel.changeAnim(3);
                            waitTime = 1.5f;
                        }
                    }
                    else
                    {
                        coolDown -= Utilities.deltaTime;
                    }
                }
                else if(state == State.reload)
                {
                    if (!atCore)
                    {
                        goToBase();
                        setDir();
                        modelDir = facingDir;
                        agroSphere.Update(pos);
                    }
                    else
                    {
                        modelDir = game.core.col.directionFrom(pos);
                    }
                    waitTime -= Utilities.deltaTime;
                    if(waitTime <= 0)
                    {
                        if (atCore)
                        {
                            enemyModel.changeAnim(2);
                            state = State.shooting;
                            shotCount = 0;
                            coolDown = 0;
                        }
                        else
                        {
                            if (game.ship.alive && agroSphere.checkCircle(game.ship.circleCol))
                            {
                                enemyModel.changeAnim(2);
                                state = State.shooting;
                                shotCount = 0;
                                coolDown = 0;
                            }
                            else
                            {
                                state = State.playerOutRange;
                                waitTime = 0.85f;
                                enemyModel.changeAnim(11);
                            }
                        }
                    }
                }
                else if (state == State.playerOutRange)
                {
                    goToBase();
                    agroSphere.Update(pos);
                    meleeRange.Update(pos);
                    meleeCol.Update(pos);
                    setDir();
                    modelDir = Vector3.Lerp(facingDir, direction, 1 - (waitTime / 0.86f));
                    waitTime -= Utilities.deltaTime;
                    if (waitTime <= 0)
                    {
                        state = State.goToBase;
                        shotCount = 0;
                        coolDown = 0;
                        enemyModel.changeAnim(0);
                    }
                    //play animation 
                }
                else if(state == State.melee)
                {
                    goToBase();
                    setDir();
                    modelDir = facingDir;
                    agroSphere.Update(pos);
                    meleeRange.Update(pos + facingDir * 2);
                    waitTime += Utilities.deltaTime;
                    if(!successHit && waitTime > 0.66f && waitTime < 0.875f)
                    {
                        meleeCol.Update(pos+facingDir);
                        if (meleeCol.checkCircle(game.ship.circleCol))
                        {
                            game.camera.setYShake(0.2f);
                            game.ship.shipHealth.onHit(35);
                            successHit = true;
                        }
                    }
                    if(waitTime > 1.16)
                    {
                        if (meleeRange.checkCircle(game.ship.circleCol))
                        {
                            waitTime = 0;
                            successHit = false;
                        }
                        else
                        {
                            successHit = false;
                            state = State.shooting;
                            enemyModel.changeAnim(2);
                            shotCount = 0;
                            coolDown = 0;
                        }
                    }
                }
                else if (state == State.atBase)
                {
                    if(enemyModel.activeIndex != 4)
                    {
                        waitTime += Utilities.deltaTime;
                        if(waitTime > 0.85f)
                        {
                            enemyModel.changeAnim(4);
                        }
                    }
                    goToCore();
                    modelDir = facingDir;
                    atCore = true;
                    //pos = target;
                }
                else if (state == State.atCore)
                {
                        state = State.shooting;
                        enemyModel.changeAnim(2);
                        coolDown = 0;
                        shotCount = 0;
                    updateMovement();
                }
                else if (state == State.shootCore && EnemyBehaviour.gunnerAttackCore)
                {
                    game.core.damage(0.05f * Utilities.deltaTime);
                    updateMovement();
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

        void shoot()
        {
            Vector3 offset = new Vector3(0.9f, -0.2f, 3);
            Vector3 shotPos = pos;
            shotPos += facingDir * offset.Z;
            shotPos += Vector3.Cross(facingDir, Vector3.Up) * offset.X;
            shotPos.Y += offset.Y;

            Vector3 shotDir;
            if(!atCore)
                shotDir = game.ship.circleCol.directionFrom(shotPos);
            else
                shotDir = game.core.col.directionFrom(shotPos);

            //if(agroSphere.checkCircle(game.ship.circleCol))
            {
                float vol = agroSphere.distFrom(game.ship.pos) / 90f;
                float sendVol = MathHelper.Lerp(0.4f,0,vol);
                if (sendVol < 0)
                    sendVol = 0;
                else if (sendVol > 0.4f)
                    sendVol = 0.4f;
                game.audioManager.addSoundEffect(AudioLibrary.laserHit, sendVol);
            }


            game.enemyManager.projectiles.Add(new GunnerProjectile(shotPos, shotDir, game));
        }

        void setDir()
        {
            facingDir = game.ship.circleCol.directionFrom(pos);
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
                        target = new Vector3(makeCentreCoordinate(nextPosition.X) + (-14 + Utilities.nextFloat() * 3), 4.5f, makeCentreCoordinate(nextPosition.Y) + (-10 + Utilities.nextFloat() * 20));
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

        void getCoreSpot()
        {
            coreSpot = game.core.getSpot(pos);
            if (coreSpot != null)
            {
                target = coreSpot.gunSpot;
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

            if (currentBaseIndex == 0)
            {
                facingDir = Vector3.SmoothStep(oldFacing, targetDir, turnTime);
            }
            else
            {
                facingDir = direction;
            }
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
                    //enemyModel.changeAnim(1);
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

        public override void startElectro()
        {
            if (state != State.goToBase)
            {
                animIndex = enemyModel.activeIndex;
                prevState = State.shooting;
                enemyModel.changeAnim(10);
            }
            else
            {
                prevState = State.goToBase;
                animIndex = 0;
                enemyModel.changeAnim(9);
            }
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
            if (damage > 5)
            {
                if (state != State.strongHit)
                {
                    animIndex = enemyModel.activeIndex;
                    prevState = state;

                    if (state != State.goToBase)
                        enemyModel.changeAnim(7);
                    else
                        enemyModel.changeAnim(8);
                }

                state = State.strongHit;
                waitTime = 0.875f;

                if (prevState == State.reload || prevState == State.melee)
                {
                    prevState = State.shooting;
                    animIndex = 2;
                }
                shotCount = 0;
                coolDown = 0;
            }
            base.damage(damage);
        }

        protected override void death()
        {
            for (int i = 0; i < 10; i++)
                game.modelManager.addEffect(new GlowStreak(game, pos, new Vector2(2, 10), 2, Color.White, 0, 2));
            game.modelManager.addEffect(new GlowStreakCenter(game, pos, 6, 2, 2));

            game.ship.moneyManager.makeMoney(125, 2, pos);

            if (Utilities.random.Next(5) == 0)
                game.ship.moneyManager.addAmmoGib(pos);

            if (coreSpot != null)
                coreSpot.taken = false;

            game.modelManager.removeEnemy(enemyModel);
            game.enemyManager.toDelete.Add(this);

            float vol = Vector3.Distance(pos, game.ship.pos) / 90f;
            float sendVol = MathHelper.Lerp(1f, 0, vol);
            if (sendVol < 0)
                sendVol = 0;
            else if (sendVol > 1f)
                sendVol = 1f;

            game.audioManager.addSoundEffect(AudioLibrary.bombExplode, sendVol);
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
            foreach(CircleCollider c in cols)
            {
                c.Update(pos);
            }
            // Get current node co-ordinates
            Vector2 nodePos = new Vector2((int)((pos.X / 30) + 0.5f), (int)((pos.Z / 30) + 0.5f));

            //For the current node check if your X component will make you collide with wall

            pos.Z += frameDiff.Z;

            boundingBox.Update(pos, direction);
            foreach (CircleCollider c in cols)
            {
                c.Update(pos);
            }
            nodePos = new Vector2((int)((pos.X / 30) + 0.5f), (int)((pos.Z / 30) + 0.5f));
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
            if(atCore == false)
            {
                path = pathfinder.findPath(new Point((int)((pos.X / 30) + 0.5f), (int)((pos.Z / 30) + 0.5f)), coreLocation);
                pathPosition = 0;

                nextPosition = path[pathPosition];
                target = new Vector3(makeCentreCoordinate(nextPosition.X) + (-5 + Utilities.nextFloat() * 10), 4.5f, makeCentreCoordinate(nextPosition.Y) + (-5 + Utilities.nextFloat() * 10));
            }
        }
    }
}