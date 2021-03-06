﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MoonCow
{
    public class TurretSelect
    {
        float imgW;
        float imgH;
        public bool active;
        Hud hud;
        Game1 game;
        WeaponSystem wepSys;
        SpriteFont font;
        QuickSelect qs;
        TurretBase activeTurr;

        Texture2D out1;
        Texture2D out1_1;

        Texture2D out3;
        Texture2D out3_1;
        Texture2D out3_2;
        Texture2D out3_3;

        Texture2D fill1;
        Texture2D fill3;

        Texture2D hi1;
        Texture2D hi2;
        Texture2D hi3;

        Texture2D currentHi;
        Texture2D currentOut;
        bool selecting;
        int selectedWep;

        float currentDam;
        float currentRof;
        float currentRan;

        bool newTurr;

        RenderTarget2D rTarg;
        SpriteBatch sb;
        TsModelManager modelManager;

        List<float> damage = new List<float>(){0.3f,0.5f,0.9f};
        List<float> rof = new List<float>{0.8f,0.6f,0.2f};
        List<float> range = new List<float>{0.7f,0.7f,0.5f};

        public TurretSelect(Hud hud, Game1 game, SpriteFont font)
        {
            this.hud = hud;
            this.game = game;
            this.font = font;
            qs = hud.quickSelect;
            wepSys = game.ship.weapons;

            imgW = qs.out4.Bounds.Width;
            imgH = qs.out4.Bounds.Height;

            currentHi = hud.quickSelect.hi1;
            newTurr = true;

            loadImages();

            rTarg = new RenderTarget2D(game.GraphicsDevice, 512, 512);
            sb = new SpriteBatch(game.GraphicsDevice);
            modelManager = new TsModelManager(game);
        }

        void loadImages()
        {
            out1 = game.Content.Load<Texture2D>(@"Hud/TurretSelect/tsOut1");
            out1_1 = game.Content.Load<Texture2D>(@"Hud/TurretSelect/tsOut1-1");

            out3 = game.Content.Load<Texture2D>(@"Hud/TurretSelect/tsOut3");
            out3_1 = game.Content.Load<Texture2D>(@"Hud/TurretSelect/tsOut3-1");
            out3_2 = game.Content.Load<Texture2D>(@"Hud/TurretSelect/tsOut3-2");
            out3_3 = game.Content.Load<Texture2D>(@"Hud/TurretSelect/tsOut3-3");

            fill1 = game.Content.Load<Texture2D>(@"Hud/TurretSelect/tsFil1");
            fill3 = game.Content.Load<Texture2D>(@"Hud/TurretSelect/tsFil3");

            hi1 = game.Content.Load<Texture2D>(@"Hud/TurretSelect/tsHi1");
            hi2 = game.Content.Load<Texture2D>(@"Hud/TurretSelect/tsHi2");
            hi3 = game.Content.Load<Texture2D>(@"Hud/TurretSelect/tsHi3");

            imgW = out1.Bounds.Width;
            imgH = out1.Bounds.Height;
        }

        public void setBase(TurretBase turBase)
        {
            activeTurr = turBase;
        }

        public void activate(TurretBase turBase, int i)
        {
            activeTurr = turBase;

            if (i == 0) //base does not have turret
            {
                active = true;
                newTurr = true;
                Utilities.softPaused = true;
            }
            else //base does have turret
            {
                newTurr = false;
                active = true;
                Utilities.softPaused = true;
            }

            modelManager.changeVisible(-1);
        }

        public void Update()
        {
            if (!active)
            {
            }
            if (active)
            {
                hud.hudMoney.Wake();
                Vector2 coords = GamePad.GetState(PlayerIndex.One).ThumbSticks.Left;
                float angle = (float)Math.Atan2(coords.Y, coords.X);

                if (!selecting && angle != 0 && angle < 0)
                {
                    if ((newTurr && angle < 0) || (!newTurr && (angle < -MathHelper.Pi / 3 && angle > -(MathHelper.Pi / 3) * 2)))
                    selecting = true;
                }

                if (newTurr)
                {
                    if (angle < -(MathHelper.Pi / 3) * 2 || Keyboard.GetState().IsKeyDown(Keys.D1))
                    {
                        if (selectedWep != 1)
                            game.audioManager.addSoundEffect(AudioLibrary.hover, 0.1f);
                        selectedWep = 1;
                        selecting = true;
                        modelManager.changeVisible(0);
                    }

                    if ((angle < -MathHelper.Pi / 3 && angle > -(MathHelper.Pi / 3) * 2) || Keyboard.GetState().IsKeyDown(Keys.D2))
                    {
                        if (selectedWep != 2)
                            game.audioManager.addSoundEffect(AudioLibrary.hover, 0.1f);
                        selectedWep = 2;
                        selecting = true;
                        modelManager.changeVisible(1);
                    }

                    if ((angle < 0 && angle > -MathHelper.Pi / 3) || Keyboard.GetState().IsKeyDown(Keys.D3))
                    {
                        if (selectedWep != 3)
                            game.audioManager.addSoundEffect(AudioLibrary.hover, 0.1f);
                        selectedWep = 3;
                        selecting = true;
                        modelManager.changeVisible(2);
                    }
                }
                else
                {
                    if ((angle < -MathHelper.Pi / 3 && angle > -(MathHelper.Pi / 3) * 2) || Keyboard.GetState().IsKeyDown(Keys.D1))
                    {
                        if (selectedWep != 0)
                            game.audioManager.addSoundEffect(AudioLibrary.hover, 0.1f);
                        selectedWep = 0;
                        selecting = true;
                        modelManager.changeVisible(3);
                    }
                }

                if(Keyboard.GetState().IsKeyDown(Keys.Back) || GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.B))
                {
                    abort();
                }

                modelManager.Update();
                game.GraphicsDevice.SetRenderTarget(rTarg);
                game.GraphicsDevice.Clear(Color.Transparent);
                //game.GraphicsDevice.Clear(ClearOptions.DepthBuffer, Color.Transparent, 0, 0);
                modelManager.Draw();
                game.GraphicsDevice.SetRenderTarget(null);

            }

            changeImgs();

            if (selecting)
            {
                if (GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Enter))
                {
                    //yHold = 0;
                    if (activeTurr.setTurret(selectedWep))
                    {
                        game.audioManager.addSoundEffect(AudioLibrary.select, 0.1f);
                        Utilities.softPaused = false;
                        active = false;
                        selecting = false;
                    }
                    else
                    {
                        //play sound
                    }
                    //currentDam = 0;
                    //currentRof = 0;
                    //currentRan = 0;
                }
            }

        }

        void abort()
        {
            game.audioManager.addSoundEffect(AudioLibrary.back, 0.1f);
            active = false;
            Utilities.softPaused = false;
            selecting = false;
        }

        void changeImgs()
        {
            if (newTurr)
            {
                if (selecting)
                {
                    switch (selectedWep)
                    {
                        default:
                            currentOut = out3_1;
                            currentHi = hi1;
                            break;
                        case 2:
                            currentOut = out3_2;
                            currentHi = hi2;
                            break;
                        case 3:
                            currentOut = out3_3;
                            currentHi = hi3;
                            break;
                    }
                }
                else
                    currentOut = out3;
            }
            else
            {
                if (selecting)
                {
                    currentOut = out1_1;
                    currentHi = hi2;
                }
                else
                    currentOut = out1;
            }
        }

        void drawStats(SpriteBatch sb)
        {
            Weapon wep = wepSys.weapons.ElementAt(selectedWep);
            string name;
            float price;

            switch(selectedWep)
            {
                default:
                    name = "dismantle Turret";
                    price = 0;
                    break;
                case 1:
                    name = "gattle turret";
                    price = activeTurr.gattPrice;
                    break;
                case 2:
                    name = "pyro turret";
                    price = activeTurr.pyroPrice;
                    break;
                case 3:
                    name = "electro turret";
                    price = activeTurr.elecPrice;
                    break;

            }

            Color c;
            if (game.ship.moneyManager.checkPurchase(price))
                c = hud.contSecondary;
            else
                c = hud.redBody;

            sb.DrawString(font, name, hud.scaledCoords(960, 576), Color.White, 0,
                new Vector2(font.MeasureString(name).X / 2, font.MeasureString(name).Y / 2), hud.scale * 22.0f / 40, SpriteEffects.None, 0);
            sb.DrawString(font, "$" + price, hud.scaledCoords(960, 606), c, 0,
                new Vector2(font.MeasureString("$" + price).X / 2, font.MeasureString("$" + price).Y / 2), hud.scale * 18.0f / 40, SpriteEffects.None, 0);


            if (newTurr)
            {
                currentDam = MathHelper.Lerp(currentDam, damage.ElementAt(selectedWep-1), Utilities.deltaTime * 8);
                currentRof = MathHelper.Lerp(currentRof, rof.ElementAt(selectedWep-1), Utilities.deltaTime * 8);
                currentRan = MathHelper.Lerp(currentRan, range.ElementAt(selectedWep-1), Utilities.deltaTime * 8);


                //level

                //stencil'd exp bar

                //statistics
                sb.DrawString(font, "Damage", hud.scaledCoords(987, 651), Color.White, 0,
                    new Vector2(font.MeasureString("Damage").X, font.MeasureString("Damage").Y / 2), hud.scale * 14.0f / 40, SpriteEffects.None, 0);
                sb.DrawString(font, "rate of fire", hud.scaledCoords(987, 670), Color.White, 0,
                    new Vector2(font.MeasureString("rate of fire").X, font.MeasureString("rate of fire").Y / 2), hud.scale * 14.0f / 40, SpriteEffects.None, 0);
                sb.DrawString(font, "no. of targets", hud.scaledCoords(987, 689), Color.White, 0,
                    new Vector2(font.MeasureString("no. of targets").X, font.MeasureString("no. of targets").Y / 2), hud.scale * 14.0f / 40, SpriteEffects.None, 0);

                //rect backs
                sb.Draw(TextureManager.pureWhite, hud.scaledRect(new Vector2(993, 640), 150, 16),
                        null, hud.blueBody, 0, Vector2.Zero, SpriteEffects.None, 0);
                sb.Draw(TextureManager.pureWhite, hud.scaledRect(new Vector2(993, 659), 150, 16),
                        null, hud.blueBody, 0, Vector2.Zero, SpriteEffects.None, 0);
                sb.Draw(TextureManager.pureWhite, hud.scaledRect(new Vector2(993, 678), 150, 16),
                        null, hud.blueBody, 0, Vector2.Zero, SpriteEffects.None, 0);

                //rect fills
                sb.Draw(TextureManager.pureWhite, hud.scaledRect(new Vector2(993, 640), 150 * currentDam, 16),
                        null, hud.contSecondary, 0, Vector2.Zero, SpriteEffects.None, 1);
                sb.Draw(TextureManager.pureWhite, hud.scaledRect(new Vector2(993, 659), 150 * currentRof, 16),
                        null, hud.contSecondary, 0, Vector2.Zero, SpriteEffects.None, 1);
                sb.Draw(TextureManager.pureWhite, hud.scaledRect(new Vector2(993, 678), 150 * currentRan, 16),
                        null, hud.contSecondary, 0, Vector2.Zero, SpriteEffects.None, 1);
            }

        }
        public void Draw(SpriteBatch sb)
        {
            if (active)
            {
                sb.Begin();
                if (newTurr)
                {
                    sb.Draw(fill3, hud.scaledRect(new Vector2(960, 540), imgW, imgH),
                        null, Color.White, 0, new Vector2(imgW / 2, imgH / 2), SpriteEffects.None, 0);
                }
                else
                {
                    sb.Draw(fill1, hud.scaledRect(new Vector2(960, 540), imgW, imgH),
                        null, Color.White, 0, new Vector2(imgW / 2, imgH / 2), SpriteEffects.None, 0);
                }

                if (selecting)
                {
                    sb.Draw(currentHi, hud.scaledRect(new Vector2(960, 540), imgW, imgH),
                        null, Color.White, 0, new Vector2(imgW / 2, imgH / 2), SpriteEffects.None, 0);
                }

                sb.Draw(currentOut, hud.scaledRect(new Vector2(960, 540), imgW, imgH),
                    null, Color.White, 0, new Vector2(imgW / 2, imgH / 2), SpriteEffects.None, 0);

                if (!newTurr)
                {
                    sb.Draw(TextureManager.icoX, hud.scaledRect(new Vector2(960, 830), 90, 90),
                        null, Color.White, 0, new Vector2(45, 45), SpriteEffects.None, 0);
                }
                else
                {
                    sb.Draw(TextureManager.icoPew, hud.scaledRect(new Vector2(840, 780), 90, 90),
                        null, Color.White, 0, new Vector2(45, 45), SpriteEffects.None, 0);
                    sb.Draw(TextureManager.icoPyr, hud.scaledRect(new Vector2(960, 830), 90, 90),
                        null, Color.White, 0, new Vector2(45, 45), SpriteEffects.None, 0);
                    sb.Draw(TextureManager.icoEle, hud.scaledRect(new Vector2(1080, 780), 90, 90),
                        null, Color.White, 0, new Vector2(45, 45), SpriteEffects.None, 0);
                }

                if (selecting)
                    drawStats(sb);

                sb.Draw((Texture2D)rTarg, hud.scaledRect(new Vector2(960, 580), 512, 512), null, hud.contSecondary * (0.2f+Utilities.nextFloat()*0.5f), 0, new Vector2(256, 256), SpriteEffects.None, 0);

                sb.Draw(hud.butA, hud.scaledRect(new Vector2(910,940), 65, 65), null, Color.White, 0,new Vector2(30,30), SpriteEffects.None, 0);
                sb.DrawString(font, "confirm", hud.scaledCoords(870, 950), Color.White, 0,
                new Vector2(font.MeasureString("confirm").X, font.MeasureString("confirm").Y / 2), hud.scale * 22.0f / 40, SpriteEffects.None, 0);

                sb.Draw(hud.butB, hud.scaledRect(new Vector2(1200, 940), 65, 65), null, Color.White, 0, new Vector2(30, 30), SpriteEffects.None, 0);
                sb.DrawString(font, "cancel", hud.scaledCoords(1160, 950), Color.White, 0,
                new Vector2(font.MeasureString("cancel").X, font.MeasureString("cancel").Y / 2), hud.scale * 22.0f / 40, SpriteEffects.None, 0);
            

                sb.End();
            }
        }
    }
}
