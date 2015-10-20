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
            left = new DrillSpinEffect[5];
            right = new DrillSpinEffect[5];
            createSpins();
            
            foreach (DrillSpinEffect d in mid)
                game.modelManager.addEffect(d);
            foreach (DrillSpinEffect d in left)
                game.modelManager.addEffect(d);
            foreach (DrillSpinEffect d in right)
                game.modelManager.addEffect(d);
        }

        void createSpins()
        {
            mid[0] = new DrillSpinEffect(game, new Vector3(0, -0.03f,-.65f), 0.006f, 0);
            mid[1] = new DrillSpinEffect(game, new Vector3(0, -0.03f, -.55f), 0.010f, 1);
            mid[2] = new DrillSpinEffect(game, new Vector3(0, -0.03f, -.45f), 0.014f, 2);
            mid[3] = new DrillSpinEffect(game, new Vector3(0, -0.03f, -.35f), 0.018f, 3);
            mid[4] = new DrillSpinEffect(game, new Vector3(0, -0.03f, -.25f), 0.022f, 4);

            right[0] = new DrillSpinEffect(game, new Vector3(.4f, 0.01f, -.39f), 0.005f, 0);
            right[1] = new DrillSpinEffect(game, new Vector3(.4f, 0.01f, -.31f), 0.0065f, 1);
            right[2] = new DrillSpinEffect(game, new Vector3(.4f, 0.01f, -.23f), 0.009f, 2);
            right[3] = new DrillSpinEffect(game, new Vector3(.4f, 0.01f, -.15f), 0.0115f, 3);
            right[4] = new DrillSpinEffect(game, new Vector3(.4f, 0.01f, -.09f), 0.014f, 4);

            left[0] = new DrillSpinEffect(game, new Vector3(-.4f, 0.01f, -.39f), 0.005f, 0);
            left[1] = new DrillSpinEffect(game, new Vector3(-.4f, 0.01f, -.31f), 0.0065f, 1);
            left[2] = new DrillSpinEffect(game, new Vector3(-.4f, 0.01f, -.23f), 0.009f, 2);
            left[3] = new DrillSpinEffect(game, new Vector3(-.4f, 0.01f, -.15f), 0.0115f, 3);
            left[4] = new DrillSpinEffect(game, new Vector3(-.4f, 0.01f, -.09f), 0.014f, 4);
        }
    }
}
