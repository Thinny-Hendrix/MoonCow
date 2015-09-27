using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    public class HudState:HudComponent
    {
        String gameState;
        String stateTimer;

        Texture2D hudStatB;
        Texture2D hudStatF;

        Vector2 statPos;
        Vector2 statNamePos;
        Vector2 statTimePos;
        public HudState(Hud hud, SpriteFont font, Game1 game):base(hud, font, game)
        {
            statPos = new Vector2(45, 884);
            statNamePos = new Vector2(200, 980);
            statTimePos = new Vector2(250, 1000);

            hudStatF = game.Content.Load<Texture2D>(@"Hud/hudStatF");
            hudStatB = game.Content.Load<Texture2D>(@"Hud/hudStatB");
        }

        public override void Update()
        {
            stateTimer = "2:30";
            if (game.waveManager.spawnState == MoonCow.WaveManager.SpawnState.deploying)
                gameState = "defend";
            else
                gameState = "explore";
        }

        public override void Draw(SpriteBatch sb)
        {
            game.GraphicsDevice.BlendState = BlendState.Additive;
            sb.Draw(hudStatB, hud.scaledRect(statPos, 425, 151), Color.White);
            sb.Draw(hudStatF, hud.scaledRect(statPos, 425, 151), Color.White);

            sb.DrawString(font, gameState, hud.scaledCoords(statNamePos), hud.contSecondary, 0,
                    new Vector2(font.MeasureString(gameState).X / 2, font.MeasureString(gameState).Y / 2), hud.scale * (28.0f / 40), SpriteEffects.None, 0);

            sb.DrawString(font, stateTimer, hud.scaledCoords(statTimePos), hud.contSecondary, 0,
                    new Vector2(font.MeasureString(stateTimer).X / 2, font.MeasureString(stateTimer).Y / 2), hud.scale * (32.0f / 40), SpriteEffects.None, 0);
        }


    }
}
