using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    public class MoneyManager:Microsoft.Xna.Framework.GameComponent
    {
        float balance;
        float displayTime;

        public float displayNo;
        public float difference;
        public float collected;
        public bool changing;
        public bool display;

        public List<MoneyGib> moneyGibs = new List<MoneyGib>();
        public List<MoneyGib> toDelete = new List<MoneyGib>();
        Game1 game;
        
        Model moneyGib1;

        float moneyTransTime;
        float prevMoney;

        public MoneyManager(Game game):base(game)
        {
            balance = 0;
            changing = false;
            display = true;
            displayTime = 0;
            collected = 0;
            this.game = (Game1)game;

            moneyGib1 = game.Content.Load<Model>(@"Models/MoneyGibs/gib1");

            moneyTransTime = 0;


        }

        public override void Update(GameTime gameTime)
        {
            cleanGibs();


            if (changing)
            {
                displayNo = MathHelper.Lerp(displayNo, balance, (float)Math.Sin(moneyTransTime));
                moneyTransTime += Utilities.deltaTime*MathHelper.PiOver4;
                game.hud.hudMoney.Wake();

                if (moneyTransTime >= MathHelper.PiOver2)
                {
                    {
                        displayNo = balance;
                        changing = false;
                        difference = 0;
                        collected = 0;
                    }
                }
                    /*
                else
                {
                    if (displayNo <= balance + 0.5f)
                    {
                        displayNo = balance;
                        changing = false;
                        difference = 0;
                    }
                }*/

            }
            if(display && !changing)
            {
                displayTime += Utilities.deltaTime;
            }
            if (displayTime > 3)
                display = false;
        }

        public void refund(float amount)
        {
            difference += amount;
            balance += amount;
            changing = true;
            game.hud.hudMoney.Wake();
            moneyTransTime = 0;
            displayTime = 0;
            prevMoney = displayNo;

            game.levelStats.moneySpent -= amount;
        }

        public void addMoney(float amount)
        {
            difference += amount;
            balance += amount;
            changing = true;
            game.hud.hudMoney.Wake();
            moneyTransTime = 0;
            displayTime = 0;
            prevMoney = displayNo;

            if(amount > 0)
            {
                game.levelStats.moneyEarnt += amount;
            }
            else
            {
                game.levelStats.moneySpent += (amount * -1);
            }

        }

        /// <summary>
        /// this will automatically subtract the money if the check runs true
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public bool canPurchase(float amount)
        {
            if(balance >= amount)
            {
                addMoney(-amount);
                return true;
            }
            return false;
        }

        public bool checkPurchase(float amount)
        {
            if (balance >= amount)
            {
                return true;
            }
            return false;
        }

        public void makeMoney(float amount, int type, Vector3 pos)
        {
            int remaining = (int)amount;

            int temp = remaining / 1000;
            if(temp > 0)
            {
                remaining %= 1000;
                for (int i = 0; i < temp; i++)
                {
                    addGib(1000, pos, type);
                    //remaining-= 100;
                }
            }

            //check for 100s
            temp = remaining / 100;
            if (temp > 0)
            {
                remaining %= 100;

                for (int i = 0; i < temp; i++)
                {
                    addGib(100, pos, type);
                    //remaining -= 100;
                }
            }

            temp = remaining / 50;
            if (temp > 0)
            {
                remaining %= 50;

                for (int i = 0; i < temp; i++)
                {
                    addGib(50, pos, type);
                    //remaining -= 50;
                }
            }

            temp = remaining / 20;
            if (temp > 0)
            {
                remaining %= 20;

                for (int i = 0; i < temp; i++)
                {
                    addGib(20, pos, type);
                    //remaining -= 20;
                }
            }

            temp = remaining / 10;
            if (temp > 0)
            {
                remaining %= 10;

                for (int i = 0; i < temp; i++)
                {
                    addGib(10, pos, type);
                    //remaining -= 10;
                }
            }

            temp = remaining / 5;
            if (temp > 0)
            {
                remaining %= 5;

                for (int i = 0; i < temp; i++)
                {
                    addGib(5, pos, type);
                    //remaining -= 5;
                }
            }

            temp = remaining / 1;
            if (temp > 0)
            {

                for (int i = 0; i < temp; i++)
                {
                    addGib(1, pos, type);
                    remaining -= 1;
                }
            }
        }

        public void addAmmoGib(Vector3 pos)
        {
            int type = game.ship.weapons.getAmmoType();
            if (type >= 0)
                game.modelManager.addObject(new AmmoGib(this, game.ship, pos, game, type));
        }

        public void addOreGib(float amount, Vector3 pos, int type)
        {
            MoneyGib g = new OreGib(amount, this, game.ship, pos, game, type);

            game.modelManager.addObject(g);
        }

        public void addGib(float amount, Vector3 position, int type)
        {
            MoneyGib g = new MoneyGib(amount, moneyGib1, this, ((Game1)Game).ship, position, (Game1)game, type);
            //moneyGibs.Add(g);
            game.modelManager.addObject(g);
        }

        public void addGib(float amount, Vector3 position)
        {
            MoneyGib g = new MoneyGib(amount, moneyGib1, this, ((Game1)Game).ship, position, (Game1)game, 0);
            //moneyGibs.Add(g);
            ((Game1)Game).modelManager.addObject(g);
        }

        void cleanGibs()
        {
            foreach (MoneyGib g in toDelete)
            {
                ((Game1)Game).modelManager.removeObject(g);
                //moneyGibs.Remove(g);
            }
            toDelete.Clear();
        }
    }
}

