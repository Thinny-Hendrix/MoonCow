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


        float flashTime;
        float fadeTime;
        float cosTime;
        float messageAlpha;
        float displayTime;
        bool displayMessage;
        String startMessage;
        String endMessage;

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

            warnMidPos = new Vector2(960, 360);
            warnSidePos = new Vector2(150, 350);
            currentWarnPos = warnMidPos;
        }

        public void startAttackMessage(int waves)
        {
            displayMessage = true;
            displayTime = 0;
            messageAlpha = 1;
            currentWarnPos = warnMidPos;
            messageType = MessageType.start;
        }

        public void endAttackMessage()
        {
            displayMessage = true;
            displayTime = 0;
            messageAlpha = 1;
            messageType = MessageType.end;
        }

        public void Update()
        {
            if (flashTime != 0)
            {
                flashTime -= Utilities.deltaTime*6;
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
            sb.DrawString(bigFont, "1 wave", hud.scaledCoords(960, 660), Color.White * messageAlpha, 0,
                new Vector2(bigFont.MeasureString("1 wave").X / 2, bigFont.MeasureString("1 wave").Y / 2), 0.3f, SpriteEffects.None, 0);
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
                drawWarn();
            }
            sb.End();
        }


    }
}
