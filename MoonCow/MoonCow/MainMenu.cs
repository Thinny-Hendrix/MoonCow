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
        //start game, how to play, level editor, options, exit game
        MenuButton currentButton;
        float scale;
        SpriteBatch sb;
        int activeButton;
        float buttonSwitchCooldown;
        float holdTime;

        bool active;
        bool loading;
        bool tutorial;

        float time;

        MenuCircle menuCircle;

        public MainMenu(Game1 game):base(game)
        {
            this.game = game;
            tutorial = false;
            sb = new SpriteBatch(game.GraphicsDevice);
            buttons = new List<MenuButton>();

            //buttons.Add(new MenuButton("start game", new Vector2(200, 300)));
            buttons.Add(new MenuButton("Select level", new Vector2(155, 800), 0, 38));
            buttons.Add(new MenuButton("How to Play", new Vector2(190, 860), 0, 38));
            buttons.Add(new MenuButton("level editor", new Vector2(225, 920), 0, 38));
            buttons.Add(new MenuButton("exit game", new Vector2(260, 980), 0, 38));
            //buttons.Add(new MenuButton("options", new Vector2(200, 500)));

            activeButton = 0;
            currentButton = buttons.ElementAt(activeButton);
            currentButton.activate();
            scale = (float)game.GraphicsDevice.Viewport.Bounds.Width / 1920.0f;
            buttonSwitchCooldown = 0;
            holdTime = 0;
            menuCircle = new MenuCircle();
            time = 0;

            foreach (MenuButton b in buttons)
            {
                b.push(true,true,true);
            }
        }

        void confirm()
        {
            switch(activeButton)
            {                  
                case 0:
                    // level select
                    game.runState = Game1.RunState.LevelSelect;
                    game.initalizeLevelMenu();
                    game.Components.Remove(this);
                    break;
                case 1:
                    tutorial = true;
                    break;
                case 2:
                    loading = true;
                    game.runState = Game1.RunState.LevelCreator;
                    game.initializeLevelCreator();
                    game.Components.Remove(this);
                    break;
                case 3:
                    game.Exit();
                    break;
                default:
                    loading = true;
                    game.runState = Game1.RunState.MainGame;
                    game.initializeGame(@"Content/MapXml/Campaign/Level1.xml");
                    game.Components.Remove(this);
                    break;
            }
        }

        void updateBg()
        {
            MenuAssets.updateLinePos();
        }

        public override void Update(GameTime gameTime)
        {

            float stickY = GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y;

            if (Keyboard.GetState().IsKeyDown(Keys.Enter) || GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed)
            {
                if (!tutorial)
                {
                    confirm();
                    game.audioManager.addSoundEffect(AudioLibrary.select, 0.1f);
                }
            }

            if(tutorial && (Keyboard.GetState().IsKeyDown(Keys.Escape) || GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.B)))
            {
                tutorial = false;
                game.audioManager.addSoundEffect(AudioLibrary.back, 0.1f);
            }

            bool buttonPressed = false;

            if (buttonSwitchCooldown > 0)
            {
                buttonSwitchCooldown -= Utilities.deltaTime;
            }
            if (!tutorial && buttonSwitchCooldown <= 0)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Down) || stickY < -0.3f || GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.DPadDown))
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
                    menuCircle.downHit();
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
                        activeButton = buttons.Count()-1;
                    }
                    currentButton = buttons.ElementAt(activeButton);
                    currentButton.activate();
                    menuCircle.upHit();
                    game.audioManager.addSoundEffect(AudioLibrary.hover, 0.1f);
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

            foreach(MenuButton b in buttons)
            {
                b.Update();
            }

            updateBg();
            menuCircle.Update();

            if(time != 1)
            {
                time += Utilities.deltaTime;
                if (time > 1)
                    time = 1;
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
        }

        public override void Draw(GameTime gameTime)
        {
            game.GraphicsDevice.SetRenderTarget(null);
            sb.Begin();
            if (!tutorial)
            {
                if (!loading)
                {
                    MenuAssets.drawMenuBackground(sb);
                    menuCircle.Draw(sb);
                    sb.Draw(MenuAssets.logo, Utilities.scaledRect(new Vector2(40, 40), 1138, 297), Color.White * MathHelper.SmoothStep(0,1,time));
                    sb.Draw(MenuAssets.border, Utilities.scaledRect(new Vector2(40, 550), 900, 500), Color.White * MathHelper.SmoothStep(0, 1, time));
                    drawMenu();
                }
                else
                {
                    drawLoading();
                }
            }
            else
            {
                MenuAssets.drawMenuBackground(sb);
                //draw the tutorial.
            }
            
            sb.End();
        }

    }
}
