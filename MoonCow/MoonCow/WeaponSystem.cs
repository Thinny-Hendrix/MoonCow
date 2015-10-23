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
        public bool hasDrill;
        Game1 game;

        Texture2D pew1;
        Texture2D pew2;
        Texture2D pew3;

        public List<Weapon> weapons = new List<Weapon>();
        public Weapon activeWeapon;
        public Weapon prevWeapon;
        public WeaponDrill drill;


        public WeaponSystem(Ship ship, Game game):base(game)
        {
            this.ship = ship;
            this.game = (Game1)game;
            currentWeapon = 1;

            pew1 = Game.Content.Load<Texture2D>(@"Models/Effects/tex1");
            pew2 = Game.Content.Load<Texture2D>(@"Models/Effects/tex2");
            pew3 = Game.Content.Load<Texture2D>(@"Models/Effects/tex3");

            drill = new WeaponDrill(this, ship, this.game);

            weapons.Add(new WeaponMissiles(this, ship, this.game));
            weapons.Add(new WeaponWave(this, ship, this.game));
            weapons.Add(new WeaponLaser(this, ship, this.game));
            weapons.Add(new WeaponBomb(this, ship, this.game));
            weapons.Add(drill);

            activeWeapon = (Weapon)weapons.ElementAt(2);

            hasDrill = true;


        }

        public override void Update(GameTime gameTime)
        {
            if (!Utilities.paused && !Utilities.softPaused)
            {
                if(!game.minigame.active)
                    if ((Keyboard.GetState().IsKeyDown(Keys.Space) || GamePad.GetState(PlayerIndex.One).Buttons.RightShoulder == ButtonState.Pressed))
                      fire();

                /*if (GamePad.GetState(PlayerIndex.One).Buttons.Y == ButtonState.Pressed)
                {
                    if (currentWeapon != 5)
                        currentWeapon++;
                    else
                        currentWeapon = 1;
                }*/

                //bool wake = false;

                if(!game.minigame.active)
                {
                    checkKeyboard();
                }


                foreach (Weapon w in weapons)
                {
                    w.Update();
                }
            }
        }

        void checkKeyboard()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.D1))
            {
                activeWeapon = (Weapon)weapons.ElementAt(2);
                game.hud.hudWeapon.Wake();
                ship.shipModel.setShipModel(0);
                weapons.ElementAt(4).disable();
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D2))
            {
                activeWeapon = (Weapon)weapons.ElementAt(3);
                game.hud.hudWeapon.Wake();
                ship.shipModel.setShipModel(1);
                weapons.ElementAt(4).disable();
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D3))
            {
                activeWeapon = (Weapon)weapons.ElementAt(0);
                game.hud.hudWeapon.Wake();
                ship.shipModel.setShipModel(2);
                weapons.ElementAt(4).disable();
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D4))
            {
                activeWeapon = (Weapon)weapons.ElementAt(1);
                game.hud.hudWeapon.Wake();
                ship.shipModel.setShipModel(3);
                weapons.ElementAt(4).disable();
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D5) && hasDrill)
            {
                activeWeapon = (Weapon)weapons.ElementAt(4);
                activeWeapon.activate();
                game.hud.hudWeapon.Wake();
                ship.shipModel.setShipModel(4);
            }
        }

        public void gotDrill()
        {
            hasDrill = true;
            changeWeapons(4);
        }

        public void changeWeapons(int wep)
        {
            prevWeapon = activeWeapon;
            activeWeapon = (Weapon)weapons.ElementAt(wep);
            ship.shipModel.setShipModel(wep);

            if (wep == 4)
                activeWeapon.activate();
            else
                weapons.ElementAt(4).disable();
        }

        public void addExp(int i, float exp)
        {
            weapons.ElementAt(i).addExp(exp);
        }

        public void swapWeapons()
        {
            Weapon temp = activeWeapon;
            activeWeapon = prevWeapon;
            prevWeapon = temp;
        }

        public void fire()
        {
            if (!ship.boosting)
            {
                game.hud.hudWeapon.Wake();
                activeWeapon.Fire();
            }
        }

        public int getAmmoType()
        {
            int lowestWep = -1;
            float lowestAmount = 1;
            foreach (Weapon w in weapons)
            {
                if (w.ammo < w.ammoMax)
                {
                    float amount = w.ammo / w.ammoMax;
                    if(amount < lowestAmount)
                    {
                        lowestAmount = amount;
                        lowestWep = weapons.IndexOf(w);
                    }
                }
            }

            float activeAmount = activeWeapon.ammo / activeWeapon.ammoMax;
            if (activeAmount < 0.4f)
                lowestWep = weapons.IndexOf(activeWeapon);

            return lowestWep;
        }

        public void addAmmo(int i)
        {
            Weapon wep = weapons.ElementAt(i);
            float added = wep.addAmmo((float)Math.Floor(wep.ammoMax / 4));

            if(added != 0)
                game.hud.hudMessage.setAmmoMessage(wep, added);
        }
    
    }
}
