using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    class JunkShipModel:BasicModel
    {
        JunkShip junkShip;
        Game1 game;
        float swayTime;
        public JunkShipModel(JunkShip junkShip, Game1 game):base()
        {
            this.junkShip = junkShip;
            this.game = game;
            pos = junkShip.pos;
            pos.Y -= 8;
            model = ModelLibrary.swarmer;

            rot.Y = (float)Math.Atan2(junkShip.dir.X, junkShip.dir.Z);
        }

        public override void Update(GameTime gameTime)
        {
            //z rot sways on sin wave
        }
    }
}
