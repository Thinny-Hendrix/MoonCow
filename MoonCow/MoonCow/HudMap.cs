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

        Texture2D hudMapB;
        Texture2D hudMapF;

        Vector2 mapPos;

        public HudMap(Hud hud, SpriteFont font, Game1 game)
            : base(hud, font, game)
        {
            minimap = new Minimap(game);

            mapPos = new Vector2(1334, 752);
            hudMapF = game.Content.Load<Texture2D>(@"Hud/hudMapF");
            hudMapB = game.Content.Load<Texture2D>(@"Hud/hudMapB");
        }

        public override void Update()
        {
            minimap.update();

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
        }

        public override void Draw(SpriteBatch sb)
        {
            game.GraphicsDevice.BlendState = BlendState.Additive;
            if (bigMap)
                sb.Draw(minimap.displayMap, hud.scaledRect(new Vector2(960, 540), minimap.map.Bounds.Width, minimap.map.Bounds.Height),
                    null, Color.White * 0.8f, 0, new Vector2(minimap.map.Bounds.Width / 2, minimap.map.Bounds.Height / 2), SpriteEffects.None, 0);
            else
            {
                sb.Draw(hudMapB, hud.scaledRect(mapPos, 541, 283), Color.White);
                sb.Draw(minimap.displayMap, hud.scaledRect(new Vector2(1675, 930), minimap.map.Bounds.Width, minimap.map.Bounds.Height), null, Color.White, -minimap.shipRot, minimap.shipPos, SpriteEffects.None, 1);
                sb.Draw(hudMapF, hud.scaledRect(mapPos, 541, 283), Color.White);
            }

        }
    }
}
