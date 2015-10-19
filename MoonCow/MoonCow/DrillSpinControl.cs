using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    class DrillSpinControl
    {
        DrillSpinEffect[] mid;
        DrillSpinEffect[] left;
        DrillSpinEffect[] right;
        Game1 game;
        WeaponDrill drill;

        public DrillSpinControl(Game1 game, WeaponDrill drill)
        {
            this.game = game;
            this.drill = drill;
            mid = new DrillSpinEffect[5];
            createSpins();
            /*
            foreach (DrillSpinEffect d in mid)
                game.modelManager.addEffect(d);*/
        }

        void createSpins()
        {
            mid[0] = new DrillSpinEffect(game, new Vector3(-2, 0,1), 0.1f, 1);
            mid[1] = new DrillSpinEffect(game, new Vector3(-1.4f, 0, 1), 0.1f, 1);
            mid[2] = new DrillSpinEffect(game, new Vector3(-1.1f, 0, 1), 0.1f, 1);
            mid[3] = new DrillSpinEffect(game, new Vector3(-0.8f, 0, 1), 0.1f, 1);
            mid[4] = new DrillSpinEffect(game, new Vector3(-0.4f, 0, 1), 0.1f, 1);
        }
    }
}
