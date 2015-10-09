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
        public CircleCollider col;
        public bool active;

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
            col = new CircleCollider(ship.pos+ship.direction, 0.3f);
        }

        public override void activate()
        {
            active = true;
        }

        public override void Update()
        {
            if (active)
            {
                col.Update(ship.pos + ship.direction);
                if (ship.moving)
                    checkCollision();
            }
            base.Update();
        }

        void checkCollision()
        {
            foreach (Asteroid a in game.asteroidManager.asteroids)
            {
                if (a.col.checkCircle(col))
                {
                    a.damage(1, ship.pos);
                    game.modelManager.addEffect(new ImpactParticleModel(game, ship.pos + ship.direction));
                }
            }
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
