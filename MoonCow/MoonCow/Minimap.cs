using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    public class Minimap
    {
        Game1 game;
        public Texture2D map;
        Texture2D player;
        Texture2D enemy;
        public Texture2D displayMap;

        public RenderTarget2D rTarg;
        RenderTarget2D itemMap;
        SpriteBatch sb;

        public Minimap(Game1 game)
        {
            this.game = game;
            sb = new SpriteBatch(game.GraphicsDevice);
            player = game.Content.Load<Texture2D>(@"Hud/Minimap/shipsmall2");
            enemy = game.Content.Load<Texture2D>(@"Hud/Minimap/enemy1");

        }

        public void update()
        {
            game.GraphicsDevice.SetRenderTarget(itemMap);
            sb.Begin();
            sb.Draw(map, Vector2.Zero, Color.White);
            sb.Draw(player, new Rectangle((int)game.ship.pos.X+15, (int)game.ship.pos.Z+15, 20,20), null, Color.White, -game.ship.rot.Y, new Vector2(10,10), SpriteEffects.None, 1);

            foreach (Enemy e in game.enemyManager.enemies)
            {
                sb.Draw(enemy, new Rectangle((int)e.pos.X+5, (int)e.pos.Z+5, 20, 20), Color.Red);
            }

            sb.End();
            displayMap = (Texture2D)itemMap;
            game.GraphicsDevice.SetRenderTarget(null);

        }

        public void drawMap(MapNode[,] map)
        {
            rTarg = new RenderTarget2D(game.GraphicsDevice, map.GetLength(0) * 30, map.GetLength(1)*30);
            itemMap = new RenderTarget2D(game.GraphicsDevice, map.GetLength(0)*30, map.GetLength(1)*30);
            game.GraphicsDevice.SetRenderTarget(rTarg);
            sb.Begin();
            foreach (MapNode m in map)
            {
                Vector2 mapPos = m.position;
                switch(m.type)
                {
                        //straight
                    case 1:
                        sb.Draw(game.Content.Load<Texture2D>(@"Hud/Minimap/straight"), mapPos * 30, Color.White);
                        break;
                    case 2:
                        sb.Draw(game.Content.Load<Texture2D>(@"Hud/Minimap/straight"), new Rectangle((int)mapPos.X*30+15, (int)mapPos.Y*30+15, 30, 30), null, Color.White, MathHelper.PiOver2,new Vector2(15,15), SpriteEffects.None, 1);
                        break;

                        //dend
                    case 3:
                        sb.Draw(game.Content.Load<Texture2D>(@"Hud/Minimap/dend"), mapPos* 30, Color.White);
                        break;
                    case 4:
                        sb.Draw(game.Content.Load<Texture2D>(@"Hud/Minimap/dend"), new Rectangle((int)mapPos.X * 30 + 15, (int)mapPos.Y * 30 + 15, 30, 30), null, Color.White, MathHelper.PiOver2*3, new Vector2(15, 15), SpriteEffects.None, 1);
                        break;
                    case 5:
                        sb.Draw(game.Content.Load<Texture2D>(@"Hud/Minimap/dend"), new Rectangle((int)mapPos.X * 30 + 15, (int)mapPos.Y * 30 + 15, 30, 30), null, Color.White, MathHelper.Pi, new Vector2(15, 15), SpriteEffects.None, 1);
                        break;
                    case 6:
                        sb.Draw(game.Content.Load<Texture2D>(@"Hud/Minimap/dend"), new Rectangle((int)mapPos.X * 30 + 15, (int)mapPos.Y * 30 + 15, 30, 30), null, Color.White, MathHelper.PiOver2, new Vector2(15, 15), SpriteEffects.None, 1);
                        break;

                    //dend 2
                    case 7:
                        sb.Draw(game.Content.Load<Texture2D>(@"Hud/Minimap/dend2"), mapPos * 30, Color.White);
                        break;
                    case 8:
                        sb.Draw(game.Content.Load<Texture2D>(@"Hud/Minimap/dend2"), new Rectangle((int)mapPos.X * 30 + 15, (int)mapPos.Y * 30 + 15, 30, 30), null, Color.White, MathHelper.PiOver2 * 3, new Vector2(15, 15), SpriteEffects.None, 1);
                        break;
                    case 9:
                        sb.Draw(game.Content.Load<Texture2D>(@"Hud/Minimap/dend2"), new Rectangle((int)mapPos.X * 30 + 15, (int)mapPos.Y * 30 + 15, 30, 30), null, Color.White, MathHelper.Pi, new Vector2(15, 15), SpriteEffects.None, 1);
                        break;
                    case 10:
                        sb.Draw(game.Content.Load<Texture2D>(@"Hud/Minimap/dend2"), new Rectangle((int)mapPos.X * 30 + 15, (int)mapPos.Y * 30 + 15, 30, 30), null, Color.White, MathHelper.PiOver2, new Vector2(15, 15), SpriteEffects.None, 1);
                        break;

                    case 11:
                        sb.Draw(game.Content.Load<Texture2D>(@"Hud/Minimap/tint4"), mapPos * 30, Color.White);
                        break;

                    //tint3
                    case 12:
                        sb.Draw(game.Content.Load<Texture2D>(@"Hud/Minimap/tint3"), mapPos * 30, Color.White);
                        break;
                    case 13:
                        sb.Draw(game.Content.Load<Texture2D>(@"Hud/Minimap/tint3"), new Rectangle((int)mapPos.X * 30 + 15, (int)mapPos.Y * 30 + 15, 30, 30), null, Color.White, MathHelper.PiOver2 * 3, new Vector2(15, 15), SpriteEffects.None, 1);
                        break;
                    case 14:
                        sb.Draw(game.Content.Load<Texture2D>(@"Hud/Minimap/tint3"), new Rectangle((int)mapPos.X * 30 + 15, (int)mapPos.Y * 30 + 15, 30, 30), null, Color.White, MathHelper.Pi, new Vector2(15, 15), SpriteEffects.None, 1);
                        break;
                    case 15:
                        sb.Draw(game.Content.Load<Texture2D>(@"Hud/Minimap/tint3"), new Rectangle((int)mapPos.X * 30 + 15, (int)mapPos.Y * 30 + 15, 30, 30), null, Color.White, MathHelper.PiOver2, new Vector2(15, 15), SpriteEffects.None, 1);
                        break;

                    //corner
                    case 16:
                        sb.Draw(game.Content.Load<Texture2D>(@"Hud/Minimap/corner"), mapPos * 30, Color.White);
                        break;
                    case 17:
                        sb.Draw(game.Content.Load<Texture2D>(@"Hud/Minimap/corner"), new Rectangle((int)mapPos.X * 30 + 15, (int)mapPos.Y * 30 + 15, 30, 30), null, Color.White, MathHelper.PiOver2 * 3, new Vector2(15, 15), SpriteEffects.None, 1);
                        break;
                    case 18:
                        sb.Draw(game.Content.Load<Texture2D>(@"Hud/Minimap/corner"), new Rectangle((int)mapPos.X * 30 + 15, (int)mapPos.Y * 30 + 15, 30, 30), null, Color.White, MathHelper.Pi, new Vector2(15, 15), SpriteEffects.None, 1);
                        break;
                    case 19:
                        sb.Draw(game.Content.Load<Texture2D>(@"Hud/Minimap/corner"), new Rectangle((int)mapPos.X * 30 + 15, (int)mapPos.Y * 30 + 15, 30, 30), null, Color.White, MathHelper.PiOver2, new Vector2(15, 15), SpriteEffects.None, 1);
                        break;

                    //base tiles
                    case 20:
                        sb.Draw(game.Content.Load<Texture2D>(@"Hud/Minimap/20"), mapPos * 30, Color.White);
                        break;
                    case 21:
                        sb.Draw(game.Content.Load<Texture2D>(@"Hud/Minimap/21"), mapPos * 30, Color.White);
                        break;
                    case 22:
                        sb.Draw(game.Content.Load<Texture2D>(@"Hud/Minimap/22"), mapPos * 30, Color.White);
                        break;
                    case 23:
                        sb.Draw(game.Content.Load<Texture2D>(@"Hud/Minimap/23"), mapPos * 30, Color.White);
                        break;
                    case 24:
                        sb.Draw(game.Content.Load<Texture2D>(@"Hud/Minimap/24"), mapPos * 30, Color.White);
                        break;
                    case 25:
                        sb.Draw(game.Content.Load<Texture2D>(@"Hud/Minimap/25"), mapPos * 30, Color.White);
                        break;
                    case 26:
                        sb.Draw(game.Content.Load<Texture2D>(@"Hud/Minimap/26"), mapPos * 30, Color.White);
                        break;
                    case 27:
                        sb.Draw(game.Content.Load<Texture2D>(@"Hud/Minimap/27"), mapPos * 30, Color.White);
                        break;
                    case 28:
                        sb.Draw(game.Content.Load<Texture2D>(@"Hud/Minimap/28"), mapPos * 30, Color.White);
                        break;
                    case 29:
                        sb.Draw(game.Content.Load<Texture2D>(@"Hud/Minimap/29"), mapPos * 30, Color.White);
                        break;
                    case 30:
                        sb.Draw(game.Content.Load<Texture2D>(@"Hud/Minimap/30"), mapPos * 30, Color.White);
                        break;
                    case 31:
                        sb.Draw(game.Content.Load<Texture2D>(@"Hud/Minimap/31"), mapPos * 30, Color.White);
                        break;
                    case 32:
                        sb.Draw(game.Content.Load<Texture2D>(@"Hud/Minimap/32"), mapPos * 30, Color.White);
                        break;
                    case 33:
                        sb.Draw(game.Content.Load<Texture2D>(@"Hud/Minimap/33"), mapPos * 30, Color.White);
                        break;
                    case 34:
                        sb.Draw(game.Content.Load<Texture2D>(@"Hud/Minimap/34"), mapPos * 30, Color.White);
                        break;

                    //ast corner
                    case 35:
                        sb.Draw(game.Content.Load<Texture2D>(@"Hud/Minimap/cor1big"), mapPos * 30, Color.White);
                        break;
                    case 36:
                        sb.Draw(game.Content.Load<Texture2D>(@"Hud/Minimap/cor1big"), new Rectangle((int)mapPos.X * 30 + 15, (int)mapPos.Y * 30 + 15, 30, 30), null, Color.White, MathHelper.PiOver2 * 3, new Vector2(15, 15), SpriteEffects.None, 1);
                        break;
                    case 37:
                        sb.Draw(game.Content.Load<Texture2D>(@"Hud/Minimap/cor1big"), new Rectangle((int)mapPos.X * 30 + 15, (int)mapPos.Y * 30 + 15, 30, 30), null, Color.White, MathHelper.Pi, new Vector2(15, 15), SpriteEffects.None, 1);
                        break;
                    case 38:
                        sb.Draw(game.Content.Load<Texture2D>(@"Hud/Minimap/cor1big"), new Rectangle((int)mapPos.X * 30 + 15, (int)mapPos.Y * 30 + 15, 30, 30), null, Color.White, MathHelper.PiOver2, new Vector2(15, 15), SpriteEffects.None, 1);
                        break;

                    //ast 1 straight
                    case 39:
                        sb.Draw(game.Content.Load<Texture2D>(@"Hud/Minimap/straight1"), mapPos * 30, Color.White);
                        break;
                    case 40:
                        sb.Draw(game.Content.Load<Texture2D>(@"Hud/Minimap/straight1"), new Rectangle((int)mapPos.X * 30 + 15, (int)mapPos.Y * 30 + 15, 30, 30), null, Color.White, MathHelper.PiOver2 * 3, new Vector2(15, 15), SpriteEffects.None, 1);
                        break;
                    case 41:
                        sb.Draw(game.Content.Load<Texture2D>(@"Hud/Minimap/straight1"), new Rectangle((int)mapPos.X * 30 + 15, (int)mapPos.Y * 30 + 15, 30, 30), null, Color.White, MathHelper.Pi, new Vector2(15, 15), SpriteEffects.None, 1);
                        break;
                    case 42:
                        sb.Draw(game.Content.Load<Texture2D>(@"Hud/Minimap/straight1"), new Rectangle((int)mapPos.X * 30 + 15, (int)mapPos.Y * 30 + 15, 30, 30), null, Color.White, MathHelper.PiOver2, new Vector2(15, 15), SpriteEffects.None, 1);
                        break;

                    //ast 2 cor
                    case 43:
                        sb.Draw(game.Content.Load<Texture2D>(@"Hud/Minimap/cor2small"), mapPos * 30, Color.White);
                        break;
                    case 44:
                        sb.Draw(game.Content.Load<Texture2D>(@"Hud/Minimap/cor2small"), new Rectangle((int)mapPos.X * 30 + 15, (int)mapPos.Y * 30 + 15, 30, 30), null, Color.White, MathHelper.PiOver2 * 3, new Vector2(15, 15), SpriteEffects.None, 1);
                        break;
                    case 45:
                        sb.Draw(game.Content.Load<Texture2D>(@"Hud/Minimap/cor2small"), new Rectangle((int)mapPos.X * 30 + 15, (int)mapPos.Y * 30 + 15, 30, 30), null, Color.White, MathHelper.Pi, new Vector2(15, 15), SpriteEffects.None, 1);
                        break;
                    case 46:
                        sb.Draw(game.Content.Load<Texture2D>(@"Hud/Minimap/cor2small"), new Rectangle((int)mapPos.X * 30 + 15, (int)mapPos.Y * 30 + 15, 30, 30), null, Color.White, MathHelper.PiOver2, new Vector2(15, 15), SpriteEffects.None, 1);
                        break;

                    //ast 1 cor
                    case 47:
                        sb.Draw(game.Content.Load<Texture2D>(@"Hud/Minimap/cor1small"), mapPos * 30, Color.White);
                        break;
                    case 48:
                        sb.Draw(game.Content.Load<Texture2D>(@"Hud/Minimap/cor1small"), new Rectangle((int)mapPos.X * 30 + 15, (int)mapPos.Y * 30 + 15, 30, 30), null, Color.White, MathHelper.PiOver2 * 3, new Vector2(15, 15), SpriteEffects.None, 1);
                        break;
                    case 49:
                        sb.Draw(game.Content.Load<Texture2D>(@"Hud/Minimap/cor1small"), new Rectangle((int)mapPos.X * 30 + 15, (int)mapPos.Y * 30 + 15, 30, 30), null, Color.White, MathHelper.Pi, new Vector2(15, 15), SpriteEffects.None, 1);
                        break;
                    case 50:
                        sb.Draw(game.Content.Load<Texture2D>(@"Hud/Minimap/cor1small"), new Rectangle((int)mapPos.X * 30 + 15, (int)mapPos.Y * 30 + 15, 30, 30), null, Color.White, MathHelper.PiOver2, new Vector2(15, 15), SpriteEffects.None, 1);
                        break;

                    //ast corst
                    case 51:
                        sb.Draw(game.Content.Load<Texture2D>(@"Hud/Minimap/corst"), mapPos * 30, Color.White);
                        break;
                    case 52:
                        sb.Draw(game.Content.Load<Texture2D>(@"Hud/Minimap/corst"), new Rectangle((int)mapPos.X * 30 + 15, (int)mapPos.Y * 30 + 15, 30, 30), null, Color.White, MathHelper.PiOver2 * 3, new Vector2(15, 15), SpriteEffects.None, 1);
                        break;
                    case 53:
                        sb.Draw(game.Content.Load<Texture2D>(@"Hud/Minimap/corst"), new Rectangle((int)mapPos.X * 30 + 15, (int)mapPos.Y * 30 + 15, 30, 30), null, Color.White, MathHelper.Pi, new Vector2(15, 15), SpriteEffects.None, 1);
                        break;
                    case 54:
                        sb.Draw(game.Content.Load<Texture2D>(@"Hud/Minimap/corst"), new Rectangle((int)mapPos.X * 30 + 15, (int)mapPos.Y * 30 + 15, 30, 30), null, Color.White, MathHelper.PiOver2, new Vector2(15, 15), SpriteEffects.None, 1);
                        break;

                    //ast corst flipped
                    case 55:
                        sb.Draw(game.Content.Load<Texture2D>(@"Hud/Minimap/corstflip"), mapPos * 30, Color.White);
                        break;
                    case 56:
                        sb.Draw(game.Content.Load<Texture2D>(@"Hud/Minimap/corstflip"), new Rectangle((int)mapPos.X * 30 + 15, (int)mapPos.Y * 30 + 15, 30, 30), null, Color.White, MathHelper.PiOver2 * 3, new Vector2(15, 15), SpriteEffects.None, 1);
                        break;
                    case 57:
                        sb.Draw(game.Content.Load<Texture2D>(@"Hud/Minimap/corstflip"), new Rectangle((int)mapPos.X * 30 + 15, (int)mapPos.Y * 30 + 15, 30, 30), null, Color.White, MathHelper.Pi, new Vector2(15, 15), SpriteEffects.None, 1);
                        break;
                    case 58:
                        sb.Draw(game.Content.Load<Texture2D>(@"Hud/Minimap/corstflip"), new Rectangle((int)mapPos.X * 30 + 15, (int)mapPos.Y * 30 + 15, 30, 30), null, Color.White, MathHelper.PiOver2, new Vector2(15, 15), SpriteEffects.None, 1);
                        break;


                    case 59:
                        sb.Draw(game.Content.Load<Texture2D>(@"Hud/Minimap/59"), mapPos * 30, Color.White);
                        break;
                    case 60:
                    default:
                        sb.Draw(game.Content.Load<Texture2D>(@"Hud/Minimap/60"), mapPos * 30, Color.White);
                        break;

                }
            }
            sb.End();

            this.map = (Texture2D)rTarg;
            //displayMap = this.map;
            game.GraphicsDevice.SetRenderTarget(null);
        }
    }
}
