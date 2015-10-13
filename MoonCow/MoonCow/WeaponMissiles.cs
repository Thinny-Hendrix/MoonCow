using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MoonCow
{
    public class WeaponMissiles:Weapon
    {
        int laserPos;
        public WeaponMissiles(WeaponSystem wepSys, Ship ship, Game1 game):base(wepSys, ship, game)
        {
            icon = TextureManager.icoAst;
            name = "Homing Spears";

            damage = 0.6f;
            rateOfFire = 0.3f;
            range = 0.9f;

            coolMax = 15;
            laserPos = 0;
            EXPMAX = 250;
        }

        public override void Fire()
        {
            if(cooldown == 0)
            {
                if (level != 3)
                {
                    if (laserPos == 0)
                    {
                        addProjectile(ship.pos + new Vector3(Vector3.Cross(Vector3.Up, ship.direction).X * 0.25f, 0, Vector3.Cross(Vector3.Up, ship.direction).Z * 0.25f), ship.direction);
                        laserPos = 1;
                        game.audioManager.addSoundEffect(AudioLibrary.shipShootLaser, 0.1f);
                        //game.audioManager.shipShootLaser.Stop();
                        //game.audioManager.shipShootLaser.Play();
                    }
                    else
                    {
                        addProjectile(ship.pos + new Vector3(-Vector3.Cross(Vector3.Up, ship.direction).X * 0.25f, 0, -Vector3.Cross(Vector3.Up, ship.direction).Z * 0.25f), ship.direction);
                        laserPos = 0;
                        game.audioManager.addSoundEffect(AudioLibrary.shipShootLaser, 0.1f);

                        //game.audioManager.shipShootLaser2.Stop();
                        //game.audioManager.shipShootLaser2.Play();
                    }
                }
                else
                {
                    //only shoots one projectile if there's only one ammo left
                    if (ammo > 1)
                        addProjectile(ship.pos + new Vector3(Vector3.Cross(Vector3.Up, ship.direction).X * 0.25f, 0, Vector3.Cross(Vector3.Up, ship.direction).Z * 0.25f), ship.direction);

                    addProjectile(ship.pos + new Vector3(-Vector3.Cross(Vector3.Up, ship.direction).X * 0.25f, 0, -Vector3.Cross(Vector3.Up, ship.direction).Z * 0.25f), ship.direction);

                    game.audioManager.shipShootLaser.Stop();
                    game.audioManager.shipShootLaser.Play();
                }
                cooldown = coolMax;
                //base.Fire();
            }
        }

        void addProjectile(Vector3 pos, Vector3 dir)
        {
            switch (level)
            {
                default:
                    projectiles.Add(new AstGunProjectile(pos, dir, game, this, level));
                    break;
                case 2:
                    projectiles.Add(new AstGunProjectile(pos, dir, game, this, level));
                    break;
                case 3:
                    projectiles.Add(new AstGunProjectile(pos, dir, game, this, level));
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
                    //ammoMax = 400;
                    break;
                case 3:
                    coolMax = 10;
                    //ammoMax = 500;
                    break;
            }
            ammo = ammoMax;
        }
        
    }
}
