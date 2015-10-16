using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    public class LcGridModifier
    {
        //plus or minus set to x or y
        bool add;
        int type;//0 is x, 1 is y

        Texture2D tex;
        Texture2D hiTex;
        bool highlighted;
        Vector2 pos;
        LevelCreator lc;
        Game1 game;
        public AABB bounds;

        public LcGridModifier(Game1 game, Vector2 pos, LevelCreator lc, bool add, int type)
        {
            this.pos = pos;
            this.game = game;
            this.lc = lc;
            this.add = add;
            this.type = type;
            bounds = new AABB(pos, 30, 30);
            setTex();
        }

        public void activate()
        {
            highlighted = true;
            setHiTex();
        }

        public void disable()
        {
            highlighted = false;
            setHiTex();
        }

        void setHiTex()
        {
            hiTex = LcAssets.bigHi;
        }

        void setTex()
        {
            if (add)
                tex = LcAssets.plus;
            else
                tex = LcAssets.minus;
        }

        public void onClick()
        {
            switch(type)
            {
                default://x
                    if (add)
                    {
                        int oldWidth = lc.width;
                        lc.width++;

                        LcTilePlace[,] tempArray = new LcTilePlace[lc.height, lc.width];


                        Vector2 tempPos = lc.topPos;
                        for (int i = 0; i < lc.height; i++)
                        {
                            //tiles.Add(new List<LcTilePlace>());
                            tempPos.X = 100;
                            for (int j = 0; j < lc.width; j++)
                            {
                                if(j < oldWidth)
                                    tempArray[i, j] = lc.tileArray[i,j];
                                else
                                    tempArray[i, j] = new LcTilePlace(game, tempPos, new Vector2(j, i));
                                //                    tiles.ElementAt(i).Add(new LcTilePlace(game, tempPos));
                                tempPos.X += 30;
                            }
                            tempPos.Y += 30;
                        }
                        lc.tileArray = tempArray;
                    }
                    else
                    {
                        int oldWidth = lc.width;
                        if(lc.width > 1)
                            lc.width--;

                        LcTilePlace[,] tempArray = new LcTilePlace[lc.height, lc.width];


                        Vector2 tempPos = lc.topPos;
                        for (int i = 0; i < lc.height; i++)
                        {
                            //tiles.Add(new List<LcTilePlace>());
                            tempPos.X = 100;
                            for (int j = 0; j < lc.width; j++)
                            {
                                tempArray[i, j] = lc.tileArray[i, j];
                                tempPos.X += 30;
                            }
                            tempPos.Y += 30;
                        }
                        lc.tileArray = tempArray;
                    }
                    break;
                case 1://y
                    if (add)
                    {
                        int oldHeight = lc.height;
                        lc.height++;

                        LcTilePlace[,] tempArray = new LcTilePlace[lc.height, lc.width];


                        Vector2 tempPos = lc.topPos;
                        for (int i = 0; i < lc.height; i++)
                        {
                            //tiles.Add(new List<LcTilePlace>());
                            tempPos.X = 100;
                            for (int j = 0; j < lc.width; j++)
                            {
                                if(i < oldHeight)
                                    tempArray[i, j] = lc.tileArray[i, j];
                                else
                                    tempArray[i,j] = new LcTilePlace(game, tempPos, new Vector2(j,i));
                                tempPos.X += 30;
                            }
                            tempPos.Y += 30;
                        }
                        lc.tileArray = tempArray;
                    }
                    else
                    {
                        if(lc.height > 1)
                            lc.height--;

                        LcTilePlace[,] tempArray = new LcTilePlace[lc.height, lc.width];


                        Vector2 tempPos = lc.topPos;
                        for (int i = 0; i < lc.height; i++)
                        {
                            tempPos.X = 100;
                            for (int j = 0; j < lc.width; j++)
                            {
                                tempArray[i, j] = lc.tileArray[i, j];
                                tempPos.X += 30;
                            }
                            tempPos.Y += 30;
                        }
                        lc.tileArray = tempArray;
                    }
                    break;
            }
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(tex, pos, Color.White);
            if (highlighted)
                sb.Draw(hiTex, new Rectangle((int)pos.X, (int)pos.Y, tex.Bounds.Width, tex.Bounds.Height), Color.White);
        }

    }
}
