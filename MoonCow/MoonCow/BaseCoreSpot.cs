using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MoonCow
{
    /// <summary>
    /// used for enemies so they don't go on top of each other
    /// </summary>
    public class BaseCoreSpot
    {
        public float rot; //the rotation relative to the core
        public bool taken;
        public Vector3 dir;
        public Vector3 pos;
        BaseCore core;

        public BaseCoreSpot(BaseCore core, float rot)
        {
            taken = false;
            this.rot = rot;
            dir.X = -(float)Math.Sin(rot);
            dir.Z = -(float)Math.Cos(rot);
            this.core = core;

            pos = core.pos + dir * 9;
        }
    }
}
