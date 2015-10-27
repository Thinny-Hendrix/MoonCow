using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MoonCow
{
    //lots of images, references ship for health/money/weapons, minimap is going to be interesting
    public class Hud : Microsoft.Xna.Framework.DrawableGameComponent
    {
        float viewportW;
        float viewportH;
        public SpriteFont font;
        Game1 game;
        Ship ship;
        public SpriteBatch spriteBatch;
        public GraphicsDevice graphicsDevice;
        public HudAttackDisplayer hudAttackDisplayer;

        public HudMoney hudMoney;
        public HudWeapon hudWeapon;
        public HudHealth hudHealth;
        public HudState hudState;
        public HudMap hudMap;
        public HudMessage hudMessage;
        public HudMg hudMg;
        public ExpSelect expSelect;
        public TurretSelect turSelect;
        public QuickSelect quickSelect;
        public HudPrompt hudPrompt;
        public HudCollectable hudCollectable;
        public HudRespawn respawn;
        public HudHelp hudHelp;
        public HudEnd hudEnd;
        public HudZoom hudZoom;

        Vector2 position;
        
        bool startingBoost;
        float boostDrawScale;
        float boostDrawAlpha;

        String frameRate;

        public Color contPrimary;
        public Color contSecondary;
        public Color outline;
        public Color fill;
        public Color redBody;
        public Color blueBody;

        public Texture2D butA;
        public Texture2D butB;        
        public Texture2D butX;
        public Texture2D butY;
        public Texture2D endO;
        public Texture2D endF;
        

        Texture2D whiteTex;
        public float flashTime;
        public float scale;

        public Hud(Game1 game, SpriteFont font, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice) : base(game)
        {
            this.game = game;
            ship = game.ship;
            this.font = font;
            this.spriteBatch = spriteBatch;
            this.graphicsDevice = graphicsDevice;

            hudMoney = new HudMoney(this, font, game);
            hudWeapon = new HudWeapon(this, font, game);
            hudHealth = new HudHealth(this, font, game);
            hudState = new HudState(this, font, game);
            hudMap = new HudMap(this, font, game);
            hudMessage = new HudMessage(this, font, game);
            hudMg = new HudMg(this, game.Content.Load<SpriteFont>(@"Hud/Venera900big"), game);
            hudCollectable = new HudCollectable(this, font, game);

            viewportW = game.GraphicsDevice.Viewport.Width;
            viewportH = game.GraphicsDevice.Viewport.Height;

            hudAttackDisplayer = new HudAttackDisplayer(game, this);
            quickSelect = new QuickSelect(this, game, font);
            expSelect = new ExpSelect(this, game, font);
            turSelect = new TurretSelect(this, game, font);
            hudPrompt = new HudPrompt(this, game, font);
            respawn = new HudRespawn(this, hudMg.font, game);
            hudHelp = new HudHelp(this, game, font);
            hudEnd = new HudEnd(this, game);
            hudZoom = new HudZoom(this, game);

            position = new Vector2(0, 0);
            contPrimary = Color.White;
            contSecondary = new Color(174, 215, 255);
            outline = new Color(0, 64, 127);
            fill = new Color(0, 16, 73, 179);
            redBody = new Color(181, 77, 102);
            blueBody = new Color(86, 124, 193);        

            whiteTex = new Texture2D(graphicsDevice, 1, 1);
            whiteTex.SetData(new Color[] { Color.White });
            flashTime = 15;

            scale = (float)game.GraphicsDevice.Viewport.Bounds.Width / 1920.0f;

            butA = game.Content.Load<Texture2D>(@"Hud/butA");
            butB = game.Content.Load<Texture2D>(@"Hud/butB");
            butX = game.Content.Load<Texture2D>(@"Hud/butX");
            butY = game.Content.Load<Texture2D>(@"Hud/butY");
            endO = game.Content.Load<Texture2D>(@"Hud/endO");
            endF = game.Content.Load<Texture2D>(@"Hud/endF");

        }

        public Rectangle scaledRect(Vector2 pos, float x, float y)
        {
            int w = Game.GraphicsDevice.Viewport.Width;
            int h = Game.GraphicsDevice.Viewport.Height;

            int scaledX = (Int32)((pos.X / 1920.0f) * w);
            int scaledY = (Int32)((pos.Y / 1080.0f) * h);

            return new Rectangle(
                scaledX,
                scaledY,
                (Int32)Math.Round((x / 1920.0f) * w), 
                (Int32)Math.Round((y / 1080.0f) * h)
                );
        }

        public Vector2 scaledCoords(Vector2 vector)
        {
            Vector2 returnVect;
            int w = Game.GraphicsDevice.Viewport.Width;
            int h = Game.GraphicsDevice.Viewport.Height;

            returnVect.X = (vector.X / 1920.0f) * w;
            returnVect.Y = (vector.Y / 1080.0f) * h;

            return returnVect;
        }

        public Vector2 scaledCoords(float x, float y)
        {
            Vector2 returnVect;
            int w = Game.GraphicsDevice.Viewport.Width;
            int h = Game.GraphicsDevice.Viewport.Height;

            returnVect.X = (x / 1920.0f) * w;
            returnVect.Y = (y / 1080.0f) * h;

            return returnVect;
        }

        protected override void LoadContent()
        {
            //load texture files
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            float fps = 1.0f / Utilities.frameRate;
            frameRate = fps + " FPS";

            if (flashTime < 10)
            {
                flashTime += Utilities.deltaTime * 60;
                if (flashTime > 15)
                    flashTime = 15;
            }


            if (Keyboard.GetState().IsKeyDown(Keys.Y))
                makeFlash();

            quickSelect.Update();

            hudAttackDisplayer.Update();
            hudMoney.Update();
            hudWeapon.Update();
            hudHealth.Update();
            hudState.Update();
            hudMap.Update();
            hudPrompt.Update();
            hudMessage.Update();
            hudMg.Update();
            expSelect.Update();
            turSelect.Update();
            hudCollectable.Update();
            respawn.Update();
            hudHelp.Update();
            hudZoom.Update();
        }

        public void makeFlash()
        {
            flashTime = 0;
        }

        public void wakeAll()
        {
            hudWeapon.Wake();
            hudMoney.Wake();
            hudHealth.Wake();
            hudState.Wake();
            hudMap.Wake();
            hudCollectable.Wake();
        }

        public override void Draw(GameTime gameTime)
        {
            game.GraphicsDevice.SetRenderTarget(null);

            spriteBatch.Begin();
            spriteBatch.Draw((Texture2D)game.worldRender, Vector2.Zero, Color.White);

            if(startingBoost)
            {
                float w = Game.GraphicsDevice.Viewport.Width;
                float h = Game.GraphicsDevice.Viewport.Height;

                spriteBatch.Draw((Texture2D)game.worldRender, 
                    new Rectangle((int)w/2, (int)(h*0.5f), (int)(w * boostDrawScale), (int)(h * boostDrawScale)), 
                    null, Color.White*boostDrawAlpha, 0, new Vector2(w/2, h*0.5f), SpriteEffects.None, 0);
                boostDrawAlpha -= Utilities.deltaTime;
                boostDrawScale += Utilities.deltaTime/3;
                if (boostDrawAlpha < 0)
                    startingBoost = false;

            }

            hudZoom.Draw(spriteBatch);

            spriteBatch.End();

            if(flashTime < 10)
            {
                graphicsDevice.BlendState = BlendState.AlphaBlend;
                spriteBatch.Begin();
                spriteBatch.Draw(whiteTex, scaledRect(Vector2.Zero, 1920, 1080), Color.White*(1-flashTime/15.0f));
                spriteBatch.End();
            }


            spriteBatch.Begin();
            hudMoney.Draw(spriteBatch);
            hudWeapon.Draw(spriteBatch);
            hudHealth.Draw(spriteBatch);
            hudState.Draw(spriteBatch);
            hudMap.Draw(spriteBatch);
            hudMessage.Draw(spriteBatch);
            hudMg.Draw(spriteBatch);
            hudPrompt.Draw(spriteBatch);
            hudCollectable.Draw(spriteBatch);
            hudHelp.Draw(spriteBatch);
            hudEnd.Draw(spriteBatch);
            spriteBatch.End();


            quickSelect.Draw(spriteBatch);
            turSelect.Draw(spriteBatch);
            expSelect.Draw(spriteBatch);

            hudAttackDisplayer.Draw();
            respawn.draw(spriteBatch);

            Color myTransparentColor = new Color(0, 0, 0, 127);

            Vector2 stringDimensions = font.MeasureString(frameRate) * scale * (20.0f/40);
            float width = stringDimensions.X;
            float height = stringDimensions.Y;
            Rectangle backgroundRectangle = new Rectangle();
            backgroundRectangle.Width = (int)width + 10;
            backgroundRectangle.Height = (int)height + 10;
            backgroundRectangle.X = (int)position.X - 5;
            backgroundRectangle.Y = (int)position.Y - 5;

            Texture2D dummyTexture = new Texture2D(graphicsDevice, 1, 1);
            dummyTexture.SetData(new Color[] { myTransparentColor });
            graphicsDevice.BlendState = BlendState.AlphaBlend;
            spriteBatch.Begin();
            spriteBatch.Draw(dummyTexture, backgroundRectangle, Color.White);
            //spriteBatch.Draw(whiteTex, scaledRect(Vector2.Zero, 1920, 1080), Color.White);//(1-(flashTime/10.0f))));

            //spriteBatch.DrawString(font, "" + GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X, new Vector2(300, 300), contPrimary);
            //spriteBatch.DrawString(font, "" + GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y, new Vector2(300, 200), contPrimary);


            //spriteBatch.DrawString(font, frameRate, position, contPrimary);
            spriteBatch.DrawString(font, frameRate, Vector2.Zero, Color.White, 0,
                    Vector2.Zero, scale * (20.0f / 40), SpriteEffects.None, 0);

            //if(game.minigame.active)
               // spriteBatch.Draw((Texture2D)game.minigame.manager.rTarg, scaledRect(new Vector2(960, 540), 1024, 1024), null, Color.White, 0, new Vector2(512), SpriteEffects.None, 0);

            spriteBatch.End();

            graphicsDevice.BlendState = BlendState.Opaque;
            graphicsDevice.DepthStencilState = DepthStencilState.Default;

            dummyTexture.Dispose();
        }

        public void startBoost()
        {
            startingBoost = true;
            boostDrawScale = 1.05f;
            boostDrawAlpha = .5f;
        }


    }
}
