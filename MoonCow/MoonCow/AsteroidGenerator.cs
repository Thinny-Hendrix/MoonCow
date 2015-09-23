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
        Game1 game;
        int maxPoints;
        float maxBig;
        float maxMid;
        float maxSmall;
        bool bigLimit = false;
        bool midLimit = false;
        float bigCount = 0;
        float midCount = 0;

        public AsteroidGenerator(AsteroidManager manager, Game1 game)
        {
            this.manager = manager;
            this.game = game;
        }
        public void run(List<MapNode> astNodes)
        {
            //20% big, 40% mid and 40% small
            //small = 1, mid = 2, big = 4
            //6 'points' per node
            maxPoints = astNodes.Count();
            maxBig = maxPoints * 0.4f;
            maxMid = maxPoints * 0.4f;
            maxSmall = maxPoints * 0.2f;
            //for each node - 
            foreach (MapNode node in astNodes)
            {
                int points = 0;

                if (Utilities.random.Next(4) == 0)
                {
                    if (Utilities.random.Next(2) == 0)
                        game.enemyManager.addSentry(node.pos);
                    else
                        manager.addShip(node.pos);
                }
                else
                {
                    if (!bigLimit && !midLimit)
                    {
                        float rando = Utilities.random.Next(3);
                        if (rando == 0)
                        {
                            manager.addBig(node.pos);
                            bigCount++;
                            points += 4;
                        }
                        else if (rando == 1)
                        {
                            manager.addMid(node.pos);
                            midCount++;
                            points += 2;
                        }
                        else
                        {
                            manager.addSmall(node.pos);
                            points += 1;
                        }

                        checkLimits();
                    }
                    else if (bigLimit ^ midLimit)
                    {
                        float rando = Utilities.random.Next(2);
                        if (rando == 0)
                        {
                            manager.addMid(node.pos);
                            midCount++;
                            points += 2;
                        }
                        else
                        {
                            manager.addSmall(node.pos);
                            points += 1;
                        }

                        checkLimits();
                    }
                    else
                    {
                        manager.addSmall(node.pos);
                        points += 1;
                    }
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
