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
        public Vector3 wallNormal;
        private Vector2[] originCorners; // The corners set with inital rotation with origin as centre - used in updates

        public OOBB(Vector2 a, Vector2 b, Vector2 c, Vector2 d)
        {
            corners = new Vector2[4];
            corners[0] = a;
            corners[1] = b;
            corners[2] = c;
            corners[3] = d;
            generateOriginCorners();
            wallNormal = Vector3.Zero;
        }

        public OOBB(Vector2 a, Vector2 b, Vector2 c, Vector2 d, Vector3 normal)
        {
            corners = new Vector2[4];
            corners[0] = a;
            corners[1] = b;
            corners[2] = c;
            corners[3] = d;
            generateOriginCorners();
            wallNormal = normal;
        }

        private void generateOriginCorners()
        {
            originCorners = new Vector2[4];
            float x = 0;
            float y = 0;
            float m1 = 0;
            float m2 = 0;
            // calculate centre
            if(Math.Abs(corners[1].X - corners[3].X) < 0.001)
            {
                x = corners[1].X;
                m2 = (corners[0].Y - corners[2].Y) / (corners[0].X - corners[2].X);
                y = m2 * (x - corners[2].X) + corners[2].Y;
            }
            else 
            {
                if(Math.Abs(corners[0].X - corners[2].X) < 0.001)
                {
                    x = corners[0].X;
                    m1 = (corners[1].Y - corners[3].Y) / (corners[1].X - corners[3].X);
                    y = m1 * (x - corners[3].X) + corners[3].Y;
                }
                else 
                {
                    m1 = (corners[1].Y - corners[3].Y) / (corners[1].X - corners[3].X);
                    m2 = (corners[0].Y - corners[2].Y) / (corners[0].X - corners[2].X);
                    x = (m1 * corners[3].X - m2 * corners[2].X + corners[2].Y - corners[3].Y) / (m1 - m2);
                    y = m1 * (x - corners[3].X) + corners[3].Y;
                }
            }

            Vector2 currentCentre = new Vector2(x, y);

            /*
            System.Diagnostics.Debug.WriteLine("NEW OOBB");
            System.Diagnostics.Debug.WriteLine("Corner A = " + corners[0]);
            System.Diagnostics.Debug.WriteLine("Corner B = " + corners[1]);
            System.Diagnostics.Debug.WriteLine("Corner C = " + corners[2]);
            System.Diagnostics.Debug.WriteLine("Corner D = " + corners[3]);
            System.Diagnostics.Debug.WriteLine("Centre of OOBB = " + currentCentre);
            */

            for(int i = 0; i < 4; i++)
            {
                originCorners[i] = corners[i] - currentCentre;
            }
        }

        public OOBB(Vector3 pos, Vector3 direction, float width, float height)
        {
            corners = new Vector2[4];
            originCorners = new Vector2[4];
            //Set current corners
            originCorners[0] = new Vector2(0 - (width / 2), 0 - (height / 2));
            originCorners[1] = new Vector2(0 + (width / 2), 0 - (height / 2));
            originCorners[2] = new Vector2(0 + (width / 2), 0 + (height / 2));
            originCorners[3] = new Vector2(0 - (width / 2), 0 + (height / 2));

            Update(pos, direction);
            wallNormal = Vector3.Zero;
        }

        /// <summary>
        /// Move the collision box to a new place and rotate accordingly
        /// currently has no rotation
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="direction"></param>
        public void Update(Vector3 pos, Vector3 direction)
        {
            // Move shape to origin in initial rotation
            for(int i = 0; i < 4; i++)
            {
                corners[i] = originCorners[i];
            }

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
                if(pointInBox(object2.corners[p]))
                {
                    return true;
                }
            }
            
            for (int p = 0; p < 4; p++) // for each corner in object 1
            {
                if(object2.pointInBox(corners[p]))
                {
                    return true;
                }
            }

            return false;
        }

        public void resize(float width, float height)
        {
            originCorners[0] = new Vector2(0 - (width / 2), 0 - (height / 2));
            originCorners[1] = new Vector2(0 + (width / 2), 0 - (height / 2));
            originCorners[2] = new Vector2(0 + (width / 2), 0 + (height / 2));
            originCorners[3] = new Vector2(0 - (width / 2), 0 + (height / 2));
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
    }
}
