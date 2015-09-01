using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MoonCow
{
    class CircleCollider
    {
        Vector2 centre;
        float radius;

        public CircleCollider(Vector2 middle, float r)
        {
            centre = middle;
            radius = r;
        }

        public bool checkPoint(Vector2 point)
        {
            // maths here
            float dist = (float)Math.Sqrt(Math.Pow(point.X - centre.X, 2) + Math.Pow(point.Y - centre.Y, 2));
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
