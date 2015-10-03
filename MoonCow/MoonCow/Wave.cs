using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MoonCow
{
    // Still needs a way of randomly spawining different enemy types to the correct ratios (see gogole doc)
    class Wave
    {
        Game1 game;
        float countDown;
        int inWave;
        int waveMax;
        int waveNumber;
        int attackNumber;
        int maxGunner;
        int maxSneaker;
        int maxHeavy;
        int maxSwarmer;

        public Wave(Game1 game, int attackNo, int waveNo)
        {
            this.game = game;
            countDown = 0f;
            inWave = 0;
            waveMax = (5 * attackNo) + (waveNo * (2 * Settings.difficulty)); // + dynamic modifier, which can be a negative or positive number
            waveNumber = waveNo;
            attackNumber = attackNo;
            generateWaveDistribution();
        }

        public void spawn()
        {
            countDown -= Utilities.deltaTime;
            if (countDown <= 0)
            {
                countDown = 0.6f;
                if (inWave < waveMax)
                {
                    inWave++;
                    List<int> availableEnemies = new List<int>();
                    if(maxSwarmer > 0)
                    {
                        availableEnemies.Add(1);
                    }
                    if(maxGunner > 0)
                    {
                        availableEnemies.Add(2);
                    }
                    if(maxSneaker > 0)
                    {
                        availableEnemies.Add(3);
                    }
                    if(maxHeavy > 0)
                    {
                        availableEnemies.Add(4);
                    }

                    Random rngeesus = new Random();
                    int spawnType = rngeesus.Next(0, availableEnemies.Count());
                    spawnType = availableEnemies[spawnType];

                    switch(spawnType)
                    {
                        case 1:
                            game.enemyManager.addEnemy(new Swarmer(game));
                            maxSwarmer--;
                            break;
                        case 2:
                            game.enemyManager.addEnemy(new Gunner(game));
                            maxGunner--;
                            break;
                        case 3:
                            game.enemyManager.addEnemy(new Sneaker(game));
                            maxSneaker--;
                            break;
                        case 4:
                            game.enemyManager.addEnemy(new Heavy(game));
                            maxHeavy--;
                            break;
                    }

                }

                if (inWave == waveMax)
                {
                    inWave = 0;
                    countDown = 0;
                    game.waveManager.activeAttack.endWave();
                }
            }
        }

        private void generateWaveDistribution()
        {
            switch(attackNumber)
            {
                case 1:
                    maxHeavy = 0;
                    maxSneaker = 0;
                    maxGunner = (int)(0.25f * waveMax);
                    if(maxGunner <= 0)
                    {
                        maxGunner = 1;
                    }
                    break;
                case 2:
                    maxHeavy = 0;
                    maxSneaker = (int)((1f/9f) * waveMax);
                    if (maxSneaker <= 0)
                    {
                        maxSneaker = 1;
                    }
                    maxGunner = (int)((2f/9f) * waveMax);
                    if(maxGunner <= 0)
                    {
                        maxGunner = 1;
                    }
                    break;
                case 3:
                    maxHeavy = (int)((1f/13f) * waveMax);
                    if (maxHeavy <= 0)
                    {
                        maxHeavy = 1;
                    }
                    maxSneaker = (int)((2f/13f) * waveMax);
                    if (maxSneaker <= 0)
                    {
                        maxSneaker = 1;
                    }
                    maxGunner = (int)((4f/13f) * waveMax);
                    if(maxGunner <= 0)
                    {
                        maxGunner = 1;
                    }
                    break;
                case 4:
                    maxHeavy = (int)((2f/15f) * waveMax);
                    if (maxHeavy <= 0)
                    {
                        maxHeavy = 1;
                    }
                    maxSneaker = (int)((3f/15f) * waveMax);
                    if (maxSneaker <= 0)
                    {
                        maxSneaker = 1;
                    }
                    maxGunner = (int)((4f/15f) * waveMax);
                    if(maxGunner <= 0)
                    {
                        maxGunner = 1;
                    }
                    break;
            }
            maxSwarmer = waveMax - maxGunner - maxSneaker - maxHeavy;

            System.Diagnostics.Debug.WriteLine("Attack Number = " + attackNumber);
            System.Diagnostics.Debug.WriteLine("Wave Number = " + waveNumber);
            System.Diagnostics.Debug.WriteLine("Max enemies in wave = " + waveMax);
            System.Diagnostics.Debug.WriteLine("Swarmers in wave = " + maxSwarmer);
            System.Diagnostics.Debug.WriteLine("Gunners in wave = " + maxGunner);
            System.Diagnostics.Debug.WriteLine("Sneakers in wave = " + maxSneaker);
            System.Diagnostics.Debug.WriteLine("Heavies in wave = " + maxHeavy);
        }
    }
}
