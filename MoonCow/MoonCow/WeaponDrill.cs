using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    class WeaponDrill:Weapon
    {
        float softCooldown;
        float softCoolmax;
        public WeaponDrill(WeaponSystem wepSys, Ship ship, Game1 game):base(wepSys, ship, game)
        {
            icon = TextureManager.icoDrill;
            name = "Omega Drill";

            damage = 1;
            rateOfFire = 0;
            range = 0.1f;

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

                    game.audioManager.shipShootLaser2.Stop();
                    game.audioManager.shipShootLaser2.Play();
                    cooldown = coolMax;
                    softCooldown = softCoolmax;
                    base.Fire();
                }
            }
        }
        
    }
}
