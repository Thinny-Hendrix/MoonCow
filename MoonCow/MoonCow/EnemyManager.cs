using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MoonCow
{
    public class EnemyManager : Microsoft.Xna.Framework.GameComponent
    {
        public List<Enemy> enemies = new List<Enemy>();
        public List<Enemy> toDelete = new List<Enemy>();
        Game1 game;
        float countdown;
        int inWave;//determines how much of the wave has spawned
        int waveMax;
        public enum SpawnState { idle, deploying}
        public SpawnState spawnState;

        public EnemyManager(Game1 game) : base(game)
        {
            this.game = game;
            countdown = 0;
            spawnState = SpawnState.idle;
            waveMax = 10;
        }

        public override void Initialize()
        {
            //addEnemy(new Enemy(this.game));
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            if(spawnState == SpawnState.idle && Keyboard.GetState().IsKeyDown(Keys.R))
            {
                spawnState = SpawnState.deploying;
            }
            if (spawnState == SpawnState.deploying)
            {
                countdown -= Utilities.deltaTime;
                if (countdown <= 0)
                {
                    countdown = 2;
                    if (inWave < waveMax)
                    {
                        inWave++;
                        addEnemy(new Enemy(game));
                    }

                    if(inWave == waveMax)
                    {
                        spawnState = SpawnState.idle;
                        inWave = 0;
                        countdown = 0;
                    }
                }
            }
            foreach (Enemy enemy in enemies)
            {
                enemy.Update(gameTime);
            }

            foreach (Enemy enemy in toDelete)
            {
                enemies.Remove(enemy);
            }
        }

        public void addEnemy(Enemy enemy)
        {
            enemies.Add(enemy);
        }

        public void removeEnemy(Enemy enemy)
        {
            enemies.Remove(enemy);
        }
    }
}
