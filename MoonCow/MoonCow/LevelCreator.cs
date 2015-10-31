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
        public List<LcTextField> textFields;
        public LcKeyboardListener keyListener;

        public float saveFadeTime;

        public LevelCreator(Game1 game):base(game)
        {
            saveFadeTime = 1;
            this.game = game;
            width = 12;
            height = 12;
            sb = new SpriteBatch(game.GraphicsDevice);

            tileArray = new LcTilePlace[height,width];
            cursor = new LcMouseCursor(game, this);
            menuTiles = new List<LcMenuTile>();
            gridModifiers = new List<LcGridModifier>();
            textFields = new List<LcTextField>();
            initMenuTiles();
            addSizeButtons();
            initTextFields();
            keyListener = new LcKeyboardListener(game, this);
            //keyListener.activeField = textFields.ElementAt(0);
            //keyListener.activeField.activate();
            saveButton = new LcSaveButton(game, new Vector2(1020, 25), this);

            topPos = new Vector2(90, 140);

            //loadFile();
            
            Vector2 tempPos = topPos;
            for(int i = 0; i < height; i++)
            {
                //tiles.Add(new List<LcTilePlace>());
                tempPos.X = 90;
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

        /// <summary>
        /// used for editing levels that are already made. Not a front end feature
        /// </summary>
        void loadFile()
        {
            MapData m = new MapData(@"Content/MapXml/Custom/level 3.xml");

            width = m.getWidth();
            height = m.getLength();
            tileArray = new LcTilePlace[height, width];

            Vector2 tempPos = topPos;
            for (int i = 0; i < height; i++)
            {
                //tiles.Add(new List<LcTilePlace>());
                tempPos.X = 90;
                for (int j = 0; j < width; j++)
                {
                    tileArray[i, j] = new LcTilePlace(game, tempPos, new Vector2(j, i), m.map[j,i]);
                    tempPos.X += 30;
                }
                tempPos.Y += 30;
            }
        }

        void initTextFields()
        {
            textFields.Add(new LcTextField(game, this, new Vector2(600, 25), "map name"));
            textFields.Add(new LcTextField(game, this, new Vector2(600, 59), "creator"));
        }

        void addSizeButtons()
        {
            gridModifiers.Add(new LcGridModifier(game, new Vector2(130, 25), this, true, 0));
            gridModifiers.Add(new LcGridModifier(game, new Vector2(253, 25), this, false, 0));
            gridModifiers.Add(new LcGridModifier(game, new Vector2(130, 57), this, true, 1));
            gridModifiers.Add(new LcGridModifier(game, new Vector2(253, 57), this, false, 1));
        }

        void initMenuTiles()
        {
            menuTiles.Add(new LcMenuTile(new Vector2(1210, 100 + 40), 1));
            menuTiles.Add(new LcMenuTile(new Vector2(1210, 132 + 40), 3));
            menuTiles.Add(new LcMenuTile(new Vector2(1210, 164 + 40), 7));
            menuTiles.Add(new LcMenuTile(new Vector2(1210, 196 + 40), 11));
            menuTiles.Add(new LcMenuTile(new Vector2(1210, 228 + 40), 12));
            menuTiles.Add(new LcMenuTile(new Vector2(1210, 260 + 40), 16));
            menuTiles.Add(new LcMenuTile(new Vector2(1210, 292 + 40), 35));
            menuTiles.Add(new LcMenuTile(new Vector2(1210, 324 + 40), 39));
            menuTiles.Add(new LcMenuTile(new Vector2(1210, 356 + 40), 43));
            menuTiles.Add(new LcMenuTile(new Vector2(1210, 388 + 40), 47));
            menuTiles.Add(new LcMenuTile(new Vector2(1210, 420 + 40), 51));
            menuTiles.Add(new LcMenuTile(new Vector2(1210, 452 + 40), 55));
            menuTiles.Add(new LcMenuTile(new Vector2(1210, 484 + 40), 59));
            menuTiles.Add(new LcMenuTile(new Vector2(1180, 516 + 40), 24));




        }

        public override void Update(GameTime gameTime)
        {
            cursor.Update();
            keyListener.checkKeys();
            foreach(LcTextField t in textFields)
            {
                t.Update();
            }
            saveButton.checkTex();
            MenuAssets.updateLinePos();

            if(saveFadeTime != 1)
            {
                saveFadeTime += Utilities.deltaTime;
                if(saveFadeTime > 1)
                    saveFadeTime = 1;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            game.GraphicsDevice.SetRenderTarget(null);
            sb.Begin();
            MenuAssets.drawMenuBackground(sb);
            sb.Draw(LcAssets.border, new Rectangle(35,120,1245, 552), Color.White);
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

            sb.Draw(LcAssets.box, new Vector2(164, 25), Color.White);
            sb.Draw(LcAssets.box, new Vector2(164, 57), Color.White);

            sb.DrawString(LcAssets.font, "" + width, new Vector2(206, 28), Color.White, 0,
                        new Vector2(LcAssets.font.MeasureString("" + height).X / 2, 0), 20.0f / 40, SpriteEffects.None, 0);

            sb.DrawString(LcAssets.font, "" + height, new Vector2(206, 60), Color.White, 0,
                        new Vector2(LcAssets.font.MeasureString("" + height).X / 2, 0), 20.0f / 40, SpriteEffects.None, 0);

            saveButton.Draw(sb);

            foreach(LcTextField t in textFields)
            {
                t.Draw(sb);
            }

            sb.Draw(LcAssets.saved, new Vector2(370, 340), Color.White * MathHelper.SmoothStep(1,0,saveFadeTime));

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

            MapData data = new MapData(88, "Custom/" + textFields.ElementAt(0).text, textFields.ElementAt(1).text, width, height, intData);
            data.writeMap();
            saveFadeTime = 0;
        }
    }
}
