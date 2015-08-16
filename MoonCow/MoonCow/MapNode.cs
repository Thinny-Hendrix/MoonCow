using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MoonCow
{
    /// <summary>
    /// A node in the map. This will be represented by a tile model in the 3D world.
    /// This node is used for many things, primarily pathfinding.
    /// </summary>
    class MapNode
    {
        public bool traversable;       //Can this node be walked through?
        public Vector2 position;       //X and Y co-ordinates in map
        public MapNode[] neighbors;    //Array of linked nodes
        public MapNode parent;         //Pointer to node in path before this one

        public bool inOpenList;        //Currently in OpenList?
        public bool inClosedList;      //Currently in ClosedList?

        public float distanceToGoal;   //How far to goal
        public float distanceSoFar;    //How far along in the path this node is

        public float damage;           //How much damage this square can take from turrets
        public float playerDamage;     //How much damage the player can currently do to this node
        

        /// <summary>
        /// Constructor for MapNode. Sets a lot of data based on the type of node it is.
        /// </summary>
        public MapNode(int type)
        {
            if(type == 0)
            {
                traversable = true;
            }
            else
            {
                traversable = false;
            }
        }
    }
}
  