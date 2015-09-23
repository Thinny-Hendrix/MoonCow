using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MoonCow
{
    public class OOBB
    {
        public Vector2[] corners;
        protected float size;
        protected float width;
        protected float height;

        public OOBB(Vector2 a, Vector2 b, Vector2 c, Vector2 d)
        {
            corners = new Vector2[4];
            corners[0] = a;
            corners[1] = b;
            corners[2] = c;
            corners[3] = d;
            getArea();
        }

        public OOBB(Vector3 pos, Vector3 direction, float w, float h)
        {
            width = w;
            height = h;
            size = w * h;
            corners = new Vector2[4];
            corners[0] = new Vector2(pos.X - (width / 2), pos.Z - (height / 2));
            corners[1] = new Vector2(pos.X + (width / 2), pos.Z - (height / 2));
            corners[2] = new Vector2(pos.X + (width / 2), pos.Z + (height / 2));
            corners[3] = new Vector2(pos.X - (width / 2), pos.Z + (height / 2));
            Update(pos, direction);
        }

        /// <summary>
        /// updates a non-rectangular OOBB
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="dir"></param>
        public void UpdateFrustum(Vector3 pos, Vector3 dir)
        {
            corners[0] = new Vector2(0 - (width / 2), 0 - (height / 2));
            corners[1] = new Vector2(0 + (width / 2), 0 - (height / 2));
            corners[2] = new Vector2(0 + (width / 2), 0 + (height / 2));
            corners[3] = new Vector2(0 - (width / 2), 0 + (height / 2));

            // Set rotation
            float rotation = (float)Math.Atan2(dir.X, dir.Z);
            for (int i = 0; i < 4; i++)
            {
                corners[i] = new Vector2((float)(corners[i].X * Math.Cos(rotation) - corners[i].Y * Math.Sin(rotation)), (float)(corners[i].X * Math.Sin(rotation) + corners[i].Y * Math.Cos(rotation)));
            }

            //Set position
            for (int i = 0; i < 4; i++)
            {
                corners[i] += new Vector2(pos.X, pos.Z);
                //System.Diagnostics.Debug.WriteLine(corners[i]);
            }
        }


        /// <summary>
        /// Move the collision box to a new place and rotate accordingly
        /// currently has no rotation
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="direction"></param>
        public void Update(Vector3 pos, Vector3 direction)
        {
            // Set size
            corners[0] = new Vector2(0 - (width / 2), 0 - (height / 2));
            corners[1] = new Vector2(0 + (width / 2), 0 - (height / 2));
            corners[2] = new Vector2(0 + (width / 2), 0 + (height / 2));
            corners[3] = new Vector2(0 - (width / 2), 0 + (height / 2));

            // Set rotation
            float rotation = (float)Math.Atan2(direction.X, direction.Z);
            for (int i = 0; i < 4; i++)
            {
                corners[i] = new Vector2((float)(corners[i].X * Math.Cos(rotation) - corners[i].Y * Math.Sin(rotation)), (float)(corners[i].X * Math.Sin(rotation) + corners[i].Y * Math.Cos(rotation)));
            }

            //Set position
            for (int i = 0; i < 4; i++)
            {
                corners[i] += new Vector2(pos.X, pos.Z);
                //System.Diagnostics.Debug.WriteLine(corners[i]);
            }
        }

        /// <summary>
        /// Does this box intesect another box?
        /// </summary>
        /// <param name="object2"></param>
        /// <returns></returns>
        public bool intersects(OOBB object2)
        {
            bool[] leftOfFace = new bool[4];

            for (int p = 0; p < 4; p++) // for each corner of object 2
            {
                for (int i = 0; i < 4; i++)
                {
                    float A = -1f * (corners[(i + 1) % 4].Y - corners[i].Y);
                    float B = corners[(i + 1) % 4].X - corners[i].X;
                    float C = -1f * ((A * corners[i].X) + (B * corners[i].Y));

                    float D = (A * object2.corners[p].X) + (B * object2.corners[p].Y) + C;

                    leftOfFace[i] = D < 0;
                }
                if(!(leftOfFace[0] || leftOfFace[1] || leftOfFace[2] || leftOfFace[3]))     // if the point is left of every plane it is inside
                {
                    return true;
                }
            }
            
            for (int p = 0; p < 4; p++) // for each corner in object 1
            {
                for (int i = 0; i < 4; i++)
                {
                    float A = -1f * (object2.corners[(i + 1) % 4].Y - object2.corners[i].Y);
                    float B = object2.corners[(i + 1) % 4].X - object2.corners[i].X;
                    float C = -1f * ((A * object2.corners[i].X) + (B * object2.corners[i].Y));

                    float D = (A * corners[p].X) + (B * corners[p].Y) + C;

                    leftOfFace[i] = D < 0;
                }
                if (!(leftOfFace[0] || leftOfFace[1] || leftOfFace[2] || leftOfFace[3]))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Checks if a point is inside the box
        /// </summary>
        /// <param name="point"></param>
        /// <returns>True if point is in box, false if not</returns>
        public bool pointInBox(Vector2 point)
        {
            bool[] leftOfFace = new bool[4];

            for (int i = 0; i < 4; i++)
            {
                float A = -1f * (corners[(i + 1) % 4].Y - corners[i].Y);
                float B = corners[(i + 1) % 4].X - corners[i].X;
                float C = -1f * ((A * corners[i].X) + (B * corners[i].Y));

                float D = (A * point.X) + (B * point.Y) + C;

                leftOfFace[i] = D < 0;
            }

            if (!(leftOfFace[0] || leftOfFace[1] || leftOfFace[2] || leftOfFace[3]))     // if the point is left of every plane it is inside
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Gets the area of the bounding box
        /// </summary>
        /// <returns>The area of the box</returns>
        public void getArea()
        {
            width = (float)Math.Sqrt(Math.Pow(corners[1].X - corners[0].X, 2) + Math.Pow(corners[1].Y - corners[0].Y, 2));
            height = (float)Math.Sqrt(Math.Pow(corners[2].X - corners[1].X, 2) + Math.Pow(corners[2].Y - corners[1].Y, 2));

            size = width * height;
        }
    }
}
