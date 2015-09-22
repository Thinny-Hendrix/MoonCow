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
        public List<Sentry> sentries = new List<Sentry>();
        public List<Sentry> sToDelete = new List<Sentry>();
        Game1 game;
        float countdown;
        int inWave;//determines how much of the wave has spawned
        int waveMax;
        public enum SpawnState { idle, deploying}
        public SpawnState spawnState;
        bool endMessageTriggered;

        public EnemyManager(Game1 game) : base(game)
        {
            this.game = game;
            countdown = 0;
            spawnState = SpawnState.idle;
            waveMax = 20;
            endMessageTriggered = true;
        }

        public override void Initialize()
        {
            //addEnemy(new Enemy(this.game));
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            if (!Utilities.paused && !Utilities.softPaused)
            {
                if (spawnState == SpawnState.idle)
                {
                    if (!endMessageTriggered && enemies.Count() == 0)
                    {
                        game.hud.hudAttackDisplayer.endAttackMessage();
                        endMessageTriggered = true;
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.R) || GamePad.GetState(PlayerIndex.One).Buttons.LeftShoulder == ButtonState.Pressed)
                    {
                        if (enemies.Count() == 0)
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
                            addEnemy(new Swarmer(game));
                        }

                        if (inWave == waveMax)
                        {
                            spawnState = SpawnState.idle;
                            inWave = 0;
                            countdown = 0;
                        }
                    }
                }
                foreach (Enemy enemy in enemies)
                    enemy.Update(gameTime);

                foreach (Enemy enemy in toDelete)
                    enemies.Remove(enemy);
                toDelete.Clear();

                foreach (Sentry s in sentries)
                    s.Update();

                foreach (Sentry s in sToDelete)
                    sentries.Remove(s);
                sToDelete.Clear();

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

        public void addSentry(Vector3 pos)
        {
            sentries.Add(new Sentry(game, this, pos));
        }
    }
}
