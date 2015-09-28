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
        List<Wave> waves = new List<Wave>();
        Wave activeWave;
        float waitCounter;
        int waveCount;
        public enum SpawnState { idle, deploying, waiting }
        public SpawnState spawnState;
        bool endMessageTriggered;
        bool startMessageTriggered;

        public WaveManager(Game1 game) : base(game)
        {
            this.game = game;
            waitCounter = 15;
            waveCount = 1;
            spawnState = SpawnState.waiting;
            endMessageTriggered = true;
            startMessageTriggered = false;
            activeWave = new Wave(game, waveCount);
            waves.Add(activeWave);
        }

        //  The next wave is created when the wave before it is killed, in this way the wave data exists during the waiting time for statistics about upcoming wave to be accessed and displayed
        public override void Update(GameTime gametime)
        {
            if (!Utilities.paused && !Utilities.softPaused)
            {
                if(spawnState == SpawnState.idle && game.enemyManager.enemies.Count() == 0) // A wave just ended, so transition into waiting state
                {
                    spawnState = SpawnState.waiting;
                    game.hud.hudAttackDisplayer.endAttackMessage();
                    waitCounter = 50f; // 50 seconds between waves
                    activeWave = new Wave(game, waveCount); // create next wave
                    waves.Add(activeWave);
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
                        game.hud.hudAttackDisplayer.startAttackMessage(waveCount);
                        startMessageTriggered = true;
                        waveCount++;
                    }
                    
                    // spawn enemies
                    activeWave.spawn();   
                }
            }
        }

        public void endWave()
        {
            spawnState = SpawnState.idle;
            startMessageTriggered = false;
        }

    }
}
