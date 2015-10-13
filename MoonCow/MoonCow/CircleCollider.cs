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
             */
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

                        float D = (A * centre.Y) + (B * centre.Y) + C;

                        if(D >= 0)
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
                            normal *= -1;
                            System.Diagnostics.Debug.WriteLine("Complex normal = " + normal + " D > 0");
                            return normal;
                        }
                        else
                        {
                            float E = (A * p2.X) + (B * p2.Y) + C;
                            Vector2 faceDir = Vector2.Zero;
                            if (E > 0)
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
                            System.Diagnostics.Debug.WriteLine("Complex normal = " + normal + " D < 0");
                            return normal;
                        }


                        /*  Attempt 1
                        List<OOBB> quadrants = new List<OOBB>();
                        quadrants.Add(new OOBB(centre, perpendicularPoints[0], perpendicularPoints[0] + (perpendicularPoints[1] - centre), perpendicularPoints[1]));
                        quadrants.Add(new OOBB(centre, perpendicularPoints[1], perpendicularPoints[1] + (perpendicularPoints[2] - centre), perpendicularPoints[2]));
                        quadrants.Add(new OOBB(centre, perpendicularPoints[2], perpendicularPoints[2] + (perpendicularPoints[3] - centre), perpendicularPoints[3]));
                        quadrants.Add(new OOBB(centre, perpendicularPoints[3], perpendicularPoints[3] + (perpendicularPoints[0] - centre), perpendicularPoints[0]));
                        
                        foreach(OOBB quad in quadrants)
                        {
                            if (quad.pointInBox(other.corners[i]))
                            {
                                System.Diagnostics.Debug.WriteLine("Quad found!");
                                return cornerNormal(quad, other, i);
                            }
                        }
                         * */

                    }
                }
            }

            //return false;

            return Vector3.Zero;
        }

        /* part of attempt 1
        private Vector3 cornerNormal(OOBB quad, OOBB box, int corner)
        {
            System.Diagnostics.Debug.WriteLine("Complex Corner Normal Called");

            bool magicPoint1Set = false;
            bool magicPoint2Set = false;
            Vector2 magicPoint1 = Vector2.Zero;
            Vector2 magicPoint2 = Vector2.Zero;
            float t;
            float u;

            // First find magic point 1 with top line
            Vector2 p = quad.corners[3];
            Vector2 p2 = quad.corners[2];

            Vector2 r = p2 - p;

            // First try and see if it intersects one side of the box
            Vector2 q = box.corners[corner];
            Vector2 q2 = box.corners[(corner + 1) % 4];

            Vector2 s = q2 - q;

            float rxs = crossProduct(r, s);

            if(rxs != 0)
            {
                t = crossProduct(q - p, s) / rxs;
                u = crossProduct(q - p, r) / rxs;
                magicPoint1 = p + (t * r);
                magicPoint1Set = true;
            }
            else // Then try the other side of the box
            {
                q2 = box.corners[(corner + 3) % 4];
                s = q2 - q;
                rxs = crossProduct(r, s);

                if (rxs != 0)
                {
                    t = crossProduct(q - p, s) / rxs;
                    u = crossProduct(q - p, r) / rxs;
                    magicPoint1 = p + (t * r);
                    magicPoint1Set = true;
                }
            }

            // Now find magic point 2
            // reset side of quad to be looking at
            p = quad.corners[1];
            p2 = quad.corners[2];

            r = p2 - p;

            // Try first side of box
            q = box.corners[corner];
            q2 = box.corners[(corner + 1) % 4];

            s = q2 - q;

            rxs = crossProduct(r, s);

            if (rxs != 0)
            {
                t = crossProduct(q - p, s) / rxs;
                u = crossProduct(q - p, r) / rxs;
                magicPoint2 = p + (t * r);
                magicPoint2Set = true;
            }
            else // Then try the other side of the box
            {
                q2 = box.corners[(corner + 3) % 4];
                s = q2 - q;
                rxs = crossProduct(r, s);

                if (rxs != 0)
                {
                    t = crossProduct(q - p, s) / rxs;
                    u = crossProduct(q - p, r) / rxs;
                    magicPoint2 = p + (t * r);
                    magicPoint2Set = true;
                }
            }


            if(magicPoint1Set && magicPoint2Set)
            {
                if(distance(box.corners[corner], magicPoint1) >= distance(box.corners[corner], magicPoint2))
                {
                    Vector2 dir = magicPoint1 - box.corners[corner];
                    Vector3 normal = new Vector3(dir.Y, 0, dir.X * -1);
                    normal.Normalize();
                    normal *= -1;
                    System.Diagnostics.Debug.WriteLine("Complex normal = " + normal);
                    return normal;
                }
                else
                {
                    Vector2 dir = magicPoint2 - box.corners[corner];
                    Vector3 normal = new Vector3(dir.Y, 0, dir.X * -1);
                    normal.Normalize();
                    //normal *= -1;
                    System.Diagnostics.Debug.WriteLine("Complex normal = " + normal);
                    return normal;
                }
            }
            else
            {
                return Vector3.Zero;
            }
        }
         */

        private float crossProduct(Vector2 a, Vector2 b)
        {
            return (a.X * b.Y - a.Y * b.X);
        }

        private float distance(Vector2 a, Vector2 b)
        {
            return (float)Math.Sqrt(Math.Pow(b.X - a.X, 2) + Math.Pow(b.Y - a.Y, 2));
        }

    }

}
