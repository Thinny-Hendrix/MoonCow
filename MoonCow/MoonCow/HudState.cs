﻿using System;
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

        WaveManager waveManager;

        public HudState(Hud hud, SpriteFont font, Game1 game):base(hud, font, game)
        {
            statPos = new Vector2(45, 884);
            statNamePos = new Vector2(330, 970);
            statTimePos = new Vector2(330, 1010);
            waveManager = game.waveManager;

            hudStatF = game.Content.Load<Texture2D>(@"Hud/hudStatF");
            hudStatB = game.Content.Load<Texture2D>(@"Hud/hudStatB");
        }

        public override void Update()
        {
            stateTimer = Utilities.formattedTime(waveManager.waitTime);

//                stateTimer = "" + (int)waveManager.activeAttack.waitTime / 60 + ":" + (int)waveManager.activeAttack.waitTime % 60;
            
            if (game.waveManager.spawnState == MoonCow.Utilities.SpawnState.deploying)
                gameState = "defend";
            else
                gameState = "explore";
        }

        public override void Draw(SpriteBatch sb)
        {
            game.GraphicsDevice.BlendState = BlendState.Additive;
            sb.Draw(hudStatB, hud.scaledRect(statPos, 425, 151), Color.White);
            sb.Draw(hudStatF, hud.scaledRect(statPos, 425, 151), Color.White);

            if (game.waveManager.spawnState != Utilities.SpawnState.deploying)
            {
                sb.DrawString(font, gameState, hud.scaledCoords(statNamePos), hud.contSecondary, 0,
                    new Vector2(font.MeasureString(gameState).X / 2, font.MeasureString(gameState).Y / 2), hud.scale * (20.0f / 40), SpriteEffects.None, 0);

                sb.DrawString(font, stateTimer, hud.scaledCoords(statTimePos), Color.White, 0,
                    new Vector2(font.MeasureString(stateTimer).X / 2, font.MeasureString(stateTimer).Y / 2), hud.scale * (32.0f / 40), SpriteEffects.None, 0);
            }
            else
            {
                sb.DrawString(font, gameState, hud.scaledCoords(new Vector2(330,990)), Color.White, 0,
                    new Vector2(font.MeasureString(gameState).X / 2, font.MeasureString(gameState).Y / 2), hud.scale * (24.0f / 40), SpriteEffects.None, 0);
            }
        }


    }
}
