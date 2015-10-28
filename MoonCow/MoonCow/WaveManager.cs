using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MoonCow
{
    public class WaveManager : Microsoft.Xna.Framework.GameComponent
    {
        Game1 game;

        //waves are going to be grouped into 'attacks' which will consist of 4ish consecutive waves
        //after all waves in an attack have been defeated, explore time starts again

        List<Attack> attacks = new List<Attack>();
        public Attack activeAttack;
        public float waitTime;
        int attackCount;
        public float attackTime;
        
        public Utilities.SpawnState spawnState;
        bool endMessageTriggered;
        bool startMessageTriggered;

        public float swaSpawnTime;
        public float sneSpawnTime;
        public float gunSpawnTime;
        public float hevSpawnTime;

        public WaveManager(Game1 game) : base(game)
        {
            this.game = game;
            waitTime = 30; //120 = 2 mins
            attackCount = 1;
            spawnState = Utilities.SpawnState.waiting;
            endMessageTriggered = true;
            startMessageTriggered = false;

            swaSpawnTime = 0.6f;
            sneSpawnTime = 1;
            gunSpawnTime = 1.5f;
            hevSpawnTime = 3;

            activeAttack = new Attack(game, this, attackCount);
            attacks.Add(activeAttack);

            waitTime = 120;
        }

        //  The next wave is created when the wave before it is killed, in this way the wave data exists during the waiting time for statistics about upcoming wave to be accessed and displayed
        public override void Update(GameTime gametime)
        {
            if (!Utilities.paused && !Utilities.softPaused)
            {
                if(spawnState == Utilities.SpawnState.idle) // An attack just ended, so transition into waiting state
                {
                    spawnState = Utilities.SpawnState.waiting;
                    if (attackCount > 4 && !game.camera.endGame)
                    {
                        game.camera.triggerEnd();
                        game.hud.hudEnd.activate(true);
                    }
                    else
                    {
                        game.hud.hudAttackDisplayer.endAttackMessage();
                    }
                    waitTime = 120; // 150 seconds = 2.5 minutes between attacks

                    //contact difficulty manager
                    attackTime = 0;

                    activeAttack = new Attack(game, this, attackCount); // create next attack
                    attacks.Add(activeAttack);
                }

                if(spawnState == Utilities.SpawnState.waiting)    // The wait between attacks
                {
                    waitTime -= Utilities.deltaTime;
                    if (waitTime <= 0 || Keyboard.GetState().IsKeyDown(Keys.R))
                    {
                        waitTime = 0;
                        spawnState = Utilities.SpawnState.deploying;
                    }
                }

                if(spawnState == Utilities.SpawnState.deploying)  // Attack is active
                {
                    if(!startMessageTriggered)
                    {
                        game.hud.hudAttackDisplayer.startAttackMessage(activeAttack);
                        startMessageTriggered = true;
                        attackCount++;
                        attackTime += Utilities.deltaTime;
                    }
                    
                    // spawn enemies
                    activeAttack.update();   
                }
            }
        }

        public void endAttack()
        {
            spawnState = Utilities.SpawnState.idle;
            startMessageTriggered = false;
        }

    }
}
