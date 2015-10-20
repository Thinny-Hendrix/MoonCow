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
            if (pointInside[0] || pointInside[1] || pointInside[2] || pointInside[3])
            {
                return true;
            }
            else
            {
                foreach(Vector2 corner in box.corners)
                {
                    if(checkPoint(corner))
                    {
                        return true;
                    }
                }
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

        public Vector3 wallCollide(OOBB box)
        {
            bool[] pointInside = new bool[2];

            for (int i = 0; i < 2; i++) // Loop through two perpendicular sides of the box
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
            if (pointInside[0] || pointInside[1])
            {
                return box.wallNormal;
            }
            else
            {
                for (int i = 0; i < 4; i++) // foreach Vector2 corner in other.corners
                {
                    if (checkPoint(box.corners[i]))
                    {
                        // corner is inside circle
                        return box.wallNormal;
                    }
                }
            }

            return Vector3.Zero;
        }

        /*
        public Vector3 boxCollide(OOBB other)
        {

            Vector2 collisionPoint = Vector2.Zero;
            Vector2[] perpendicularPoints = new Vector2[4];

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

                perpendicularPoints[i] = circlePoint;
                perpendicularPoints[i + 2] = otherCirclePoint;

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

            // If any of the circle points are inside the box
            if (!(collisionPoint.Equals(Vector2.Zero)))
            {
                Vector3 normal = new Vector3(centre.X, 0, centre.Y) - new Vector3(collisionPoint.X, 0, collisionPoint.Y);
                normal.Normalize();
                normal.Y = 0;
                return normal;
            }
            else
            {
             /*
             * The problem now is detecting the corners of the box
             * If there is a convex join between two boxes the normal will not be correct
             * This is due to the perpenducular point no longer being on the side of the box
             * Need to write code here that will determine the normal if just a corner of the box is in the circle
             * This may not be easy
             * /
                for(int i = 0; i < 4; i++) // foreach Vector2 corner in other.corners
                {
                    if(checkPoint(other.corners[i]))
                    {
                        // corner is inside circle
                        System.Diagnostics.Debug.WriteLine("Problem Collision Detected!");

                        Vector2 p1 = other.corners[i];
                        Vector2 p2 = Vector2.Zero;
                        Vector2 p3 = Vector2.Zero;
                        Vector2 p4 = Vector2.Zero;
                        if (distance(other.corners[i], other.corners[(i + 1) % 4]) < distance(other.corners[i], other.corners[(i + 3) % 4]))
                        {
                            p2 = other.corners[(i + 1) % 4];
                            Vector2 longDir = other.corners[(i + 2) % 4] - p2;
                            longDir.Normalize();
                            p3 = p2 + (longDir * distance(p1, p2));
                            p4 = p1 + (longDir * distance(p1, p4));
                        }
                        else
                        {
                            p2 = other.corners[(i + 1) % 4];
                            Vector2 longDir = other.corners[(i + 2) % 4] - p2;
                            longDir.Normalize();
                            p3 = p2 + (longDir * distance(p1, p2));
                            p4 = p1 + (longDir * distance(p1, p2));
                        }

                        Vector2 lineDir = p1 - p3;
                        lineDir.Normalize();

                        Vector2 endPoint = p3 + (lineDir * distance(p1, p2));

                        float A = -1f * (endPoint.Y - p3.Y);
                        float B = endPoint.X - p3.X;
                        float C = -1f * ((A * p3.X) + (B * endPoint.Y));

                        float D = (A * centre.X) + (B * centre.Y) + C;

                        if(D > 0)
                        {
                            float E = (A * p2.X) + (B * p2.Y) + C;
                            Vector2 faceDir = Vector2.Zero;
                            if(E < 0)
                            {
                                faceDir = p2 - p1;
                            }
                            else
                            {
                                faceDir = p4 - p1;
                            }
                            
                            Vector3 normal = new Vector3(faceDir.Y, 0, faceDir.X * -1f);
                            normal.Normalize();
                            normal.Y = 0;
                            //normal *= -1;
                            System.Diagnostics.Debug.WriteLine("Complex normal = " + normal);
                            return normal;
                        }
                        else
                        {
                            if (D == 0)
                            {
                                Vector3 normal = new Vector3(lineDir.X, 0, lineDir.Y);
                                System.Diagnostics.Debug.WriteLine("Complex normal = " + normal);
                                return normal;
                            }
                            else
                            {
                                float E = (A * p2.X) + (B * p2.Y) + C;
                                Vector2 faceDir = Vector2.Zero;
                                if (E < 0)
                                {
                                    faceDir = p4 - p1;
                                }
                                else
                                {
                                    faceDir = p2 - p1;
                                }

                                Vector3 normal = new Vector3(faceDir.Y, 0, faceDir.X * -1f);
                                normal.Normalize();
                                normal.Y = 0;
                                normal *= -1;
                                System.Diagnostics.Debug.WriteLine("Complex normal = " + normal);
                                return normal;
                            }
                        }
                    }
                }
            }

            //return false;

            return Vector3.Zero;
        }
        */

        /*
        private float distance(Vector2 a, Vector2 b)
        {
            return (float)Math.Sqrt(Math.Pow(b.X - a.X, 2) + Math.Pow(b.Y - a.Y, 2));
        }
        */
    }

}
