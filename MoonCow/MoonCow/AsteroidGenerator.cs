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
        float maxSent;
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

            for (int i = 0; i < 6; i++)//always 6 minigame ships
            {
                int targetNode = Utilities.random.Next(astNodes.Count());
                while(astNodes.ElementAt(targetNode).hasAstItem) //if selected node has something, roll again
                {
                    targetNode = Utilities.random.Next(astNodes.Count());
                }

                manager.addShip(astNodes.ElementAt(targetNode).pos);
                astNodes.ElementAt(targetNode).hasAstItem = true;
            }

            maxPoints = astNodes.Count();
            maxBig = (int)(maxPoints * 0.35f);
            maxMid = maxPoints * 0.4f;
            maxSmall = maxPoints * 0.2f;
            maxSent = (int)(maxPoints * 0.15f);

            for (int i = 0; i < maxBig; i++)
            {
                int targetNode = Utilities.random.Next(astNodes.Count());
                bool goodNode = false;
                while (!goodNode) //if selected node has something, roll again
                {
                    targetNode = Utilities.random.Next(astNodes.Count());

                    MapNode node = astNodes.ElementAt(targetNode);
                    if(!node.hasAstItem)//if it doesn't have anything yet
                    {
                        if (node.type > 38 && !(node.type >= 43 && node.type <= 46) && !(node.type >= 51 && node.type <= 58))//not a big corner node or entrance to ast field
                        {
                            goodNode = true;
                        }
                    }
                }

                manager.addBig(astNodes.ElementAt(targetNode).pos);
                astNodes.ElementAt(targetNode).hasAstItem = true;
            }

            for (int i = 0; i < maxSent; i++)
            {
                int targetNode = Utilities.random.Next(astNodes.Count());
                bool goodNode = false;
                int tryCount = 0;
                while (!goodNode) //if selected node has something, roll again
                {
                    targetNode = Utilities.random.Next(astNodes.Count());

                    MapNode node = astNodes.ElementAt(targetNode);
                    if (!node.hasAstItem)//if it doesn't have anything yet
                    {
                        if (!(node.type >= 43 && node.type <= 46) && !(node.type >= 51 && node.type <= 58))//not entrance to ast field
                        {
                            goodNode = true;
                        }
                    }

                    tryCount++;
                    if (tryCount > 10)//if it fails to find a good node 10 times, give up
                        break;
                }
                if (goodNode)
                {
                    game.enemyManager.addSentry(astNodes.ElementAt(targetNode).pos);
                    astNodes.ElementAt(targetNode).hasAstItem = true;
                }
            }

            foreach(MapNode node in astNodes)
            {
                if(!node.hasAstItem)
                {
                    if (Utilities.random.Next(10) > 1)
                    {
                        manager.addMid(node.pos, true);
                    }
                    else
                    {
                        manager.addSmall(node.pos, true);
                    }
                }
            }

            /*
                //for each node - 
                foreach (MapNode node in astNodes)
                {
                    int points = 0;

                    //System.Diagnostics.Debug.WriteLine("ASteroid node at " + node.pos);

                    if (Utilities.random.Next(4) == 0)
                    {
                        if (Utilities.random.Next(2) == 0)
                            game.enemyManager.addSentry(node.pos);
                        //else
                            //manager.addShip(node.pos);
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
                }*/

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
