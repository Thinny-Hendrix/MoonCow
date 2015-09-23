using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MoonCow
{
    public class MgInstance
    {
        //list of ints for marker type
        //list of times for how long until next thing (marker count-1)
        public List<int> markTypes;
        public List<float> nextTimes;
        Minigame minigame;
        MgManager manager;
        Game1 game;

        public MgInstance(Game1 game, Minigame minigame)
        {
            this.game = game;
            this.minigame = minigame;
            manager = minigame.manager;
            markTypes = new List<int>();
            nextTimes = new List<float>();
        }

        public void generateNew()
        {
            markTypes.Clear();
            nextTimes.Clear();

            int dubCount = 0;

            for(int i = 0; i < minigame.maxBeats; i++)
            {
                if(dubCount < minigame.maxDubs && Utilities.random.Next(2) == 0)
                {
                    markTypes.Add(Utilities.random.Next(4));
                    markTypes.Add(Utilities.random.Next(4));
                    nextTimes.Add(0.5f);
                    nextTimes.Add(0.5f);
                    dubCount++;
                }
                else
                {
                    markTypes.Add(Utilities.random.Next(4));
                    nextTimes.Add(1);
                }
            }
        }
    }
}
