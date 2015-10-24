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
        float dist;

        public PyroDamage(Enemy enemy, Game1 game, int enemyType)
        {
            this.enemy = enemy;
            this.game = game;
            maxTime = 3f;
            damage = 5;

            switch(enemyType)
            {
                default:
                    dist = 1;
                    break;
                case 3:
                    dist = 5;
                    break;
            }
        }

        public void Update()
        {
            if (active)
            {
                game.modelManager.addEffect(new FireParticle(enemy.pos, game, dist));
                if(dist == 5)
                {
                    game.modelManager.addEffect(new FireParticle(enemy.pos, game, dist));
                    game.modelManager.addEffect(new FireParticle(enemy.pos, game, dist));
                }
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
            }
            time = maxTime;
        }
    }
}
