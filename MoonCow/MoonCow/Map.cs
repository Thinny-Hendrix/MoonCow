using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MoonCow
{
    class Map
    {
        public MapNode[,] map;
        private Vector2 mapSize;

        public Map(int[,] newMap)
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
                    map[x, y] = new MapNode(newMap[x, y]);
                    map[x, y].position = new Vector2(x, y);
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

    }
}
