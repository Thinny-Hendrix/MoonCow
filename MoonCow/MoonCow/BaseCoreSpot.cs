using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MoonCow
{
    /// <summary>
    /// used for enemies so they don't go on top of each other
    /// </summary>
    public class BaseCoreSpot
    {
        public float rot; //the rotation relative to the core
        public bool taken;

        public BaseCoreSpot(float rot)
        {
            taken = false;
            this.rot = rot;
        }
    }
}
