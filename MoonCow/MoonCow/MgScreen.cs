using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    public class MgScreen
    {
        public RenderTarget2D rTarg;
        SpriteBatch sb;
        public List<SpriteParticle> particles;
        public List<SpriteParticle> pToDelete;
        public List<SpriteParticle> frontParticles;

        public List<MgMessage> messages;
        public List<MgMessage> mToDelete;

        Minigame minigame;
        MgManager manager;
        Game1 game;

        Texture2D overlay;
        Texture2D cross;
        SpriteFont font;

        Color blue;
        Color red;

        string count;
        string moneyDesc;
        string money;

        float displayMoney;
        float moneyTransTime;
        float oldMoney;

        public MgScreen(Minigame minigame, MgManager manager, Game1 game)
        {
            this.game = game;
            this.minigame = minigame;
            this.manager = manager;
            font = manager.font;

            overlay = game.Content.Load<Texture2D>(@"Minigame/mgOverlay");
            cross = game.Content.Load<Texture2D>(@"Minigame/mgX");

            rTarg = new RenderTarget2D(game.GraphicsDevice, 1365, 1024);
            sb = new SpriteBatch(game.GraphicsDevice);

            blue = new Color(200, 250, 255);
            red = new Color(249, 59, 43);

            particles = new List<SpriteParticle>();
            pToDelete = new List<SpriteParticle>();
            frontParticles = new List<SpriteParticle>();

            messages = new List<MgMessage>();
            mToDelete = new List<MgMessage>();

            moneyDesc = "Money earned:";
            displayMoney = 0;
        }

        public void Update()
        {
            foreach (SpriteParticle p in particles)
                p.Update();

            foreach (SpriteParticle p in frontParticles)
                p.Update();

            foreach (SpriteParticle p in pToDelete)
            {
                particles.Remove(p);
                frontParticles.Remove(p);
            }
            pToDelete.Clear();

            foreach (MgMessage m in messages)
                m.Update();
            foreach (MgMessage m in mToDelete)
                messages.Remove(m);
            mToDelete.Clear();

            if (moneyTransTime < 1)
            {
                displayMoney = MathHelper.Lerp(oldMoney, minigame.moneyEarned, moneyTransTime);
                if(minigame.moneyEarned != 0)
                    moneyTransTime += Utilities.deltaTime * 4;
                else
                    moneyTransTime += Utilities.deltaTime;
            }
            else
                displayMoney = minigame.moneyEarned;

            count = manager.hitCount + "/" + manager.markerMax;

            money = "$" + (int)displayMoney;
        }

        public void addMessage(string s)
        {
            messages.Add(new MgMessage(s, new Vector2(1195, 290), mToDelete));

            foreach (MgMessage m in messages)
                m.pos.Y += 40;
        }

        public void setMoney()
        {
            oldMoney = displayMoney;
            moneyTransTime = 0;
        }

        public void Draw()
        {
            game.GraphicsDevice.SetRenderTarget(rTarg);

            rTarg.GraphicsDevice.Clear(Color.Black * 0.5f);
            game.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            minigame.models.Draw();

            game.GraphicsDevice.BlendState = BlendState.Additive;
            sb.Begin();
            foreach (SpriteParticle p in particles)
                p.Draw(sb);

            game.GraphicsDevice.BlendState = BlendState.AlphaBlend;
            sb.Draw((Texture2D)manager.rTarg, Vector2.Zero, Color.White);
            sb.Draw(overlay, Vector2.Zero, Color.White);
            foreach (SpriteParticle p in frontParticles)
                p.Draw(sb);
            drawStrikes();

            sb.DrawString(font, count, new Vector2(1340,42), Color.White, 0,
                new Vector2(font.MeasureString(count).X, font.MeasureString(count).Y / 2), 30.0f / 40, SpriteEffects.None, 0);

            sb.DrawString(font, moneyDesc, new Vector2(1340, 102), Color.White, 0,
                new Vector2(font.MeasureString(moneyDesc).X, font.MeasureString(moneyDesc).Y / 2), 12.0f / 40, SpriteEffects.None, 0);

            sb.DrawString(font, money, new Vector2(1340, 132), Color.White, 0,
                new Vector2(font.MeasureString(money).X, font.MeasureString(money).Y / 2), 24.0f / 40, SpriteEffects.None, 0);

            foreach (MgMessage m in messages)
                m.Draw(sb, font);

            sb.End();
            
        }

        void drawStrikes()
        {
            if(manager.missCount == 3)
                sb.Draw(cross, new Vector2(1250, 180), red);
            else
                sb.Draw(cross, new Vector2(1250, 180), blue*0.5f);

            if(manager.missCount >= 2)
                sb.Draw(cross, new Vector2(1150, 180), red);
            else
                sb.Draw(cross, new Vector2(1150, 180), blue * 0.5f);

            if (manager.missCount >= 1)
                sb.Draw(cross, new Vector2(1050, 180), red);
            else
                sb.Draw(cross, new Vector2(1050, 180), blue * 0.5f);
        }

        public void reset()
        {
            particles.Clear();
            pToDelete.Clear();
        }
    }
}
