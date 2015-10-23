using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    public class HudMessage:HudComponent
    {
        string message;
        Texture2D mesFil;
        Texture2D mesOut;
        Vector2 pos;
        public HudMessage(Hud hud, SpriteFont font, Game1 game):base(hud, font, game)
        {
            mesFil = game.Content.Load<Texture2D>(@"Hud/mesFill");
            mesOut = game.Content.Load<Texture2D>(@"Hud/mesOut");
            wakeThresh = 2;
            wakeTime = wakeThresh;
            message = "";

            pos = new Vector2(960, 960);
        }

        public void setAmmoMessage(Weapon wep, float count)
        {
            message = "Got " + count + " " + wep.name + " " + wep.ammoName;
            if (count > 1 && !wep.name.Contains("bomb"))
                message += "s";
            message += "!";

            wakeTime = 0;
        }

        public void setLevelUpMessage(Weapon wep)
        {
            message = "got the " + wep.formattedLevel() + " " + wep.name + "!";
            wakeTime = -3;
        }

        public void setTextMessage(string s)
        {
            message = s;
            wakeTime = -1;
        }

        public void drillDoorCheck()
        {
            message = "need " + (hud.hudCollectable.count - 4) + " more keys to unlock this door";
            wakeTime = 0;
        }

        public void drillDoorUnlock()
        {
            message = "drill door now unlockable!";
            wakeTime = -5;
        }

        public override void Draw(SpriteBatch sb)
        {
            if (wakeTime < wakeThresh)
            {
                sb.Draw(mesFil, hud.scaledRect(pos, mesFil.Bounds.Width, mesFil.Bounds.Height), null, Color.White, 0, new Vector2(mesFil.Bounds.Width / 2, mesFil.Bounds.Height / 2), SpriteEffects.None, 0);
                sb.Draw(mesOut, hud.scaledRect(pos, mesFil.Bounds.Width, mesFil.Bounds.Height), null, Color.White, 0, new Vector2(mesFil.Bounds.Width / 2, mesFil.Bounds.Height / 2), SpriteEffects.None, 0);

                sb.DrawString(font, message, hud.scaledCoords(pos), Color.White, 0,
                        new Vector2(font.MeasureString(message).X / 2, font.MeasureString(message).Y / 2), hud.scale * (20.0f / 40), SpriteEffects.None, 0);
            }
        }

    }
}
