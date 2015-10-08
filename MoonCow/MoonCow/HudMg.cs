using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    public class HudMg:HudComponent
    {
        ///get ready... 3 2 1 start!
        int state;
        float speed;
        public bool active;
        float alpha;
        float time;
        float initScale;
        float scale;
        string text;


        public HudMg(Hud hud, SpriteFont font, Game1 game):base(hud, font, game)
        {
            active = false;

            updateText();
        }

        public void trigger(float speed)
        {
            this.speed = speed;
            state = 0;
            active = true;
            updateText();

        }

        public void reset()
        {
            active = false;
            state = 0;
            updateText();
            time = 0;
        }

        void updateText()
        {
            switch(state)
            {
                default:
                    text = "get ready...";
                    scale = 40;
                    alpha = 1;
                    break;
                case 1:
                    text = "3";
                    scale = 80;
                    alpha = 1;
                    break;
                case 2:
                    text = "2";
                    scale = 80;
                    alpha = 1;
                    break;
                case 3:
                    text = "1";
                    scale = 80;
                    alpha = 1;
                    break;
                case 4:
                    text = "start!";
                    scale = 40;
                    alpha = 1;
                    break;

            }
        }

        public override void Update()
        {
            if (active)
            {
                if (state == 0)
                {
                    time += Utilities.deltaTime;

                    if (time > 0.5f)
                        alpha = MathHelper.Lerp(1, 0, (time - 0.5f) * 2);
                    if (time >= 1)
                    {
                        alpha = 1;
                        state++;
                        updateText();
                        time = 0;
                    }
                }
                else
                {
                    time += Utilities.deltaTime * speed / 650;

                    if (state != 4)
                    {
                        scale -= Utilities.deltaTime * speed / 10;
                    }
                    else
                    {
                        scale += Utilities.deltaTime * speed / 10;
                    }

                    if (time > 1)
                    {
                        if (state < 4)
                        {
                            state++;
                            updateText();
                            time = 0;
                        }
                        else
                            reset();
                    }
                    else
                    {
                        if (time > 0.5f)
                            alpha = MathHelper.Lerp(1, 0, (time - 0.5f) * 2);
                    }
                }
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            if (active)
            {
                sb.DrawString(font, text, hud.scaledCoords(965, 545), Color.Black * (alpha/2), 0,
                   new Vector2(font.MeasureString(text).X / 2, font.MeasureString(text).Y / 2-20), scale / 80, SpriteEffects.None, 0);

                sb.DrawString(font, text, hud.scaledCoords(960, 540), Color.White * alpha, 0,
                    new Vector2(font.MeasureString(text).X/2, font.MeasureString(text).Y / 2-20), scale / 80, SpriteEffects.None, 0);
            }
        }
    }
}
