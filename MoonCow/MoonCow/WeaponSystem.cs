using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MoonCow
{
    public class WeaponSystem
    {
        Ship ship;
        int currentWeapon; //1=lasers, 2=missiles, 3=bomb, 4=shockwave, 5=drill 
        int laserPos; //0 = left, 1 = right
        float cooldown;
        bool hasDrill;
        List<Projectile> projectiles = new List<Projectile>();


        public WeaponSystem(Ship ship)
        {
            this.ship = ship;
            currentWeapon = 1;
            laserPos = 0;
        }

        public void update()
        {
            if(Keyboard.GetState().IsKeyDown(Keys.Space) && cooldown == 0)
                fire();

            if (Keyboard.GetState().IsKeyDown(Keys.D1))
                currentWeapon = 1;
            if (Keyboard.GetState().IsKeyDown(Keys.D2))
                currentWeapon = 2;
            if (Keyboard.GetState().IsKeyDown(Keys.D3))
                currentWeapon = 3;
            if (Keyboard.GetState().IsKeyDown(Keys.D4))
                currentWeapon = 4;
            if (Keyboard.GetState().IsKeyDown(Keys.D5))
                currentWeapon = 5;

            foreach (Projectile p in projectiles)
                p.update();
        }


        public void fire()
        {

        }
    
    }
}
