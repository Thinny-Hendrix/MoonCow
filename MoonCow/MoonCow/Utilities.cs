using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MoonCow
{
    public static class Utilities
    {
        public static float deltaTime;
        public static bool paused = false;
        public static Random random = new Random();

        public static void Update(GameTime gameTime)
        {
            deltaTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000;

            //System.Diagnostics.Debug.WriteLine(deltaTime);

            if(Keyboard.GetState().IsKeyDown(Keys.P))
                paused = !paused;
        } 
    }
}
