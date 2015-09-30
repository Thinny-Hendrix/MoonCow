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
        
        public Utilities.SpawnState spawnState;
        bool endMessageTriggered;
        bool startMessageTriggered;

        public WaveManager(Game1 game) : base(game)
        {
            this.game = game;
            waitTime = 120;
            attackCount = 1;
            spawnState = Utilities.SpawnState.waiting;
            endMessageTriggered = true;
            startMessageTriggered = false;
            activeAttack = new Attack(game, attackCount);
            attacks.Add(activeAttack);
        }

        //  The next wave is created when the wave before it is killed, in this way the wave data exists during the waiting time for statistics about upcoming wave to be accessed and displayed
        public override void Update(GameTime gametime)
        {
            if (!Utilities.paused && !Utilities.softPaused)
            {
                if(spawnState == Utilities.SpawnState.idle) // An attack just ended, so transition into waiting state
                {
                    spawnState = Utilities.SpawnState.waiting;
                    game.hud.hudAttackDisplayer.endAttackMessage();
                    waitTime = 150; // 2.5 minutes between attacks
                    activeAttack = new Attack(game, attackCount); // create next attack
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
                        game.hud.hudAttackDisplayer.startAttackMessage(attackCount);
                        startMessageTriggered = true;
                        attackCount++;
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
