using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MoonCow
{
    class Wave
    {
        Game1 game;
        float countDown;
        int inWave;
        int waveMax;
        int waveNumber;

        public Wave(Game1 game, int waveNo)
        {
            this.game = game;
            countDown = 0f;
            inWave = 0;
            waveMax = 20;
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
                    game.enemyManager.addEnemy(new Swarmer(game));
                }

                if (inWave == waveMax)
                {
                    inWave = 0;
                    countDown = 0;
                    game.waveManager.endWave();
                }
            }
        }
    }
}
