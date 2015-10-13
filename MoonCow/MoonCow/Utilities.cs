using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    public static class Utilities
    {
        public static TimeSpan elapsedTime = TimeSpan.Zero;
        public static int frameCount = 0;
        public static int fps;
        public static float deltaTime;
        public static float frameRate;
        public static bool paused = false;
        public static bool softPaused = false;
        public static Random random = new Random();

        public static float windowScale;

        public enum SpawnState { idle, deploying, waiting }

        public static void setScale(Game1 game)
        {
            windowScale = (float)game.GraphicsDevice.Viewport.Bounds.Width / 1920.0f;
        }



        public static void Update(GameTime gameTime)
        {
            elapsedTime += gameTime.ElapsedGameTime;

            if (elapsedTime > TimeSpan.FromSeconds(1))
            {
                elapsedTime -= TimeSpan.FromSeconds(1);
                fps = frameCount;
                frameCount = 0;
            }

            deltaTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000;

            if (fps <= 0)
            {
                frameRate = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000;
            }
            else
            {
                frameRate = 1.0f / (float)fps;
            }

            //System.Diagnostics.Debug.WriteLine("FPS = " + fps);
            //System.Diagnostics.Debug.WriteLine("DeltaTime = " + deltaTime);

        } 

        public static float nextFloat()
        {
            return (float)random.NextDouble();
        }

        /// <summary>
        /// Returns the length between two points in a 3D space
        /// </summary>
        /// <param name="pos1"></param>
        /// <param name="pos2"></param>
        /// <returns></returns>
        public static float tdTrig(Vector3 pos1, Vector3 pos2)
        {
            float x = pos1.X - pos2.X;
            float y = pos1.Y - pos2.Y;
            float z = pos1.Z - pos2.Z;

            float a = (float)Math.Sqrt(x * x + z * z);
            float b = (float)Math.Sqrt(a * a + y * y); 

            return b;
        }
        
        public static string formattedTime(float time)
        {
            string s = (int)time / 60 + ":";
            if ((int)time % 60 < 10)
                s += "0";
            s += (int)time % 60;

            return s;
        }

        public static float hypotenuseOf(float x, float y)
        {
            return (float)Math.Sqrt(x * x + y * y);
        }

        public static float tdTrig(Vector3 pos)
        {
            float a = (float)Math.Sqrt(pos.X * pos.X + pos.Z * pos.Z);
            float b = (float)Math.Sqrt(a * a + pos.Y * pos.Y);

            return b;
        }

        public static float tdTan(Vector3 pos1, Vector3 pos2)
        {
            float x = pos1.X - pos2.X;
            float y = pos1.Y - pos2.Y;
            float z = pos1.Z - pos2.Z;

            float a = (float)Math.Sqrt(x * x + z * z);
            //float b = (float)Math.Sqrt(a * a + y * y);

            float angle = (float)Math.Tan(y / a);

            return angle;
        }

        public static float tdTan(Vector3 pos)
        {
            float a = (float)Math.Sqrt(pos.X * pos.X + pos.Z * pos.Z);
            return (float)Math.Tan(pos.Y / a);
        }

        public static Rectangle scaledRect(Vector2 pos, float x, float y)
        {
            int scaledX = (Int32)(pos.X * windowScale);
            int scaledY = (Int32)(pos.Y * windowScale);

            return new Rectangle(
                scaledX,
                scaledY,
                (Int32)Math.Round(x * windowScale),
                (Int32)Math.Round(y * windowScale)
                );
        }

        public static Vector2 scaledCoords(Vector2 vector)
        {
            Vector2 returnVect;
            returnVect.X = vector.X * windowScale;
            returnVect.Y = vector.Y * windowScale;

            return returnVect;
        }

        public static Vector2 scaledCoords(float x, float y)
        {
            Vector2 returnVect;

            returnVect.X = x * windowScale;
            returnVect.Y = y * windowScale;

            return returnVect;
        }
    }
}
