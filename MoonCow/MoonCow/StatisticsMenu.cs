using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MoonCow
{
    public class StatisticsMenu : DrawableGameComponent
    {
        Game1 game;
        SpriteBatch sb;
        Vector2 loopPos1;
        Vector2 loopPos2;

        float lasAc;
        float bombAc;
        float wavAc;
        float astAc;

        float mgRate;

        string drill1;
        string drill2;

        float fadeTime;

        Texture2D favIco;

        public StatisticsMenu(Game1 game):base(game)
        {
            this.game = game;
            sb = new SpriteBatch(game.GraphicsDevice);
            fadeTime = 0;

            loopPos1 = new Vector2(-191,50);
            loopPos2 = new Vector2(0, 190);

            if(game.levelStats.laserShotsFired > 0)
            {
                lasAc = game.levelStats.laserShotsHit / game.levelStats.laserShotsFired;
                lasAc *= 100;
                lasAc = (float)Math.Ceiling(lasAc); 
            }
            else
            {
                lasAc = 0;
            }
            if (game.levelStats.bombsFired > 0)
            {
                bombAc = game.levelStats.bombsHit / game.levelStats.bombsFired;
                bombAc *= 100;
                bombAc = (float)Math.Ceiling(bombAc);
            }
            else
            {
                bombAc = 0;
            }
            if (game.levelStats.wavesFired > 0)
            {
                wavAc = game.levelStats.wavesHit / game.levelStats.wavesFired;
                wavAc *= 100;
                wavAc = (float)Math.Ceiling(wavAc);
            }
            else
            {
                wavAc = 0;
            }
            if (game.levelStats.astShotsFired > 0)
            {
                astAc = game.levelStats.astShotsHit / game.levelStats.astShotsFired;
                astAc *= 100;
                astAc = (float)Math.Ceiling(astAc);
            }
            else
            {
                astAc = 0;
            }

            if(game.levelStats.gotDrill)
            {
                drill1 = "you unlocked";
                drill2 = "the omega drill!";
            }
            else
            {
                drill1 = "did not unlock";
                drill2 = "the omega drill";
            }

            if (game.levelStats.mgAttempts > 0)
            {
                mgRate = game.levelStats.mgSuccess / game.levelStats.mgAttempts;
                mgRate *= 100;
                mgRate = (float)Math.Ceiling(mgRate);
            }
            else
                mgRate = 0;

            favIco = TextureManager.icoPew;
            if (game.levelStats.flamers > game.levelStats.gatlings)
                favIco = TextureManager.icoPyr;
            if (game.levelStats.electrics > game.levelStats.gatlings
                && game.levelStats.electrics > game.levelStats.flamers)
                favIco = TextureManager.icoEle;

        }

        public override void Update(GameTime gameTime)
        {
            MenuAssets.updateLinePos();

            loopPos1.X += Utilities.deltaTime * 100;
            if (loopPos1.X > 0)
                loopPos1.X -= 191;

            loopPos2.X -= Utilities.deltaTime * 100;
            if (loopPos2.X < -191)
                loopPos2.X += 191;

            if(fadeTime != 1)
            {
                fadeTime += Utilities.deltaTime;
                if (fadeTime > 1)
                    fadeTime = 1;
            }
        }

        void drawWeps()
        {
            sb.DrawString(MenuAssets.font, "shots fired: "+game.levelStats.laserShotsFired, Utilities.scaledCoords(550, 390), MenuAssets.contSecondary, 0,
                        new Vector2(MenuAssets.font.MeasureString("shots fired: " + game.levelStats.laserShotsFired).X / 2, 0), Utilities.windowScale * 18 / 40, SpriteEffects.None, 0);
            sb.DrawString(MenuAssets.font, "accuracy: %" + lasAc, Utilities.scaledCoords(550, 420), MenuAssets.contSecondary, 0,
                        new Vector2(MenuAssets.font.MeasureString("accuracy: " + lasAc + "%").X / 2, 0), Utilities.windowScale * 18 / 40, SpriteEffects.None, 0);

            sb.DrawString(MenuAssets.font, "bombs fired: " + game.levelStats.bombsFired, Utilities.scaledCoords(550, 510), MenuAssets.contSecondary, 0,
                        new Vector2(MenuAssets.font.MeasureString("bombs fired: " + game.levelStats.bombsFired).X / 2, 0), Utilities.windowScale * 18 / 40, SpriteEffects.None, 0);
            sb.DrawString(MenuAssets.font, "accuracy: %" + bombAc, Utilities.scaledCoords(550, 540), MenuAssets.contSecondary, 0,
                        new Vector2(MenuAssets.font.MeasureString("accuracy: " + bombAc + "%").X / 2, 0), Utilities.windowScale * 18 / 40, SpriteEffects.None, 0);

            sb.DrawString(MenuAssets.font, "shots fired: " + game.levelStats.astShotsFired, Utilities.scaledCoords(550, 630), MenuAssets.contSecondary, 0,
                        new Vector2(MenuAssets.font.MeasureString("shots fired: " + game.levelStats.astShotsFired).X / 2, 0), Utilities.windowScale * 18 / 40, SpriteEffects.None, 0);
            sb.DrawString(MenuAssets.font, "accuracy: %" + astAc, Utilities.scaledCoords(550, 660), MenuAssets.contSecondary, 0,
                        new Vector2(MenuAssets.font.MeasureString("accuracy: " + astAc+"%").X / 2, 0), Utilities.windowScale * 18 / 40, SpriteEffects.None, 0);

            sb.DrawString(MenuAssets.font, "waves fired: " + game.levelStats.wavesFired, Utilities.scaledCoords(550, 750), MenuAssets.contSecondary, 0,
                        new Vector2(MenuAssets.font.MeasureString("waves fired: " + game.levelStats.wavesFired).X / 2, 0), Utilities.windowScale * 18 / 40, SpriteEffects.None, 0);
            sb.DrawString(MenuAssets.font, "accuracy: %" + wavAc, Utilities.scaledCoords(550, 780), MenuAssets.contSecondary, 0,
                        new Vector2(MenuAssets.font.MeasureString("accuracy: " + wavAc + "%").X / 2, 0), Utilities.windowScale * 18 / 40, SpriteEffects.None, 0);

            sb.DrawString(MenuAssets.font, drill1, Utilities.scaledCoords(550, 850), MenuAssets.contSecondary, 0,
                        new Vector2(MenuAssets.font.MeasureString(drill1).X / 2, 0), Utilities.windowScale * 20 / 40, SpriteEffects.None, 0);
            sb.DrawString(MenuAssets.font, drill2, Utilities.scaledCoords(550, 880), MenuAssets.contSecondary, 0,
                        new Vector2(MenuAssets.font.MeasureString(drill2).X / 2, 0), Utilities.windowScale * 20 / 40, SpriteEffects.None, 0);
        }

        void drawTurr()
        {
            sb.DrawString(MenuAssets.font, "" + game.levelStats.gatlings, Utilities.scaledCoords(1000, 620), MenuAssets.contSecondary, 0,
                        new Vector2(MenuAssets.font.MeasureString("" + game.levelStats.gatlings).X / 2, 0), Utilities.windowScale * 18 / 40, SpriteEffects.None, 0);
            sb.DrawString(MenuAssets.font, "" + game.levelStats.flamers, Utilities.scaledCoords(1145, 620), MenuAssets.contSecondary, 0,
                        new Vector2(MenuAssets.font.MeasureString("" + game.levelStats.gatlings).X / 2, 0), Utilities.windowScale * 18 / 40, SpriteEffects.None, 0);
            sb.DrawString(MenuAssets.font, "" + game.levelStats.electrics, Utilities.scaledCoords(1305, 620), MenuAssets.contSecondary, 0,
                        new Vector2(MenuAssets.font.MeasureString("" + game.levelStats.gatlings).X / 2, 0), Utilities.windowScale * 18 / 40, SpriteEffects.None, 0);

            sb.Draw(favIco, Utilities.scaledRect(new Vector2(1210, 780), 91, 91), Color.White);
        }

        void drawRight()
        {
            //money
            sb.DrawString(MenuAssets.font, "earned: $" + game.levelStats.moneyEarnt, Utilities.scaledCoords(1585, 460), MenuAssets.contSecondary, 0,
                        new Vector2(MenuAssets.font.MeasureString("earned: $" + game.levelStats.moneyEarnt).X / 2, 0), Utilities.windowScale * 18f / 40, SpriteEffects.None, 0);
            sb.DrawString(MenuAssets.font, "spent: $" + game.levelStats.moneySpent, Utilities.scaledCoords(1585, 490), MenuAssets.contSecondary, 0,
                        new Vector2(MenuAssets.font.MeasureString("spent: $" + game.levelStats.moneySpent).X / 2, 0), Utilities.windowScale * 18f / 40, SpriteEffects.None, 0);
            //minigame
            sb.DrawString(MenuAssets.font, "attempts", Utilities.scaledCoords(1430, 645), Color.White, 0,
                        new Vector2(0, 0), Utilities.windowScale * 20 / 40, SpriteEffects.None, 0);
            sb.DrawString(MenuAssets.font, "" + game.levelStats.mgAttempts, Utilities.scaledCoords(1580, 670), MenuAssets.contSecondary, 0,
                        new Vector2(MenuAssets.font.MeasureString("" + game.levelStats.mgAttempts).X / 2, 0), Utilities.windowScale * 18f / 40, SpriteEffects.None, 0);

            sb.DrawString(MenuAssets.font, "success rate", Utilities.scaledCoords(1430, 695), Color.White, 0,
                        new Vector2(0, 0), Utilities.windowScale * 20 / 40, SpriteEffects.None, 0);
            sb.DrawString(MenuAssets.font, mgRate + "%", Utilities.scaledCoords(1580, 720), MenuAssets.contSecondary, 0,
                        new Vector2(MenuAssets.font.MeasureString(mgRate + "%").X / 2, 0), Utilities.windowScale * 18f / 40, SpriteEffects.None, 0);

            //time
            sb.DrawString(MenuAssets.font, "elapsed time", Utilities.scaledCoords(1585, 810), Color.White, 0,
                        new Vector2(MenuAssets.font.MeasureString("elapsed time").X / 2, 0), Utilities.windowScale * 24f / 40, SpriteEffects.None, 0);

            sb.DrawString(MenuAssets.font, Utilities.formattedTime(game.levelStats.timeInLevel), Utilities.scaledCoords(1585, 860), MenuAssets.contSecondary, 0,
                        new Vector2(MenuAssets.font.MeasureString(Utilities.formattedTime(game.levelStats.timeInLevel)).X / 2, 0), Utilities.windowScale * 20 / 40, SpriteEffects.None, 0);
        }

        public override void Draw(GameTime gameTime)
        {
            game.GraphicsDevice.SetRenderTarget(null);
            sb.Begin();
            MenuAssets.drawMenuBackground(sb);
            sb.Draw(MenuAssets.statBack, Utilities.scaledRect(new Vector2(960, 640), 1750, 650), null, Color.White, 0, new Vector2(875, 325), SpriteEffects.None, 0);

            sb.Draw(MenuAssets.statLoop, Utilities.scaledRect(loopPos1, 2291, 23), Color.White);
            sb.Draw(MenuAssets.statLoop, Utilities.scaledRect(loopPos2, 2291, 23), Color.White);
            sb.Draw(MenuAssets.statHead, Utilities.scaledRect(new Vector2(40,70), 1000, 136), Color.White);


            drawWeps();

            drawTurr();

            drawRight();

            sb.Draw(MenuAssets.pureWhite, Utilities.scaledRect(Vector2.Zero, 1920, 1080), Color.Black * MathHelper.SmoothStep(1, 0, fadeTime));

            sb.End();
        }
    }
}
