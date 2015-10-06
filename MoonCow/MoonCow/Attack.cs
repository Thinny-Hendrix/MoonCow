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
        public List<Wave> waves = new List<Wave>();
        Wave activeWave;
        int inAttack;
        int currentWaveNumber;
        int attackNumber;
        public bool active;
        public Utilities.SpawnState spawnState;

        int maxEnemies;
        int maxGunner;
        int maxSneaker;
        int maxHeavy;
        int maxSwarmer;

        public Attack(Game1 game, int attackNo)
        {
            this.game = game;
            waitTime = 0f;
            currentWaveNumber = 0; 
            attackNumber = attackNo;
            active = true;
            generateAttackNumbers();
            spawnState = Utilities.SpawnState.waiting;
            //createWaves();
            tempWaveCreator();
            activeWave = waves[currentWaveNumber];
        }

        public void update()
        {
            if (!Utilities.paused && !Utilities.softPaused)
            {
                if (spawnState == Utilities.SpawnState.idle) // An wave just finished spawning, so transition into waiting state
                {
                    if (currentWaveNumber < inAttack - 1)
                    {
                        spawnState = Utilities.SpawnState.waiting;
                        //game.hud.hudAttackDisplayer.endAttackMessage();
                        waitTime = 15; // 20 seconds between waves
                        currentWaveNumber++;
                        activeWave = waves[currentWaveNumber];
                    }
                    else
                    {
                        if (game.enemyManager.enemies.Count() == 0)
                        {
                            game.waveManager.endAttack();
                        }
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

        private void tempWaveCreator()
        {
            // Purely to fill in while I get the actual dynamic system working
            inAttack = 8;
            waves.Add(new Wave(game, attackNumber, 1, 5, 1));
            waves.Add(new Wave(game, attackNumber, 2, 5, 2));
            waves.Add(new Wave(game, attackNumber, 3, 4, 3));
            waves.Add(new Wave(game, attackNumber, 4, 10, 1));
            waves.Add(new Wave(game, attackNumber, 5, 1, 4));
            waves.Add(new Wave(game, attackNumber, 6, 5, 2));
            waves.Add(new Wave(game, attackNumber, 7, 5, 1));
            waves.Add(new Wave(game, attackNumber, 8, 4, 3));
        }

        public void createWaves()
        {
            // calculate how many waves will be needed, set inAttack to equal this number
            // set up each wave and add it to the list

        }

        private void generateAttackNumbers()
        {
            //could be a useful function for calculating some numbers separate from wave creation - may not be needed, do not know yet
        }

    }
}
