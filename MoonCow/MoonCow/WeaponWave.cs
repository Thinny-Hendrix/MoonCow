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

            coolMax = 90;
            ammoMax = 24;
            ammo = ammoMax;

            EXPMAX = 250;
        }

        public override void Fire()
        {
            if (ammo > 0)
            {
                if (cooldown == 0)
                {
                    if (level == 3)
                    {
                        game.hud.hudZoom.activate(level);
                        for (int i = 0; i < 20; i++)
                        {
                            game.modelManager.addEffect(new ElectroDir(ship.pos, Color.White, Color.Red, game, ship.direction));
                        }
                    }
                    else if (level == 2)
                    {
                        for (int i = 0; i < 10; i++)
                        {
                            game.modelManager.addEffect(new ElectroDir(ship.pos, Color.White, Color.SeaGreen, game, ship.direction));
                        }
                    }

                    

                    projectiles.Add(new WaveProjectile(ship.pos + ship.direction * 0.1f, ship.direction, game, this, level));
                    game.levelStats.wavesFired++;
                    game.audioManager.addSoundEffect(AudioLibrary.shipShootLaser, 0.1f);

                    cooldown = coolMax;
                    base.Fire();
                }
            }
        }

        public override void levelUp()
        {
            switch (level)
            {
                default: //level 2
                    coolMax = 75;
                    EXPMAX = 500;
                    ammoMax = 28;
                    break;
                case 3:
                    coolMax = 60;
                    ammoMax = 32;
                    break;
            }
            ammo = ammoMax;

            game.hud.hudMessage.setLevelUpMessage(this);
        }        
    }
}
