using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MoonCow
{
    class Camera : Microsoft.Xna.Framework.GameComponent
    {
        public Matrix viewMatrix { get; protected set; }
        public Matrix projectionMatrix { get; protected set; }

        public Camera(Game game, Vector3 pos, Vector3 target, Vector3 up) : base(game)
        {
            viewMatrix = Matrix.CreateLookAt(pos, target, up);
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4,
                (float)game.Window.ClientBounds.Width / (float)game.Window.ClientBounds.Height,
                1, 100);

        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
