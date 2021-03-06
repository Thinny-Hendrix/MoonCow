﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MoonCow
{
    public class WeaponBomb:Weapon
    {
        float softCooldown;
        float softCoolmax;
        public List<BombExplosion> splos;
        public List<BombExplosion> sploToDelete;
        public WeaponBomb(WeaponSystem wepSys, Ship ship, Game1 game):base(wepSys, ship, game)
        {
            icon = TextureManager.icoBomb;
            name = "Exobomb";

            rateOfFire = 0.2f;
            damage = 1;
            range = 0.8f;
            coolMax = 90;
            softCoolmax = 60;
            ammoMax = 8;
            ammo = ammoMax;
            EXPMAX = 250;

            splos = new List<BombExplosion>();
            sploToDelete = new List<BombExplosion>();
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

            foreach (BombExplosion b in splos)
                b.Update();
            foreach (BombExplosion b in sploToDelete)
                splos.Remove(b);
            sploToDelete.Clear();
        }

        public override void levelUp()
        {
            switch (level)
            {
                default: //level 2
                    EXPMAX = 500;
                    ammoMax = 12;
                    break;
                case 3:
                    ammoMax = 16;
                    break;
            }
            ammo = ammoMax;
            game.hud.hudMessage.setLevelUpMessage(this);
        }

        public override void Fire()
        {
            if (ammo > 0)
            {
                if (cooldown == 0 || (projectiles.Count() == 0 && softCooldown == 0))
                {
                    projectiles.Add(new BombProjectile(ship.pos + new Vector3(0, 0, ship.direction.Z * 0.25f), ship.direction, game, this));
                    game.levelStats.bombsFired++;
                    game.audioManager.shipShootBomb.Stop();
                    game.audioManager.shipShootBomb.Play();
                    cooldown = coolMax;
                    softCooldown = softCoolmax;
                    base.Fire();
                }
            }
        }
        
    }
}
