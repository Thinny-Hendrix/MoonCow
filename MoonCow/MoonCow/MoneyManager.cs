using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MoonCow
{
    public class MoneyManager
    {
        float balance;
        float displayTime;

        public float displayNo;
        public float difference;
        public bool changing;
        public bool display;

        public MoneyManager()
        {
            balance = 0;
            changing = false;
            display = true;
            displayTime = 0;
        }

        public void update()
        {
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



    }
}
