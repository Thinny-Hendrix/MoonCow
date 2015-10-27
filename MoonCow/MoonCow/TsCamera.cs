using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MoonCow
{
    class TsCamera
    {
        public Matrix view { get; protected set; }
        public Matrix projection { get; protected set; }

        Vector3 pos;
        Vector3 look;
        Game1 game;

        public TsCamera(Game1 game)
        {
            this.game = game;
            pos = new Vector3(0,0,-10);
            look = new Vector3(0, 0, 10);
            CreateLookAt();
            projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.Pi / 3, 1, 1, 3000);
        }

        void CreateLookAt()
        {
            view = Matrix.CreateLookAt(pos, look, Vector3.Up);
        }
    }
}
