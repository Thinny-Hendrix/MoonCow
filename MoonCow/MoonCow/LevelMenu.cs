using System;
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

        public LevelMenu(Game1 game):base(game)
        {
            this.game = game;
            sb = new SpriteBatch(game.GraphicsDevice);
            campaignButtons = new List<MenuButton>();
            customButtons = new List<MenuButton>();
            campaignMaps = Directory.GetFiles(@"Content/MapXml/Campaign/", "*.xml");
            customMaps = Directory.GetFiles(@"Content/MapXml/Custom/", "*.xml");

            campaignLabel = new MenuButton("Campaign", new Vector2(150, 100));
            customLabel = new MenuButton("Custom Maps", new Vector2(1200, 100));
            loadingDrawn = false;

            int yPos = 250;

            foreach(string file in campaignMaps)
            {
                string levelName = file.Substring(24, file.Length - 28);
                campaignButtons.Add(new MenuButton(levelName, new Vector2(200, yPos)));
                yPos += 50;
            }

            yPos = 250;

            foreach (string file in customMaps)
            {
                string levelName = file.Substring(22, file.Length - 26);
                customButtons.Add(new MenuButton(levelName, new Vector2(1250, yPos)));
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
            if(campaign)
            {
                customLabel.disable();
                campaignLabel.activate();
            }
            else
            {
                customLabel.activate();
                campaignLabel.disable();
            }

            float stickY = GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y;
            float stickX = GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X;

            if ((Keyboard.GetState().IsKeyDown(Keys.Enter) || GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed) && cooldown <= 0)
            {
                confirm();
            }

            bool buttonPressed = false;

            if (buttonSwitchCooldown > 0)
                buttonSwitchCooldown -= Utilities.deltaTime;

            if(buttonSwitchCooldown <= 0)
            {
                if(Keyboard.GetState().IsKeyDown(Keys.Left) || stickX < -0.3f)
                {
                    campaign = true;
                    activeButton = 0;
                    currentButton.disable();
                    currentButton = campaignButtons[activeButton];
                    currentButton.activate();
                }

                if (Keyboard.GetState().IsKeyDown(Keys.Right) || stickX > 0.3f)
                {
                    campaign = false;
                    activeButton = 0;
                    currentButton.disable();
                    currentButton = customButtons[activeButton];
                    currentButton.activate();
                }
            }

            if (buttonSwitchCooldown <= 0)
            {
                if (campaign)
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.Down) || stickY < -0.3f)
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
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.Up) || stickY > 0.3f)
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
                    }
                }
                else
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.Down) || stickY < -0.3f)
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
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.Up) || stickY > 0.3f)
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

            
        }

        void drawLoading()
        {
            sb.Draw(MenuAssets.pureWhite, Utilities.scaledRect(Utilities.scaledCoords(300,400),500*game.loadPercentage,40), Color.Red);
            sb.Draw(MenuAssets.load, Utilities.scaledCoords(300, 700), Color.White);
        }
        void drawMenu()
        {
            if (!loading)
            {
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
