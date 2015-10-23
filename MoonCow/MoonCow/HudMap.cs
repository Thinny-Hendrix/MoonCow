using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MoonCow
{
    public class HudMap:HudComponent
    {
        bool rStickToggle;
        bool mToggle;
        bool bigMap;
        public Minimap minimap;
        string coreHealth;

        Texture2D hudMapB;
        Texture2D hudMapF;
        Texture2D mask;

        Vector2 mapPos;

        Effect alphaMap;
        RenderTarget2D rTarg;
        RenderTarget2D mapTarg;
        RenderTarget2D barTarg;
        RenderTarget2D barMaskTarg;
        Texture2D barMask;
        SpriteBatch spB;

        public HudMap(Hud hud, SpriteFont font, Game1 game)
            : base(hud, font, game)
        {
            minimap = new Minimap(game);

            mapPos = new Vector2(1455, 752);
            hudMapF = game.Content.Load<Texture2D>(@"Hud/mapF");
            mask = game.Content.Load<Texture2D>(@"Hud/mapMask");
            hudMapB = game.Content.Load<Texture2D>(@"Hud/mapO");
            alphaMap = game.Content.Load<Effect>(@"Effects/AlphaMap");
            barMask = game.Content.Load<Texture2D>(@"Hud/Masks/mapBar");

            barTarg = new RenderTarget2D(game.GraphicsDevice, 272, 16);
            rTarg = new RenderTarget2D(game.GraphicsDevice, 402, 283);
            mapTarg = new RenderTarget2D(game.GraphicsDevice, 402, 283);
            spB = new SpriteBatch(game.GraphicsDevice);
        }

        public override void Update()
        {
            minimap.update();

            coreHealth = game.core.health + "/1000";

            if (!mToggle && Keyboard.GetState().IsKeyDown(Keys.M))
            {
                mToggle = true;
                bigMap = !bigMap;
            }
            if (!rStickToggle &&
                GamePad.GetState(PlayerIndex.One).Buttons.RightStick == ButtonState.Pressed)
            {
                rStickToggle = true;
                bigMap = !bigMap;
            }

            if (mToggle && (Keyboard.GetState().IsKeyUp(Keys.M)))
                mToggle = false;

            if (rStickToggle && GamePad.GetState(PlayerIndex.One).Buttons.RightStick == ButtonState.Released)
                rStickToggle = false;

            float scale = game.core.health / game.core.maxHealth;
            Color c = hud.contSecondary;
            if(scale < 0.3f)
                c = hud.redBody;

            game.GraphicsDevice.SetRenderTarget(barTarg);
            game.GraphicsDevice.Clear(hud.blueBody);
            spB.Begin();
            spB.Draw(TextureManager.pureWhite, new Rectangle(0, 0, (int)Math.Ceiling(272*scale), 16), c);
            spB.End();


            game.GraphicsDevice.SetRenderTarget(mapTarg);
            game.GraphicsDevice.Clear(Color.Transparent);
            spB.Begin();
            spB.Draw(minimap.displayMap, new Rectangle(201, 180, minimap.map.Bounds.Width, minimap.map.Bounds.Height), null, Color.White, -minimap.shipRot, minimap.shipPos, SpriteEffects.None, 1);
            spB.End();

            game.GraphicsDevice.SetRenderTarget(rTarg);
            game.GraphicsDevice.Clear(Color.Transparent);
            alphaMap.Parameters["MaskTexture"].SetValue(mask);

            // start a spritebatch for our effect
            spB.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend,
                null, null, null, alphaMap);
            //spB.Begin();
            spB.Draw((Texture2D)mapTarg, Vector2.Zero, Color.White);
            //spB.Draw(TextureManager.artefact, new Vector2(-30,-60), Color.White);
            alphaMap.Parameters["MaskTexture"].SetValue(barMask);
            spB.Draw((Texture2D)barTarg, new Vector2(68,262), Color.White);


            spB.End();
            game.GraphicsDevice.SetRenderTarget(null);
        }

        public override void Draw(SpriteBatch sb)
        {
            game.GraphicsDevice.BlendState = BlendState.Additive;
            if (bigMap)
                sb.Draw(minimap.displayMap, hud.scaledRect(new Vector2(960, 540), minimap.map.Bounds.Width, minimap.map.Bounds.Height),
                    null, Color.White * 0.8f, 0, new Vector2(minimap.map.Bounds.Width / 2, minimap.map.Bounds.Height / 2), SpriteEffects.None, 0);
            else
            {


                sb.Draw(hudMapF, hud.scaledRect(mapPos, 402, 283), Color.White);
                //sb.Draw((Texture2D)rTarg, hud.scaledRect(Vector2.Zero, 402, 283), Color.White);
                sb.Draw((Texture2D)rTarg, hud.scaledRect(mapPos,402,283), Color.White);
                //sb.Draw(minimap.displayMap, hud.scaledRect(new Vector2(1675, 930), minimap.map.Bounds.Width, minimap.map.Bounds.Height), null, Color.White, -minimap.shipRot, minimap.shipPos, SpriteEffects.None, 1);
                sb.Draw(hudMapB, hud.scaledRect(mapPos, 402, 283), Color.White);
                /*sb.DrawString(font, coreHealth, hud.scaledCoords(new Vector2(1750, 1030)), hud.redBody, 0,
                    new Vector2(font.MeasureString(coreHealth).X, font.MeasureString(coreHealth).Y / 2), hud.scale * (16.0f / 40), SpriteEffects.None, 0);*/
            }

        }
    }
}
