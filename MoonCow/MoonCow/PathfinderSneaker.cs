using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MoonCow
{
    class PathfinderSneaker : Pathfinder
    {
        public PathfinderSneaker(Map level) :base(level)
        {
            // Constructors are the same.
        }

        /// <summary>
        /// Returns the theorectical distance between two points
        /// </summary>
        protected override float heuristic(Point point1, Point point2)
        {
            return Math.Abs(point1.X - point2.X) + Math.Abs(point1.Y - point2.Y) + map.map[(int)point1.X, (int)point1.Y].damage + map.map[(int)point1.X, (int)point1.Y].playerDamage;
        }
    }
}
