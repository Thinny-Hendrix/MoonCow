﻿using System;
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
        public Minimap minimap;
        SpriteFont font;
        Game1 game;
        Ship ship;
        public SpriteBatch spriteBatch;
        public GraphicsDevice graphicsDevice;
        public HudAttackDisplayer hudAttackDisplayer;

        public HudMoney hudMoney;

        QuickSelect quickSelect;

        Vector2 position;
        bool rStickToggle;
        bool mToggle;
        bool bigMap;
        bool startingBoost;
        float boostDrawScale;
        float boostDrawAlpha;

        
        String frameRate;
        String gameState;
        String stateTimer;
        String weaponAmmo;
        String shieldValue;
        String hpValue;
        String roundStart;
        String message;

        public Color contPrimary;
        public Color contSecondary;
        public Color outline;
        public Color fill;
        public Color redBody;
        public Color blueBody;

        Texture2D hudHealthB;
        Texture2D hudHealthF;
        Texture2D hudWepB;
        Texture2D hudWepF;
        Texture2D hudStatB;
        Texture2D hudStatF;
        Texture2D hudMapB;
        Texture2D hudMapF;


        Texture2D whiteTex;
        public float flashTime;
        public float scale;

        Vector2 wepPos;
        Vector2 wepAmmoPos;

        

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
            ship = game.ship;
            this.font = font;
            this.spriteBatch = spriteBatch;
            this.graphicsDevice = graphicsDevice;
            hudMoney = new HudMoney(this, font, game);
            minimap = new Minimap(game);

            viewportW = game.GraphicsDevice.Viewport.Width;
            viewportH = game.GraphicsDevice.Viewport.Height;

            hudAttackDisplayer = new HudAttackDisplayer(game, this);
            quickSelect = new QuickSelect(this, game, font);

            position = new Vector2(0, 0);
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
            

            statPos = new Vector2(45, 884);
            statTimePos = new Vector2(150, 950);
            mapPos = new Vector2(1334, 752);

            whiteTex = new Texture2D(graphicsDevice, 1, 1);
            whiteTex.SetData(new Color[] { Color.White });
            flashTime = 15;

            weaponAmmo = ship.weapons.activeWeapon.ammo + " / " + ship.weapons.activeWeapon.ammoMax;
            scale = (float)game.GraphicsDevice.Viewport.Bounds.Width / 1920.0f;
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

            hudHealthF = Game.Content.Load<Texture2D>(@"Hud/hudHealthF");
            hudHealthB = Game.Content.Load<Texture2D>(@"Hud/hudHealthB");

            hudWepF = Game.Content.Load<Texture2D>(@"Hud/hudWepF");
            hudWepB = Game.Content.Load<Texture2D>(@"Hud/hudWepB");

            hudStatF = Game.Content.Load<Texture2D>(@"Hud/hudStatF");
            hudStatB = Game.Content.Load<Texture2D>(@"Hud/hudStatB");

            hudMapF = Game.Content.Load<Texture2D>(@"Hud/hudMapF");
            hudMapB = Game.Content.Load<Texture2D>(@"Hud/hudMapB");



            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if(!mToggle && Keyboard.GetState().IsKeyDown(Keys.M))
            {
                mToggle = true;
                bigMap = !bigMap;
            }
            if(!rStickToggle && 
                GamePad.GetState(PlayerIndex.One).Buttons.RightStick == ButtonState.Pressed)
            {
                rStickToggle = true;
                bigMap = !bigMap;
            }

            if(mToggle && (Keyboard.GetState().IsKeyUp(Keys.M)))
                mToggle = false;

            if (rStickToggle && GamePad.GetState(PlayerIndex.One).Buttons.RightStick == ButtonState.Released)
                rStickToggle = false;


            float fps = 1.0f / Utilities.frameRate;
            frameRate = fps + " FPS";

            weaponAmmo = ship.weapons.activeWeapon.ammo + " / " + ship.weapons.activeWeapon.ammoMax;

            shieldValue = "SHIELDS AT " + (int)game.ship.shipHealth.shieldVal + "%";
            hpValue = game.ship.shipHealth.hpVal + " HP";

            if (game.enemyManager.spawnState == MoonCow.EnemyManager.SpawnState.deploying)
            gameState = "deploying\nenemies";
            else
                gameState = "Press R\nfor enemies";


            


            if (flashTime < 10)
            {
                flashTime += Utilities.deltaTime * 60;
                if (flashTime > 15)
                    flashTime = 15;
            }


            if (Keyboard.GetState().IsKeyDown(Keys.Y))
                makeFlash();

            minimap.update();
            hudAttackDisplayer.Update();
            hudMoney.Update();
            quickSelect.Update();

        }

        public void makeFlash()
        {
            flashTime = 0;
        }

        void drawHealth()
        {
            graphicsDevice.BlendState = BlendState.Additive;
            spriteBatch.Begin();
            spriteBatch.Draw(hudHealthB, scaledRect(healthPos, 603, 104), Color.White);
            spriteBatch.Draw(hudHealthF, scaledRect(healthPos, 603, 104), Color.White);

            spriteBatch.DrawString(font, shieldValue, scaledCoords(shieldBarPos), blueBody, 0,
                new Vector2(font.MeasureString(shieldValue).X / 2, font.MeasureString(shieldValue).Y / 2), scale, SpriteEffects.None, 0);


           // spriteBatch.DrawString(font, shieldValue, scaledCoords(shieldBarPos), blueBody);
            //spriteBatch.DrawString(font, hpValue, scaledCoords(hpBarPos), redBody);

            spriteBatch.End();
        }

        void drawWep()
        {
            graphicsDevice.BlendState = BlendState.Additive;
            spriteBatch.Begin();
            spriteBatch.Draw(hudWepB, scaledRect(wepPos, 425, 151), Color.White);
            spriteBatch.Draw(hudWepF, scaledRect(wepPos, 425, 151), Color.White);
            spriteBatch.Draw(ship.weapons.activeWeapon.icon, scaledRect(new Vector2(140, 115), 90, 90),
                    null, Color.White, 0, new Vector2(45, 45), SpriteEffects.None, 0);

            spriteBatch.DrawString(font, weaponAmmo, scaledCoords(wepAmmoPos), Color.White, 0,
                new Vector2(font.MeasureString(weaponAmmo).X / 2, font.MeasureString(weaponAmmo).Y / 2), scale, SpriteEffects.None, 0);
            spriteBatch.End();
        }

        void drawMon()
        {
            
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
            if (bigMap)
                spriteBatch.Draw(minimap.displayMap, scaledRect(new Vector2(960,540), minimap.map.Bounds.Width, minimap.map.Bounds.Height), 
                    null, Color.White * 0.8f, 0, new Vector2(minimap.map.Bounds.Width/2,minimap.map.Bounds.Height/2), SpriteEffects.None, 0);
            else
            {
                spriteBatch.Draw(hudMapB, scaledRect(mapPos, 541, 283), Color.White);
                spriteBatch.Draw(minimap.displayMap, scaledRect(new Vector2(1572, 760), minimap.map.Bounds.Width / 2, minimap.map.Bounds.Height / 2), Color.White);
                spriteBatch.Draw(hudMapF, scaledRect(mapPos, 541, 283), Color.White);
            }

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
            spriteBatch.Begin();
            hudMoney.Draw(spriteBatch);
            spriteBatch.End();
            drawMon();
            drawStat();
            drawMap();

            quickSelect.Draw(spriteBatch);

            hudAttackDisplayer.Draw();

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

            //spriteBatch.DrawString(font, "" + GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X, new Vector2(300, 300), contPrimary);
            //spriteBatch.DrawString(font, "" + GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y, new Vector2(300, 200), contPrimary);


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
