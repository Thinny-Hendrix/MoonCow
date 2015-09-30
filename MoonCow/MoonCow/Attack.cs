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
        int inAttack;
        int waveMax;
        int waveNumber;
        int attackNumber;
        public bool active;
        public Utilities.SpawnState spawnState;

        public Attack(Game1 game, int waveNo)
        {
            this.game = game;
            waitTime = 0f;
            inAttack = 0;
            waveMax = 4;
            waveNumber = waveNo;
            active = true;
        }

        public void update()
        {

        }

        public void endWave()
        {
            spawnState = Utilities.SpawnState.idle;
            //startMessageTriggered = false;
        }

    }
}
