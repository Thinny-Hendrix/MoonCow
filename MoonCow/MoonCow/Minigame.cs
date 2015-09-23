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

        public float maxBeats;
        public float maxDubs;
        public float challengeBeats;
        public float challengeDubs;

        public int successCount;
        public int attempts;

        MgInstance defaultInstance;

        public Minigame(Game1 game):base(game)
        {
            this.game = game;
            manager = new MgManager(this, game);

            active = false;

            successCount = 0;
            maxBeats = 12;
            maxDubs = 4;

            defaultInstance = new MgInstance(game, this);
            defaultInstance.generateNew();
            manager.loadInstance(defaultInstance);
            manager.reset();
        }

        public override void Update(GameTime gameTime)
        {
            if(active)
            {
                manager.Update();
            }
        }

        void runGame()
        {
            active = true;
            manager.reset();
        }

        public void abort()
        {
            active = false;
            game.camera.followShip();
        }

        public void activate()
        {
            active = true;
            manager.reset();
        }

        public void updateStats()
        {

        }

    }
}
