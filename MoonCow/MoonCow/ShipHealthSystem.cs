using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MoonCow
{
    public class ShipHealthSystem:Microsoft.Xna.Framework.GameComponent
    {
        public float shieldVal;
        public float shieldMax;
        public float hpVal;
        public float hpMax;
        float shieldIdleTime;
        Ship ship;
        Game1 game;
        public enum ShieldState { max, recharging, idle }
        public ShieldState shieldState;


        public ShipHealthSystem(Game1 game, Ship ship):base(game)
        {
            this.game = game;
            this.ship = ship;

            reset();
        }

        public override void Update(GameTime gameTime)
        {
            if(shieldState == ShieldState.idle)
            {
                shieldIdleTime -= Utilities.deltaTime;
                if(shieldIdleTime < 0)
                {
                    shieldIdleTime = 0;
                    shieldState = ShieldState.recharging;
                }
            }

            if(shieldState == ShieldState.recharging)
            {
                shieldVal += Utilities.deltaTime * 40;
                if(shieldVal >= shieldMax)
                {
                    shieldVal = shieldMax;
                    shieldState = ShieldState.max;
                }
            }
        }

        public void onHit(float damage)
        {
            if (game.minigame.active)
                game.minigame.abort();
            if(damage < shieldVal)
            {
                shieldVal -= damage;
            }
            else if (shieldVal == 0)
            {
                hpVal -= damage;
            }
            else
            {
                damage -= shieldVal;
                shieldVal = 0;
                hpVal -= damage;
            }

            shieldState = ShieldState.idle;
            shieldIdleTime = 3;

            if(hpVal <= 0)
            {
                ship.onDeath();
                reset();
            }
        }

        void reset()
        {
            shieldMax = 100;
            shieldVal = 100;
            hpVal = 100;
            hpMax = 100;
            shieldIdleTime = 0;
            shieldState = ShieldState.max;
        }
    }
}
