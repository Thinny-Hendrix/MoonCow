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
        public bool changing;
        public bool display;

        public List<MoneyGib> moneyGibs = new List<MoneyGib>();
        public List<MoneyGib> toDelete = new List<MoneyGib>();
        Game game;
        
        Model moneyGib1;

        public MoneyManager(Game game):base(game)
        {
            balance = 0;
            changing = false;
            display = true;
            displayTime = 0;
            this.game = game;

            moneyGib1 = game.Content.Load<Model>(@"Models/MoneyGibs/gib1");



        }

        public override void Update(GameTime gameTime)
        {
            cleanGibs();


            if (changing)
            {
                displayNo = MathHelper.Lerp(displayNo, balance, Utilities.deltaTime*3);

                if (difference >= 0)
                {
                    if (displayNo >= balance - 0.5f)
                    {
                        displayNo = balance;
                        changing = false;
                        difference = 0;
                    }
                }
                else
                {
                    if (displayNo <= balance + 0.5f)
                    {
                        displayNo = balance;
                        changing = false;
                        difference = 0;
                    }
                }

            }
            if(display && !changing)
            {
                displayTime += Utilities.deltaTime;
            }
            if (displayTime > 3)
                display = false;
        }
        public void addMoney(float amount)
        {
            difference += amount;
            balance += amount;
            changing = true;
            display = true;
            displayTime = 0;

        }

        public bool canPurchase(float amount)
        {
            if(balance >= amount)
            {
                addMoney(-amount);
                return true;
            }
            return false;
        }

        public void addGib(float amount, Vector3 position)
        {
            MoneyGib g = new MoneyGib(amount, moneyGib1, this, ((Game1)Game).ship, position, (Game1)game);
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

