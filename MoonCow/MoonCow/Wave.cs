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
        public int enemyType;
        public bool started;

        public Wave(Game1 game, int attackNo, int waveNo, int enemies, int eType)
        {
            this.game = game;
            countDown = 0f;
            inWave = 0;
            waveNumber = waveNo;
            attackNumber = attackNo;
            waveMax = enemies;
            enemyType = eType;
            started = false;
        }

        public void spawn()
        {
            started = true;
            countDown -= Utilities.deltaTime;
            if (countDown <= 0)
            {
                countDown = 0.6f;
                if (inWave < waveMax)
                {
                    inWave++;
                    switch(enemyType)
                    {
                        case 1:
                            game.enemyManager.addEnemy(new Swarmer(game));
                            break;
                        case 2:
                            game.enemyManager.addEnemy(new Gunner(game));
                            break;
                        case 3:
                            game.enemyManager.addEnemy(new Sneaker(game));
                            break;
                        case 4:
                            game.enemyManager.addEnemy(new Heavy(game));
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
    }
}
