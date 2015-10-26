using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MoonCow
{
    public class ExpSelect
    {
        float imgW;
        float imgH;
        public bool active;
        Hud hud;
        Game1 game;
        WeaponSystem wepSys;
        SpriteFont font;
        QuickSelect qs;
        Minigame mg;

        Texture2D currentHi;
        Texture2D currentOut;
        Texture2D helpO;
        Texture2D helpF;
        bool selecting;
        int selectedWep;

        float currentDam;
        float currentRof;
        float currentRan;

        RenderTarget2D targ1;
        RenderTarget2D targ2;
        SpriteBatch sb;
        Effect alphaMap;

        public ExpSelect(Hud hud, Game1 game, SpriteFont font)
        {
            this.hud = hud;
            this.game = game;
            this.font = font;
            qs = hud.quickSelect;
            wepSys = game.ship.weapons;

            imgW = qs.out4.Bounds.Width;
            imgH = qs.out4.Bounds.Height;

            currentHi = hud.quickSelect.hi1;

            helpO = game.Content.Load<Texture2D>(@"Hud/qhelpO");
            helpF = game.Content.Load<Texture2D>(@"Hud/qhelpF");

            targ1 = new RenderTarget2D(game.GraphicsDevice, 123, 15);
            targ2 = new RenderTarget2D(game.GraphicsDevice, 123, 15);
            sb = new SpriteBatch(game.GraphicsDevice);
            alphaMap = TextureManager.alphaMap;
        }

        public void setMinigame(Minigame mg)
        {
            this.mg = mg;
        }

        public void activate()
        {
            active = true;
            Utilities.softPaused = true;
        }

        void drawBar()
        {

            Weapon wep = wepSys.weapons.ElementAt(selectedWep);
            float scale = 1;
            if (wep.level != 3)
                scale = wep.exp / wep.EXPMAX;
            game.GraphicsDevice.SetRenderTarget(targ1);
            game.GraphicsDevice.Clear(Color.Transparent);
            if (selecting)
            {
                sb.Begin();
                sb.Draw(TextureManager.pureWhite, new Rectangle(0, 0, (int)Math.Ceiling(123 * scale), 15), null, hud.redBody, 0, Vector2.Zero, SpriteEffects.None, 1);
                sb.End();

                game.GraphicsDevice.SetRenderTarget(targ2);
                game.GraphicsDevice.Clear(Color.Transparent);
                alphaMap.Parameters["MaskTexture"].SetValue(qs.barMask);
                // start a spritebatch for our effect
                sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend,
                    null, null, null, alphaMap);
                //spB.Begin();
                sb.Draw((Texture2D)targ1, Vector2.Zero, Color.White);

                sb.End();
            }
            game.GraphicsDevice.SetRenderTarget(null);
        }

        public void Update()
        {
            if (!active)
            {
            }
            if (active)
            {
                Vector2 coords = GamePad.GetState(PlayerIndex.One).ThumbSticks.Left;
                float angle = (float)Math.Atan2(coords.Y, coords.X);
                if (!selecting && angle != 0)
                    selecting = true;

                if (angle > MathHelper.PiOver2 || Keyboard.GetState().IsKeyDown(Keys.D1))
                {
                    selectedWep = 0;
                    selecting = true;
                }

                if (angle > 0 && angle < MathHelper.PiOver2 || Keyboard.GetState().IsKeyDown(Keys.D2))
                {
                    selectedWep = 2;
                    selecting = true;
                }

                if (angle < -MathHelper.PiOver2 || Keyboard.GetState().IsKeyDown(Keys.D3))
                {
                    selectedWep = 1;
                    selecting = true;
                }

                if (angle < 0 && angle > -MathHelper.PiOver2 || Keyboard.GetState().IsKeyDown(Keys.D4))
                {
                    selectedWep = 3;
                    selecting = true;
                }
                drawBar();
            }

            changeImgs();

            if (selecting)
            {
                if (GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Enter))
                {
                    //yHold = 0;
                    Utilities.softPaused = false;
                    active = false;
                    selecting = false;
                    wepSys.addExp(selectedWep, 200);
                    mg.abort();
                    //currentDam = 0;
                    //currentRof = 0;
                    //currentRan = 0;
                }
            }

        }

        void changeImgs()
        {
            if (selecting)
            {
                switch (selectedWep)
                {
                    default:
                        currentOut = qs.out4_1;
                        currentHi = qs.hi1;
                        break;
                    case 1:
                        currentOut = qs.out4_2;
                        currentHi = qs.hi2;
                        break;
                    case 2:
                        currentOut = qs.out4_3;
                        currentHi = qs.hi3;
                        break;
                    case 3:
                        currentOut = qs.out4_4;
                        currentHi = qs.hi4;
                        break;
                }
            }
            else
                currentOut = qs.out4;
        }

        void drawStats(SpriteBatch sb)
        {
            Weapon wep = wepSys.weapons.ElementAt(selectedWep);
            string name = wep.name;
            string ammo = wep.formattedAmmo();
            string level = wep.formattedLevel();

            currentDam = MathHelper.Lerp(currentDam, wep.damage, Utilities.deltaTime * 8);
            currentRof = MathHelper.Lerp(currentRof, wep.rateOfFire, Utilities.deltaTime * 8);
            currentRan = MathHelper.Lerp(currentRan, wep.range, Utilities.deltaTime * 8);

            Color c;
            if (wep.ammo > 0)
                c = hud.contSecondary;
            else
                c = hud.redBody;

                sb.DrawString(font, name, hud.scaledCoords(960, 325), Color.White, 0,
                    new Vector2(font.MeasureString(name).X / 2, font.MeasureString(name).Y / 2), hud.scale * 22.0f / 40, SpriteEffects.None, 0);

            if (wep.name.Contains("ster"))
            {
                sb.Draw(TextureManager.infinity, hud.scaledRect(new Vector2(960, 347), 41, 18), null, hud.contSecondary, 0, new Vector2(41, 18), SpriteEffects.None, 0);
            }
            else
            {
                sb.DrawString(font, ammo, hud.scaledCoords(960, 350), c, 0,
                    new Vector2(font.MeasureString(ammo).X / 2, font.MeasureString(ammo).Y / 2), hud.scale * 18.0f / 40, SpriteEffects.None, 0);
            }

            //level
            sb.DrawString(font, level, hud.scaledCoords(960, 500), hud.contSecondary, 0,
                new Vector2(font.MeasureString(level).X / 2, font.MeasureString(level).Y / 2), hud.scale * 20.0f / 40, SpriteEffects.None, 0);
            //stencil'd exp bar

            //statistics
            sb.DrawString(font, "Damage", hud.scaledCoords(957, 390), Color.White, 0,
                new Vector2(font.MeasureString("Damage").X, font.MeasureString("Damage").Y / 2), hud.scale * 14.0f / 40, SpriteEffects.None, 0);
            sb.DrawString(font, "rate of fire", hud.scaledCoords(957, 409), Color.White, 0,
                new Vector2(font.MeasureString("rate of fire").X, font.MeasureString("rate of fire").Y / 2), hud.scale * 14.0f / 40, SpriteEffects.None, 0);
            sb.DrawString(font, "range", hud.scaledCoords(957, 428), Color.White, 0,
                new Vector2(font.MeasureString("range").X, font.MeasureString("range").Y / 2), hud.scale * 14.0f / 40, SpriteEffects.None, 0);

            //rect backs
            sb.Draw(TextureManager.pureWhite, hud.scaledRect(new Vector2(963, 379), 150, 16),
                    null, hud.blueBody, 0, Vector2.Zero, SpriteEffects.None, 0);
            sb.Draw(TextureManager.pureWhite, hud.scaledRect(new Vector2(963, 398), 150, 16),
                    null, hud.blueBody, 0, Vector2.Zero, SpriteEffects.None, 0);
            sb.Draw(TextureManager.pureWhite, hud.scaledRect(new Vector2(963, 417), 150, 16),
                    null, hud.blueBody, 0, Vector2.Zero, SpriteEffects.None, 0);

            //rect fills
            sb.Draw(TextureManager.pureWhite, hud.scaledRect(new Vector2(963, 379), 150 * currentDam, 16),
                    null, hud.contSecondary, 0, Vector2.Zero, SpriteEffects.None, 1);
            sb.Draw(TextureManager.pureWhite, hud.scaledRect(new Vector2(963, 398), 150 * currentRof, 16),
                    null, hud.contSecondary, 0, Vector2.Zero, SpriteEffects.None, 1);
            sb.Draw(TextureManager.pureWhite, hud.scaledRect(new Vector2(963, 417), 150 * currentRan, 16),
                    null, hud.contSecondary, 0, Vector2.Zero, SpriteEffects.None, 1);

            sb.Draw((Texture2D)targ2, hud.scaledRect(new Vector2(960, 455), 123, 15), null, Color.White, 0, new Vector2(61, 0), SpriteEffects.None, 0);


        }
        public void Draw(SpriteBatch sb)
        {
            if (active)
            {
                sb.Begin();
                sb.Draw(qs.fill4, hud.scaledRect(new Vector2(960, 540), imgW, imgH),
                    null, Color.White, 0, new Vector2(imgW / 2, imgH / 2), SpriteEffects.None, 0);

                if (selecting)
                {
                    sb.Draw(currentHi, hud.scaledRect(new Vector2(960, 540), imgW, imgH),
                        null, Color.White, 0, new Vector2(imgW / 2, imgH / 2), SpriteEffects.None, 0);
                }

                sb.Draw(currentOut, hud.scaledRect(new Vector2(960, 540), imgW, imgH),
                    null, Color.White, 0, new Vector2(imgW / 2, imgH / 2), SpriteEffects.None, 0);

                sb.Draw(TextureManager.icoAst, hud.scaledRect(new Vector2(850, 515), 90, 90),
                    null, Color.White, 0, new Vector2(45, 45), SpriteEffects.None, 0);
                sb.Draw(TextureManager.icoWave, hud.scaledRect(new Vector2(850, 650), 90, 90),
                    null, Color.White, 0, new Vector2(45, 45), SpriteEffects.None, 0);
                sb.Draw(TextureManager.icoPew, hud.scaledRect(new Vector2(1070, 515), 90, 90),
                    null, Color.White, 0, new Vector2(45, 45), SpriteEffects.None, 0);
                sb.Draw(TextureManager.icoBomb, hud.scaledRect(new Vector2(1070, 650), 90, 90),
                    null, Color.White, 0, new Vector2(45, 45), SpriteEffects.None, 0);

                if (selecting)
                    drawStats(sb);

                /*
                sb.Draw(hud.butA, hud.scaledRect(new Vector2(1000, 750), 65, 65), null, Color.White, 0, new Vector2(30, 30), SpriteEffects.None, 0);
                sb.DrawString(font, "confirm", hud.scaledCoords(960, 750), Color.White, 0,
                new Vector2(font.MeasureString("confirm").X, font.MeasureString("confirm").Y / 2), hud.scale * 22.0f / 40, SpriteEffects.None, 0);
                */
                sb.Draw(helpF, hud.scaledRect(new Vector2(960, 230), 514, 139), null, Color.White, 0, new Vector2(257, 69), SpriteEffects.None, 0);
                sb.Draw(helpO, hud.scaledRect(new Vector2(960,230),514,139), null, Color.White, 0, new Vector2(257,69), SpriteEffects.None, 0);
                sb.DrawString(font, "select a weapon", hud.scaledCoords(960, 200), Color.White, 0,
                new Vector2(font.MeasureString("select a weapon").X / 2, 0), hud.scale * 22.0f / 40, SpriteEffects.None, 0);
                
                sb.DrawString(font, "to give exp!", hud.scaledCoords(960, 240), Color.White, 0,
                new Vector2(font.MeasureString("to give exp!").X / 2, 0), hud.scale * 22.0f / 40, SpriteEffects.None, 0);

                sb.End();


            }
        }
    }
}

