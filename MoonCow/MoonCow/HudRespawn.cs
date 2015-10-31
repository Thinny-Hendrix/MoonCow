using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    public class HudRespawn
    {
        float time;
        int state;
        string text;
        float scale;
        float alpha;
        Ship ship;
        bool active;
        float fadeAlpha;
        SpriteFont font;
        Hud hud;
        Game1 game;
        bool respawned;

        public HudRespawn(Hud hud, SpriteFont font, Game1 game)
        {
            active = true;
            state = 5;
            time = 1.5f;
            this.hud = hud;
            this.font = font;
            this.game = game;
            ship = game.ship;
            updateText();
        }

        public void Update()
        {
            if(active)
            {
                time += Utilities.deltaTime;
                if(state < 5)
                {
                    if(time > 0.5f)
                    {
                        alpha = MathHelper.Lerp(1, 0, (time - 0.5f) * 2);
                    }
                    if(time >= 1)
                    {
                        state++;
                        updateText();
                        time = 0;
                    }
                }
                else
                {
                    if (time < 1)
                    {
                        fadeAlpha = MathHelper.Lerp(0, 1, time);
                    }
                    else if (time < 1.5f)
                    {
                        fadeAlpha = 1;
                    }
                    else
                    {
                        if(!respawned)
                        {
                            ship.respawn();
                            respawned = true;
                        }
                        fadeAlpha = MathHelper.Lerp(1, 0, time - 1.5f);
                    }
                    if(time > 2.5f)
                    {
                        active = false;
                    }
                }
            }
        }

        public void startCountdown()
        {
            state = -2;
            time = 0;
            respawned = false;
            active = true;
        }

        void updateText()
        {
            switch (state)
            {
                default:
                    text = "5";
                    scale = 80;
                    alpha = 1;
                    break;
                case 1:
                    text = "4";
                    scale = 80;
                    alpha = 1;
                    break;
                case 2:
                    text = "3";
                    scale = 80;
                    alpha = 1;
                    break;
                case 3:
                    text = "2";
                    scale = 80;
                    alpha = 1;
                    break;
                case 4:
                    text = "1";
                    scale = 80;
                    alpha = 1;
                    break;

            }
        }

        public void draw(SpriteBatch sb)
        {
            if (active && !game.camera.endGame)
            {
                sb.Begin();
                if (state < 5 && state > -1)
                {
                    sb.DrawString(font, text, hud.scaledCoords(965, 545), Color.Black * (alpha / 2), 0,
                       new Vector2(font.MeasureString(text).X / 2, font.MeasureString(text).Y / 2 - 20), scale / 80, SpriteEffects.None, 0);

                    sb.DrawString(font, text, hud.scaledCoords(960, 540), Color.White * alpha, 0,
                        new Vector2(font.MeasureString(text).X / 2, font.MeasureString(text).Y / 2 - 20), scale / 80, SpriteEffects.None, 0);
                }
                else
                {
                    sb.Draw(TextureManager.pureWhite, hud.scaledRect(Vector2.Zero, 1920, 1080), Color.Black * fadeAlpha);
                }
                sb.End();
            }
        }
    }
}
