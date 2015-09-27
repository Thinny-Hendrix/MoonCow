using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MoonCow
{
    public class WaveManager : Microsoft.Xna.Framework.GameComponent
    {
        Game1 game;
        float countdown;
        float waitCounter;
        int waveCount;
        int inWave;//determines how much of the wave has spawned
        int waveMax;
        public enum SpawnState { idle, deploying, waiting }
        public SpawnState spawnState;
        bool endMessageTriggered;
        bool startMessageTriggered;

        public WaveManager(Game1 game) : base(game)
        {
            this.game = game;
            countdown = 0;
            waitCounter = 15;
            waveCount = 0;
            spawnState = SpawnState.waiting;
            waveMax = 20;
            endMessageTriggered = true;
            startMessageTriggered = false;
        }

        //  need to add incremental difficulty for each wave with a modifier for dynamic difficulty, also a end to the level (read in how many waves the level lasts for from the xml?)
        public override void Update(GameTime gametime)
        {
            if (!Utilities.paused && !Utilities.softPaused)
            {
                if(spawnState == SpawnState.idle && game.enemyManager.enemies.Count() == 0) // A wave just ended, so transition into waiting state
                {
                    spawnState = SpawnState.waiting;
                    game.hud.hudAttackDisplayer.endAttackMessage();
                    waitCounter = 50f; // 50 seconds between waves
                }
                if(spawnState == SpawnState.waiting)    // The wait between waves
                {
                    waitCounter -= Utilities.deltaTime;
                    if(waitCounter <= 0)
                    {
                        spawnState = SpawnState.deploying;
                    }
                }
                if(spawnState == SpawnState.deploying)  // Deplaying enemies
                {
                    if(!startMessageTriggered)
                    {
                        waveCount++;
                        game.hud.hudAttackDisplayer.startAttackMessage(waveCount);
                        startMessageTriggered = true;
                    }
                    
                    // spawn enemies
                    countdown -= Utilities.deltaTime;
                    if (countdown <= 0)
                    {
                        countdown = 0.6f;
                        if (inWave < waveMax)
                        {
                            inWave++;
                            game.enemyManager.addEnemy(new Swarmer(game));
                        }

                        if (inWave == waveMax)
                        {
                            spawnState = SpawnState.idle;
                            inWave = 0;
                            countdown = 0;
                            startMessageTriggered = false;
                        }
                    }
                }
            }
        }
    }
}
