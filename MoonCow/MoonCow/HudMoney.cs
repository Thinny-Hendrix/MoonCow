using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    public class HudMoney:HudComponent
    {
        Vector2 monPos;
        Vector2 monTotPos;
        Vector2 monDifPos;

        String moneyTot;
        String moneyDif;

        Texture2D hudMonB;
        Texture2D hudMonF;

        public HudMoney(Hud hud, SpriteFont font, Game1 game):base(hud, font, game)
        {
            monPos = new Vector2(1450, 45);
            monTotPos = new Vector2(1700, 100);
            monDifPos = new Vector2(1690, 180);
            wakeThresh = 3;

            hudMonF = game.Content.Load<Texture2D>(@"Hud/hudMonF");
            hudMonB = game.Content.Load<Texture2D>(@"Hud/hudMonB");
        }

        public override void Update()
        {
            moneyTot = "" + Math.Floor(game.ship.moneyManager.displayNo);
            float diff = game.ship.moneyManager.difference;
            if (diff < 0)
                moneyDif = "" + game.ship.moneyManager.difference;
            else
                moneyDif = "+" + game.ship.moneyManager.difference;

            base.Update();
        }

        public override void Draw(SpriteBatch sb)
        {
            if(wakeTime < wakeThresh)
            { 
                float totDim = font.MeasureString(moneyTot).X;
                float diffDim = font.MeasureString(moneyDif).X;


                game.GraphicsDevice.BlendState = BlendState.AlphaBlend;
                sb.Draw(hudMonB, hud.scaledRect(monPos, 425, 151), Color.White);
                sb.Draw(hudMonF, hud.scaledRect(monPos, 425, 151), Color.White);

                sb.DrawString(font, moneyTot, hud.scaledCoords(monTotPos), Color.White, 0,
                    new Vector2(font.MeasureString(moneyTot).X, font.MeasureString(moneyTot).Y / 2), hud.scale * (28.0f / 40), SpriteEffects.None, 0);

                if (game.ship.moneyManager.changing)
                {
                    sb.DrawString(font, moneyDif, hud.scaledCoords(monDifPos.X + 3, monDifPos.Y + 3), hud.outline, 0,
                            new Vector2(font.MeasureString(moneyDif).X, font.MeasureString(moneyDif).Y / 2), hud.scale * (28.0f / 40), SpriteEffects.None, 0);

                    if (game.ship.moneyManager.difference < 0)
                        sb.DrawString(font, moneyDif, hud.scaledCoords(monDifPos), hud.redBody, 0,
                            new Vector2(font.MeasureString(moneyDif).X, font.MeasureString(moneyDif).Y / 2), hud.scale * (28.0f / 40), SpriteEffects.None, 0);
                    else
                        sb.DrawString(font, moneyDif, hud.scaledCoords(monDifPos.X, monDifPos.Y), hud.contSecondary, 0,
                            new Vector2(font.MeasureString(moneyDif).X, font.MeasureString(moneyDif).Y / 2), hud.scale * (28.0f / 40), SpriteEffects.None, 0);
                }
            }
        }
    }
}
