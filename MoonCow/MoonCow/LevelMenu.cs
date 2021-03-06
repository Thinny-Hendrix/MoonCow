﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;	
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MoonCow
{
    public class LevelMenu : DrawableGameComponent
    {
        Game1 game;
        public List<MenuButton> campaignButtons;
        public List<MenuButton> customButtons;
        MenuButton currentButton;
        MenuButton campaignLabel;
        MenuButton customLabel;
        float scale;
        SpriteBatch sb;
        int activeButton;
        float buttonSwitchCooldown;
        float holdTime;
        float cooldown;
        string[] campaignMaps;
        string[] customMaps;
        bool campaign;

        bool active;
        bool loading;
        bool loadingDrawn;

        LsHeader lsHeader;

        public LevelMenu(Game1 game):base(game)
        {
            this.game = game;
            sb = new SpriteBatch(game.GraphicsDevice);
            campaignButtons = new List<MenuButton>();
            customButtons = new List<MenuButton>();
            campaignMaps = Directory.GetFiles(@"Content/MapXml/Campaign/", "*.xml");
            customMaps = Directory.GetFiles(@"Content/MapXml/Custom/", "*.xml");

            campaignLabel = new MenuButton("Campaign", new Vector2(560, 490), 0);
            customLabel = new MenuButton("Custom", new Vector2(1100, 490), 0);
            campaignLabel.activate();
            loadingDrawn = false;

            int yPos = 600;

            foreach(string file in campaignMaps)
            {
                string levelName = file.Substring(24, file.Length - 28);
                campaignButtons.Add(new MenuButton(levelName, new Vector2(960, yPos), 1, true));
                yPos += 50;
            }

            yPos = 600;

            foreach (string file in customMaps)
            {
                string levelName = file.Substring(22, file.Length - 26);
                customButtons.Add(new MenuButton(levelName, new Vector2(960, yPos), 1, false));
                yPos += 50;
            }

            activeButton = 0;
            currentButton = campaignButtons.ElementAt(activeButton);
            currentButton.activate();
            scale = (float)game.GraphicsDevice.Viewport.Bounds.Width / 1920.0f;
            buttonSwitchCooldown = 0;
            holdTime = 0;
            cooldown = 40;
            campaign = true;

            lsHeader = new LsHeader();
        }

        void confirm()
        {
            loading = true;
        }

        void loadMap()
        {
            if (campaign)
            {
                loading = true;
                game.runState = Game1.RunState.MainGame;
                game.initializeGame(@"" + campaignMaps[activeButton]);
                game.audioManager.shutup();
                game.audioManager.Initialize();
                game.Components.Remove(this);
            }
            else
            {
                loading = true;
                game.runState = Game1.RunState.MainGame;
                game.initializeGame(@"" + customMaps[activeButton]);
                game.audioManager.shutup();
                game.audioManager.Initialize();
                game.Components.Remove(this);
            }
        }

        public override void Update(GameTime gameTime)
        {
            if(loading && loadingDrawn)
            {
                loadMap();
            }

            if(cooldown > 0)
            {
                cooldown--;
            }

            float stickY = GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y;
            float stickX = GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X;

            if ((Keyboard.GetState().IsKeyDown(Keys.Enter) || GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed) && cooldown <= 0)
            {
                if(!loading)
                    game.audioManager.addSoundEffect(AudioLibrary.select, 0.1f);
                confirm();
            }

            bool buttonPressed = false;

            if (buttonSwitchCooldown > 0)
                buttonSwitchCooldown -= Utilities.deltaTime;

            if(buttonSwitchCooldown <= 0)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Left) || stickX < -0.3f || GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.DPadLeft))
                {
                    if(!campaign)
                    {
                        campaign = true;
                        activeButton = 0;
                        currentButton.disable();
                        currentButton = campaignButtons[activeButton];
                        currentButton.activate();
                        lsHeader.push(true);
                        customLabel.disable();
                        campaignLabel.activate();

                        foreach (MenuButton b in campaignButtons)
                        {
                            b.push(true, true, true);
                        }
                        foreach (MenuButton b in customButtons)
                        {
                            b.push(true, false, false);
                        }
                        game.audioManager.addSoundEffect(AudioLibrary.hover, 0.1f);
                    }
                }

                if (Keyboard.GetState().IsKeyDown(Keys.Right) || stickX > 0.3f || GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.DPadRight))
                {
                    if (campaign)
                    {
                        campaign = false;
                        activeButton = 0;
                        currentButton.disable();
                        currentButton = customButtons[activeButton];
                        currentButton.activate();
                        lsHeader.push(false);
                        customLabel.activate();
                        campaignLabel.disable();

                        foreach (MenuButton b in campaignButtons)
                        {
                            b.push(false, false, false);
                        }
                        foreach (MenuButton b in customButtons)
                        {
                            b.push(false, true, true);
                        }
                        game.audioManager.addSoundEffect(AudioLibrary.hover, 0.1f);
                    }
                }
            }

            if (buttonSwitchCooldown <= 0)
            {
                if (campaign)
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.Down) || stickY < -0.3f || GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.DPadDown))
                    {
                        buttonPressed = true;
                        currentButton.disable();
                        if (activeButton < campaignButtons.Count() - 1)
                        {
                            activeButton++;
                        }
                        else
                        {
                            activeButton = 0;
                        }
                        currentButton = campaignButtons.ElementAt(activeButton);
                        currentButton.activate();
                        game.audioManager.addSoundEffect(AudioLibrary.hover, 0.1f);
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.Up) || stickY > 0.3f || GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.DPadUp))
                    {
                        buttonPressed = true;
                        currentButton.disable();
                        if (activeButton > 0)
                        {
                            activeButton--;
                        }
                        else
                        {
                            activeButton = campaignButtons.Count() - 1;
                        }
                        currentButton = campaignButtons.ElementAt(activeButton);
                        currentButton.activate();
                        game.audioManager.addSoundEffect(AudioLibrary.hover, 0.1f);
                    }
                }
                else
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.Down) || stickY < -0.3f || GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.DPadDown))
                    {
                        buttonPressed = true;
                        currentButton.disable();
                        if (activeButton < customButtons.Count() - 1)
                        {
                            activeButton++;
                        }
                        else
                        {
                            activeButton = 0;
                        }
                        currentButton = customButtons.ElementAt(activeButton);
                        currentButton.activate();
                        game.audioManager.addSoundEffect(AudioLibrary.hover, 0.1f);
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.Up) || stickY > 0.3f || GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.DPadUp))
                    {
                        buttonPressed = true;
                        currentButton.disable();
                        if (activeButton > 0)
                        {
                            activeButton--;
                        }
                        else
                        {
                            activeButton = customButtons.Count() - 1;
                        }
                        currentButton = customButtons.ElementAt(activeButton);
                        currentButton.activate();
                        game.audioManager.addSoundEffect(AudioLibrary.hover, 0.1f);
                    }
                }
            }


            if (buttonPressed)
            {
                if (holdTime < 0.2f)
                {
                    holdTime += Utilities.deltaTime;
                    buttonSwitchCooldown = 0.15f;
                }
                else
                {
                    buttonSwitchCooldown = 0.1f;
                }
            }
            else
            {
                holdTime = 0;
            }


            foreach (MenuButton b in campaignButtons)
            {
                b.Update();
            }
            foreach (MenuButton b in customButtons)
            {
                b.Update();
            }

            lsHeader.Update();
            MenuAssets.updateLinePos();

            
        }

        void drawLoading()
        {
            sb.Draw(MenuAssets.load, Utilities.scaledRect(new Vector2(950,720), 900,300), Color.White);
        }
        void drawMenu()
        {
            if (!loading)
            {
                MenuAssets.drawMenuBackground(sb);
                lsHeader.Draw(sb);

                foreach (MenuButton b in campaignButtons)
                {
                    b.Draw(sb);
                }
                foreach (MenuButton b in customButtons)
                {
                    b.Draw(sb);
                }
                campaignLabel.Draw(sb);
                customLabel.Draw(sb);
            }
            else
            {
                drawLoading();
            }
        }

        public override void Draw(GameTime gameTime)
        {
            game.GraphicsDevice.SetRenderTarget(null);
            sb.Begin();
            if (!loading)
            {
                drawMenu();
            }
            else
            {
                drawLoading();
                loadingDrawn = true;
            }
            
            sb.End();
        }
    }
}
