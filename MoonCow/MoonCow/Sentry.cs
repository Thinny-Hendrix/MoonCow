using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    public class Sentry
    {
        float health;

        public Vector3 pos;
        public Vector3 targetDir;
        public Vector3 eyeDir;
        public Vector3 cannonDir;
        Ship ship;
        public enum State { idle, wake, active, fail, success, agro, hit, knockBack}
        public State state;
        State prevState;
        State nextState;
        CircleCollider wakeRange;
        CircleCollider sleepRange;
        SentryModel model;
        EnemyManager manager;
        Game1 game;
        public float shockTime;
        float cooldownTime;
        float agroFireCount;
        float timeSinceLastDrill;
        public Vector3 knockDir;
        Vector3 frameDiff;


        EnemyShotTelegraph telegraph;
        bool triggeredTele;

        public Vector2 nodePos;

        public float emoteTime;
        public float distFromShip;
        public CircleCollider col;

        float cannonTransTime;
        Vector3 prevCannon;
        float visorTransTime;
        Vector3 prevVisor;

        float cannonTurnSpeed;
        float cannonAngle;
        public Sentry(Game1 game, EnemyManager manager, Vector3 pos)
        {
            this.game = game;
            this.manager = manager;
            this.pos = pos;
            ship = game.ship;

            wakeRange = new CircleCollider(pos, 30);
            sleepRange = new CircleCollider(pos, 40);
            col = new CircleCollider(pos, 1.5f);
            state = State.idle;
            prevState = state;

            model = new SentryModel(this, game);
            game.modelManager.addEnemy(model);

            targetDir = Vector3.Forward;
            eyeDir = targetDir;
            cannonDir = targetDir;
            cooldownTime = 2;
            emoteTime = 0;
            health = 25;
            agroFireCount = 0;

            triggeredTele = false;

            telegraph = new EnemyShotTelegraph(game, pos - targetDir*2, targetDir, 1);
            game.modelManager.addEffect(telegraph);

            nodePos = new Vector2((int)((pos.X / 30) + 0.5f), (int)((pos.Z / 30) + 0.5f));
        }

        public void Update()
        {
            if (timeSinceLastDrill > 0)
            {
                timeSinceLastDrill -= Utilities.deltaTime;
            }

            if(state == State.idle)
            {
                //do a spin around thing

                if (shipInRange() && wakeRange.checkPoint(ship.pos))
                {
                    state = State.wake;
                    model.wake();
                    prevCannon = cannonDir;
                    prevVisor = eyeDir;
                    cannonTransTime = 0;
                    visorTransTime = 0;
                }
            }
            if(state == State.wake)
            {
                calcDir();
                updateVisor();
                shockTime += Utilities.deltaTime*1.5f;
                if (shockTime >= 1)
                {
                    shockTime = 0;
                    state = State.active;
                }
            }

            if(state == State.hit)
            {
                shockTime += Utilities.deltaTime * 3.5f;
                if (shockTime >= 1)
                {
                    shockTime = 0;
                    state = State.agro;

                    prevCannon = cannonDir;
                    prevVisor = eyeDir;
                    cannonTransTime = 0;
                    visorTransTime = 0;
                }
            }
            if(state == State.active)
            {
                calcDir();
                updateCannon();
                updateVisor();


                cooldownTime -= Utilities.deltaTime;

                if (cooldownTime <= 0.4f)
                {
                    telegraph.setPos(pos - targetDir * 1.5f, targetDir);
                    if (!triggeredTele)
                    {
                        telegraph.wake();
                        triggeredTele = true;
                    }
                }

                if (cooldownTime <= 0)
                {
                    fire();
                    cooldownTime = 2;
                    triggeredTele = false;
                }

                if (!sleepRange.checkPoint(ship.pos))
                    state = State.idle;
            }
            if(state == State.agro)
            {
                calcDir();
                updateCannon();
                updateVisor();

                if (cooldownTime <= 0.4f)
                {
                    telegraph.setPos(pos - targetDir * 1.5f, targetDir);
                    if (!triggeredTele)
                    {
                        telegraph.wake();
                        triggeredTele = true;
                    }
                }

                cooldownTime -= Utilities.deltaTime;
                if (cooldownTime <= 0)
                {
                    fire();
                    agroFireCount++;
                    if (agroFireCount < 3)
                    {
                        cooldownTime = 0.4f;
                    }
                    else
                    {
                        agroFireCount = 0;
                        cooldownTime = 2;
                        state = nextState;
                    }
                }

                if (!sleepRange.checkPoint(ship.pos))
                    state = State.idle;
            }
            if(state == State.fail || state == State.success)
            {
                emoteTime += Utilities.deltaTime;
                if (emoteTime >= 1)
                {
                    prevCannon = cannonDir;
                    prevVisor = eyeDir;
                    cannonTransTime = 0;
                    visorTransTime = 0; 
                    state = State.active;
                }
            }
            if(state == State.knockBack)
            {
                if (shockTime < 1)
                {
                    shockTime += Utilities.deltaTime * 2.5f;
                    if (shockTime > 1)
                        shockTime = 1;
                }
                frameDiff = knockDir * Utilities.deltaTime * 40;
                checkCollisions();
            }
        }

        void calcDir()
        {
            targetDir.X = pos.X - ship.pos.X;
            targetDir.Z = pos.Z - ship.pos.Z;
            targetDir.Normalize();

            distFromShip = Utilities.hypotenuseOf(pos.X - ship.pos.X, pos.Z - ship.pos.Z);
        }

        void updateCannon()
        {
            if (cannonTransTime != 1)
            {
                cannonDir = Vector3.SmoothStep(prevCannon, targetDir, cannonTransTime);
                cannonTransTime += Utilities.deltaTime * 2;
                if (cannonTransTime > 1)
                    cannonTransTime = 1;
            }
            else
                cannonDir = targetDir;
        }

        void updateVisor()
        {
            if (visorTransTime != 1)
            {
                eyeDir = Vector3.SmoothStep(prevVisor, targetDir, visorTransTime);
                visorTransTime += Utilities.deltaTime * 4;
                if (visorTransTime > 1)
                    visorTransTime = 1;
            }
            else
                eyeDir = targetDir;
        }

        public void hitShip()
        {
            if (state != State.agro && state != State.hit && state != State.knockBack)
                state = State.success;
            else
                nextState = State.success;
            emoteTime = 0;
            triggeredTele = false;
        }

        public void missedShip()
        {
            if (state != State.agro && state != State.hit && state != State.knockBack)
                state = State.fail;
            else
                nextState = State.fail;
            emoteTime = 0;
            triggeredTele = false;
        }

        void fire()
        {
            manager.projectiles.Add(new SentryProjectile(pos, targetDir*-1, game, this));
        }

        bool shipInRange()
        {
            return ship.nodePos.X >= nodePos.X - 1 && ship.nodePos.X <= nodePos.X + 1 &&
                        ship.nodePos.Y >= nodePos.Y - 1 && ship.nodePos.Y <= nodePos.Y + 1;
        }

        public void drillDamage(float amount, Vector3 dir, bool boosting)
        {
            health -= amount;
            if (health <= 0)
                onDeath();

            if (boosting)
            {
                game.camera.setYShake(0.1f);
                state = State.knockBack;
                knockDir = dir + col.directionFrom(game.ship.pos);
                knockDir.Normalize();
            }
            else
            {
                game.camera.setYShake(0.03f);
                state = State.hit;
            }

            if (timeSinceLastDrill <= 0)
            {
                timeSinceLastDrill = 0.2f;
                model.hit(dir);
                shockTime = 0;
                model.wake();
            }
            triggeredTele = false;
            cooldownTime = 2;
        }

        public void damage(float amount, Vector3 dir)
        {
            health -= amount;
            if (health <= 0)
                onDeath();

            state = State.hit;
            model.hit(dir);
            shockTime = 0;
            model.wake();
            triggeredTele = false;
            cooldownTime = 2;

            game.audioManager.addSoundEffect(AudioLibrary.shipMetallicWallHit, 0.1f);
        }

        void checkCollisions()
        {
            pos += frameDiff;
            col.Update(pos);
            bool collided = false;
            foreach (Asteroid a in game.asteroidManager.asteroids)
            {
                if(a.col.checkCircle(col))
                {
                    collided = true;
                }
            }

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
                foreach (OOBB box in node.collisionBoxes)
                {
                    if (col.checkOOBB(box))
                    {
                        collided = true;
                    }
                }
            }

            foreach(Sentry s in game.enemyManager.sentries)
            {
                if (s != this)
                {
                    if (s.nodePos == nodePos)
                    {
                        if (col.checkCircle(s.col))
                        {
                            collided = true;
                            s.damage(5, knockDir);
                        }
                    }
                }
            }

            if(collided)
            {
                onDeath();
            }
        }

        void onDeath()
        {
            telegraph.Dispose();
            game.modelManager.removeEffect(telegraph);
            manager.sToDelete.Add(this);
            game.modelManager.removeEnemy(model);
            for(int i = 0; i < 10; i++)
                game.modelManager.addEffect(new GlowStreak(game, pos, new Vector2(2,7), 2, Color.White, 0,-1));
            game.modelManager.addEffect(new GlowStreakCenter(game, pos, 3, 2,-1));
            for (int i = 0; i < 3; i++)
                ship.moneyManager.addGib(20, pos, -1);
        }
    }
}
