using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MoonCow
{
    class WeaponLaser:Weapon
    {
        int laserPos;
        public WeaponLaser(WeaponSystem wepSys, Ship ship, Game1 game):base(wepSys, ship, game)
        {
            coolMax = 15;
            laserPos = 0;
        }

        public override void Fire()
        {
            if(cooldown == 0)
            {
                if (laserPos == 0)
                {
                    projectiles.Add(new Projectile(ship.pos + new Vector3(Vector3.Cross(Vector3.Up, ship.direction).X * 0.25f, 0, Vector3.Cross(Vector3.Up, ship.direction).Z * 0.25f), ship.direction, game, this, 1));
                    laserPos = 1;
                    game.audioManager.shootLaser();
                }
                else
                {
                    projectiles.Add(new Projectile(ship.pos + new Vector3(-Vector3.Cross(Vector3.Up, ship.direction).X * 0.25f, 0, -Vector3.Cross(Vector3.Up, ship.direction).Z * 0.25f), ship.direction, game, this, 1));
                    laserPos = 0;
                    game.audioManager.shootLaser2();
                }
                cooldown = coolMax;
            }
        }
        
    }
}
