﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MoonCow
{
    class Pathfinder
    {
        private Map map;
        private MapNode[,] searchNodes;     //May replace with pointer to map, need to see how it works more closely
        private int levelWidth;             //The width of the level
        private int levelHeight;            //The height of the level

        private List<MapNode> openList = new List<MapNode>();       //The Open List for pathfinding
        private List<MapNode> closedList = new List<MapNode>();     //The Closed List for pathfinding

        public Pathfinder(Map level)
        {
            levelWidth = level.getWidth();
            levelHeight = level.getHeight();
            map = level;
            initializeSearchNodes();
        }

        /// <summary>
        /// Initializes the searchNode array to contain only mapNodes that can be walked upon, then links the neighbors
        /// As the traversable state of the nodes is not changed by our game during play, this only needs to be called once per enemy, per level
        /// If we add the function to be able to block off paths or otherwise change the traversable bool of a node, then this will need to be called again for each enemy
        /// </summary>
        private void initializeSearchNodes()
        {
            // Fill the searchNode array with only the traversable mapNodes in the map
            levelWidth = map.getWidth();
            levelHeight = map.getHeight();
            searchNodes = new MapNode[levelWidth, levelHeight];
            for (int y = 0; y < levelWidth; y++)
            {
                for (int x = 0; x < levelHeight; x++)
                {
                    if (map.map[x,y].traversable == true)
                    {
                        searchNodes[x, y] = map.map[x, y];
                        searchNodes[x, y].neighbors = new MapNode[4];
                    }
                }
            }

            // Connect each mapNode in searchNode array to it's neightbors
            for (int y = 0; y < levelWidth; y++)
            {
                for (int x = 0; x < levelHeight; x++)
                {
                    MapNode node = searchNodes[x, y];
                    if (node == null || node.traversable == false)  //Only do stuff to nodes that exist
                    {
                        continue;
                    }

                    // An array of all of the possible neighbors
                    Point[] neighbors = new Point[]
                    {
                        new Point (x, y - 1), // The node above the current node
                        new Point (x, y + 1), // The node below the current node.
                        new Point (x - 1, y), // The node left of the current node.
                        new Point (x + 1, y), // The node right of the current node
                    };

                    // We loop through each of the possible neighbors
                    for (int i = 0; i < neighbors.Length; i++)
                    {
                        Point position = neighbors[i];
                        // Ensure this neighbour is part of the level.
                        if (position.X < 0 || position.X > levelWidth - 1 || position.Y < 0 || position.Y > levelHeight - 1)
                        {
                            continue;
                        }

                        MapNode neighbor = searchNodes[position.X, position.Y];
                        if (neighbor == null || neighbor.traversable == false)
                        {
                            continue;
                        }

                        // Store a reference to the neighbor.
                        node.neighbors[i] = neighbor;
                    }
                }
            }
        }

        /// <summary>
        /// Returns the theorectical distance between two points
        /// </summary>
        private float heuristic(Point point1, Point point2)
        {
            return Math.Abs(point1.X - point2.X) + Math.Abs(point1.Y - point2.Y);
        }

        /// <summary>
        /// Reset the state of the searchNodes so that a new path may be found if required
        /// </summary>
        private void resetSearchNodes()
        {
            openList.Clear();
            closedList.Clear();

            for (int y = 0; y < levelWidth; y++)
            {
                for (int x = 0; x < levelHeight; x++)
                { 
                    if (searchNodes[x, y] == null)
                    {
                        continue;
                    }
                    MapNode node = searchNodes[x, y];

                    node.inOpenList = false;
                    node.inClosedList = false;

                    node.distanceSoFar = float.MaxValue;
                    node.distanceToGoal = float.MaxValue;
                }
            }
        }

        /// <summary>
        /// Finds the node with the shortest distance to goal
        /// </summary>
        private MapNode findBestNode()
        {
            MapNode currentNode = openList[0];

            float smallestDistanceToGoal = float.MaxValue;

            // Find the closest node to the goal.
            for (int i = 0; i < openList.Count; i++)
            {
                if (openList[i].distanceToGoal < smallestDistanceToGoal)
                {
                    currentNode = openList[i];
                    smallestDistanceToGoal = currentNode.distanceToGoal;
                }
            }
            return currentNode;
        }

        /// <summary>
        /// Use the parent field of the search nodes to trace a path from the end node to the start node.
        /// </summary>
        private List<Vector2> findFinalPath(MapNode startNode, MapNode endNode)
        {
            closedList.Add(endNode);
            MapNode parentTile = endNode.parent;

            // Trace back through the nodes using the parent fields to find the best path.
            while (parentTile != startNode)
            {
                closedList.Add(parentTile);
                parentTile = parentTile.parent;
            }

            List<Vector2> finalPath = new List<Vector2>();

            // Reverse the path and transform into world space.
            for (int i = closedList.Count - 1; i >= 0; i--)
            {
                finalPath.Add(new Vector2(closedList[i].position.X, closedList[i].position.Y));
            }
            return finalPath; //Yay!
        }

        /// <summary>
        /// Finds the optimal path from one point to another.
        /// </summary>
        public List<Vector2> findPath(Point startPoint, Point endPoint)
        {
            if (startPoint == endPoint)
            {
                return new List<Vector2>();
            }

            /////////////////////////////////////////////////////////////////////
            // Step 1 : Clear the Open and Closed Lists and reset each node’s F 
            //          and G values in case they are still set from the last 
            //          time we tried to find a path. 
            /////////////////////////////////////////////////////////////////////
            resetSearchNodes();

            // Store references to the start and end nodes for convenience.
            MapNode startNode = searchNodes[startPoint.X, startPoint.Y];
            MapNode endNode = searchNodes[endPoint.X, endPoint.Y];

            /////////////////////////////////////////////////////////////////////
            // Step 2 : Set the start node’s G value to 0 and its F value to the 
            //          estimated distance between the start node and goal node 
            //          (this is where our H function comes in) and add it to the 
            //          Open List. 
            /////////////////////////////////////////////////////////////////////
            startNode.inOpenList = true;

            startNode.distanceToGoal = heuristic(startPoint, endPoint);
            startNode.distanceSoFar = 0;

            openList.Add(startNode);

            /////////////////////////////////////////////////////////////////////
            // Setp 3 : While there are still nodes to look at in the Open list : 
            /////////////////////////////////////////////////////////////////////
            while (openList.Count > 0)
            {
                /////////////////////////////////////////////////////////////////
                // a) : Loop through the Open List and find the node that 
                //      has the smallest F value.
                /////////////////////////////////////////////////////////////////
                MapNode currentNode = findBestNode();

                /////////////////////////////////////////////////////////////////
                // b) : If the Open List empty or no node can be found, 
                //      no path can be found so the algorithm terminates.
                /////////////////////////////////////////////////////////////////
                if (currentNode == null)
                {
                    break;
                }

                /////////////////////////////////////////////////////////////////
                // c) : If the Active Node is the goal node, we will 
                //      find and return the final path.
                /////////////////////////////////////////////////////////////////
                if (currentNode == endNode)
                {
                    // Trace our path back to the start.
                    return findFinalPath(startNode, endNode);
                }

                /////////////////////////////////////////////////////////////////
                // d) : Else, for each of the Active Node’s neighbours :
                /////////////////////////////////////////////////////////////////
                for (int i = 0; i < currentNode.neighbors.Length; i++)
                {
                    MapNode neighbor = currentNode.neighbors[i];

                    //////////////////////////////////////////////////
                    // i) : Make sure that the neighbouring node can 
                    //      be walked across. 
                    //////////////////////////////////////////////////
                    if (neighbor == null || neighbor.traversable == false)
                    {
                        continue;
                    }

                    //////////////////////////////////////////////////
                    // ii) Calculate a new G value for the neighbouring node.
                    //////////////////////////////////////////////////
                    float distanceTraveled = currentNode.distanceSoFar + 1;

                    // An estimate of the distance from this node to the end node.
                    Point nPos = new Point((int)neighbor.position.X, (int)neighbor.position.Y);
                    float hValue = heuristic(nPos, endPoint);

                    //////////////////////////////////////////////////
                    // iii) If the neighbouring node is not in either the Open 
                    //      List or the Closed List : 
                    //////////////////////////////////////////////////
                    if (neighbor.inOpenList == false && neighbor.inClosedList == false)
                    {
                        // (1) Set the neighbouring node’s G value to the G value 
                        //     we just calculated.
                        neighbor.distanceSoFar = distanceTraveled;
                        // (2) Set the neighbouring node’s F value to the new G value + 
                        //     the estimated distance between the neighbouring node and
                        //     goal node.
                        neighbor.distanceToGoal = distanceTraveled + hValue;
                        // (3) Set the neighbouring node’s Parent property to point at the Active 
                        //     Node.
                        neighbor.parent = currentNode;
                        // (4) Add the neighbouring node to the Open List.
                        neighbor.inOpenList = true;
                        openList.Add(neighbor);
                    }
                    //////////////////////////////////////////////////
                    // iv) Else if the neighbouring node is in either the Open 
                    //     List or the Closed List :
                    //////////////////////////////////////////////////
                    else if (neighbor.inOpenList || neighbor.inClosedList)
                    {
                        // (1) If our new G value is less than the neighbouring 
                        //     node’s G value, we basically do exactly the same 
                        //     steps as if the nodes are not in the Open and 
                        //     Closed Lists except we do not need to add this node 
                        //     the Open List again.
                        if (neighbor.distanceSoFar > distanceTraveled)
                        {
                            neighbor.distanceSoFar = distanceTraveled;
                            neighbor.distanceToGoal = distanceTraveled + hValue;

                            neighbor.parent = currentNode;
                        }
                    }
                }

                /////////////////////////////////////////////////////////////////
                // e) Remove the Active Node from the Open List and add it to the 
                //    Closed List
                /////////////////////////////////////////////////////////////////
                openList.Remove(currentNode);
                currentNode.inClosedList = true;
            }

            // No path could be found.
            return new List<Vector2>();
        }

        ///End of class
    }
}
