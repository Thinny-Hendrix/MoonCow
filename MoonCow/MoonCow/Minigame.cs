using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    public class Minigame:GameComponent
    {
        Game1 game;
        public MgManager manager;
        public bool active;
        public bool start; //true when beginning game, false when pause on end;
        public bool success;

        public float maxBeats;
        public float maxDubs;
        public float challengeBeats;
        public float challengeDubs;

        public int successCount;
        public int attempts;

        MgInstance defaultInstance;
        MgDisplayer displayer;
        public MgScreen screen;
        public MgModelManager models;
        public HudMg hudMg;
        JunkShip activeSource;

        public float holdTime;
        public float holdThresh;

        public float moneyEarned;

        bool drillGame;

        public Minigame(Game1 game):base(game)
        {
            this.game = game;
            manager = new MgManager(this, game);
            screen = new MgScreen(this, manager, game);
            manager.screen = screen;
            hudMg = game.hud.hudMg;

            active = false;

            successCount = 0;
            maxBeats = 12;
            maxDubs = 4;
            holdTime = 0;

            displayer = new MgDisplayer(this, manager, game);
            game.modelManager.addEffect(displayer);

            models = new MgModelManager(game, this);

            defaultInstance = new MgInstance(game, this);
            defaultInstance.generateNew();
            manager.loadInstance(defaultInstance);
            manager.reset();

            game.hud.expSelect.setMinigame(this);
        }

        public override void Update(GameTime gameTime)
        {
            if(active)
            {
                if (holdTime > holdThresh && !hudMg.active)
                {
                    if (start)
                        manager.Update();
                    else
                    {
                        if(success && !drillGame)
                        {
                            game.hud.expSelect.activate();
                        }
                        else
                        {
                            abort();
                        }
                    }
                }
                else
                    holdTime += Utilities.deltaTime;

                models.Update();
                manager.Draw();
                screen.Update();
                screen.Draw();
            }
        }

        void runGame()
        {
            active = true;
            start = true;
            holdTime = 0;
            holdThresh = 2;
            manager.reset();
        }

        public void abort()
        {
            if (success)
            {
                activeSource.beatMinigame(moneyEarned);
            }
            active = false;
            game.camera.followShip();
            displayer.shut();
            hudMg.reset();
        }

        public void close()
        {
            holdTime = 0;
            holdThresh = 2;
            start = false;
        }

        public void activate(Vector3 pos, Vector3 dir, JunkShip source)
        {
            drillGame = false;
            this.activeSource = source;
            displayer.wake(pos, dir);
            active = true;
            start = true;
            holdTime = 0;
            holdThresh = 2;
            hudMg.trigger(manager.speed);
            manager.reset();
            moneyEarned = 0;
            success = false;
        }

        public void activateHard(Vector3 pos, Vector3 dir, JunkShip source)
        {
            drillGame = true;
            this.activeSource = source;
            displayer.wake(pos, dir);
            active = true;
            start = true;
            holdTime = 0;
            holdThresh = 2;
            hudMg.trigger(manager.speed);
            manager.reset();
            moneyEarned = 0;
            success = false;
        }

        public void addMoney(float amount)
        {
            if (amount != 0)
                moneyEarned += amount;
            else
                moneyEarned = 0;
            screen.setMoney();
        }

        public void updateStats(bool win)
        {
            if(win)
            {
                manager.normSpeed += 100;
                maxBeats += 2;
                maxDubs = (int)Math.Floor((float)maxBeats / 2);
            }
            if(!win)
            {
                manager.normSpeed -= 15;
            }

        }

    }
}
