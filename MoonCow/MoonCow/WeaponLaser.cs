using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MoonCow
{
    public class WeaponLaser:Weapon
    {
        int laserPos;
        public WeaponLaser(WeaponSystem wepSys, Ship ship, Game1 game):base(wepSys, ship, game)
        {
            icon = TextureManager.icoPew;
            name = "Laser Cutters";
            ammoName = "Charge";

            damage = 0.3f;
            rateOfFire = 0.9f;
            range = 0.9f;

            coolMax = 15;
            laserPos = 0;

            ammoMax = 200;
            ammo = ammoMax;

            EXPMAX = 250;
        }

        public override void Fire()
        {
            if (cooldown == 0 && ammo > 0)
            {
                if (level != 3)
                {
                    if (laserPos == 0)
                    {
                        addProjectile(ship.pos + new Vector3(Vector3.Cross(Vector3.Up, ship.direction).X * 0.25f, 0, Vector3.Cross(Vector3.Up, ship.direction).Z * 0.25f), ship.direction);
                        laserPos = 1;
                        game.audioManager.shootLaser();
                    }
                    else
                    {
                        addProjectile(ship.pos + new Vector3(-Vector3.Cross(Vector3.Up, ship.direction).X * 0.25f, 0, -Vector3.Cross(Vector3.Up, ship.direction).Z * 0.25f), ship.direction);
                        laserPos = 0;
                        game.audioManager.shootLaser2();
                    }
                }
                else
                {
                    //only shoots one projectile if there's only one ammo left
                    if(ammo > 1)
                        addProjectile(ship.pos + new Vector3(Vector3.Cross(Vector3.Up, ship.direction).X * 0.25f, 0, Vector3.Cross(Vector3.Up, ship.direction).Z * 0.25f), ship.direction);

                    addProjectile(ship.pos + new Vector3(-Vector3.Cross(Vector3.Up, ship.direction).X * 0.25f, 0, -Vector3.Cross(Vector3.Up, ship.direction).Z * 0.25f), ship.direction);

                    game.audioManager.shootLaser();
                }
                cooldown = coolMax;
                base.Fire();
            }
            
        }

        void addProjectile(Vector3 pos, Vector3 dir)
        {
            switch(level)
            {
                default:
                    projectiles.Add(new LaserProjectile(pos, dir, game, this, level));
                    break;
                case 2:
                    projectiles.Add(new LaserProjectile(pos, dir, game, this, level));
                    break;
                case 3:
                    projectiles.Add(new LaserProjectile(pos, dir, game, this, level));
                    break;
            }
        }

        public override void levelUp()
        {
            switch (level)
            {
                default: //level 2
                    coolMax = 10;
                    EXPMAX = 500;
                    ammoMax = 400;
                    break;
                case 3:
                    coolMax = 8;
                    ammoMax = 500;
                    break;
            }
            ammo = ammoMax;
        }
        
    }
}
