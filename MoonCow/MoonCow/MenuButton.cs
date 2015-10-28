using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    public class MenuButton
    {
        public bool active;
        public string name;
        Vector2 pos;
        Vector2 mainPos;
        Vector2 offsetPos;
        Vector2 oldPos;
        Vector2 goalPos;
        float moveTime;
        float alpha;
        float goalAlpha;
        float oldAlpha;
        public MainMenu menu;
        Color col;
        Color disabCol;
        float scale;
        float xOffset;

        public MenuButton(string name, Vector2 pos, int type, bool visible)
        {
            active = false;
            this.name = name;
            this.pos = pos;
            this.scale = 28;

            if (visible)
                alpha = 1;
            else
                alpha = 0;

            setPos();

            switch (type)
            {
                default:
                    disabCol = MenuAssets.blueBody;
                    break;
                case 1:
                    disabCol = MenuAssets.contSecondary;
                    break;
            }

            col = disabCol;

            xOffset = MenuAssets.font.MeasureString(name).X / 2;
            moveTime = 1;
        }

        public MenuButton(string name, Vector2 pos, int type, float scale)
        {
            active = false;
            this.name = name;
            this.pos = pos;
            this.scale = scale;
            alpha = 0;
            setPos();

            switch (type)
            {
                default:
                    disabCol = MenuAssets.blueBody;
                    break;
                case 1:
                    disabCol = MenuAssets.contSecondary;
                    break;
            }

            col = disabCol;
            moveTime = 1;
        }

        public MenuButton(string name, Vector2 pos, int type)
        {
            active = false;
            this.name = name;
            this.pos = pos;
            alpha = 1;
            setPos();

            scale = 28;

            switch(type)
            {
                default:
                    disabCol = MenuAssets.blueBody;
                    break;
                case 1:
                    disabCol = MenuAssets.contSecondary;
                    break;
            }

            col = disabCol;
            moveTime = 1;
        }

        void setPos()
        {
            mainPos = pos;

            if (alpha == 0)
            {
                pos += new Vector2(-30, 0);
            }
            offsetPos = pos;
            oldPos = pos;
            goalPos = pos;
        }

        public void activate()
        {
            active = true;
            col = Color.White;
        }

        public void disable()
        {
            active = false;
            col = disabCol;
        }

        public void push(bool left, bool visible, bool toRealPos)
        {
            oldPos = pos;
            oldAlpha = alpha;
            if (toRealPos)
            {
                goalPos = mainPos;
            }
            else
            {
                if (left)
                {
                    goalPos = mainPos + new Vector2(-60,0);
                }
                else
                {
                    goalPos = mainPos + new Vector2(60,0);
                }
            }

            if(visible)
            {
                goalAlpha = 1;
            }
            else
            {
                goalAlpha = 0;
            }

            moveTime = 0;
        }


        public void Update()
        {
            if(moveTime != 1)
            {
                moveTime += Utilities.deltaTime * 4;
                if (moveTime > 1)
                    moveTime = 1;

                pos = Vector2.SmoothStep(oldPos, goalPos, moveTime);
                alpha = MathHelper.SmoothStep(oldAlpha, goalAlpha, moveTime);
            }
        }

        public void Draw(SpriteBatch sb)
        {
            sb.DrawString(MenuAssets.font, name, Utilities.scaledCoords(pos), col*alpha, 0,
                        new Vector2(xOffset, MenuAssets.font.MeasureString(name).Y / 2), Utilities.windowScale * scale / 40, SpriteEffects.None, 0);
        }
    }
}
