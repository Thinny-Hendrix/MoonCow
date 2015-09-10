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
        public Minimap minimap;
        SpriteFont font;
        Game1 game;
        SpriteBatch spriteBatch;
        GraphicsDevice graphicsDevice;
        Vector2 position;
        bool toggleMap;
        bool bigMap;
        bool startingBoost;
        float boostDrawScale;
        float boostDrawAlpha;

        String moneyTot;
        String moneyDif;
        String frameRate;
        String gameState;
        String stateTimer;
        String weaponAmmo;
        String shieldValue;
        String hpValue;
        String roundStart;
        String message;

        Color contPrimary;
        Color contSecondary;
        Color outline;
        Color fill;
        Color redBody;
        Color blueBody;

        Texture2D hudHealthB;
        Texture2D hudHealthF;
        Texture2D hudWepB;
        Texture2D hudWepF;
        Texture2D hudMonB;
        Texture2D hudMonF;
        Texture2D hudStatB;
        Texture2D hudStatF;
        Texture2D hudMapB;
        Texture2D hudMapF;


        Texture2D whiteTex;
        public float flashTime;

        Vector2 wepPos;
        Vector2 wepAmmoPos;

        Vector2 monPos;
        Vector2 monTotPos;
        Vector2 monDifPos;

        Vector2 healthPos;
        Vector2 shieldBarPos;
        Vector2 hpBarPos;

        Vector2 statPos;
        Vector2 statNamePos;
        Vector2 statTimePos;

        Vector2 mapPos;


        /*
        Rectangle healthRect;
        Rectangle wepRect;
        Rectangle statRect;
        Rectangle monRect;
        Rectangle mapRect;*/

        public Hud(Game1 game, SpriteFont font, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice) : base(game)
        {
            this.game = game;
            this.font = font;
            this.spriteBatch = spriteBatch;
            this.graphicsDevice = graphicsDevice;
            minimap = new Minimap(game);

            position = new Vector2(0, 0);
            //money = "900 dollarydoos";
            contPrimary = Color.White;
            contSecondary = new Color(174, 215, 255);
            outline = new Color(0, 64, 127);
            fill = new Color(0, 16, 73, 179);
            redBody = new Color(181, 77, 102);
            blueBody = new Color(86, 124, 193);

            healthPos = new Vector2(659, 45);
            shieldBarPos = new Vector2(850, 55);
            hpBarPos = new Vector2(900, 95);

            wepPos = new Vector2(45, 45);
            wepAmmoPos = new Vector2(240, 80);
            monPos = new Vector2(1450, 45);
            monTotPos = new Vector2(1715, 80);
            monDifPos = new Vector2(1715, 160);

            statPos = new Vector2(45, 884);
            statTimePos = new Vector2(150, 950);
            mapPos = new Vector2(1334, 752);

            whiteTex = new Texture2D(graphicsDevice, 1, 1);
            whiteTex.SetData(new Color[] { Color.White });
            flashTime = 15;

            weaponAmmo = "FIRING";
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

        protected override void LoadContent()
        {
            //load texture files

            hudHealthF = Game.Content.Load<Texture2D>(@"Hud/hudHealthF");
            hudHealthB = Game.Content.Load<Texture2D>(@"Hud/hudHealthB");

            hudWepF = Game.Content.Load<Texture2D>(@"Hud/hudWepF");
            hudWepB = Game.Content.Load<Texture2D>(@"Hud/hudWepB");

            hudMonF = Game.Content.Load<Texture2D>(@"Hud/hudMonF");
            hudMonB = Game.Content.Load<Texture2D>(@"Hud/hudMonB");

            hudStatF = Game.Content.Load<Texture2D>(@"Hud/hudStatF");
            hudStatB = Game.Content.Load<Texture2D>(@"Hud/hudStatB");

            hudMapF = Game.Content.Load<Texture2D>(@"Hud/hudMapF");
            hudMapB = Game.Content.Load<Texture2D>(@"Hud/hudMapB");



            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if(!toggleMap && Keyboard.GetState().IsKeyDown(Keys.M))
            {
                toggleMap = true;
                bigMap = !bigMap;
            }
            if(toggleMap && Keyboard.GetState().IsKeyUp(Keys.M))
            {
                toggleMap = false;
            }

            float fps = 1.0f / Utilities.deltaTime;
            frameRate = fps + " FPS";

            shieldValue = "SHIELDS AT " + (int)game.ship.shipHealth.shieldVal + "%";
            hpValue = game.ship.shipHealth.hpVal + " HP";

            if (game.enemyManager.spawnState == MoonCow.EnemyManager.SpawnState.deploying)
            gameState = "deploying\nenemies";
            else
                gameState = "Press R\nfor enemies";


            moneyTot = "" + Math.Floor(game.ship.moneyManager.displayNo);
            float diff = game.ship.moneyManager.difference;
            if(diff < 0)
                moneyDif = "" + game.ship.moneyManager.difference;
            else
                moneyDif = "+" + game.ship.moneyManager.difference;


            if (flashTime < 10)
            {
                flashTime += Utilities.deltaTime * 60;
                if (flashTime > 15)
                    flashTime = 15;
            }


            if (Keyboard.GetState().IsKeyDown(Keys.Y))
                flashTime = 0;

            minimap.update();

        }

        void drawHealth()
        {
            graphicsDevice.BlendState = BlendState.Additive;
            spriteBatch.Begin();
            spriteBatch.Draw(hudHealthB, scaledRect(healthPos, 603, 104), Color.White);
            spriteBatch.Draw(hudHealthF, scaledRect(healthPos, 603, 104), Color.White);

            spriteBatch.DrawString(font, shieldValue, scaledCoords(shieldBarPos), blueBody);
            spriteBatch.DrawString(font, hpValue, scaledCoords(hpBarPos), redBody);

            spriteBatch.End();
        }

        void drawWep()
        {
            graphicsDevice.BlendState = BlendState.Additive;
            spriteBatch.Begin();
            spriteBatch.Draw(hudWepB, scaledRect(wepPos, 425, 151), Color.White);
            spriteBatch.Draw(hudWepF, scaledRect(wepPos, 425, 151), Color.White);

            if (Keyboard.GetState().IsKeyDown(Keys.Space))
                spriteBatch.DrawString(font, weaponAmmo, scaledCoords(wepAmmoPos), Color.White);
            spriteBatch.End();
        }

        void drawMon()
        {
            float totDim = font.MeasureString(moneyTot).X;
            float diffDim = font.MeasureString(moneyDif).X;


            graphicsDevice.BlendState = BlendState.AlphaBlend;
            spriteBatch.Begin();
            spriteBatch.Draw(hudMonB, scaledRect(monPos, 425, 151), Color.White);
            spriteBatch.Draw(hudMonF, scaledRect(monPos, 425, 151), Color.White);

            spriteBatch.DrawString(font, moneyTot, scaledCoords(new Vector2(monTotPos.X - totDim * 2, monTotPos.Y)), Color.White);
            if (((Game1)Game).ship.moneyManager.changing)
            {
                spriteBatch.DrawString(font, moneyDif, scaledCoords(new Vector2(monDifPos.X - diffDim * 2+3, monDifPos.Y+3)), outline);

                if (((Game1)Game).ship.moneyManager.difference < 0)
                    spriteBatch.DrawString(font, moneyDif, scaledCoords(new Vector2(monDifPos.X-diffDim*2, monDifPos.Y)), redBody);
                else
                    spriteBatch.DrawString(font, moneyDif, scaledCoords(new Vector2(monDifPos.X - diffDim*2, monDifPos.Y)), contSecondary);
            }


            spriteBatch.End();
        }

        void drawStat()
        {
            graphicsDevice.BlendState = BlendState.Additive;
            spriteBatch.Begin();
            spriteBatch.Draw(hudStatB, scaledRect(statPos, 425, 151), Color.White);
            spriteBatch.Draw(hudStatF, scaledRect(statPos, 425, 151), Color.White);
            spriteBatch.DrawString(font, gameState, scaledCoords(statTimePos), Color.White);
            spriteBatch.End();
        }

        void drawMap()
        {
            graphicsDevice.BlendState = BlendState.Additive;
            spriteBatch.Begin();
            spriteBatch.Draw(hudMapB, scaledRect(mapPos, 541, 283), Color.White);
            if(bigMap)
                spriteBatch.Draw(minimap.displayMap, scaledRect(new Vector2(860, 200), minimap.map.Bounds.Width, minimap.map.Bounds.Height), Color.White*0.8f);

            else
                spriteBatch.Draw(minimap.displayMap, scaledRect(new Vector2(1572, 760), minimap.map.Bounds.Width / 2, minimap.map.Bounds.Height / 2), Color.White);
            spriteBatch.Draw(hudMapF, scaledRect(mapPos, 541, 283), Color.White);

            spriteBatch.End();
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

            spriteBatch.End();

            if(flashTime < 10)
            {
                graphicsDevice.BlendState = BlendState.AlphaBlend;
                spriteBatch.Begin();
                spriteBatch.Draw(whiteTex, scaledRect(Vector2.Zero, 1920, 1080), Color.White*(1-flashTime/15.0f));
                spriteBatch.End();
            }


            drawHealth();
            drawWep();
            if(((Game1)Game).ship.moneyManager.display)
                drawMon();
            drawStat();
            drawMap();

            Color myTransparentColor = new Color(0, 0, 0, 127);

            Vector2 stringDimensions = font.MeasureString(frameRate);
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

            spriteBatch.DrawString(font, "" + GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X, new Vector2(300, 300), contPrimary);
            spriteBatch.DrawString(font, "" + GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y, new Vector2(300, 200), contPrimary);


            spriteBatch.DrawString(font, frameRate, position, contPrimary);
            spriteBatch.End();

            graphicsDevice.BlendState = BlendState.Opaque;
            graphicsDevice.DepthStencilState = DepthStencilState.Default;

            base.Draw(gameTime);
        }

        public void startBoost()
        {
            startingBoost = true;
            boostDrawScale = 1.05f;
            boostDrawAlpha = .5f;
        }


    }
}
