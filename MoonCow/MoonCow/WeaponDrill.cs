using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    public class WeaponDrill:Weapon
    {
        float softCooldown;
        float softCoolmax;
        public CircleCollider col;
        public bool active;
        public DrillDome dome;

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
            active = false;

            dome = new DrillDome(game, this);
            game.modelManager.addEffect(dome);
        }

        public override void activate()
        {
            active = true;
        }

        public override void disable()
        {
            active = false;
        }

        public override void Update()
        {
            if (active)
            {
                if (ship.boosting)
                {
                    game.hud.hudWeapon.Wake();
                }

                col.Update(ship.pos + ship.direction);
                if (ship.moving && ship.colliding)
                    checkCollision();
                else
                {
                    if (dome.active)
                        dome.disable();
                }
            }
            else
            {
                if (dome.active)
                    dome.disable();
            }
            base.Update();
        }

        void checkCollision()
        {
            bool colliding = false;
            foreach (Asteroid a in game.asteroidManager.asteroids)
            {
                if (a.col.checkCircle(col))
                {
                    a.damage(1, ship.pos);
                    game.camera.setYShake(0.03f);
                    colliding = true;
                }
            }

            foreach(Sentry s in game.enemyManager.sentries)
            {
                if(s.col.checkCircle(col))
                {
                    s.drillDamage(Utilities.deltaTime * 31.25f, ship.direction, ship.boosting);
                    colliding = true;
                }
            }

            foreach (Enemy e in game.enemyManager.enemies)
            {
                foreach(CircleCollider c in e.cols)
                {
                    if(c.checkCircle(col))
                    {
                        e.drillDamage(Utilities.deltaTime * 31.25f, ship.direction, ship.boosting);
                        colliding = true;
                    }
                }
            }

            if (colliding)
            {
                game.modelManager.addEffect(new ImpactParticleModel(game, ship.pos + ship.direction*0.8f, 0.15f));
                game.hud.hudWeapon.Wake();
                if (!dome.active)
                    dome.activate();
            }
            else
            {
                if (dome.active)
                    dome.disable();
            }
        }

        public override void Fire()
        {
            if (ammo > 0)
            {
                if (cooldown == 0 || (projectiles.Count() == 0 && softCooldown == 0))
                {
                    cooldown = coolMax;
                    softCooldown = softCoolmax;
                    base.Fire();
                }
            }
        }
        
    }
}
