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
        public static TimeSpan elapsedTime = TimeSpan.Zero;
        public static int frameCount = 0;
        public static int fps;
        public static float deltaTime;
        public static bool paused = false;
        public static Random random = new Random();

        public static void Update(GameTime gameTime)
        {
            elapsedTime += gameTime.ElapsedGameTime;

            if (elapsedTime > TimeSpan.FromSeconds(1))
            {
                elapsedTime -= TimeSpan.FromSeconds(1);
                fps = frameCount;
                frameCount = 0;
            }

            if (fps <= 0)
            {
                deltaTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000;
            }
            else
            {
                deltaTime = 1.0f / (float)fps;
            }

            //System.Diagnostics.Debug.WriteLine("FPS = " + fps);
            //System.Diagnostics.Debug.WriteLine("DeltaTime = " + deltaTime);

            if(Keyboard.GetState().IsKeyDown(Keys.P))
                paused = !paused;
        } 

        public static float nextFloat()
        {
            return (float)random.NextDouble();
        }
    }
}
