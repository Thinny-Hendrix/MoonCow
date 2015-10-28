using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    class LsHeader
    {
        Vector2 scrollPos1;
        Vector2 scrollPos2;
        Vector2 headPos;

        float alpha1;
        float alpha2;
        float inTime;

        Vector2 tabPos;
        Vector2 oldTabPos;
        Vector2 goalTabPos;
        Vector2 boxPos;
        float tabTime;

        public LsHeader()
        {
            scrollPos1 = new Vector2(-245, 100);
            scrollPos2 = new Vector2(0, 240);
            headPos = new Vector2(40, 120);

            boxPos = new Vector2(960, 530);
            tabPos = new Vector2(449, boxPos.Y + 5);
            oldTabPos = tabPos;
            goalTabPos = tabPos;
            tabTime = 1;
        }

        public void push(bool left)
        {
            oldTabPos = tabPos;
            if(left)
            {
                goalTabPos = new Vector2(449, boxPos.Y + 5);
            }
            else
            {
                goalTabPos = new Vector2(956, boxPos.Y + 5);
            }
            tabTime = 0;
        }

        public void Update()
        {
            if(tabTime != 1)
            {
                tabTime += Utilities.deltaTime * 4;
                if (tabTime > 1)
                    tabTime = 1;

                tabPos = Vector2.SmoothStep(oldTabPos, goalTabPos, tabTime);
            }
            scrollPos1.X += Utilities.deltaTime * 100;
            if (scrollPos1.X > 0)
                scrollPos1.X -= 245;

            scrollPos2.X -= Utilities.deltaTime * 100;
            if (scrollPos2.X < -245)
                scrollPos2.X += 245;
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(MenuAssets.lsScroll, Utilities.scaledRect(scrollPos1, 2207,23), Color.White);
            sb.Draw(MenuAssets.lsHead, Utilities.scaledRect(headPos, 1275,136), Color.White);
            sb.Draw(MenuAssets.lsScroll, Utilities.scaledRect(scrollPos2, 2207, 23), Color.White);

            sb.Draw(MenuAssets.lsBody, Utilities.scaledRect(boxPos, 1022, 419), null, Color.White, 0, new Vector2(511, 0), SpriteEffects.None, 0);
            sb.Draw(MenuAssets.lsTab, Utilities.scaledRect(tabPos, 514, 99), null, Color.White, 0, new Vector2(0, 99), SpriteEffects.None, 0);
        }
    }
}
