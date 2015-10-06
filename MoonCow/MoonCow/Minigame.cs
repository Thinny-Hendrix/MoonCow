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

        public float holdTime;
        public float holdThresh;

        public float moneyEarned;

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
                        abort();
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
            active = false;
            game.camera.followShip();
            displayer.shut();
        }

        public void close()
        {
            holdTime = 0;
            holdThresh = 2;
            start = false;
        }

        public void activate(Vector3 pos, Vector3 dir)
        {
            displayer.wake(pos, dir);
            active = true;
            start = true;
            holdTime = 0;
            holdThresh = 2;
            hudMg.trigger(manager.speed);
            manager.reset();
            moneyEarned = 0;
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
                manager.speed += 100;
                maxBeats += 2;
                maxDubs = (int)Math.Floor((float)maxBeats / 2);
            }
            if(!win)
            {
                manager.speed -= 15;
            }

        }

    }
}
