using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MoonCow
{
    public class AsteroidGenerator
    {
        AsteroidManager manager;
        int maxPoints;
        float maxBig;
        float maxMid;
        float maxSmall;
        bool bigLimit = false;
        bool midLimit = false;
        float bigCount = 0;
        float midCount = 0;

        public AsteroidGenerator(AsteroidManager manager)
        {
            this.manager = manager;
        }
        public void run(List<Vector3> astNodes)
        {
            //20% big, 40% mid and 40% small
            //small = 1, mid = 2, big = 4
            //6 'points' per node
            maxPoints = astNodes.Count() * 6;
            maxBig = maxPoints * 0.2f;
            maxMid = maxPoints * 0.4f;
            maxSmall = maxPoints * 0.4f;
            //for each node - 
            foreach (Vector3 pos in astNodes)
            {
                int points = 0;

                if (!bigLimit && !midLimit)
                {
                    float rando = Utilities.random.Next(3);
                    if(rando == 0)
                    {
                        manager.addBig(pos);
                        bigCount++;
                        points += 4;
                    }
                    else if(rando == 1)
                    {
                        manager.addMid(pos);
                        midCount++;
                        points += 2;
                    }
                    else
                    {
                        manager.addSmall(pos);
                        points += 1;
                    }

                    checkLimits();
                }
                else if(bigLimit ^ midLimit)
                {
                    float rando = Utilities.random.Next(2);
                    if (rando == 0)
                    {
                        manager.addMid(pos);
                        midCount++;
                        points += 2;
                    }
                    else
                    {
                        manager.addSmall(pos);
                        points += 1;
                    }

                    checkLimits();
                }
                else
                {
                    manager.addSmall(pos);
                    points += 1;
                }
            }

        }

        void checkLimits()
        {
            if (midCount >= maxMid)
                midLimit = true;
            if (bigCount >= maxBig)
                bigLimit = true;
        }
    }
}
