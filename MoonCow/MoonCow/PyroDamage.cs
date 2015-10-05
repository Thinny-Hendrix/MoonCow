using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MoonCow
{
    public class PyroDamage
    {
        //freezes the enemy for a time, inflicts damage on the enemy after time over
        //every enemy has one of these, functions activate it
        Enemy enemy;
        Game1 game;
        public bool active;
        float time;
        float maxTime;
        float damage;

        public PyroDamage(Enemy enemy, Game1 game)
        {
            this.enemy = enemy;
            this.game = game;
            maxTime = 3f;
            damage = 2;
        }

        public void Update()
        {
            if (active)
            {
                game.modelManager.addEffect(new FireParticle(enemy.pos, game, 1));
                time -= Utilities.deltaTime;
                enemy.health -= Utilities.deltaTime * damage;
                if(time <= 0)
                {
                    active = false;
                }
            }
        }

        public void activate()
        {
            if (!active)
            {
                game.modelManager.addEffect(new LaserHitEffect(game, enemy.pos, Color.Red));
                active = true;
                time = maxTime;
            }
        }
    }
}
