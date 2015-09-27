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
        int waveCount;
        int inWave;//determines how much of the wave has spawned
        int waveMax;
        public enum SpawnState { idle, deploying }
        public SpawnState spawnState;
        bool endMessageTriggered;

        public WaveManager(Game1 game) : base(game)
        {
            this.game = game;
            countdown = 0;
            waveCount = 0;
            spawnState = SpawnState.idle;
            waveMax = 20;
            endMessageTriggered = true;
        }

        // new update - not yet active - will automatically spawn waves, wait for period of time, then spawn a harder wave, dynamic difficulty will be a thing
        public void update(GameTime gametime)
        {
            if (!Utilities.paused && !Utilities.softPaused)
            {
                if(spawnState == SpawnState.idle && game.enemyManager.enemies.Count() == 0)
                {

                }
            }
        }

        // previous update - still active - press R for enemies
        public override void Update(GameTime gameTime)
        {
            if (!Utilities.paused && !Utilities.softPaused)
            {
                if (spawnState == SpawnState.idle)
                {
                    if (!endMessageTriggered && game.enemyManager.enemies.Count() == 0)
                    {
                        game.hud.hudAttackDisplayer.endAttackMessage();
                        endMessageTriggered = true;
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.R) || GamePad.GetState(PlayerIndex.One).Buttons.LeftShoulder == ButtonState.Pressed)
                    {
                        if (game.enemyManager.enemies.Count() == 0)
                            game.hud.hudAttackDisplayer.startAttackMessage(1);
                        spawnState = SpawnState.deploying;
                        endMessageTriggered = false;
                    }
                }
                if (spawnState == SpawnState.deploying)
                {
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
                        }
                    }
                }
            }


        }
    }
}
