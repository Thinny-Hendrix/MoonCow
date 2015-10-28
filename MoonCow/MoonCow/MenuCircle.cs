using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace MoonCow
{
    class MenuCircle
    {
        float outRot;
        float rot2;
        List<Texture2D> pictures;
        Texture2D newPic;
        Texture2D oldPic;
        int activeindex;
        float time;
        float targetOutRot;
        float oldOutRot;
        float rotTransTime;
        Vector2 pos;
        float fadeTime;
        float alpha;
        float entryTime;
        float scale;
        public MenuCircle()
        {
            pos = new Vector2(1450, 650);
            pictures = new List<Texture2D>();
            pictures.Add(MenuAssets.ringIn0);
            pictures.Add(MenuAssets.ringIn1);
            pictures.Add(MenuAssets.ringIn2);
            pictures.Add(MenuAssets.ringIn3);
            pictures.Add(MenuAssets.ringIn4);

            newPic = pictures.ElementAt(0);
            oldPic = pictures.ElementAt(4);

            alpha = 1;
            fadeTime = 1;

            outRot = 0;
            oldOutRot = 0;
            targetOutRot = 0;
            rotTransTime = 1;
            scale = 0.5f;
        }

        void switchPictures()
        {
            activeindex = (activeindex+1+5)%5;

            newPic = pictures.ElementAt(activeindex);
            oldPic = pictures.ElementAt((activeindex - 1 + 5) % 5);

            fadeTime = 0;
        }

        public void downHit()
        {
            targetOutRot += MathHelper.Pi / 3;
            if(targetOutRot > MathHelper.Pi*2)
            {
                targetOutRot -= MathHelper.Pi * 2;
                outRot -= MathHelper.Pi * 2;
            }
            oldOutRot = outRot;
            rotTransTime = 0;
        }

        public void upHit()
        {
            targetOutRot -= MathHelper.Pi / 3;
            if (targetOutRot < MathHelper.Pi * 2)
            {
                targetOutRot += MathHelper.Pi * 2;
                outRot += MathHelper.Pi * 2;
            }
            oldOutRot = outRot;
            rotTransTime = 0;
        }

        public void Update()
        {
            time += Utilities.deltaTime;
            if(time > 2f)
            {
                switchPictures();
                time = 0;
            }

            if(fadeTime != 1)
            {
                fadeTime += Utilities.deltaTime;
                if (fadeTime > 1)
                    fadeTime = 1;
            }

            if(rotTransTime != 1)
            {
                rotTransTime += Utilities.deltaTime * 5;
                if (rotTransTime > 1)
                    rotTransTime = 1;

                outRot = MathHelper.SmoothStep(oldOutRot, targetOutRot, rotTransTime);
            }

            if(entryTime != 1)
            {
                entryTime += Utilities.deltaTime*2;
                if (entryTime > 1)
                    entryTime = 1;

                alpha = MathHelper.SmoothStep(0, 1, entryTime);
                scale = MathHelper.SmoothStep(0.5f, 1, entryTime);
            }

            rot2 += Utilities.deltaTime * MathHelper.PiOver4;
            if (rot2 > MathHelper.Pi * 2)
                rot2 -= MathHelper.Pi * 2;
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(oldPic, Utilities.scaledRect(pos, 400 * scale, 400 * scale), null, Color.White * alpha * MathHelper.SmoothStep(1, 0, fadeTime), 0, new Vector2(200, 200), SpriteEffects.None, 0);
            sb.Draw(newPic, Utilities.scaledRect(pos, 400 * scale, 400 * scale), null, Color.White * alpha * MathHelper.SmoothStep(0, 1, fadeTime), 0, new Vector2(200, 200), SpriteEffects.None, 0);

            sb.Draw(MenuAssets.ring2, Utilities.scaledRect(pos, 500 * scale, 500 * scale), null, Color.White * alpha, rot2, new Vector2(250, 250), SpriteEffects.None, 0);
            sb.Draw(MenuAssets.ring1, Utilities.scaledRect(pos, 600 * scale, 600 * scale), null, Color.White * alpha, outRot, new Vector2(300, 300), SpriteEffects.None, 0);
        }
    }
}
