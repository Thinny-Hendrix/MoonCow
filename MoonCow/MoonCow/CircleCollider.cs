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

        public bool checkOOBB(OOBB box)
        {
            // super maths here
            for(int i = 0; i < 4; i++)
            {
                if(!checkPoint(box.corners[i]) && !checkPoint(box.corners[(i + 1) % 4]))
                {
                   if (Math.Pow(box.corners[i].X, 2) + Math.Pow(box.corners[i].Y, 2) > radius * radius && Math.Pow(box.corners[(i + 1) % 4].X, 2) + Math.Pow(box.corners[(i + 1) % 4].Y, 2) > radius * radius)
                   {
                       if (!(Math.Pow((box.corners[i].X * box.corners[(i + 1) % 4].Y) - (box.corners[(i + 1) % 4].X * box.corners[i].Y), 2) > Math.Pow(radius, 2) * (Math.Pow(box.corners[i].X - box.corners[(i + 1) % 4].X, 2) + Math.Pow(box.corners[i].Y - box.corners[(i + 1) % 4].Y, 2))))
                       {
                           return true;
                       }
                   }
                   else
                   {
                       return true;
                   }
                }
                else 
                { 
                    return false; 
                }
            }
            return false;
        }
    }
}
