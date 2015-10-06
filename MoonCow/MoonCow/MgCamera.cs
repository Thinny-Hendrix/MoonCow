using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MoonCow
{
    public class MgCamera
    {
        public Matrix view { get; protected set; }
        public Matrix projection { get; protected set; }

        Vector3 pos;
        Vector3 look;
        Game1 game;

        public MgCamera(Game1 game)
        {
            this.game = game;
            pos = Vector3.Zero;
            look = new Vector3(0, 0, 10);
            CreateLookAt();
            projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.Pi / 3, 4.0f/3, 1, 3000);
        }

        void CreateLookAt()
        {
            view = Matrix.CreateLookAt(pos, look, Vector3.Up);
        }
    }
}
