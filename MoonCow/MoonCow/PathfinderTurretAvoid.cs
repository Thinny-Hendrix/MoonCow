using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MoonCow
{
    class PathfinderTurretAvoid : Pathfinder
    {
        public PathfinderTurretAvoid(Map level) :base(level)
        {
            // Constructors are the same.
        }

        /// <summary>
        /// Finds the optimal path from one point to another.
        /// </summary>
        public override List<Vector2> findPath(Point startPoint, Point endPoint)
        {
            if (startPoint == endPoint)
            {
                return new List<Vector2>();
            }

            // Step 1 : Clear the Open and Closed Lists and reset each node’s F and G values 
            resetSearchNodes();

            // Store references to the start and end nodes for convenience.
            MapNode startNode = searchNodes[startPoint.X, startPoint.Y];
            MapNode endNode = searchNodes[endPoint.X, endPoint.Y];

            // Step 2 : Set the start node’s G value to 0 and its F value to the estimated distance between the start and end, also add to open list
            startNode.inOpenList = true;

            startNode.distanceToGoal = heuristic(startPoint, endPoint);
            startNode.distanceSoFar = 0;

            openList.Add(startNode);

            // Setp 3 : While there are still nodes to look at in the Open list : 
            while (openList.Count > 0)
            {
                // a) : Loop through the Open List and find the node that has the smallest F value.
                MapNode currentNode = findBestNode();

                // b) : If the Open List empty or no node can be found, no path can be found so the algorithm terminates.
                if (currentNode == null)
                {
                    break;
                }

                // c) : If the Active Node is the goal node, we will find and return the final path.
                if (currentNode == endNode)
                {
                    // Trace our path back to the start.
                    return findFinalPath(startNode, endNode);
                }

                // d) : Else, for each of the Active Node’s neighbours :
                for (int i = 0; i < currentNode.neighbors.Length; i++)
                {
                    MapNode neighbor = currentNode.neighbors[i];

                    // i) : Make sure that the neighbouring node can be walked across. 
                    if (neighbor == null || neighbor.traversable == false)
                    {
                        continue;
                    }

                    // ii) Calculate a new G value for the neighbouring node.
                    float distanceTraveled = currentNode.distanceSoFar + 1 + neighbor.damage;

                    // An estimate of the distance from this node to the end node.
                    Point nPos = new Point((int)neighbor.position.X, (int)neighbor.position.Y);
                    float hValue = heuristic(nPos, endPoint);

                    // iii) If the neighbouring node is not in either the Open List or the Closed List : 
                    if (neighbor.inOpenList == false && neighbor.inClosedList == false)
                    {
                        // (1) Set the neighbouring node’s G value to the G value we just calculated.
                        neighbor.distanceSoFar = distanceTraveled;
                        // (2) Set the neighbouring node’s F value to the new G value + the estimated distance between the neighbouring node and goal node.
                        neighbor.distanceToGoal = distanceTraveled + hValue;
                        // (3) Set the neighbouring node’s Parent property to point at the Active Node.
                        neighbor.parent = currentNode;
                        // (4) Add the neighbouring node to the Open List.
                        neighbor.inOpenList = true;
                        openList.Add(neighbor);
                    }
                    // iv) Else if the neighbouring node is in either the Open List or the Closed List :
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
                //Remove the Active Node from the Open List and add it to the Closed List
                openList.Remove(currentNode);
                currentNode.inClosedList = true;
            }

            // No path could be found.
            return new List<Vector2>();
        }
    }
}
