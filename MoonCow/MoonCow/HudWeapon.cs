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
        Texture2D hudWepB2;
        Texture2D hudWepF2;

        Vector2 wepPos;
        Vector2 wepAmmoPos;
        WeaponSystem wepSys;

        string weaponAmmo;
        string level;
        string exp;

        RenderTarget2D barTarg;
        RenderTarget2D maskTarg;
        SpriteBatch sb;
        Texture2D mask;
        Effect alphaMap;

        public HudWeapon(Hud hud, SpriteFont font, Game1 game)
            : base(hud, font, game)
        {
            wepSys = game.ship.weapons;
            wakeThresh = 5;
            wepPos = new Vector2(45, 45);
            wepAmmoPos = new Vector2(325, 90);

            barTarg = new RenderTarget2D(game.GraphicsDevice, 180, 15);
            maskTarg = new RenderTarget2D(game.GraphicsDevice, 180, 15);
            sb = new SpriteBatch(game.GraphicsDevice);

            hudWepF = game.Content.Load<Texture2D>(@"Hud/hudWepF");
            hudWepB = game.Content.Load<Texture2D>(@"Hud/hudWepB");
            hudWepF2 = game.Content.Load<Texture2D>(@"Hud/hudWepO2");
            hudWepB2 = game.Content.Load<Texture2D>(@"Hud/hudWepF2");
            mask = game.Content.Load<Texture2D>(@"Hud/Masks/expBar");
            alphaMap = TextureManager.alphaMap;
        }

        public override void Update()
        {
            weaponAmmo = wepSys.activeWeapon.formattedAmmo();
            level = wepSys.activeWeapon.formattedLevel();
            base.Update();

            exp = "" + (wepSys.activeWeapon.exp / wepSys.activeWeapon.EXPMAX)*100 + "%";
            drawBar();
        }

        void drawBar()
        {
            float scale = 1;
            if(wepSys.activeWeapon.level != 3)
            {
                scale = wepSys.activeWeapon.exp / wepSys.activeWeapon.EXPMAX;
            }

            game.GraphicsDevice.SetRenderTarget(barTarg);
            game.GraphicsDevice.Clear(Color.Transparent);
            sb.Begin();
            sb.Draw(TextureManager.pureWhite, new Rectangle(0, 0, (int)Math.Ceiling(180 * scale), 15), null, hud.redBody, 0, Vector2.Zero, SpriteEffects.None, 1);
            sb.End();

            game.GraphicsDevice.SetRenderTarget(maskTarg);
            game.GraphicsDevice.Clear(Color.Transparent);
            alphaMap.Parameters["MaskTexture"].SetValue(mask);

            // start a spritebatch for our effect
            sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend,
                null, null, null, alphaMap);
            sb.Draw((Texture2D)barTarg, Vector2.Zero, Color.White);
            sb.End();

            game.GraphicsDevice.SetRenderTarget(null);
        }

        public override void Draw(SpriteBatch sb)
        {
            if (wakeTime < wakeThresh)
            {
                game.GraphicsDevice.BlendState = BlendState.Additive;
                if (wepSys.activeWeapon.name.Contains("rill"))
                {
                    sb.Draw(hudWepB2, hud.scaledRect(wepPos, 425, 151), Color.White);
                    sb.Draw(hudWepF2, hud.scaledRect(wepPos, 425, 151), Color.White);
                }
                else
                {
                    sb.Draw(hudWepB, hud.scaledRect(wepPos, 425, 151), Color.White);
                    sb.Draw(hudWepF, hud.scaledRect(wepPos, 425, 151), Color.White);
                    if (!wepSys.activeWeapon.name.Contains("steroid"))
                    {
                        sb.DrawString(font, weaponAmmo, hud.scaledCoords(wepAmmoPos), Color.White, 0,
                        new Vector2(font.MeasureString(weaponAmmo).X / 2, font.MeasureString(weaponAmmo).Y / 2), hud.scale * 24.0f / 40, SpriteEffects.None, 0);
                    }
                    else
                    {
                        sb.Draw(TextureManager.infinity, hud.scaledRect(wepAmmoPos, 59, 24), null, Color.White, 0, new Vector2(41,26), SpriteEffects.None, 0);
                    }

                    //exp
                    sb.DrawString(font, level, hud.scaledCoords(220, 130), hud.contSecondary, 0,
                        new Vector2(0, font.MeasureString(level).Y / 2), hud.scale * 16.0f / 40, SpriteEffects.None, 0);

                    sb.Draw((Texture2D)maskTarg, hud.scaledRect(wepPos+new Vector2(217,74),180,15),null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);

                    /*sb.DrawString(font, exp, hud.scaledCoords(295, 130), Color.White, 0,
                        new Vector2(0, font.MeasureString(exp).Y / 2), hud.scale * 16.0f / 40, SpriteEffects.None, 0);*/
                }
                sb.Draw(game.ship.weapons.activeWeapon.icon, hud.scaledRect(new Vector2(140, 115), 90, 90),
                        null, Color.White, 0, new Vector2(45, 45), SpriteEffects.None, 0);
            }
        }
    }
}
