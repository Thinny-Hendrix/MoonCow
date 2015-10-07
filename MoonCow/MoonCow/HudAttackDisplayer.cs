using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    public class HudAttackDisplayer
    {
        Texture2D waveStartFill;
        Texture2D waveStartOut;
        Texture2D warnMark;
        Texture2D warnFill;
        Texture2D warnOut1;
        Texture2D warnOut2;
        Texture2D warnOut2w;
        Texture2D attackOverFill;
        Texture2D attackOverOut;


        Vector2 currentWarnPos;
        Vector2 warnMidPos;
        Vector2 warnSidePos;

        public Texture2D wavOut;
        public Texture2D wavOutW;
        public Texture2D wavFill;
        public Texture2D swaBig;
        public Texture2D swaSml;
        public Texture2D sneBig;
        public Texture2D sneSml;
        public Texture2D gunBig;
        public Texture2D gunSml;
        public Texture2D hevBig;
        public Texture2D hevSml;

        public Attack activeAttack;
        List<HudWave> waveDisplays;
        int displayedWaves;
        int totalWaves;

        float flashTime;
        float fadeTime;
        float cosTime;
        float messageAlpha;
        float displayTime;
        public bool displayMessage;
        String startMessage;
        String endMessage;
        int waveNo = 0;
        bool addNew;
        public bool active;

        int lastFlash;
        float flashChainCool;
        Vector2 spawnPos;

        SpriteFont bigFont;
        SpriteBatch sb;

        EnemyManager enemyManager;
        Game1 game;
        Hud hud;

        enum MessageType {start, end};
        MessageType messageType;

        public HudAttackDisplayer(Game1 game, Hud hud)
        {
            this.game = game;
            this.hud = hud;
            this.enemyManager = game.enemyManager;
            sb = new SpriteBatch(game.GraphicsDevice);
            messageType = MessageType.end;

            waveDisplays = new List<HudWave>();

            bigFont = game.Content.Load<SpriteFont>(@"Hud/Venera900big");
            waveStartFill = game.Content.Load<Texture2D>(@"Hud/AttackDisplay/attackStartFill");
            waveStartOut = game.Content.Load<Texture2D>(@"Hud/AttackDisplay/attackStartOut");
            warnMark = game.Content.Load<Texture2D>(@"Hud/AttackDisplay/warnMark");
            warnFill = game.Content.Load<Texture2D>(@"Hud/AttackDisplay/warnFill");
            warnOut1 = game.Content.Load<Texture2D>(@"Hud/AttackDisplay/warnOut1");
            warnOut2 = game.Content.Load<Texture2D>(@"Hud/AttackDisplay/warnOut2");
            warnOut2w = game.Content.Load<Texture2D>(@"Hud/AttackDisplay/warnOut2-w");
            attackOverFill = game.Content.Load<Texture2D>(@"Hud/AttackDisplay/attackOverFill");
            attackOverOut = game.Content.Load<Texture2D>(@"Hud/AttackDisplay/attackOverOut");

            swaSml = game.Content.Load<Texture2D>(@"Hud/AttackDisplay/swaSml");
            swaBig = game.Content.Load<Texture2D>(@"Hud/AttackDisplay/swaBig");
            sneSml = game.Content.Load<Texture2D>(@"Hud/AttackDisplay/sneSml");
            sneBig = game.Content.Load<Texture2D>(@"Hud/AttackDisplay/sneBig");
            gunSml = game.Content.Load<Texture2D>(@"Hud/AttackDisplay/gunSml");
            gunBig = game.Content.Load<Texture2D>(@"Hud/AttackDisplay/gunBig");
            hevSml = game.Content.Load<Texture2D>(@"Hud/AttackDisplay/hevSml");
            hevBig = game.Content.Load<Texture2D>(@"Hud/AttackDisplay/hevBig");

            wavOut = game.Content.Load<Texture2D>(@"Hud/AttackDisplay/waveOut");
            wavOutW = game.Content.Load<Texture2D>(@"Hud/AttackDisplay/waveOutW");
            wavFill = game.Content.Load<Texture2D>(@"Hud/AttackDisplay/waveFill");





            warnMidPos = new Vector2(960, 360);
            warnSidePos = new Vector2(150, 350);
            currentWarnPos = warnMidPos;
        }

        public void startAttackMessage(Attack a)
        {
            waveNo = a.inAttack;
            displayMessage = true;
            displayTime = 0;
            messageAlpha = 1;
            currentWarnPos = warnMidPos;
            messageType = MessageType.start;

            activeAttack = a;
            spawnPos = new Vector2(60,417);
            totalWaves = activeAttack.waves.Count();
            displayedWaves = 0;
            addNew = false;
            flashChainCool = 0.1f;
            active = true;
        }

        public void endAttackMessage()
        {
            displayMessage = true;
            displayTime = 0;
            messageAlpha = 1;
            messageType = MessageType.end;

            displayedWaves = 0;
            totalWaves = 0;

            waveDisplays.Clear();
            active = false;
        }

        void startFlashChain()
        {
            lastFlash = waveDisplays.Count() - 1;
            flashChainCool = 0.1f;
            if (lastFlash != -1)
                waveDisplays.ElementAt(lastFlash).flash();
            else
                flashTime = 1;
        }

        void updateFlashChain()
        {
            if (lastFlash > -1)
            {
                flashChainCool -= Utilities.deltaTime;
                if (flashChainCool <= 0)
                {
                    flashChainCool = 0.1f;
                    lastFlash--;
                    if (lastFlash == -1)
                    {
                        flashTime = 1;
                    }
                    else
                        waveDisplays.ElementAt(lastFlash).flash();
                }
            }
        }

        void spawnInitWaves()
        {
            flashChainCool -= Utilities.deltaTime;
            if (flashChainCool <= 0)
            {
                waveDisplays.Add(new HudWave(hud, this, spawnPos, activeAttack.waves.ElementAt(displayedWaves)));
                spawnPos.Y += 86;

                if(displayedWaves == 0)
                    waveDisplays.ElementAt(0).firstInList = true;

                displayedWaves++;
                flashChainCool = 0.1f;
            }
            
        }

        public void Update()
        {
            if (flashTime != 0)
            {
                flashTime -= Utilities.deltaTime*4;
                if (flashTime < 0)
                    flashTime = 0;
            }

            if (displayMessage)
            {
                if (!Utilities.paused && !Utilities.softPaused)
                {
                    if (displayTime < 3)
                        displayTime += Utilities.deltaTime;
                    else
                    {
                        messageAlpha -= Utilities.deltaTime;

                        if (displayTime > 3)
                            cosTime = (float)(Math.Cos((messageAlpha) * MathHelper.Pi) + 1) / 2.0f;
                        currentWarnPos = Vector2.Lerp(warnMidPos, warnSidePos, cosTime);

                        if (messageAlpha <= 0)
                        {
                            flashTime = 1;
                            displayMessage = false;
                            currentWarnPos = warnSidePos;
                        }
                    }
                }                
            }
            else
            {
                if (active && displayedWaves < 2) // 2 used to be 4 but would cause a crash
                    spawnInitWaves();
            }

            foreach (HudWave w in waveDisplays)
            {
                w.Update();
            }

            if (addNew && !waveDisplays.ElementAt(0).moving)
            {
                if (displayedWaves < totalWaves)
                {
                    waveDisplays.Add(new HudWave(hud, this, new Vector2(60, 417 + 86 * 3), activeAttack.waves.ElementAt(displayedWaves)));
                    displayedWaves++;
                }

                addNew = false;
                startFlashChain();
            }

            updateFlashChain();            
        }
        public void updateWaves()
        {
            if (waveDisplays.Count() != 0)
            {
                waveDisplays.RemoveAt(0);
                if(waveDisplays.Count() != 0)
                    waveDisplays.ElementAt(0).firstInList = true;
            }
            foreach (HudWave w in waveDisplays)
                w.nudge(new Vector2(0, -86));

            if (waveDisplays.Count() != 0)
                addNew = true;
            else
                startFlashChain();
            //if number of displayed waves is less than total attack waves, add the next wave to the list
        }

        void drawWarn()
        {
            sb.Draw(warnFill, hud.scaledRect(currentWarnPos, warnOut1.Width, warnOut1.Height),
                null, Color.White, 0, new Vector2(warnOut1.Width / 2 + 5, warnOut1.Height / 2), SpriteEffects.None, 0);
            if (displayMessage)
                sb.Draw(warnOut1, hud.scaledRect(currentWarnPos, warnOut1.Width, warnOut1.Height),
                   null, Color.White, 0, new Vector2(warnOut1.Width / 2 + 5, warnOut1.Height / 2), SpriteEffects.None, 0);
            else
            {
                sb.Draw(warnOut2, hud.scaledRect(currentWarnPos, warnOut1.Width, warnOut1.Height),
                   null, Color.White, 0, new Vector2(warnOut1.Width / 2 + 5, warnOut1.Height / 2), SpriteEffects.None, 0);
                sb.Draw(warnOut2w, hud.scaledRect(currentWarnPos, warnOut1.Width, warnOut1.Height),
                   null, Color.White*flashTime, 0, new Vector2(warnOut1.Width / 2 + 5, warnOut1.Height / 2), SpriteEffects.None, 0);
            }
            sb.Draw(warnMark, hud.scaledRect(currentWarnPos, warnOut1.Width, warnOut1.Height),
                null, Color.White, 0, new Vector2(warnOut1.Width / 2 + 5, warnOut1.Height / 2), SpriteEffects.None, 0);
        }

        void drawStartMessage()
        {
            sb.Draw(waveStartFill, hud.scaledRect(new Vector2(960, 540), waveStartFill.Width, waveStartFill.Height),
                        null, Color.White * messageAlpha, 0, new Vector2(waveStartFill.Width / 2, waveStartFill.Height / 2), SpriteEffects.None, 0);

            sb.Draw(waveStartOut, hud.scaledRect(new Vector2(960, 540), waveStartFill.Width, waveStartFill.Height),
                    null, Color.White * messageAlpha, 0, new Vector2(waveStartFill.Width / 2, waveStartFill.Height / 2), SpriteEffects.None, 0);

            sb.DrawString(bigFont, "attack", hud.scaledCoords(960, 490), Color.White * messageAlpha, 0,
                new Vector2(bigFont.MeasureString("attack").X / 2, bigFont.MeasureString("attack").Y / 2), 0.55f, SpriteEffects.None, 0);
            sb.DrawString(bigFont, "start", hud.scaledCoords(960, 575), Color.White * messageAlpha, 0,
                new Vector2(bigFont.MeasureString("start").X / 2, bigFont.MeasureString("start").Y / 2), 0.55f, SpriteEffects.None, 0);
            sb.DrawString(bigFont, waveNo + " waves", hud.scaledCoords(960, 660), Color.White * messageAlpha, 0,
                new Vector2(bigFont.MeasureString(waveNo + " waves").X / 2, bigFont.MeasureString(waveNo + " waves").Y / 2), 0.3f, SpriteEffects.None, 0);
        }

        void drawEndMessage()
        {
            sb.Draw(attackOverFill, hud.scaledRect(new Vector2(960, 540), attackOverFill.Width, attackOverFill.Height),
                        null, Color.White * messageAlpha, 0, new Vector2(attackOverFill.Width / 2, attackOverFill.Height / 2), SpriteEffects.None, 0);

            sb.Draw(attackOverOut, hud.scaledRect(new Vector2(960, 540), attackOverFill.Width, attackOverFill.Height),
                    null, Color.White * messageAlpha, 0, new Vector2(attackOverFill.Width / 2, attackOverFill.Height / 2), SpriteEffects.None, 0);

            sb.DrawString(bigFont, "attack", hud.scaledCoords(960, 515), Color.White * messageAlpha, 0,
                new Vector2(bigFont.MeasureString("attack").X / 2, bigFont.MeasureString("attack").Y / 2), 0.5f, SpriteEffects.None, 0);
            sb.DrawString(bigFont, "over!", hud.scaledCoords(960, 600), Color.White * messageAlpha, 0,
                new Vector2(bigFont.MeasureString("over!").X / 2, bigFont.MeasureString("over!").Y / 2), 0.5f, SpriteEffects.None, 0);
        }


        public void Draw()
        {
            sb.Begin();

            if (displayMessage && !hud.quickSelect.active)
            {
                if (messageType == MessageType.start)
                    drawStartMessage();
                if (messageType == MessageType.end)
                    drawEndMessage();
            }

            if (messageType == MessageType.start)//if the attack hasn't ended yet
            {
                if ((!hud.quickSelect.active && displayMessage) || !displayMessage)
                {
                    drawWarn();
                    foreach (HudWave w in waveDisplays)
                        w.Draw(sb);
                }
            }
            sb.End();
        }


    }
}
