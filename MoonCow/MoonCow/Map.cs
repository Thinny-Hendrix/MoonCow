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

            linkNeighbors();
            game.hud.minimap.drawMap(map);

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

        void linkNeighbors()
        {
            // Connect each mapNode in searchNode array to it's neightbors
            for (int y = 0; y < mapSize.Y; y++)
            {
                for (int x = 0; x < mapSize.X; x++)
                {
                    MapNode node = map[x, y];
                    if (node == null)  //Only do stuff to nodes that exist - redundant as grid should be full, but good to check
                    {
                        continue;
                    }

                    // An array of all of the possible neighbors
                    Vector2[] neighbors = new Vector2[]
                    {
                        new Vector2 (x, y - 1), // The node above the current node
                        new Vector2 (x, y + 1), // The node below the current node.
                        new Vector2 (x - 1, y), // The node left of the current node.
                        new Vector2 (x + 1, y), // The node right of the current node
                    };

                    // We loop through each of the possible neighbors
                    for (int i = 0; i < neighbors.Length; i++)
                    {
                        Vector2 position = neighbors[i];
                        // Ensure this neighbour is part of the level.
                        if (position.X < 0 || position.X > mapSize.X - 1 || position.Y < 0 || position.Y > mapSize.Y - 1)
                        {
                            continue;
                        }

                        MapNode neighbor = map[(int)position.X, (int)position.Y];
                        if (neighbor == null)   // again redundant, due to spatial partitioning we want even non traversable AI nodes to keep track of neighbors
                        {
                            continue;
                        }

                        // Store a reference to the neighbor.
                        node.neighbors[i] = neighbor;
                    }
                }
            }
        }
    }
}
