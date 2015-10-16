using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MoonCow
{
    public class LevelCreator:DrawableGameComponent
    {
        Game1 game;
        List<List<LcTilePlace>> tiles = new List<List<LcTilePlace>>();
        public int width;
        public int height;
        SpriteBatch sb;
        LcMouseCursor cursor;
        public Vector2 topPos;

        public LcTilePlace[,] tileArray;
        public List<LcMenuTile> menuTiles;
        public List<LcGridModifier> gridModifiers;
        public LcSaveButton saveButton;

        public LevelCreator(Game1 game):base(game)
        {
            this.game = game;
            width = 12;
            height = 12;
            sb = new SpriteBatch(game.GraphicsDevice);

            tileArray = new LcTilePlace[height,width];
            cursor = new LcMouseCursor(game, this);
            menuTiles = new List<LcMenuTile>();
            gridModifiers = new List<LcGridModifier>();
            initMenuTiles();
            addSizeButtons();
            saveButton = new LcSaveButton(game, new Vector2(700, 35), this);

            topPos = new Vector2(100, 100);

            Vector2 tempPos = topPos;
            for(int i = 0; i < height; i++)
            {
                //tiles.Add(new List<LcTilePlace>());
                tempPos.X = 100;
                for(int j = 0; j < width; j++)
                {
                    tileArray[i,j] = new LcTilePlace(game, tempPos, new Vector2(j,i));
//                    tiles.ElementAt(i).Add(new LcTilePlace(game, tempPos));
                    tempPos.X += 30;
                }
                tempPos.Y += 30;
            }
            game.IsMouseVisible = true;
        }

        void addSizeButtons()
        {
            gridModifiers.Add(new LcGridModifier(game, new Vector2(300, 20), this, true, 0));
            gridModifiers.Add(new LcGridModifier(game, new Vector2(332, 20), this, false, 0));
            gridModifiers.Add(new LcGridModifier(game, new Vector2(300, 52), this, true, 1));
            gridModifiers.Add(new LcGridModifier(game, new Vector2(332, 52), this, false, 1));
        }

        void initMenuTiles()
        {
            menuTiles.Add(new LcMenuTile(new Vector2(1200,100), 1));
            menuTiles.Add(new LcMenuTile(new Vector2(1200, 132), 3));
            menuTiles.Add(new LcMenuTile(new Vector2(1200, 164), 7));
            menuTiles.Add(new LcMenuTile(new Vector2(1200, 196), 11));
            menuTiles.Add(new LcMenuTile(new Vector2(1200, 228), 12));
            menuTiles.Add(new LcMenuTile(new Vector2(1200, 260), 16));
            menuTiles.Add(new LcMenuTile(new Vector2(1200, 292), 35));
            menuTiles.Add(new LcMenuTile(new Vector2(1200, 324), 39));
            menuTiles.Add(new LcMenuTile(new Vector2(1200, 356), 43));
            menuTiles.Add(new LcMenuTile(new Vector2(1200, 388), 47));
            menuTiles.Add(new LcMenuTile(new Vector2(1200, 420), 51));
            menuTiles.Add(new LcMenuTile(new Vector2(1200, 452), 55));
            menuTiles.Add(new LcMenuTile(new Vector2(1200, 484), 59));
            menuTiles.Add(new LcMenuTile(new Vector2(1170, 516), 24));




        }

        public override void Update(GameTime gameTime)
        {
            cursor.Update();
        }

        public override void Draw(GameTime gameTime)
        {
            game.GraphicsDevice.SetRenderTarget(null);
            sb.Begin();
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    tileArray[i, j].Draw(sb);
                }
            }
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    tileArray[i, j].DrawHighlight(sb);
                }
            }

            foreach (LcMenuTile m in menuTiles)
            {
                m.Draw(sb);
            }

            foreach (LcGridModifier m in gridModifiers)
            {
                m.Draw(sb);
            }
            sb.DrawString(LcAssets.font, "" + width, Utilities.scaledCoords(300, 20), Color.White, 0,
                        new Vector2(0, LcAssets.font.MeasureString("" + width).Y / 2), Utilities.windowScale * 24.0f / 40, SpriteEffects.None, 0);

            sb.DrawString(LcAssets.font, "" + height, Utilities.scaledCoords(300, 50), Color.White, 0,
                        new Vector2(0, LcAssets.font.MeasureString("" + height).Y / 2), Utilities.windowScale * 24.0f / 40, SpriteEffects.None, 0);

            saveButton.Draw(sb);

            cursor.Draw(sb);
            sb.End();
        }

        public void saveLevel()
        {
            int[,] intData = new int[height,width];

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    intData[i,j] = tileArray[i,j].type;
                }
            }

            MapData data = new MapData(88, "pac-man", "jason", width, height, intData);
            data.writeMap();
        }
    }
}
