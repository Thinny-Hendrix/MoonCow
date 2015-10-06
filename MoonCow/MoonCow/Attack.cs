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
        WaveManager manager;
        public int inAttack;
        int currentWaveNumber;
        int attackNumber;
        public bool active;

        int maxEnemies;
        int maxGunner;
        int maxSneaker;
        int maxHeavy;
        int maxSwarmer;

        public Attack(Game1 game, WaveManager manager, int attackNo)
        {
            this.game = game;
            this.manager = manager;
            waitTime = 5f;
            currentWaveNumber = -1; 
            attackNumber = attackNo;
            active = true;
            generateAttackNumbers();
            //createWaves();
            tempWaveCreator();
            //activeWave = waves[currentWaveNumber];
        }

        public void update()
        {
            if (!Utilities.paused && !Utilities.softPaused && !game.hud.hudAttackDisplayer.displayMessage)
            {
                if(currentWaveNumber != -1)//if the waves have started
                    activeWave.update();

                if (currentWaveNumber < inAttack - 1)//if there are still waves remaining to be activated
                {
                    waitTime -= Utilities.deltaTime;
                    if (waitTime <= 0)
                    {
                        changeWaves();
                        setWaitTime();
                        game.hud.hudAttackDisplayer.updateWaves();
                    }
                }
                else
                    if (game.enemyManager.enemies.Count() == 0)//once all waves have been activated, wait until all enemies have been killed before ending the attack
                        manager.endAttack();
            }

        }

        void changeWaves()
        {
            if (currentWaveNumber < inAttack-1)
            {
                //spawnState = Utilities.SpawnState.waiting;
                //game.hud.hudAttackDisplayer.endAttackMessage();
                //waitTime = 15; // 20 seconds between waves
                currentWaveNumber++;
                activeWave = waves[currentWaveNumber];
            }
        }

        void setWaitTime()
        {
            waitTime = (float)Math.Ceiling(activeWave.cDownThresh * activeWave.waveMax + 5);
        }

        private void tempWaveCreator()
        {
            // Purely to fill in while I get the actual dynamic system working
            inAttack = 8;
            waves.Add(new Wave(game, manager, attackNumber, 1, 5, 0));
            waves.Add(new Wave(game, manager, attackNumber, 2, 5, 2));
            waves.Add(new Wave(game, manager, attackNumber, 3, 4, 1));
            waves.Add(new Wave(game, manager, attackNumber, 4, 10, 0));
            waves.Add(new Wave(game, manager, attackNumber, 5, 1, 3));
            waves.Add(new Wave(game, manager, attackNumber, 6, 5, 1));
            waves.Add(new Wave(game, manager, attackNumber, 7, 5, 0));
            waves.Add(new Wave(game, manager, attackNumber, 8, 4, 2));
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
