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
        public enum State { idle, wake, active, fail, success, agro, hit}
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
        bool cannonLock;
        bool visorLock;
        float agroFireCount;

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
            if (state != State.agro || state != State.hit)
                state = State.success;
            else
                nextState = State.success;
            emoteTime = 0;
            cannonLock = false;
            triggeredTele = false;
        }

        public void missedShip()
        {
            if (state != State.agro || state != State.hit)
                state = State.fail;
            else
                nextState = State.fail;
            emoteTime = 0;
            cannonLock = false;
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
        }

        void onDeath()
        {
            manager.sToDelete.Add(this);
            game.modelManager.removeEnemy(model);
            for (int i = 0; i < 3; i++)
                ship.moneyManager.addGib(20, pos, -1);
        }
    }
}
