using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MoonCow
{
    public class Map
    {
        public MapNode[,] map;
        private Vector2 mapSize;
        private List<Vector2> enemySpawn = new List<Vector2>();
        private Vector2 coreLocation;

        public Map(Game1 game, int[,] newMap)
        {
            mapSize = new Vector2(newMap.GetLength(0), newMap.GetLength(1));
            map = new MapNode[(int)mapSize.X, (int)mapSize.Y];

            //Generate array of mapNodes from int array
            //As each node is created, it's position and traversable values must be set
            //The model data will also need to bet set for each node here

            for(int y = 0; y < mapSize.Y; y++)
            {
                for (int x = 0; x < mapSize.X; x++)
                {
                    map[x, y] = new MapNode(game, newMap[x, y], new Vector2(x,y));
                    
                    //add enemy spawn points to a list for easy access
                    if(newMap[x,y] == 7 || newMap[x,y] == 8 ||
                        newMap[x, y] == 9 || newMap[x, y] == 10)
                    {
                        enemySpawn.Add(new Vector2(x, y));
                    }

                    if (newMap[x, y] == 24)
                    {
                        coreLocation = new Vector2(x, y);
                    }
                }
            }

        }

        public int getWidth()
        {
            return (int)mapSize.X;
        }

        public int getHeight()
        {
            return (int)mapSize.Y;
        }

        public List<Vector2> getEnemySpawn()
        {
            return enemySpawn;
        }

        public Vector2 getCoreLocation()
        {
            return coreLocation;
        }

    }
}
