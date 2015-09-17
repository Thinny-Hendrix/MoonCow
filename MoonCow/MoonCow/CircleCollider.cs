using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MoonCow
{
    public class CircleCollider
    {
        Vector2 centre;
        public float radius{get; set;}

        public CircleCollider(Vector2 middle, float r)
        {
            centre = middle;
            radius = r;
        }

        public CircleCollider(Vector3 center, float r)
        {
            this.centre = new Vector2(center.X, center.Z);
            radius = r;
        }

        public void Update(Vector3 point)
        {
            centre = new Vector2(point.X, point.Z);
        }

        public bool checkPoint(Vector2 point)
        {
            // maths here
            float dist = (float)Math.Sqrt(Math.Pow(point.X - centre.X, 2) + Math.Pow(point.Y - centre.Y, 2));
            return dist <= radius;
        }

        //vector3 version saves me a lot of time
        public bool checkPoint(Vector3 point)
        {
            // maths here
            float dist = (float)Math.Sqrt(Math.Pow(point.X - centre.X, 2) + Math.Pow(point.Z - centre.Y, 2));
            return dist <= radius;
        }

        public float distFrom(Vector3 point)
        {
            return (float)Math.Sqrt(Math.Pow(point.X - centre.X, 2) + Math.Pow(point.Z - centre.Y, 2));
        }

        public bool checkOOBB(OOBB box)
        {
            // super maths here
            bool[] pointInside = new bool[4];

            for(int i = 0; i < 4; i++) // Loop through two perpendicular sides of the box
            {
                // Get normalised direction vector for box side
                Vector2 ABdir = box.corners[(i + 1) % 4] - box.corners[i];
                ABdir.Normalize();

                // Get perpendicular direction vector
                Vector2 PerpAB = new Vector2(ABdir.Y, ABdir.X * -1);

                // get the two points on the circle that intersect the perpendicular direction vector
                Vector2 circlePoint = centre + (radius * PerpAB);
                Vector2 otherCirclePoint = centre + (radius * (-1 * PerpAB));

                // See if either of those points is inisde the box
                pointInside[i] = box.pointInBox(circlePoint) || box.pointInBox(otherCirclePoint);
            }

            // If any of the circle points are inside the box
            if(pointInside[0] || pointInside[1] || pointInside[2] || pointInside [3])
            {
                return true;
            }

            return false;
        }
    }
}
