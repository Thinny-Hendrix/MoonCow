using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    public class WeaponSystem:GameComponent
    {
        Ship ship;
        int currentWeapon; //1=lasers, 2=missiles, 3=bomb, 4=shockwave, 5=drill 
        int laserPos; //0 = left, 1 = right
        float cooldown;
        public bool hasDrill;
        Game1 game;

        Texture2D pew1;
        Texture2D pew2;
        Texture2D pew3;

        public List<Weapon> weapons = new List<Weapon>();
        public Weapon activeWeapon;
        public Weapon prevWeapon;


        public WeaponSystem(Ship ship, Game game):base(game)
        {
            this.ship = ship;
            this.game = (Game1)game;
            currentWeapon = 1;
            laserPos = 0;

            pew1 = Game.Content.Load<Texture2D>(@"Models/Effects/tex1");
            pew2 = Game.Content.Load<Texture2D>(@"Models/Effects/tex2");
            pew3 = Game.Content.Load<Texture2D>(@"Models/Effects/tex3");

            weapons.Add(new WeaponMissiles(this, ship, this.game));
            weapons.Add(new WeaponWave(this, ship, this.game));
            weapons.Add(new WeaponLaser(this, ship, this.game));
            weapons.Add(new WeaponBomb(this, ship, this.game));
            weapons.Add(new WeaponDrill(this, ship, this.game));

            activeWeapon = (Weapon)weapons.ElementAt(2);

            hasDrill = false;


        }

        public override void Update(GameTime gameTime)
        {
            if (!Utilities.paused && !Utilities.softPaused)
            {
                if ((Keyboard.GetState().IsKeyDown(Keys.Space) || GamePad.GetState(PlayerIndex.One).Buttons.RightShoulder == ButtonState.Pressed) && cooldown == 0)
                    fire();

                if (GamePad.GetState(PlayerIndex.One).Buttons.Y == ButtonState.Pressed)
                {
                    if (currentWeapon != 5)
                        currentWeapon++;
                    else
                        currentWeapon = 1;
                }

                if (Keyboard.GetState().IsKeyDown(Keys.D1))
                    activeWeapon = (Weapon)weapons.ElementAt(2);
                if (Keyboard.GetState().IsKeyDown(Keys.D2))
                    activeWeapon = (Weapon)weapons.ElementAt(3);
                if (Keyboard.GetState().IsKeyDown(Keys.D3))
                    activeWeapon = (Weapon)weapons.ElementAt(0);
                if (Keyboard.GetState().IsKeyDown(Keys.D4))
                    activeWeapon = (Weapon)weapons.ElementAt(1);
                if (Keyboard.GetState().IsKeyDown(Keys.D5) && hasDrill)
                    activeWeapon = (Weapon)weapons.ElementAt(4);

                if (cooldown > 0)
                {
                    cooldown -= Utilities.deltaTime * 60;
                    if (cooldown < 0)
                        cooldown = 0;
                }

                foreach (Weapon w in weapons)
                {
                    w.Update();
                }
            }
        }

        public void changeWeapons(int wep)
        {
            prevWeapon = activeWeapon;
            activeWeapon = (Weapon)weapons.ElementAt(wep);
        }

        public void swapWeapons()
        {
            Weapon temp = activeWeapon;
            activeWeapon = prevWeapon;
            prevWeapon = temp;
        }

        public void fire()
        {
            if (!ship.boosting && cooldown <= 0)
                activeWeapon.Fire();
        }
    
    }
}
