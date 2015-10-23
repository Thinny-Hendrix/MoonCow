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
        Game1 game;

        public Matrix transform;
        public Vector3 pos;
        public Vector3 rot;
        public Vector3 scale;
        public Vector3 direction;
        Vector3 frameDiff;//values are added to this in update before doing one collision check
        public Vector3 respawnPoint;


        public float moveSpeed;
        public float maxSpeed;
        public float boostSpeed;
        public float accel;
        float sideSpeed;
        float targetRoll;

        public float currentTurnSpeed;
        public float maxTurnSpeed;

        public float roll;

        public bool moving;
        bool turning;
        public bool boosting;
        public bool inUTurn;
        float uTurnYaw;
        float totaluTurn;

        public bool finishingMove;
        public bool colliding;
        public bool freezeCollide;

        public Vector2 nodePos;

        float rollCooldown = 0;
        float tilt = 0;
        enum RollState {not, rolling, recovering};
        RollState rollState;

        public OOBB boundingBox;
        public CircleCollider circleCol;

        bool justHitWall = false;

        float damagePotential;

        

        enum RollDir { left, right };
        RollDir rollDir = RollDir.left;
        float rollRecoverTime = MathHelper.PiOver2;

        public ShipModel shipModel;
        SkyboxModel skyboxModel;
        SpeedCylModel speedCyl;
        RainbowTunnelModel rbowTun;

        public ShipParticleSystem particles;
        public WeaponSystem weapons;
        public MoneyManager moneyManager;
        public ShipHealthSystem shipHealth;

        public Ship(Game1 game) : base(game)
        {
            this.game = game;

            pos = new Vector3(90, 4.5f, 0);
            moveSpeed = 0;
            accel = 0.5f;
            maxSpeed = 0.2f;
            boostSpeed = 0.6f;
            direction = new Vector3(0, 0, -1);
            currentTurnSpeed = 0;
            maxTurnSpeed = MathHelper.PiOver4 / 30;

            boundingBox = new OOBB(pos, direction, 1.5f, 1.5f); // Need to be changed to be actual ship dimentions
            circleCol = new CircleCollider(pos, 0.75f);

            shipModel = new ShipModel(this);
            //shipModel = new ShipModel(game.Content.Load<Model>(@"Models/Enemies/Cubes/guncube"), this);
            skyboxModel = new SkyboxModel(game.Content.Load<Model>(@"Models/Misc/Skybox/skybox"), this);
            speedCyl = new SpeedCylModel(game.Content.Load<Model>(@"Models/Misc/speedCyl"), this, ((Game1)Game));
            rbowTun = new RainbowTunnelModel(game.Content.Load<Model>(@"Models/Misc/Rbow/rbowTun"), this, ((Game1)Game));

            game.modelManager.addObject(shipModel);
            game.modelManager.add(skyboxModel);
            game.modelManager.addObject(rbowTun);
            game.modelManager.addTransparent(speedCyl);

            weapons = new WeaponSystem(this, this.game);
            moneyManager = new MoneyManager(this.game);
            particles = new ShipParticleSystem(this.game, this);
            shipHealth = new ShipHealthSystem(this.game, this);

            game.Components.Add(weapons);
            game.Components.Add(moneyManager);
            game.Components.Add(particles);
            game.Components.Add(shipHealth);

            ((WeaponDrill)weapons.weapons.ElementAt(4)).dome.setShip(this);

        }

        public override void Initialize()
        {
            pos = respawnPoint;

            base.Initialize();
        }


        public override void Update(GameTime gameTime)
        {
            frameDiff = Vector3.Zero;
            if (!Utilities.paused && !Utilities.softPaused)
            {
                if (inUTurn)
                {
                    uTurn();
                }
                else
                {
                    //## SHIP TURNING CODE ## - Need to add some collision detection in here so you can't turn yourself into a wall and get stuck
                    updateTurn();

                    //## SHIP MOVEMENT CODE ##
                    updateSpeed();

                    if (boosting)
                        GamePad.SetVibration(PlayerIndex.One, 1, 1);
                    else
                        GamePad.SetVibration(PlayerIndex.One, 0, 0);

                    if (!game.minigame.active && (Keyboard.GetState().IsKeyDown(Keys.S) || 
                        GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y < -0.3f ||
                        GamePad.GetState(PlayerIndex.One).ThumbSticks.Right.Y < -0.3f))
                    {
                        inUTurn = true;
                        boosting = false;
                        currentTurnSpeed = 0;
                    }
                }

                //## BARREL ROLL ##
                updateBarrelRoll();

                //checkFinishingMove();

                //########### Collision detection and final movement application #########
                updateMovement();
                updateDamagePotential();
            }
            
            if(Keyboard.GetState().IsKeyDown(Keys.I))
                moneyManager.addMoney(80);
            /*if (Keyboard.GetState().IsKeyDown(Keys.U))
                moneyManager.addMoney(1024);
            if (Keyboard.GetState().IsKeyDown(Keys.J))
                moneyManager.addMoney(-35);
            if (Keyboard.GetState().IsKeyDown(Keys.K))
                moneyManager.addMoney(-1329);

            if (Keyboard.GetState().IsKeyDown(Keys.L))
                shipHealth.onHit(3);*/

            if (Keyboard.GetState().IsKeyDown(Keys.T))
                respawn();


            //weapons.update();
            //moneyManager.update();
            //System.Diagnostics.Debug.WriteLine("Current angle is " + rot + ", direction is " + direction);

        }

        void respawn()
        {
            pos = respawnPoint;

            //stop moving
            moveSpeed = 0;
            sideSpeed = 0;

            //face the right direction
            direction = Vector3.Forward;
            rot.Y = 0;

            //reset turning
            currentTurnSpeed = 0;
            tilt = 0;

            //reset barrel roll
            rollState = RollState.not;
            rollCooldown = 0;
            roll = 0;

            //reset uturn
            inUTurn = false;
            totaluTurn = 0;
            pos.Y = 4.5f;
            uTurnYaw = 0;
            rot.X = 0;

            game.camera.reset();
        }

        void uTurn()
        {
            //rotate 180 degrees over an amount of time
            //if angle + rot less than 180, add angle else rot is 180
            //once hit 180, inuturn false
            //currentTurnSpeed = MathHelper.Lerp(currentTurnSpeed, maxTurnSpeed / 2, Utilities.deltaTime * 5);

            uTurnYaw += MathHelper.Pi * Utilities.deltaTime / (13);
            rot.Y += uTurnYaw;
            totaluTurn += uTurnYaw;

            direction.X = -(float)Math.Sin(rot.Y);
            direction.Z = -(float)Math.Cos(rot.Y);

            direction.Normalize();

            pos.Y = 4.5f - (float)(Math.Cos((totaluTurn*2))-1)/4;
            rot.X = (float)(Math.Sin((totaluTurn * 2))/2);
            rot.Z = tilt -(float)(Math.Cos((totaluTurn * 2)) - 1) / 2;

            //System.Diagnostics.Debug.WriteLine(uTurnYaw);

            moveSpeed = maxSpeed;// MathHelper.Lerp(moveSpeed, maxSpeed, Utilities.deltaTime * 5);


            //pos.X += direction.X * moveSpeed;
            //pos.Z += direction.Z * moveSpeed;

            frameDiff.X += direction.X * moveSpeed;
            frameDiff.Z += direction.Z * moveSpeed;

            //if (uTurnYaw > MathHelper.Pi / 20)
            if(totaluTurn > MathHelper.Pi)
            {
                rot.X = 0;
                totaluTurn = 0;
                pos.Y = 4.5f;
                uTurnYaw = 0;
                inUTurn = false;
            }
        }

        void updateTurn()
        {
            turning = false;
            bool keys = false;
            float stickX = GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X;
            //cap the threshhold
            if (stickX > 0.7f)
                stickX = 0.7f;
            if (stickX < -0.7f)
                stickX = -0.7f;

            if (Keyboard.GetState().IsKeyDown(Keys.A) || Keyboard.GetState().IsKeyDown(Keys.D))
                keys = true;
            if (!finishingMove)
            {
                if (!game.minigame.active && (Keyboard.GetState().IsKeyDown(Keys.A) || stickX < -0.3f))
                {
                    turning = true;
                    if (boosting)
                    {
                        if (keys)
                            currentTurnSpeed = MathHelper.Lerp(currentTurnSpeed, maxTurnSpeed / 2, Utilities.deltaTime * 5);
                        else
                            currentTurnSpeed = MathHelper.Lerp(currentTurnSpeed,
                                                        MathHelper.Lerp(0, maxTurnSpeed / 2, -(stickX + 0.3f) * (1 / 0.4f)),
                                                        Utilities.deltaTime * 5);
                        tilt = MathHelper.Lerp(tilt, 10 * currentTurnSpeed, Utilities.deltaTime * 2); //note -20*maxTurnSpeed is pi/6
                    }
                    else
                    {
                        if (keys)
                            currentTurnSpeed = MathHelper.Lerp(currentTurnSpeed, maxTurnSpeed, Utilities.deltaTime * 5);
                        else
                            currentTurnSpeed = MathHelper.Lerp(currentTurnSpeed,
                                MathHelper.Lerp(0, maxTurnSpeed, -(stickX + 0.3f) * (1 / 0.4f)),
                                Utilities.deltaTime * 5);
                        tilt = MathHelper.Lerp(tilt, 20 * currentTurnSpeed, Utilities.deltaTime * 2);
                    }
                }
                if (!game.minigame.active && (Keyboard.GetState().IsKeyDown(Keys.D) || stickX > 0.3f))
                {
                    turning = true;
                    if (boosting)
                    {
                        if (keys)
                            currentTurnSpeed = MathHelper.Lerp(currentTurnSpeed, -maxTurnSpeed / 2, Utilities.deltaTime * 5);
                        else
                            currentTurnSpeed = MathHelper.Lerp(currentTurnSpeed,
                                MathHelper.Lerp(0, -maxTurnSpeed / 2, (stickX - 0.3f) * (1 / 0.4f)),
                                Utilities.deltaTime * 5);
                        tilt = MathHelper.Lerp(tilt, 10 * currentTurnSpeed, Utilities.deltaTime * 2); //note -20*maxTurnSpeed is pi/6
                    }
                    else
                    {
                        if (keys)
                            currentTurnSpeed = MathHelper.Lerp(currentTurnSpeed, -maxTurnSpeed, Utilities.deltaTime * 5);
                        else
                            currentTurnSpeed = MathHelper.Lerp(currentTurnSpeed,
                                MathHelper.Lerp(0, -maxTurnSpeed, (stickX - 0.3f) * (1 / 0.4f)),
                                Utilities.deltaTime * 5);
                        tilt = MathHelper.Lerp(tilt, 20 * currentTurnSpeed, Utilities.deltaTime * 2);
                    }

                }

                if (!turning)
                {
                    currentTurnSpeed = MathHelper.Lerp(currentTurnSpeed, 0, Utilities.deltaTime * 5);
                    tilt = MathHelper.Lerp(tilt, 0, Utilities.deltaTime * 2);

                }

                rot.Z = tilt;

                rot.Y += currentTurnSpeed;
                direction.X = -(float)Math.Sin(rot.Y);
                direction.Z = -(float)Math.Cos(rot.Y);
                direction.Normalize();
            }
            else
            {
                tilt = MathHelper.Lerp(tilt, 0, Utilities.deltaTime * 2);
            }
        }

        void updateSpeed()
        {
            if (!game.minigame.active && (Keyboard.GetState().IsKeyDown(Keys.W) || GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y > 0.1f))
            {
                moving = true;
                if (Keyboard.GetState().IsKeyDown(Keys.LeftShift) || GamePad.GetState(PlayerIndex.One).Triggers.Right > 0.3f)
                {
                    if (!boosting)
                        game.hud.startBoost();
                    boosting = true;
                    moveSpeed = MathHelper.Lerp(moveSpeed, boostSpeed, Utilities.deltaTime * 7);
                }
                else
                {
                    boosting = false;
                    moveSpeed = MathHelper.Lerp(moveSpeed, maxSpeed, Utilities.deltaTime * 3);
                }
            }
            else
            {
                moving = false;
                boosting = false;
                if (moveSpeed != 0)
                {
                    moveSpeed = MathHelper.Lerp(moveSpeed, 0, Utilities.deltaTime * 1);
                }
            }

            frameDiff.X += direction.X * moveSpeed;
            frameDiff.Z += direction.Z * moveSpeed;


            game.audioManager.shipSpaceEngine.Pitch = moveSpeed * 1.6f;
        }

        void updateBarrelRoll()
        {
            if (!game.minigame.active)
            {
                if ((Keyboard.GetState().IsKeyDown(Keys.Q) || GamePad.GetState(PlayerIndex.One).ThumbSticks.Right.X < -0.3f) && rollCooldown <= 0)
                {
                    rollCooldown = 30;
                    rollRecoverTime = MathHelper.PiOver2;

                    if (roll < 0 && rollState == RollState.rolling)
                        targetRoll = 0;
                    else
                        targetRoll = MathHelper.Pi * 2;

                    rollState = RollState.rolling;
                    rollDir = RollDir.left;

                }

                if ((Keyboard.GetState().IsKeyDown(Keys.E) || GamePad.GetState(PlayerIndex.One).ThumbSticks.Right.X > 0.3f) && rollCooldown <= 0)
                {
                    rollCooldown = 30;
                    rollRecoverTime = MathHelper.PiOver2;

                    if (roll > 0 && rollState == RollState.rolling)
                        targetRoll = 0;
                    else
                        targetRoll = -MathHelper.Pi * 2;

                    rollState = RollState.rolling;
                    rollDir = RollDir.right;


                }
            }

            if (rollState == RollState.rolling) //todo - treat sideways movement like forwards with lerping
            {
                if (rollCooldown > 0)
                    rollCooldown -= 60 * Utilities.deltaTime;

                if (rollDir == RollDir.left)
                {
                    sideSpeed = MathHelper.Lerp(sideSpeed, 15, Utilities.deltaTime*5);
                    roll += MathHelper.Pi * Utilities.deltaTime * 4 * (sideSpeed/15);
                    
                    if (roll > targetRoll)// && rollCooldown >= 10)
                    {
                        rollCooldown = 0;
                        roll = 0;
                        rollState = RollState.recovering;

                    }
                }
                else
                {
                    sideSpeed = MathHelper.Lerp(sideSpeed, -15, Utilities.deltaTime * 5);
                    roll += MathHelper.Pi * Utilities.deltaTime * 4 * (sideSpeed / 15);


                    if (roll < targetRoll)// && rollCooldown >= 10)
                    {
                        rollCooldown = 0;
                        roll = 0;
                        rollState = RollState.recovering;
                    }
                }
            }
            else
            {
                sideSpeed = MathHelper.Lerp(sideSpeed, 0, Utilities.deltaTime * 8);
            }
            if(rollState == RollState.recovering)
            {
                if(rollDir == RollDir.left)
                    roll = (float)((Math.Cos(rollRecoverTime) - 1) * 0.5f) * -MathHelper.Pi * Utilities.deltaTime * 8;
                if (rollDir == RollDir.right)
                    roll = (float)((Math.Cos(rollRecoverTime) - 1) * 0.5f) * MathHelper.Pi * Utilities.deltaTime * 8;

                //                    roll = (float)((Math.Cos(rollRecoverTime) - 1) * 0.5f) * MathHelper.PiOver4*0.65f;


                rollRecoverTime += MathHelper.Pi * 3.5f * Utilities.deltaTime;
                if(rollRecoverTime >= MathHelper.Pi*2)
                {
                    rollState = RollState.not;
                    roll = 0;
                    rollRecoverTime = MathHelper.PiOver2;
                }

            }

            frameDiff.X += Vector3.Cross(Vector3.Up, direction).X * sideSpeed * Utilities.deltaTime;
            frameDiff.Z += Vector3.Cross(Vector3.Up, direction).Z * sideSpeed * Utilities.deltaTime;
            rot.Z += roll;
        }

        void checkFinishingMove()
        {
            if(boosting && (Keyboard.GetState().IsKeyDown(Keys.F) || GamePad.GetState(PlayerIndex.One).Triggers.Left > 0.2f))//to change - if boosting, drill equipped and raycast detects enemy within range with low health
            {
                if(!finishingMove)
                {
                    game.hud.flashTime = 0;
                    finishingMove = true;
                }
            }
            else
            {
                if (finishingMove)
                {
                    game.hud.flashTime = 0;
                    finishingMove = false;
                }
            }
        }

        void updateMovement()
        {
            colliding = false; // Used for determining if sound needs playing - does not seem to work 100% with new code. Will need reworking anyway when new audio system is implimented
            pos += frameDiff;
            List<Vector3> normals = checkCollision();
            if(normals.Count > 0)
            {
                colliding = true;
                for (int i = 0; i < normals.Count(); i++)
                {

                    Vector3 normalForce = Vector3.Zero;
                    // calculate how much to take off from movement here
                    normalForce += normals[i] * Math.Abs(Vector3.Dot(frameDiff, normals[i]));

                    if (freezeCollide && weapons.drill.drillColliding)
                    {
                        pos -= frameDiff;
                    }
                    else
                    {
                        pos += normalForce;
                    }

                    List<Vector3> test = checkCollision();
                    if (test.Count() == 0)
                    {
                        break;
                    }
                }
            }

            if (colliding)  // If sound needs playing
            {
                collisionSound();   // Play sound
            }
            else    // Otherwise do not play sound and stop if already playing sound
            {
                justHitWall = false;
                game.audioManager.shipMetallicWallScrape.Stop();
            }

            /* Old movement code, remove when finsihed porting to new collision system
             * 
            colliding = false; // Used for determining if sound needs playing, holds true if collision in either X or Z component of movement
            pos.X += frameDiff.X;   // Update the ship movment with X component of direction * speed
            if (checkCollision())   // See if that caused a collision
            {
                pos.X -= frameDiff.X;   // Undo movment if so
                colliding = true;       // Also play the sound
            }
            pos.Z += frameDiff.Z;   // Now update the Z component
            if (checkCollision())   // See if that caused collision
            {
                pos.Z -= frameDiff.Z;   // Undo movement if so
                colliding = true;       // Also play the sound
            }
            if (colliding)  // If sound needs playing
            {
                collisionSound();   // Play sound
            }
            else    // Otherwise do not play sound and stop if already playing sound
            {
                justHitWall = false;
                game.audioManager.shipMetallicWallScrape.Stop();
            }
            */
        }

        private void updateDamagePotential()
        {
            damagePotential = 1;
            foreach(Weapon pew in weapons.weapons)
            {
                damagePotential += pew.level;
            }

            damagePotential /= 2;

            game.map.map[(int)nodePos.X, (int)nodePos.Y].playerDamage = damagePotential;

            for (int i = 0; i < 4; i++)
            {
                if (game.map.map[(int)nodePos.X, (int)nodePos.Y].neighbors[i] != null)
                {
                    game.map.map[(int)nodePos.X, (int)nodePos.Y].neighbors[i].playerDamage = 0;
                }
            }
        }

        List<Vector3> checkCollision()
        {
            freezeCollide = false;

            // Move the bounding circle to new position
            circleCol.Update(pos);
            boundingBox.Update(pos, direction); // legacy, need to update everything to use the new circle collider
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
                    Vector3 normal = circleCol.wallCollide(box);
                    if(!(normal.Equals(Vector3.Zero)))
                    {
                        normals.Add(normal);
                    }

                }

                foreach (JunkShip j in game.asteroidManager.junkShips)
                {
                    if (node.position.X == j.nodePos.X && node.position.Y == j.nodePos.Y)
                    {
                        foreach (OOBB box in j.cols)
                        {
                            Vector3 normal = circleCol.wallCollide(box);
                            if (!(normal.Equals(Vector3.Zero)))
                            {
                                normals.Add(normal);
                            }
                        }
                    }

                }

                // Check enemies within spatial partitioning
                foreach (Enemy e in game.enemyManager.enemies)
                {
                    if (node.position.X == e.nodePos.X && node.position.Y == e.nodePos.Y)
                    {
                        foreach (CircleCollider c in e.cols)
                        {
                            Vector3 normal = circleCol.circleCollide(c);
                            if (!(normal.Equals(Vector3.Zero)))
                            {
                                freezeCollide = true;
                                normals.Add(normal);
                                //c.push(moveSpeed, pos, 4.0f);
                                //game.modelManager.addEffect(new ImpactParticleModel(game, pos));
                            }
                        }
                    }
                }

                // Check asteroids within spatial partitioning
                foreach (Asteroid a in game.asteroidManager.asteroids)
                {
                    if (node.position.X == a.nodePos.X && node.position.Y == a.nodePos.Y)
                    {
                        Vector3 normal = circleCol.circleCollide(a.col);
                        if (!(normal.Equals(Vector3.Zero)))
                        {
                            if (!(((WeaponDrill)weapons.weapons.ElementAt(4)).active && moving))
                            {
                                a.push(moveSpeed, direction, 2f);
                            }
                            else
                            {
                                a.stop();
                                freezeCollide = true;
                            }
                            normals.Add(normal);
                            //game.modelManager.addEffect(new ImpactParticleModel(game, pos));
                        }
                    }
                }

                // Check sentries within spatial partitioning
                foreach (Sentry s in game.enemyManager.sentries)
                {
                    if (node.position.X == s.nodePos.X && node.position.Y == s.nodePos.Y)
                    {
                        Vector3 normal = circleCol.circleCollide(s.col);
                        if (!(normal.Equals(Vector3.Zero)))
                        {
                            //c.push(moveSpeed, pos, 4.0f);
                            //game.modelManager.addEffect(new ImpactParticleModel(game, pos));
                            normals.Add(normal);
                            freezeCollide = true;
                        }
                    }
                }

            }

            // Check collision with core
            Vector3 coreNormal = circleCol.circleCollide(game.core.col);
            if (!(coreNormal.Equals(Vector3.Zero)))
            {
                normals.Add(coreNormal);
            }

            return normals;
        }

        /// <summary>
        /// Old collisions code, remove when fishied porting to new code
        /// </summary>
        /// <returns></returns>
        /*
        bool checkCollision()
        {
            //## COLLISIONS WHOOO! ##

            // Move the bounding box to new pos
            boundingBox.Update(pos, direction);
            circleCol.Update(pos);

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

            // Set current collision state to false
            bool collision = false;

            //For the current node check if your X component will make you collide with wall
            foreach (MapNode node in currentNodes)
            {
                foreach (OOBB box in node.collisionBoxes)
                {
                    if (boundingBox.intersects(box))
                    {
                        collision = true;
                    }
                }
            }

            if(game.core.col.checkCircle(circleCol))
            {
                collision = true;
            }

            foreach(Enemy e in game.enemyManager.enemies)
            {
                if (nodePos.X == e.nodePos.X && nodePos.Y == e.nodePos.Y)
                {
                    //System.Diagnostics.Debug.WriteLine("Bullet in same node as enemy");
                    foreach (CircleCollider c in e.cols)
                    {
                        if (c.checkOOBB(boundingBox))
                        {
                            //c.push(moveSpeed, pos, 4.0f);
                            //game.modelManager.addEffect(new ImpactParticleModel(game, pos));
                            collision = true;
                        }
                    }
                }
            }

            foreach(Sentry s in game.enemyManager.sentries)
            {
                if (nodePos.X == s.nodePos.X && nodePos.Y == s.nodePos.Y)
                {
                    //System.Diagnostics.Debug.WriteLine("Bullet in same node as enemy");
                    if (s.col.checkCircle(circleCol))
                    {
                        //c.push(moveSpeed, pos, 4.0f);
                        //game.modelManager.addEffect(new ImpactParticleModel(game, pos));
                        collision = true;
                    }
                }
            }

            foreach (Asteroid a in game.asteroidManager.asteroids)
            {
                if (nodePos.X == a.nodePos.X && nodePos.Y == a.nodePos.Y)
                {
                    //System.Diagnostics.Debug.WriteLine("Bullet in same node as enemy");
                    if (a.col.checkCircle(circleCol))
                    {
                        if (!(((WeaponDrill)weapons.weapons.ElementAt(4)).active && moving))
                            a.push(moveSpeed, direction, 2f);
                        else
                            a.stop();
                        //game.modelManager.addEffect(new ImpactParticleModel(game, pos));
                        collision = true;
                    }
                }
            }
            return collision;
        }
        */

        void collisionSound()
        {
            if (!justHitWall)  //plays the wallHit noise only if a collision is occurring
            {
                justHitWall = true;
                game.audioManager.shipMetallicWallHit.Stop();
                game.audioManager.shipMetallicWallHit.Play();
            }
            if (moveSpeed > 0.1f)
            {
                game.audioManager.shipMetallicWallScrape.Play();
            }
        }

        public void setRespawn(Vector3 respawn)
        {
            respawnPoint = respawn;
            pos = respawnPoint;
        }

        public void onDeath()
        {
            respawn();
        }

    }
}
