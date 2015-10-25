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
    class LevelMenu : DrawableGameComponent
    {
        Game1 game;
        public List<MenuButton> buttons;
        //start game, level editor, options, exit game
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

        bool active;
        bool loading;

        public LevelMenu(Game1 game):base(game)
        {
            this.game = game;
            sb = new SpriteBatch(game.GraphicsDevice);
            buttons = new List<MenuButton>();
            campaignMaps = Directory.GetFiles(@"Content/MapXml/Campaign/", "*.xml");

            campaignLabel = new MenuButton("Campaign", new Vector2(150, 100));
            customLabel = new MenuButton("Custom Maps", new Vector2(1200, 100));

            int yPos = 250;

            foreach(string file in campaignMaps)
            {
                string levelName = file.Substring(24, file.Length - 28);
                buttons.Add(new MenuButton(levelName, new Vector2(200, yPos)));
                yPos += 50;
            }

            activeButton = 0;
            currentButton = buttons.ElementAt(activeButton);
            currentButton.activate();
            scale = (float)game.GraphicsDevice.Viewport.Bounds.Width / 1920.0f;
            buttonSwitchCooldown = 0;
            holdTime = 0;
            cooldown = 40;
        }

        void confirm()
        {
            loading = true;
            game.runState = Game1.RunState.MainGame;
            game.initializeGame(@"" + campaignMaps[activeButton]);
            game.Components.Remove(this);
        }

        public override void Update(GameTime gameTime)
        {

            if(cooldown > 0)
            {
                cooldown--;
            }

            float stickY = GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y;

            if ((Keyboard.GetState().IsKeyDown(Keys.Enter) || GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed) && cooldown <= 0)
            {
                confirm();
            }

            bool buttonPressed = false;

            if (buttonSwitchCooldown > 0)
                buttonSwitchCooldown -= Utilities.deltaTime;

            if (buttonSwitchCooldown <= 0)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Down) || stickY < -0.3f)
                {
                    buttonPressed = true;
                    currentButton.disable();
                    if (activeButton < buttons.Count() - 1)
                    {
                        activeButton++;
                    }
                    else
                    {
                        activeButton = 0;
                    }
                    currentButton = buttons.ElementAt(activeButton);
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
                        activeButton = buttons.Count()-1;
                    }
                    currentButton = buttons.ElementAt(activeButton);
                    currentButton.activate();
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
            foreach (MenuButton b in buttons)
            {
                b.Draw(sb);
            }
            campaignLabel.Draw(sb);
            customLabel.Draw(sb);
        }

        public override void Draw(GameTime gameTime)
        {
            game.GraphicsDevice.SetRenderTarget(null);
            sb.Begin();
            if (!loading)
                drawMenu();
            else
                drawLoading();
            
            sb.End();
        }
    }
}
