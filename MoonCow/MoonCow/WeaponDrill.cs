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
        public bool drillColliding;
        public bool active;
        public DrillDome dome;
        public OOBB finishingRange;
        DrillSpinControl spins;
        Enemy target;

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
            col = new CircleCollider(ship.pos+ship.direction, 0.5f);
            active = false;

            dome = new DrillDome(game, this);
            game.modelManager.addEffect(dome);
            finishingRange = new OOBB(ship.pos + ship.direction * 15, ship.direction, 2, 35);
            spins = new DrillSpinControl(game, this);
        }

        void drillSpawnEffect()
        {
            game.modelManager.addEffect(new SpinParticle(game, 1));
            for (int i = 0; i < 10; i++)
                game.modelManager.addEffect(new DirLineParticle(ship.pos, game));
        }

        public override void activate()
        {
            if (!active)
                drillSpawnEffect();
            active = true;

        }

        public override void disable()
        {
            if (active)
                drillSpawnEffect();
            active = false;
            drillColliding = false;
        }

        public override void Update()
        {
            if (active)
            {
                if (ship.boosting)
                {
                    game.hud.hudWeapon.Wake();
                    finishingRange.Update(ship.pos + ship.direction * 15, ship.direction);
                    checkFinishing();
                }
                else
                {
                    if (ship.finishingMove)
                        game.hud.flashTime = 0;
                        ship.finishingMove = false;
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

        void checkFinishing()
        {
            bool collided = false;
            List<Enemy> targets = new List<Enemy>();

            foreach(Enemy e in game.enemyManager.enemies)
            {
                if (e.enemyType > 0)//not a swarmer
                {
                    foreach (CircleCollider c in e.cols)
                    {
                        if (c.checkOOBB(finishingRange))
                        {
                            collided = true;
                            if (!targets.Contains(e))
                                targets.Add(e);
                        }
                    }
                }
            }
            if(collided)
            {
                if (!game.ship.finishingMove)
                {
                    game.hud.flashTime = 0;
                    ship.finishingMove = true;
                    setTarget(targets);
                }
            }
            else
            {
                if (game.ship.finishingMove)
                {
                    game.hud.flashTime = 0;
                    ship.finishingMove = false;
                }
            }
        }

        void setTarget(List<Enemy> targets)
        {
            int closestIndex = 0;
            float shortestDist = 1000;

            foreach(Enemy e in targets)
            {
                float dist = col.distFrom(e.pos);
                if(dist < shortestDist)
                {
                    shortestDist = dist;
                    closestIndex = targets.IndexOf(e);
                }
            }

            target = targets.ElementAt(closestIndex);
            target.frozen = true;

            ship.direction = -1*ship.circleCol.directionFrom(target.pos);
            ship.rot.Y = (float)Math.Atan2(ship.direction.X, ship.direction.Z)+MathHelper.Pi;
        }

        void checkCollision()
        {
            bool colliding = false;
            foreach (Asteroid a in game.asteroidManager.asteroids)
            {
                if (a.col.checkCircle(col))
                {
                    a.drillDamage(60*Utilities.deltaTime, ship.pos, ship.boosting);
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
                drillColliding = true;
                game.modelManager.addEffect(new ImpactParticleModel(game, ship.pos + ship.direction*0.8f, 0.15f));
                game.hud.hudWeapon.Wake();
                if (!dome.active)
                    dome.activate();
            }
            else
            {
                drillColliding = false;
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
