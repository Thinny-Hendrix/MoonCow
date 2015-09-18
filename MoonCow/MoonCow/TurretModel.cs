using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    public class TurretModel:BasicModel
    {
        public Turret turret;
        public Game1 game;

        public TurretModel(Game1 game):base()
        {
            this.game = game;
            this.scale = new Vector3(1);
        }
        public TurretModel(Turret turret, Game1 game):base()
        {
            this.turret = turret;
            this.pos = turret.pos;
            this.game = game;
            this.scale = new Vector3(1);

            rot.Y = (float)Math.Atan2(turret.currentDir.X, turret.currentDir.Z) + MathHelper.PiOver2;
        }

        public virtual void fire()
        {

        }
    }
}
