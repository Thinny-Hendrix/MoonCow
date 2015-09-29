using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MoonCow
{
    public class MainMenu:DrawableGameComponent
    {
        Game1 game;
        public List<MenuButton> buttons;
        //start game, level editor, options, exit game
        MenuButton currentButton;

        public MainMenu(Game1 game):base(game)
        {
            this.game = game;
            buttons = new List<MenuButton>();

            buttons.Add(new MenuButton("start game", new Vector2(20, 30)));

            buttons.ElementAt(0).active = true;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

    }
}
