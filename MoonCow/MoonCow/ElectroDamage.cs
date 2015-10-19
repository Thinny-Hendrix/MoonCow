using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MoonCow
{
    public class ElectroDamage
    {
        //freezes the enemy for a time, inflicts damage on the enemy after time over
        //every enemy has one of these, functions activate it
        Enemy enemy;
        Game1 game;
        public bool active;
        float time;
        float maxTime;
        float damage;

        public ElectroDamage(Enemy enemy, Game1 game)
        {
            this.enemy = enemy;
            this.game = game;
            maxTime = 0.5f;
            enemy.frozen = true;
        }

        public void Update()
        {
            if (active)
            {
                game.modelManager.addEffect(new ElecParticle(enemy.pos, game));
                time -= Utilities.deltaTime;
                if(time <= 0)
                {
                    active = false;
                    enemy.health -= damage;
                    enemy.frozen = false;
                    damage = 0;
                }
            }
        }

        public void activate(float damage)
        {
            this.damage += damage;
            if (!active)
            {
                game.modelManager.addEffect(new LaserHitEffect(game, enemy.pos, Color.SeaGreen));
                active = true;
                time = maxTime;
            }
        }
    }
}
