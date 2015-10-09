using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MoonCow
{
    public class QuickSelect
    {
        float yHold;
        const float HOLD_THRESH = 10;
        Hud hud;
        Game1 game;
        WeaponSystem wepSys;
        public bool active;
        float imgW;
        float imgH;

        float currentDam;
        float currentRof;
        float currentRan;

        Texture2D solidWhite;

        public Texture2D out4;
        public Texture2D out4_1;
        public Texture2D out4_2;
        public Texture2D out4_3;
        public Texture2D out4_4;

        Texture2D out5;
        Texture2D out5_1;
        Texture2D out5_2;
        Texture2D out5_3;
        Texture2D out5_4;
        Texture2D out5_5;

        public Texture2D fill4;
        Texture2D fill5;

        public Texture2D hi1;
        public Texture2D hi2;
        public Texture2D hi3;
        public Texture2D hi4;
        public Texture2D hi5;

        Texture2D currentOut;
        Texture2D currentFill;
        Texture2D currentHi;

        bool selecting;
        int selectedWep;
        enum QsWheel { Four, Five}
        QsWheel qsWheel = QsWheel.Five;

        SpriteFont font;

        public QuickSelect(Hud hud, Game1 game, SpriteFont font)
        {
            this.hud = hud;
            this.game = game;
            this.font = font;
            wepSys = game.ship.weapons;

            solidWhite = new Texture2D(game.GraphicsDevice, 1, 1);
            solidWhite.SetData(new Color[] { Color.White });

            out4 = game.Content.Load<Texture2D>(@"Hud/QuickSelect/qsOut4");
            out4_1 = game.Content.Load<Texture2D>(@"Hud/QuickSelect/qsOut4-1");
            out4_2 = game.Content.Load<Texture2D>(@"Hud/QuickSelect/qsOut4-2");
            out4_3 = game.Content.Load<Texture2D>(@"Hud/QuickSelect/qsOut4-3");
            out4_4 = game.Content.Load<Texture2D>(@"Hud/QuickSelect/qsOut4-4");

            out5 = game.Content.Load<Texture2D>(@"Hud/QuickSelect/qsOut5");
            out5_1 = game.Content.Load<Texture2D>(@"Hud/QuickSelect/qsOut5-1");
            out5_2 = game.Content.Load<Texture2D>(@"Hud/QuickSelect/qsOut5-2");
            out5_3 = game.Content.Load<Texture2D>(@"Hud/QuickSelect/qsOut5-3");
            out5_4 = game.Content.Load<Texture2D>(@"Hud/QuickSelect/qsOut5-4");
            out5_5 = game.Content.Load<Texture2D>(@"Hud/QuickSelect/qsOut5-5");

            fill4 = game.Content.Load<Texture2D>(@"Hud/QuickSelect/qsFill4");
            fill5 = game.Content.Load<Texture2D>(@"Hud/QuickSelect/qsFill5");

            hi1 = game.Content.Load<Texture2D>(@"Hud/QuickSelect/qsHi1");
            hi2 = game.Content.Load<Texture2D>(@"Hud/QuickSelect/qsHi2");
            hi3 = game.Content.Load<Texture2D>(@"Hud/QuickSelect/qsHi3");
            hi4 = game.Content.Load<Texture2D>(@"Hud/QuickSelect/qsHi4");
            hi5 = game.Content.Load<Texture2D>(@"Hud/QuickSelect/qsHi5");

            imgW = out4.Bounds.Width;
            imgH = out4.Bounds.Height;

            currentFill = fill4;
            currentOut = out4;
            currentHi = hi1;
        }

        public void Update()
        {
            if(!active)
            {
                if (!hud.expSelect.active && !hud.turSelect.active && !game.minigame.active)
                {
                    if (GamePad.GetState(PlayerIndex.One).Buttons.Y == ButtonState.Pressed)
                    {
                        yHold += Utilities.deltaTime * 60;
                        if (yHold > HOLD_THRESH)
                        {
                            active = true;
                            Utilities.softPaused = true;
                        }
                    }
                }
            }
            if(active)
            {
                hud.wakeAll();
                Vector2 coords = GamePad.GetState(PlayerIndex.One).ThumbSticks.Left;
                float angle = (float)Math.Atan2(coords.Y, coords.X);
                if (!selecting && angle != 0)
                    selecting = true;

                if(angle > MathHelper.PiOver2)
                    selectedWep = 0;

                if (angle > 0 && angle < MathHelper.PiOver2)
                    selectedWep = 2;

                if (qsWheel == QsWheel.Five)
                {
                    if (angle < -(MathHelper.Pi / 3) * 2)
                        selectedWep = 1;
                    if (angle < 0 && angle > -MathHelper.Pi / 3)
                        selectedWep = 3;

                    if (angle < -MathHelper.Pi / 3 && angle > -(MathHelper.Pi / 3) * 2)
                        selectedWep = 4;
                }
                else
                {
                    if (angle < -MathHelper.PiOver2)
                        selectedWep = 1;

                    if (angle < 0 && angle > -MathHelper.PiOver2)
                        selectedWep = 3;
                }

                changeImgs();

                if (GamePad.GetState(PlayerIndex.One).Buttons.Y == ButtonState.Released)
                {
                    yHold = 0;
                    Utilities.softPaused = false;
                    active = false;
                    selecting = false;
                    wepSys.changeWeapons(selectedWep);
                    currentDam = 0;
                    currentRof = 0;
                    currentRan = 0;
                }

            }
        }

        void changeWheel(int i)
        {
            qsWheel = (QsWheel)i;
        }

        void changeImgs()
        {
            switch (qsWheel)
            {
                default:
                    currentFill = fill4;
                    if (selecting)
                    {
                        switch (selectedWep)
                        {
                            default:
                                currentOut = out4_1;
                                currentHi = hi1;
                                break;
                            case 1:
                                currentOut = out4_2;
                                currentHi = hi2;
                                break;
                            case 2:
                                currentOut = out4_3;
                                currentHi = hi3;
                                break;
                            case 3:
                                currentOut = out4_4;
                                currentHi = hi4;
                                break;
                        }
                    }
                    else
                        currentOut = out4;
                    break;

                case QsWheel.Five:
                    currentFill = fill5;
                    if (selecting)
                    {
                        switch (selectedWep)
                        {
                            default:
                                currentOut = out5_1;
                                currentHi = hi1;
                                break;
                            case 1:
                                currentOut = out5_2;
                                currentHi = hi2;
                                break;
                            case 2:
                                currentOut = out5_3;
                                currentHi = hi3;
                                break;
                            case 3:
                                currentOut = out5_4;
                                currentHi = hi4;
                                break;
                            case 4:
                                currentOut = out5_5;
                                currentHi = hi5;
                                break;
                        }
                    }
                    else
                        currentOut = out5;
                    break;                
            }
        }

        void drawStats(SpriteBatch sb)
        {
            Weapon wep = wepSys.weapons.ElementAt(selectedWep);
            string name = wep.name;
            string ammo = wep.formattedAmmo();
            string level = wep.formattedLevel();

            currentDam = MathHelper.Lerp(currentDam, wep.damage, Utilities.deltaTime * 8);
            currentRof = MathHelper.Lerp(currentRof, wep.rateOfFire, Utilities.deltaTime*8);
            currentRan = MathHelper.Lerp(currentRan, wep.range, Utilities.deltaTime*8);

            Color c;
            if (wep.ammo > 0)
                c = hud.contSecondary;
            else
                c = hud.redBody;

            sb.DrawString(font, name, hud.scaledCoords(960, 325), Color.White, 0,
                new Vector2(font.MeasureString(name).X / 2, font.MeasureString(name).Y / 2), hud.scale*22.0f/40, SpriteEffects.None, 0);
            sb.DrawString(font, ammo, hud.scaledCoords(960, 350), c, 0,
                new Vector2(font.MeasureString(ammo).X / 2, font.MeasureString(ammo).Y / 2), hud.scale * 18.0f / 40, SpriteEffects.None, 0);

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
            sb.Draw(solidWhite, hud.scaledRect(new Vector2(963, 379), 150, 16),
                    null, hud.blueBody, 0, Vector2.Zero, SpriteEffects.None, 0);
            sb.Draw(solidWhite, hud.scaledRect(new Vector2(963, 398), 150, 16),
                    null, hud.blueBody, 0, Vector2.Zero, SpriteEffects.None, 0);
            sb.Draw(solidWhite, hud.scaledRect(new Vector2(963, 417), 150, 16),
                    null, hud.blueBody, 0, Vector2.Zero, SpriteEffects.None, 0);

            //rect fills
            sb.Draw(solidWhite, hud.scaledRect(new Vector2(963, 379), 150 * currentDam, 16),
                    null, hud.contSecondary, 0, Vector2.Zero, SpriteEffects.None, 1);
            sb.Draw(solidWhite, hud.scaledRect(new Vector2(963, 398), 150 * currentRof, 16),
                    null, hud.contSecondary, 0, Vector2.Zero, SpriteEffects.None, 1);
            sb.Draw(solidWhite, hud.scaledRect(new Vector2(963, 417), 150 * currentRan, 16),
                    null, hud.contSecondary, 0, Vector2.Zero, SpriteEffects.None, 1);

        }
        public void Draw(SpriteBatch sb)
        {
            if (active)
            {
                sb.Begin();
                sb.Draw(currentFill, hud.scaledRect(new Vector2(960, 540), imgW, imgH),
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

                if(qsWheel == QsWheel.Five)
                    sb.Draw(TextureManager.icoDrill, hud.scaledRect(new Vector2(960, 700), 90, 90),
                    null, Color.White, 0, new Vector2(45, 45), SpriteEffects.None, 0);

                if(selecting)
                    drawStats(sb);

                sb.End();
            }
        }
    }
}
