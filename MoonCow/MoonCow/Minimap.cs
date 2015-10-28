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

        public Vector2 shipPos;
        public float shipRot;

        public Minimap(Game1 game)
        {
            this.game = game;
            sb = new SpriteBatch(game.GraphicsDevice);
            player = game.Content.Load<Texture2D>(@"Hud/Minimap/shipsmall2");
            enemy = game.Content.Load<Texture2D>(@"Hud/Minimap/enemy1");

        }

        public void update()
        {
            shipPos = new Vector2((int)game.ship.pos.X + 15, (int)game.ship.pos.Z+15);
            shipRot = -game.ship.rot.Y;
            game.GraphicsDevice.SetRenderTarget(itemMap);
            game.GraphicsDevice.Clear(Color.Transparent);
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
            game.GraphicsDevice.Clear(Color.Transparent);
            sb.Begin();
            foreach (MapNode m in map)
            {
                Vector2 mapPos = m.position;
                switch(m.type)
                {
                        //straight
                    case 1:
                        sb.Draw(LcAssets.t1, mapPos * 30, Color.White);
                        break;
                    case 2:
                    case 62:
                        sb.Draw(LcAssets.t1, new Rectangle((int)mapPos.X*30+15, (int)mapPos.Y*30+15, 30, 30), null, Color.White, MathHelper.PiOver2,new Vector2(15,15), SpriteEffects.None, 1);
                        break;

                        //dend
                    case 3:
                        sb.Draw(LcAssets.t3, mapPos * 30, Color.White);
                        break;
                    case 4:
                        sb.Draw(LcAssets.t4, mapPos * 30, Color.White);
                        break;
                    case 5:
                        sb.Draw(LcAssets.t5, mapPos * 30, Color.White);
                        break;
                    case 6:
                        sb.Draw(LcAssets.t6, mapPos * 30, Color.White);
                        break;

                    //dend 2
                    case 7:
                        sb.Draw(LcAssets.t7, mapPos * 30, Color.White);
                        break;
                    case 8:
                        sb.Draw(LcAssets.t8, mapPos * 30, Color.White);
                        break;
                    case 9:
                        sb.Draw(LcAssets.t9, mapPos * 30, Color.White);
                        break;
                    case 10:
                        sb.Draw(LcAssets.t10, mapPos * 30, Color.White);
                        break;

                    case 11:
                        sb.Draw(LcAssets.t11, mapPos * 30, Color.White);
                        break;

                    //tint3
                    case 12:
                        sb.Draw(LcAssets.t12, mapPos * 30, Color.White);
                        break;
                    case 13:
                        sb.Draw(LcAssets.t12, new Rectangle((int)mapPos.X * 30 + 15, (int)mapPos.Y * 30 + 15, 30, 30), null, Color.White, MathHelper.PiOver2 * 3, new Vector2(15, 15), SpriteEffects.None, 1);
                        break;
                    case 14:
                        sb.Draw(LcAssets.t12, new Rectangle((int)mapPos.X * 30 + 15, (int)mapPos.Y * 30 + 15, 30, 30), null, Color.White, MathHelper.Pi, new Vector2(15, 15), SpriteEffects.None, 1);
                        break;
                    case 15:
                        sb.Draw(LcAssets.t12, new Rectangle((int)mapPos.X * 30 + 15, (int)mapPos.Y * 30 + 15, 30, 30), null, Color.White, MathHelper.PiOver2, new Vector2(15, 15), SpriteEffects.None, 1);
                        break;

                    //corner
                    case 16:
                        sb.Draw(LcAssets.t16, mapPos * 30, Color.White);
                        break;
                    case 17:
                        sb.Draw(LcAssets.t16, new Rectangle((int)mapPos.X * 30 + 15, (int)mapPos.Y * 30 + 15, 30, 30), null, Color.White, MathHelper.PiOver2 * 3, new Vector2(15, 15), SpriteEffects.None, 1);
                        break;
                    case 18:
                        sb.Draw(LcAssets.t16, new Rectangle((int)mapPos.X * 30 + 15, (int)mapPos.Y * 30 + 15, 30, 30), null, Color.White, MathHelper.Pi, new Vector2(15, 15), SpriteEffects.None, 1);
                        break;
                    case 19:
                        sb.Draw(LcAssets.t16, new Rectangle((int)mapPos.X * 30 + 15, (int)mapPos.Y * 30 + 15, 30, 30), null, Color.White, MathHelper.PiOver2, new Vector2(15, 15), SpriteEffects.None, 1);
                        break;

                    //base tiles
                    case 20:
                        sb.Draw(LcAssets.t20, mapPos * 30, Color.White);
                        break;
                    case 21:
                        sb.Draw(LcAssets.t21, mapPos * 30, Color.White);
                        break;
                    case 22:
                        sb.Draw(LcAssets.t22, mapPos * 30, Color.White);
                        break;
                    case 23:
                        sb.Draw(LcAssets.t23, mapPos * 30, Color.White);
                        break;
                    case 24:
                        sb.Draw(LcAssets.t24, mapPos * 30, Color.White);
                        break;
                    case 25:
                        sb.Draw(LcAssets.t25, mapPos * 30, Color.White);
                        break;
                    case 26:
                        sb.Draw(LcAssets.t26, mapPos * 30, Color.White);
                        break;
                    case 27:
                        sb.Draw(LcAssets.t27, mapPos * 30, Color.White);
                        break;
                    case 28:
                        sb.Draw(LcAssets.t28, mapPos * 30, Color.White);
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
                        sb.Draw(LcAssets.t35, mapPos * 30, Color.White);
                        break;
                    case 36:
                        sb.Draw(LcAssets.t35, new Rectangle((int)mapPos.X * 30 + 15, (int)mapPos.Y * 30 + 15, 30, 30), null, Color.White, MathHelper.PiOver2 * 3, new Vector2(15, 15), SpriteEffects.None, 1);
                        break;
                    case 37:
                        sb.Draw(LcAssets.t35, new Rectangle((int)mapPos.X * 30 + 15, (int)mapPos.Y * 30 + 15, 30, 30), null, Color.White, MathHelper.Pi, new Vector2(15, 15), SpriteEffects.None, 1);
                        break;
                    case 38:
                        sb.Draw(LcAssets.t35, new Rectangle((int)mapPos.X * 30 + 15, (int)mapPos.Y * 30 + 15, 30, 30), null, Color.White, MathHelper.PiOver2, new Vector2(15, 15), SpriteEffects.None, 1);
                        break;

                    //ast 1 straight
                    case 39:
                        sb.Draw(LcAssets.t39, mapPos * 30, Color.White);
                        break;
                    case 40:
                        sb.Draw(LcAssets.t39, new Rectangle((int)mapPos.X * 30 + 15, (int)mapPos.Y * 30 + 15, 30, 30), null, Color.White, MathHelper.PiOver2 * 3, new Vector2(15, 15), SpriteEffects.None, 1);
                        break;
                    case 41:
                        sb.Draw(LcAssets.t39, new Rectangle((int)mapPos.X * 30 + 15, (int)mapPos.Y * 30 + 15, 30, 30), null, Color.White, MathHelper.Pi, new Vector2(15, 15), SpriteEffects.None, 1);
                        break;
                    case 42:
                        sb.Draw(LcAssets.t39, new Rectangle((int)mapPos.X * 30 + 15, (int)mapPos.Y * 30 + 15, 30, 30), null, Color.White, MathHelper.PiOver2, new Vector2(15, 15), SpriteEffects.None, 1);
                        break;

                    //ast 2 cor
                    case 43:
                        sb.Draw(LcAssets.t43, mapPos * 30, Color.White);
                        break;
                    case 44:
                        sb.Draw(LcAssets.t43, new Rectangle((int)mapPos.X * 30 + 15, (int)mapPos.Y * 30 + 15, 30, 30), null, Color.White, MathHelper.PiOver2 * 3, new Vector2(15, 15), SpriteEffects.None, 1);
                        break;
                    case 45:
                        sb.Draw(LcAssets.t43, new Rectangle((int)mapPos.X * 30 + 15, (int)mapPos.Y * 30 + 15, 30, 30), null, Color.White, MathHelper.Pi, new Vector2(15, 15), SpriteEffects.None, 1);
                        break;
                    case 46:
                        sb.Draw(LcAssets.t43, new Rectangle((int)mapPos.X * 30 + 15, (int)mapPos.Y * 30 + 15, 30, 30), null, Color.White, MathHelper.PiOver2, new Vector2(15, 15), SpriteEffects.None, 1);
                        break;

                    //ast 1 cor
                    case 47:
                        sb.Draw(LcAssets.t47, mapPos * 30, Color.White);
                        break;
                    case 48:
                        sb.Draw(LcAssets.t47, new Rectangle((int)mapPos.X * 30 + 15, (int)mapPos.Y * 30 + 15, 30, 30), null, Color.White, MathHelper.PiOver2 * 3, new Vector2(15, 15), SpriteEffects.None, 1);
                        break;
                    case 49:
                        sb.Draw(LcAssets.t47, new Rectangle((int)mapPos.X * 30 + 15, (int)mapPos.Y * 30 + 15, 30, 30), null, Color.White, MathHelper.Pi, new Vector2(15, 15), SpriteEffects.None, 1);
                        break;
                    case 50:
                        sb.Draw(LcAssets.t47, new Rectangle((int)mapPos.X * 30 + 15, (int)mapPos.Y * 30 + 15, 30, 30), null, Color.White, MathHelper.PiOver2, new Vector2(15, 15), SpriteEffects.None, 1);
                        break;

                    //ast corst
                    case 51:
                        sb.Draw(LcAssets.t51, mapPos * 30, Color.White);
                        break;
                    case 52:
                        sb.Draw(LcAssets.t51, new Rectangle((int)mapPos.X * 30 + 15, (int)mapPos.Y * 30 + 15, 30, 30), null, Color.White, MathHelper.PiOver2 * 3, new Vector2(15, 15), SpriteEffects.None, 1);
                        break;
                    case 53:
                        sb.Draw(LcAssets.t51, new Rectangle((int)mapPos.X * 30 + 15, (int)mapPos.Y * 30 + 15, 30, 30), null, Color.White, MathHelper.Pi, new Vector2(15, 15), SpriteEffects.None, 1);
                        break;
                    case 54:
                        sb.Draw(LcAssets.t51, new Rectangle((int)mapPos.X * 30 + 15, (int)mapPos.Y * 30 + 15, 30, 30), null, Color.White, MathHelper.PiOver2, new Vector2(15, 15), SpriteEffects.None, 1);
                        break;

                    //ast corst flipped
                    case 55:
                        sb.Draw(LcAssets.t55, mapPos * 30, Color.White);
                        break;
                    case 56:
                        sb.Draw(LcAssets.t55, new Rectangle((int)mapPos.X * 30 + 15, (int)mapPos.Y * 30 + 15, 30, 30), null, Color.White, MathHelper.PiOver2 * 3, new Vector2(15, 15), SpriteEffects.None, 1);
                        break;
                    case 57:
                        sb.Draw(LcAssets.t55, new Rectangle((int)mapPos.X * 30 + 15, (int)mapPos.Y * 30 + 15, 30, 30), null, Color.White, MathHelper.Pi, new Vector2(15, 15), SpriteEffects.None, 1);
                        break;
                    case 58:
                        sb.Draw(LcAssets.t55, new Rectangle((int)mapPos.X * 30 + 15, (int)mapPos.Y * 30 + 15, 30, 30), null, Color.White, MathHelper.PiOver2, new Vector2(15, 15), SpriteEffects.None, 1);
                        break;


                    case 59:
                        sb.Draw(LcAssets.t59, mapPos * 30, Color.White);
                        break;
                    case 60:
                    default:
                        //sb.Draw(game.Content.Load<Texture2D>(@"Hud/Minimap/60"), mapPos * 30, Color.White);
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
