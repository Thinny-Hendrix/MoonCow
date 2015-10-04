using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    public class HudWeapon:HudComponent
    {
        Texture2D hudWepB;
        Texture2D hudWepF;

        Vector2 wepPos;
        Vector2 wepAmmoPos;
        WeaponSystem wepSys;

        string weaponAmmo;
        string level;
        string exp;

        public HudWeapon(Hud hud, SpriteFont font, Game1 game)
            : base(hud, font, game)
        {
            wepSys = game.ship.weapons;
            wakeThresh = 5;
            wepPos = new Vector2(45, 45);
            wepAmmoPos = new Vector2(325, 90);

            hudWepF = game.Content.Load<Texture2D>(@"Hud/hudWepF");
            hudWepB = game.Content.Load<Texture2D>(@"Hud/hudWepB");
        }

        public override void Update()
        {
            weaponAmmo = wepSys.activeWeapon.formattedAmmo();
            level = wepSys.activeWeapon.formattedLevel();
            base.Update();

            exp = "" + (wepSys.activeWeapon.exp / wepSys.activeWeapon.EXPMAX)*100 + "%";
        }

        public override void Draw(SpriteBatch sb)
        {
            if (wakeTime < wakeThresh)
            {
                game.GraphicsDevice.BlendState = BlendState.Additive;
                sb.Draw(hudWepB, hud.scaledRect(wepPos, 425, 151), Color.White);
                sb.Draw(hudWepF, hud.scaledRect(wepPos, 425, 151), Color.White);
                sb.Draw(game.ship.weapons.activeWeapon.icon, hud.scaledRect(new Vector2(140, 115), 90, 90),
                        null, Color.White, 0, new Vector2(45, 45), SpriteEffects.None, 0);

                sb.DrawString(font, weaponAmmo, hud.scaledCoords(wepAmmoPos), Color.White, 0,
                    new Vector2(font.MeasureString(weaponAmmo).X / 2, font.MeasureString(weaponAmmo).Y / 2), hud.scale* 24.0f/40, SpriteEffects.None, 0);

                //exp
                sb.DrawString(font, level, hud.scaledCoords(225,125), Color.White, 0,
                    new Vector2(0, font.MeasureString(level).Y / 2), hud.scale * 16.0f / 40, SpriteEffects.None, 0);

                sb.DrawString(font, exp, hud.scaledCoords(300, 125), Color.White, 0,
                    new Vector2(0, font.MeasureString(exp).Y / 2), hud.scale * 16.0f / 40, SpriteEffects.None, 0);

            }
        }
    }
}
