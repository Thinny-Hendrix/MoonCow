using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MoonCow
{
    public class AABB
    {
        //this will just be super basic, I'm just writing this for menu/level creator stuff with the mouse
        public Vector2 max;
        public Vector2 min;
        /// <summary>
        /// min must be a point closer to the origin than max
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        public AABB(Vector2 min, Vector2 max)
        {
            this.min = min;
            this.max = max;
        }

        /// <summary>
        /// top left corner, then width and height
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        public AABB(Vector2 min, float w, float h)
        {
            this.min = min;
            this.max.X = min.X + w;
            this.max.Y = min.Y + h;
        }

        /// <summary>
        /// pos is the top left corner
        /// </summary>
        /// <param name="pos"></param>
        public void Update(Vector2 pos)
        {
            float yDif = max.Y - min.Y;
            float xDif = max.X - min.X;

            min = pos;
            max.X = min.X + xDif;
            max.Y = min.Y + yDif;
        }

        public void Update(Vector2 min, float w, float h)
        {
            this.min = min;
            this.max.X = min.X + w;
            this.max.Y = min.Y + h;
        }

        public bool checkPoint(Vector2 pos)
        {
            if(pos.X >= min.X && pos.X < max.X)
            {
                if(pos.Y >= min.Y && pos.Y < max.Y)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
