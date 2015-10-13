using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MoonCow
{
    public class MainMenu:DrawableGameComponent
    {
        Game1 game;
        public List<MenuButton> buttons;
        //start game, level editor, options, exit game
        MenuButton currentButton;
        float scale;
        SpriteBatch sb;
        int activeButton;
        float buttonSwitchCooldown;
        float holdTime;

        bool active;

        public MainMenu(Game1 game):base(game)
        {
            this.game = game;
            sb = new SpriteBatch(game.GraphicsDevice);
            buttons = new List<MenuButton>();

            buttons.Add(new MenuButton("start game", new Vector2(200, 300)));
            buttons.Add(new MenuButton("Select level", new Vector2(200, 350)));
            buttons.Add(new MenuButton("level editor", new Vector2(200, 400)));
            buttons.Add(new MenuButton("options", new Vector2(200, 450)));
            buttons.Add(new MenuButton("exit game", new Vector2(200, 500)));


            activeButton = 0;
            currentButton = buttons.ElementAt(activeButton);
            currentButton.activate();
            scale = (float)game.GraphicsDevice.Viewport.Bounds.Width / 1920.0f;
            buttonSwitchCooldown = 0;
            holdTime = 0;
        }

        void confirm()
        {
            switch(activeButton)
            {
                default:
                    game.runState = Game1.RunState.MainGame;
                    game.initializeGame();
                    game.Components.Remove(this);
                    break;
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                confirm();
            }

            bool buttonPressed = false;

            if (buttonSwitchCooldown > 0)
                buttonSwitchCooldown -= Utilities.deltaTime;

            if (buttonSwitchCooldown <= 0)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Down))
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
                if (Keyboard.GetState().IsKeyDown(Keys.Up))
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

        public override void Draw(GameTime gameTime)
        {
            game.GraphicsDevice.SetRenderTarget(null);
            sb.Begin();
            foreach (MenuButton b in buttons)
            {
                b.Draw(sb);
            }
            sb.End();
        }

    }
}
