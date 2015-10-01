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

        public Wave(Game1 game, int attackNo, int waveNo)
        {
            this.game = game;
            countDown = 0f;
            inWave = 0;
            waveMax = (5 * attackNo) + (waveNo * (2 * Settings.difficulty)); // + dynamic modifier, which can be a negative or positive number
            waveNumber = waveNo;
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
                    game.enemyManager.addEnemy(new Gunner(game));
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
