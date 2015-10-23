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
        public float maxWait;
        public List<Wave> waves = new List<Wave>();
        Wave activeWave;
        WaveManager manager;
        public int inAttack;
        int currentWaveNumber;
        int attackNumber;
        public bool active;

        public Attack(Game1 game, WaveManager manager, int attackNo)
        {
            this.game = game;
            this.manager = manager;
            waitTime = 5f;
            maxWait = waitTime;
            currentWaveNumber = -1; 
            attackNumber = attackNo;
            active = true;
            testWave();
            //manualWaves(attackNo);
            //createWaves();
            //tempWaveCreator();
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
                {
                    if (game.enemyManager.enemies.Count() == 0)//once all waves have been activated, wait until all enemies have been killed before ending the attack         
                    {
                        manager.endAttack();
                    }
                }
            }

        }

        void manualWaves(int i)
        {
            switch(i)
            {
                case 1:
                    inAttack = 3;
                    waves.Add(new Wave(game, manager, attackNumber, 1, 4, 0));
                    waves.Add(new Wave(game, manager, attackNumber, 2, 2, 2));
                    waves.Add(new Wave(game, manager, attackNumber, 3, 8, 0));
                    break;
                case 2:
                    inAttack = 4;
                    waves.Add(new Wave(game, manager, attackNumber, 1, 2, 2));
                    waves.Add(new Wave(game, manager, attackNumber, 2, 6, 0));
                    waves.Add(new Wave(game, manager, attackNumber, 3, 4, 1));
                    waves.Add(new Wave(game, manager, attackNumber, 4, 12, 0));
                    break;
                case 3:
                    inAttack = 5;
                    waves.Add(new Wave(game, manager, attackNumber, 1, 1, 3));
                    waves.Add(new Wave(game, manager, attackNumber, 2, 2, 2));
                    waves.Add(new Wave(game, manager, attackNumber, 3, 8, 0));
                    waves.Add(new Wave(game, manager, attackNumber, 4, 2, 1));
                    waves.Add(new Wave(game, manager, attackNumber, 5, 16, 0));
                    break;
                default:
                    inAttack = 6;
                    waves.Add(new Wave(game, manager, attackNumber, 1, 12, 0));
                    waves.Add(new Wave(game, manager, attackNumber, 2, 1, 3));
                    waves.Add(new Wave(game, manager, attackNumber, 3, 3, 2));
                    waves.Add(new Wave(game, manager, attackNumber, 4, 6, 1));
                    waves.Add(new Wave(game, manager, attackNumber, 5, 6, 2));
                    waves.Add(new Wave(game, manager, attackNumber, 6, 2, 3));
                    break;
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
            waitTime = (float)Math.Ceiling(activeWave.cDownThresh * activeWave.waveMax + 8);
            maxWait = waitTime;
        }

        void testWave()
        {
            inAttack = 2;
            waves.Add(new Wave(game, manager, attackNumber, 1, 10, 0));
            waves.Add(new Wave(game, manager, attackNumber, 2, 20, 0));
            //waves.Add(new Wave(game, manager, attackNumber, 3, 3, 3));
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

            // Define which enemies are available in the attack based on attack number
            bool swarm = false;
            bool gun = false;
            bool sneak = false;
            bool heavy = false;

            switch(attackNumber)
            {
                case 1:
                    swarm = true;
                    gun = true;
                    sneak = false;
                    heavy = false;
                    break;
                case 2:
                    swarm = true;
                    gun = true;
                    sneak = false;
                    heavy = false;
                    break;
                case 3:
                    swarm = true;
                    gun = true;
                    sneak = true;
                    heavy = false;
                    break;
                default:
                    swarm = true;
                    gun = true;
                    sneak = true;
                    heavy = true;
                    break;
            }
            // Currently does not populate waves correctly, onle one enemy each wave
            // Will eventually decide upon how many enemies to put in the wave based on: enemyType, attack, wave and dynamic modifier
            if(swarm)
            {
                int waveCount = (int)((attackNumber + (int)Settings.difficulty) / 3) + 1;
                for(int i = 0; i < waveCount; i++)
                {
                    int enemyCount = (attackNumber / 2) + (i + 1) + 7; // +/- dynamic thingy
                    waves.Add(new Wave(game, manager, attackNumber, 1, enemyCount, 0));
                }
            }
            if (gun)
            {
                int waveCount = (int)((attackNumber + (int)Settings.difficulty) / 3);
                for (int i = 0; i < waveCount; i++)
                {
                    int enemyCount = (attackNumber / 4) + (i + 1) + 3; // +/- dynamic thingy
                    waves.Add(new Wave(game, manager, attackNumber, 1, enemyCount, 2));
                }
            }
            if (sneak)
            {
                int waveCount = (int)((attackNumber + (int)Settings.difficulty) / 4);
                for (int i = 0; i < waveCount; i++)
                {
                    int enemyCount = (attackNumber / 4) + (i + 1) + 2; // +/- dynamic thingy
                    waves.Add(new Wave(game, manager, attackNumber, 1, enemyCount, 1));
                }
            }
            if (heavy)
            {
                int waveCount = (int)((attackNumber + (int)Settings.difficulty) / 5);
                for (int i = 0; i < waveCount; i++)
                {
                    int enemyCount = i + 1; // +/- dynamic thingy
                    waves.Add(new Wave(game, manager, attackNumber, 1, enemyCount, 3));
                }
            }
            
            inAttack = waves.Count();

            if(attackNumber != 1)
            {
                shuffleList();
            }
        }

        private void shuffleList()
        {
            //shuffle the list then give each wave the right wave number
            Random rng = new Random();
            List<Wave> tempList = new List<Wave>();
            int c = waves.Count();
            for(int i = 0; i < c; i++)
            {
                int randomNumber = rng.Next(0, waves.Count());
                tempList.Add(waves[randomNumber]);
                waves.RemoveAt(randomNumber);
            }

            waves = tempList;
            for(int i = 0; i < waves.Count(); i++)
            {
                waves[i].waveNumber = i + 1;
            }
        }

    }
}
