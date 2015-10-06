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
        Wave activeWave;
        int inAttack;
        int attackNumber;
        public bool active;
        public Utilities.SpawnState spawnState;

        int maxEnemies;
        int maxGunner;
        int maxSneaker;
        int maxHeavy;
        int maxSwarmer;
        bool attackDone;

        public Attack(Game1 game, int attackNo)
        {
            this.game = game;
            waitTime = 0f;
            inAttack = 1; 
            attackNumber = attackNo;
            active = true;
            generateAttackNumbers();
            spawnState = Utilities.SpawnState.waiting;
            createWave();
        }

        public void update()
        {
            if (!Utilities.paused && !Utilities.softPaused)
            {
                if (spawnState == Utilities.SpawnState.idle) // An wave just finished spawning, so transition into waiting state
                {
                    if (active)
                    {
                        spawnState = Utilities.SpawnState.waiting;
                        inAttack++;
                        //game.hud.hudAttackDisplayer.endAttackMessage();
                        waitTime = 15; // 20 seconds between waves
                        createWave(); // create next attack
                        waves.Add(activeWave);
                    }
                    else
                    {
                        if (game.enemyManager.enemies.Count() == 0)
                        {
                            game.waveManager.endAttack();
                        }
                    }
                }

                if (spawnState == Utilities.SpawnState.waiting)    // The wait between attacks
                {
                    waitTime -= Utilities.deltaTime;
                    if (waitTime <= 0 )//|| Keyboard.GetState().IsKeyDown(Keys.R))
                    {
                        waitTime = 0;
                        spawnState = Utilities.SpawnState.deploying;
                    }
                }

                if (spawnState == Utilities.SpawnState.deploying)  // Attack is active
                {
                    // spawn enemies
                    activeWave.spawn();
                }
            }

        }

        public void createWave()
        {
            activeWave = new Wave(game, attackNumber, inAttack, 5, 1); // temp to keep the game going
            waves.Add(activeWave);
        }

        public void endWave()
        {
            spawnState = Utilities.SpawnState.idle;
            //startMessageTriggered = false;
        }

        private void generateAttackNumbers()
        {

            maxEnemies = (20 * attackNumber) + (5 * (4 * Settings.difficulty)); // + dynamic thingy

            switch (attackNumber)
            {
                case 1:
                    maxHeavy = 0;
                    maxSneaker = 0;
                    maxGunner = (int)(0.25f * maxEnemies);
                    if (maxGunner <= 0)
                    {
                        maxGunner = 1;
                    }
                    break;
                case 2:
                    maxHeavy = 0;
                    maxSneaker = (int)((1f / 9f) * maxEnemies);
                    if (maxSneaker <= 0)
                    {
                        maxSneaker = 1;
                    }
                    maxGunner = (int)((2f / 9f) * maxEnemies);
                    if (maxGunner <= 0)
                    {
                        maxGunner = 1;
                    }
                    break;
                case 3:
                    maxHeavy = (int)((1f / 13f) * maxEnemies);
                    if (maxHeavy <= 0)
                    {
                        maxHeavy = 1;
                    }
                    maxSneaker = (int)((2f / 13f) * maxEnemies);
                    if (maxSneaker <= 0)
                    {
                        maxSneaker = 1;
                    }
                    maxGunner = (int)((4f / 13f) * maxEnemies);
                    if (maxGunner <= 0)
                    {
                        maxGunner = 1;
                    }
                    break;
                case 4:
                    maxHeavy = (int)((2f / 15f) * maxEnemies);
                    if (maxHeavy <= 0)
                    {
                        maxHeavy = 1;
                    }
                    maxSneaker = (int)((3f / 15f) * maxEnemies);
                    if (maxSneaker <= 0)
                    {
                        maxSneaker = 1;
                    }
                    maxGunner = (int)((4f / 15f) * maxEnemies);
                    if (maxGunner <= 0)
                    {
                        maxGunner = 1;
                    }
                    break;
            }
            maxSwarmer = maxEnemies - maxGunner - maxSneaker - maxHeavy;

            System.Diagnostics.Debug.WriteLine("Attack Number = " + attackNumber);
            System.Diagnostics.Debug.WriteLine("Max enemies in attack = " + maxEnemies);
            System.Diagnostics.Debug.WriteLine("Swarmers in attack = " + maxSwarmer);
            System.Diagnostics.Debug.WriteLine("Gunners in attack = " + maxGunner);
            System.Diagnostics.Debug.WriteLine("Sneakers in attack = " + maxSneaker);
            System.Diagnostics.Debug.WriteLine("Heavies in attack = " + maxHeavy);
        }

    }
}
