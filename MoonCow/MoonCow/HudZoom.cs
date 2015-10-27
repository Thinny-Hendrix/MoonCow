using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    public class HudZoom
    {
        bool active;
        float alpha;
        float scale;
        Hud hud;
        Game1 game;
        float time;
        public HudZoom(Hud hud, Game1 game)
        {
            this.hud = hud;
            this.game = game;
            alpha = 0;
        }

        public void activate(int type)
        {
            alpha = 0.5f;
            active = true;
            time = 0;
        }

        public void Update()
        {
            if(active)
            {
                time += Utilities.deltaTime*2;
                if(time >= 1)
                {
                    active = false;
                }
                scale = MathHelper.Lerp(.1f, 0, time);
                alpha = MathHelper.SmoothStep(0.5f, 0, time);
            }
        }

        public void Draw(SpriteBatch sb)
        {
            if (active)
            {
                sb.Draw((Texture2D)game.worldRender, hud.scaledRect(new Vector2(960, 540), 1920 * (1 - (scale * 2)), 1080 * (1 - (scale * 2))), null, Color.White * alpha, 0, hud.scaledCoords(960, 540), SpriteEffects.None, 0);
                sb.Draw((Texture2D)game.worldRender, hud.scaledRect(new Vector2(960, 540), 1920 * (1 - (scale)), 1080 * (1 - (scale))), null, Color.White * alpha, 0, hud.scaledCoords(960, 540), SpriteEffects.None, 0);
                sb.Draw((Texture2D)game.worldRender, hud.scaledRect(new Vector2(960, 540), 1920, 1080), null, Color.White * alpha, 0, hud.scaledCoords(960, 540), SpriteEffects.None, 0);
                sb.Draw((Texture2D)game.worldRender, hud.scaledRect(new Vector2(960, 540), 1920 * (1 + (scale)), 1080 * (1 + (scale))), null, Color.White * alpha, 0, hud.scaledCoords(960, 540), SpriteEffects.None, 0);
                sb.Draw((Texture2D)game.worldRender, hud.scaledRect(new Vector2(960, 540), 1920 * (1 + (scale * 2)), 1080 * (1 + (scale * 2))), null, Color.White * alpha, 0, hud.scaledCoords(960, 540), SpriteEffects.None, 0);
            }
        }
    }
}
