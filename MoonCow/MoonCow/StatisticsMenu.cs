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
    public class StatisticsMenu : DrawableGameComponent
    {
        Game game;
        public StatisticsMenu(Game game):base(game)
        {
            this.game = game;
        }
    }
}
