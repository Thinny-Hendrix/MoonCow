using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MoonCow
{
    public class StatTracker
    {
        public float laserShotsFired { get; set; }
        public float laserShotsHit { get; set; }
        public float bombsFired { get; set; }
        public float bombsHit { get; set; }
        public float wavesFired { get; set; }
        public float wavesHit { get; set; }
        public float timeInLevel { get; set; }
        public float moneyEarnt { get; set; }
        public float moneySpent { get; set; }

        // Turret placement tracking;
        public int gatlings { get; set; }
        public int flamers { get; set; }
        public int electrics { get; set; }


        public StatTracker()
        {
            resetData();
        }

        public void resetData()
        {
            laserShotsFired = 0;
            laserShotsHit = 0;
            bombsFired = 0;
            bombsHit = 0;
            wavesFired = 0;
            wavesHit = 0;
            timeInLevel = 0;
            moneyEarnt = 0;
            moneySpent = 0;

            gatlings = 0;
            flamers = 0;
            electrics = 0;
        }
    }
}
