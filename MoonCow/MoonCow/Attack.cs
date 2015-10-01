using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MoonCow
{
    public class Attack
    {
        Game1 game;
        public float waitTime;
        List<Wave> waves = new List<Wave>();
        Wave activeWave;
        int inAttack;
        int waveMax;
        int attackNumber;
        public bool active;
        public Utilities.SpawnState spawnState;

        public Attack(Game1 game, int attackNo)
        {
            this.game = game;
            waitTime = 0f;
            inAttack = 1;
            waveMax = 4;
            attackNumber = attackNo;
            active = true;
            spawnState = Utilities.SpawnState.waiting;
            activeWave = new Wave(game, attackNumber, inAttack);
        }

        public void update()
        {
            if (!Utilities.paused && !Utilities.softPaused)
            {
                if (spawnState == Utilities.SpawnState.idle && game.enemyManager.enemies.Count() == 0) // An wave just ended, so transition into waiting state
                {
                    if (inAttack < 4)
                    {
                        spawnState = Utilities.SpawnState.waiting;
                        inAttack++;
                        //game.hud.hudAttackDisplayer.endAttackMessage();
                        waitTime = 10; // 30 seconds between waves
                        activeWave = new Wave(game, attackNumber, inAttack); // create next attack
                        waves.Add(activeWave);
                    }
                    else
                    {
                        game.waveManager.endAttack();
                    }
                }

                if (spawnState == Utilities.SpawnState.waiting)    // The wait between attacks
                {
                    waitTime -= Utilities.deltaTime;
                    if (waitTime <= 0 )//|| Keyboard.GetState().IsKeyDown(Keys.R))
                    {
                        waitTime = 0;
                        spawnState = Utilities.SpawnState.deploying;
                    }
                }

                if (spawnState == Utilities.SpawnState.deploying)  // Attack is active
                {
                    // spawn enemies
                    activeWave.spawn();
                }
            }

        }

        public void endWave()
        {
            spawnState = Utilities.SpawnState.idle;
            //startMessageTriggered = false;
        }

    }
}
