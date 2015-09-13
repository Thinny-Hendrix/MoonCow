using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    public class Weapon
    {
        //general info used for hud
        public string name { get; protected set; }
        public Texture2D icon { get; protected set; }
        //these three are values out of 20, displayed on quick select as a bar
        public float rateOfFire { get; protected set; }
        public float range { get; protected set; }
        public float damage { get; protected set; }


        //internal stuff
        public float exp { get; protected set; }
        public float EXPMAX { get; protected set; }
        public float coolMax { get; protected set; }
        public int level { get; protected set; }
        public float cooldown { get; protected set; }
        public float ammo { get; protected set; }
        public float ammoMax { get; protected set; }
        public List<Projectile> projectiles { get; protected set; }
        public List<Projectile> toDelete { get; protected set; }
        public WeaponSystem wepSys { get; protected set; }
        public Ship ship { get; protected set; }
        public Game1 game { get; protected set; }

        public Weapon(WeaponSystem wepSys, Ship ship, Game1 game)
        {
            this.wepSys = wepSys;
            this.ship = ship;
            this.game = game;
            cooldown = 0;
            level = 1;
            projectiles = new List<Projectile>();
            toDelete = new List<Projectile>();
        }

        public virtual void Update()
        {
            if(cooldown != 0)
            {
                cooldown -= Utilities.deltaTime * 60;
                if (cooldown < 0)
                    cooldown = 0;
            }
            foreach (Projectile p in projectiles)
            {
                p.update();
            }

            foreach (Projectile p in toDelete)
            {
                projectiles.Remove(p);
            }
            toDelete.Clear();
        }

        public virtual void Fire()
        {
            ammo--;
        }

        public virtual void addAmmo(int amount)
        {
            ammo += amount;
            if (ammo > ammoMax)
                ammo = ammoMax;
        }

        public virtual void addExp(float amount)
        {
            if(level < 3)
            {
                exp += amount;
                if (exp >= EXPMAX)
                {
                    levelUp();
                    exp -= EXPMAX;
                }
            }
        }

        public virtual void levelUp()
        {

        }



    }
}
