using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MoonCow
{
    class WeaponWave:Weapon
    {
        int laserPos;
        public WeaponWave(WeaponSystem wepSys, Ship ship, Game1 game):base(wepSys, ship, game)
        {
            icon = TextureManager.icoWave;
            name = "Pulse Generator";

            damage = 0.7f;
            rateOfFire = 0.4f;
            range = 0.3f;

            coolMax = 15;
            laserPos = 0;
        }

        public override void Fire()
        {
            if(cooldown == 0)
            {
                if (laserPos == 0)
                {
                    projectiles.Add(new Projectile(ship.pos + new Vector3(Vector3.Cross(Vector3.Up, ship.direction).X * 0.25f, 0, Vector3.Cross(Vector3.Up, ship.direction).Z * 0.25f), ship.direction, game, this, 2));
                    laserPos = 1;

                    game.audioManager.shipShootLaser.Stop();
                    game.audioManager.shipShootLaser.Play();
                }
                else
                {
                    projectiles.Add(new Projectile(ship.pos + new Vector3(-Vector3.Cross(Vector3.Up, ship.direction).X * 0.25f, 0, -Vector3.Cross(Vector3.Up, ship.direction).Z * 0.25f), ship.direction, game, this, 2));
                    laserPos = 0;

                    game.audioManager.shipShootLaser2.Stop();
                    game.audioManager.shipShootLaser2.Play();
                }
                cooldown = coolMax;
            }
        }
        
    }
}
