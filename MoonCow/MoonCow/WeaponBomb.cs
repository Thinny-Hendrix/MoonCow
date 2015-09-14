using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MoonCow
{
    class WeaponBomb:Weapon
    {
        float softCooldown;
        float softCoolmax;
        public WeaponBomb(WeaponSystem wepSys, Ship ship, Game1 game):base(wepSys, ship, game)
        {
            icon = TextureManager.icoBomb;
            name = "Exobomb";

            rateOfFire = 0.2f;
            damage = 1;
            range = 0.8f;
            coolMax = 90;
            softCoolmax = 60;
            ammoMax = 16;
            ammo = ammoMax;
        }

        public override void Update()
        {
            if (softCooldown != 0)
            {
                softCooldown -= Utilities.deltaTime * 60;
                if (softCooldown < 0)
                    softCooldown = 0;
            }
            base.Update();
        }

        public override void Fire()
        {
            if (ammo > 0)
            {
                if (cooldown == 0 || (projectiles.Count() == 0 && softCooldown == 0))
                {
                    projectiles.Add(new Projectile(ship.pos + new Vector3(0, 0, ship.direction.Z * 0.25f), ship.direction, game, this, 0));
                    game.audioManager.shootLaser2();
                    cooldown = coolMax;
                    softCooldown = softCoolmax;
                    base.Fire();
                }
            }
        }
        
    }
}
