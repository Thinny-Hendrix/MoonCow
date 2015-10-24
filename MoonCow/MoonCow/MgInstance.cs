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
        int prevNo;
        int dupeCount;

        public MgInstance(Game1 game, Minigame minigame)
        {
            this.game = game;
            this.minigame = minigame;
            manager = minigame.manager;
            markTypes = new List<int>();
            nextTimes = new List<float>();
            dupeCount = 0;
            prevNo = -1;
        }

        public void generateNew()
        {
            markTypes.Clear();
            nextTimes.Clear();
            dupeCount = 0;
            prevNo = -1;

            int dubCount = 0;

            for(int i = 0; i < minigame.maxBeats; i++)
            {
                if(dubCount < minigame.maxDubs && Utilities.random.Next(2) == 0)
                {
                    addMarker(Utilities.random.Next(4));
                    addMarker(Utilities.random.Next(4));
                    nextTimes.Add(0.5f);
                    nextTimes.Add(0.5f);
                    dubCount++;
                }
                else
                {
                    addMarker(Utilities.random.Next(4));
                    nextTimes.Add(1);
                }
            }
        }

        void addMarker(int no)
        {
            //if this works properly it should prevent there being any more than 4 of the same thing in a row

            if (dupeCount < 4)
            {
                markTypes.Add(no);
                if (no == prevNo)
                    dupeCount++;
                prevNo = no;
            }
            else
            {
                if(no != prevNo)
                {
                    markTypes.Add(no);
                    dupeCount = 0;
                    prevNo = no;
                }
                else
                {
                    //roll random again, if it's still the same thing then just use the next number
                    int temp = Utilities.random.Next(4);
                    if(temp != no)
                    {
                        markTypes.Add(temp);
                        dupeCount = 0;
                        prevNo = temp;
                    }
                    else
                    {
                        no++;
                        if (no > 3)
                            no = 0;
                        markTypes.Add(no);
                        prevNo = no;
                        dupeCount = 0;
                    }
                }
            }
            
        }

        public void generateNewHard()
        {
            markTypes.Clear();
            nextTimes.Clear();
            dupeCount = 0;
            prevNo = -1;
            float maxBeats = minigame.maxBeats * 1.2f;
            float maxDubs = maxBeats * 0.8f;

            int dubCount = 0;

            for (int i = 0; i < maxBeats; i++)
            {
                if (dubCount < maxDubs && Utilities.random.Next(2) == 0)
                {
                    addMarker(Utilities.random.Next(4));
                    addMarker(Utilities.random.Next(4));
                    nextTimes.Add(0.5f);
                    nextTimes.Add(0.5f);
                    dubCount++;
                }
                else
                {
                    addMarker(Utilities.random.Next(4));
                    nextTimes.Add(1);
                }
            }
        }
    }
}
