using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MoonCow
{
    public class WeaponWave:Weapon
    {
        public WeaponWave(WeaponSystem wepSys, Ship ship, Game1 game):base(wepSys, ship, game)
        {
            icon = TextureManager.icoWave;
            name = "Pulse Generator";

            damage = 0.7f;
            rateOfFire = 0.4f;
            range = 0.3f;

            coolMax = 15;
        }

        public override void Fire()
        {
            if(cooldown == 0)
            {
                projectiles.Add(new WaveProjectile(ship.pos + ship.direction * 0.1f, ship.direction, game, this, 2));
                game.levelStats.wavesFired++;
                game.audioManager.addSoundEffect(AudioLibrary.shipShootLaser, 0.1f);

                cooldown = coolMax;
            }
        }
        
    }
}
