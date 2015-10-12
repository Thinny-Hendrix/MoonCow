using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MoonCow
{
    public class CircleCollider
    {
        public Vector2 centre;
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

        public void Update(Vector2 point)
        {
            centre = point;
        }

        public void Update(Vector3 point)
        {
            centre = new Vector2(point.X, point.Z);
        }

        public bool checkCircle(CircleCollider other)
        {
            float dist = distFrom(other.centre);
            return dist <= radius + other.radius;
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

        public Vector3 directionFrom(Vector3 point)
        {
            float x = centre.X - point.X;
            float z = centre.Y - point.Z;
            Vector3 dir = new Vector3(x, 0, z);
            dir.Normalize();
            return dir;
        }

        public float distFrom(Vector2 point)
        {
            return (float)Math.Sqrt(Math.Pow(point.X - centre.X, 2) + Math.Pow(point.Y - centre.Y, 2));
        }

        public float distFrom(Vector3 point)
        {
            return (float)Math.Sqrt(Math.Pow(point.X - centre.X, 2) + Math.Pow(point.Z - centre.Y, 2));
        }

        public bool checkOOBB(OOBB box)
        {
            // super maths here
            bool[] pointInside = new bool[2];

            for(int i = 0; i < 2; i++) // Loop through two perpendicular sides of the box
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
            if(pointInside[0] || pointInside[1])
            {
                return true;
            }

            return false;
        }

        public Vector3 circleCollide(CircleCollider other)
        {

            float dist = distFrom(other.centre);

            if(dist <= radius + other.radius)
            {
                Vector3 normal = new Vector3(centre.X, 0, centre.Y) - new Vector3(other.centre.X, 0, other.centre.Y);
                normal.Normalize();
                normal.Y = 0;
                return normal;
            }

            return Vector3.Zero;
        }

        public Vector3 boxCollide(OOBB other)
        {

            Vector2 collisionPoint = Vector2.Zero;

            for (int i = 0; i < 2; i++) // Loop through two perpendicular sides of the box
            {
                // Get normalised direction vector for box side
                Vector2 ABdir = other.corners[(i + 1) % 4] - other.corners[i];
                ABdir.Normalize();

                // Get perpendicular direction vector
                Vector2 PerpAB = new Vector2(ABdir.Y, ABdir.X * -1);

                // get the two points on the circle that intersect the perpendicular direction vector
                Vector2 circlePoint = centre + (radius * PerpAB);
                Vector2 otherCirclePoint = centre + (radius * (-1 * PerpAB));

                // See if either of those points is inisde the box, save them if so
                if(other.pointInBox(circlePoint))
                {
                    collisionPoint = circlePoint;
                }
                if(other.pointInBox(otherCirclePoint))
                {
                    collisionPoint = otherCirclePoint;
                }
            }

            /*
             * The problem now is detecting the corners of the box
             * If there is a convex join between two boxes the normal will not be correct
             * This is due to the perpenducular point no longer being on the side of the box
             * Need to write code here that will determine the normal if just a corner of the box is in the circle
             * This may not be easy
            */

            // If any of the circle points are inside the box
            if (!(collisionPoint.Equals(Vector2.Zero)))
            {
                Vector3 normal = new Vector3(centre.X, 0, centre.Y) - new Vector3(collisionPoint.X, 0, collisionPoint.Y);
                normal.Normalize();
                normal.Y = 0;
                return normal;
            }

            //return false;

            return Vector3.Zero;
        }
    }
}
