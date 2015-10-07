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
    public class Wave
    {
        Game1 game;
        float countDown;
        int inWave;
        public int waveMax;
        public int waveNumber;
        int attackNumber;
        public int enemyType;
        public float cDownThresh;
        WaveManager manager;

        public Wave(Game1 game, WaveManager manager, int attackNo, int waveNo, int enemies, int eType)
        {
            this.game = game;
            this.manager = manager;
            inWave = 0;
            waveNumber = waveNo;
            attackNumber = attackNo;
            waveMax = enemies;
            enemyType = eType;
            setTime();
            countDown = 0;
        }

        void setTime()
        {
            switch(enemyType)
            {
                default:
                    cDownThresh = manager.swaSpawnTime;
                    break;
                case 1:
                    cDownThresh = manager.sneSpawnTime;
                    break;
                case 2:
                    cDownThresh = manager.gunSpawnTime;
                    break;
                case 3:
                    cDownThresh = manager.hevSpawnTime;
                    break;
            }
        }

        public void update()
        {
            if(inWave < waveMax)
            { 
                countDown -= Utilities.deltaTime;
                if (countDown <= 0)
                {
                    countDown = cDownThresh;
                    inWave++;
                    switch (enemyType)
                    {
                        default:
                            game.enemyManager.addEnemy(new Swarmer(game));
                            break;
                        case 1:
                            game.enemyManager.addEnemy(new Sneaker(game));
                            break;
                        case 2:
                            game.enemyManager.addEnemy(new Gunner(game));
                            break;
                        case 3:
                            game.enemyManager.addEnemy(new Heavy(game));
                            break;
                    }
                }
            }
        }
    }
}
