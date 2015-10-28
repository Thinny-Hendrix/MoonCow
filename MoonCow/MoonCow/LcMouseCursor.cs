using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace MoonCow
{
    public class LcMouseCursor
    {
        Vector2 pos;
        Vector2 clickPos;
        LevelCreator levelCreator;
        bool colliding;
        bool leftClick;
        bool rightClick;
        LcTilePlace activeTile;
        LcTilePlace selectedTile;
        LcMenuTile activeMenuTile;
        LcGridModifier activeGridModifier;
        LcTextField activeText;
        Texture2D tex;

        Game1 game;
        int holdType;
        LcTilePlace prevTile;
        bool swapDrag;
        bool canPlaceBase;
        bool dragging;

        public LcMouseCursor(Game1 game, LevelCreator lc)
        {
            this.levelCreator = lc;
            this.game = game;
        }

        void setTex()
        {
            switch (holdType)
            {
                default:
                    tex = LcAssets.back;
                    break;
                case 1:
                    tex = LcAssets.t1;
                    break;
                case 2:
                    tex = LcAssets.t2;
                    break;

                case 3:
                case 7:
                    tex = LcAssets.t3;
                    break;
                case 4:
                case 8:
                    tex = LcAssets.t4;
                    break;
                case 5:
                case 9:
                    tex = LcAssets.t5;
                    break;
                case 6:
                case 10:
                    tex = LcAssets.t6;
                    break;

                case 11:
                    tex = LcAssets.t11;
                    break;

                case 12:
                    tex = LcAssets.t12;
                    break;
                case 13:
                    tex = LcAssets.t13;
                    break;
                case 14:
                    tex = LcAssets.t14;
                    break;
                case 15:
                    tex = LcAssets.t15;
                    break;
                case 16:
                    tex = LcAssets.t16;
                    break;
                case 17:
                    tex = LcAssets.t17;
                    break;
                case 18:
                    tex = LcAssets.t18;
                    break;
                case 19:
                    tex = LcAssets.t19;
                    break;
                case 20:
                    tex = LcAssets.t20;
                    break;
                case 21:
                    tex = LcAssets.t21;
                    break;
                case 22:
                    tex = LcAssets.t22;
                    break;
                case 23:
                    tex = LcAssets.t23;
                    break;
                case 24:
                    tex = LcAssets.base1;
                    break;
                case 25:
                    tex = LcAssets.t25;
                    break;
                case 26:
                    tex = LcAssets.t26;
                    break;
                case 27:
                    tex = LcAssets.t27;
                    break;
                case 28:
                    tex = LcAssets.t28;
                    break;

                case 35:
                    tex = LcAssets.t35;
                    break;
                case 36:
                    tex = LcAssets.t36;
                    break;
                case 37:
                    tex = LcAssets.t37;
                    break;
                case 38:
                    tex = LcAssets.t38;
                    break;
                case 39:
                    tex = LcAssets.t39;
                    break;
                case 40:
                    tex = LcAssets.t40;
                    break;
                case 41:
                    tex = LcAssets.t41;
                    break;
                case 42:
                    tex = LcAssets.t42;
                    break;
                case 43:
                    tex = LcAssets.t43;
                    break;
                case 44:
                    tex = LcAssets.t44;
                    break;
                case 45:
                    tex = LcAssets.t45;
                    break;
                case 46:
                    tex = LcAssets.t46;
                    break;
                case 47:
                    tex = LcAssets.t47;
                    break;
                case 48:
                    tex = LcAssets.t48;
                    break;
                case 49:
                    tex = LcAssets.t49;
                    break;
                case 50:
                    tex = LcAssets.t50;
                    break;
                case 51:
                    tex = LcAssets.t51;
                    break;
                case 52:
                    tex = LcAssets.t52;
                    break;
                case 53:
                    tex = LcAssets.t53;
                    break;
                case 54:
                    tex = LcAssets.t54;
                    break;
                case 55:
                    tex = LcAssets.t55;
                    break;
                case 56:
                    tex = LcAssets.t56;
                    break;
                case 57:
                    tex = LcAssets.t57;
                    break;
                case 58:
                    tex = LcAssets.t58;
                    break;
                case 59:
                    tex = LcAssets.t59;
                    break;
            }
        }

        public void Update()
        {
            pos = Mouse.GetState().Position.ToVector2();
            checkCollision();

            if(Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                if (!leftClick)
                    onLeftClick();
                else
                {
                    onLeftHold();
                }
            }
            else
            {
                if (leftClick)
                    onLeftRelease();
            }

            if (Mouse.GetState().RightButton == ButtonState.Pressed)
            {
                if (!leftClick && !rightClick)
                    onRightClick();
            }
            else
            {
                if (rightClick)
                    rightClick = false;
            }

            if(selectedTile != null)
            {
                if(Keyboard.GetState().IsKeyDown(Keys.Back) || Keyboard.GetState().IsKeyDown(Keys.Delete))
                {
                    if (selectedTile.type == 24)
                    {
                        activeTile = selectedTile;
                        clearBase();
                        activeTile = null;
                    }
                    else
                        selectedTile.setType(60);

                    selectedTile.selected = false;
                    selectedTile = null;
                }
            }
        }

        void onRightClick()
        {
            rightClick = true;
            if (activeTile != null)
                activeTile.rotate();
        }

        void onLeftHold()
        {
            if (holdType == 24)
                checkBasePlace();

            if (!dragging)
            {
                if (clickPos.X != pos.X || clickPos.Y != pos.Y)
                {
                    dragging = true;
                    if (selectedTile != null)
                    {
                        selectedTile.selected = false;
                        selectedTile = null;
                    }

                    if (activeTile != null)
                    {
                        if (activeTile.type != 60)
                        {
                            holdType = activeTile.type;
                            if (holdType == 24)
                                clearBase();
                            setTex();
                            if (!Keyboard.GetState().IsKeyDown(Keys.LeftAlt))
                            {
                                activeTile.setType(60);
                                swapDrag = true;
                                prevTile = activeTile;
                            }
                        }
                    }
                }
            }
        }

        void onLeftClick()
        {
            leftClick = true;
            clickPos = pos;

            if(activeMenuTile != null)
            {
                holdType = activeMenuTile.type;
                setTex();
            }
            if(activeTile != null)
            {
                if(activeTile != selectedTile)
                {
                    if (selectedTile != null)
                        selectedTile.selected = false;
                }
                //activeTile.selected = true;
                selectedTile = activeTile;
                selectedTile.selected = true;
            }
            if(activeGridModifier != null)
            {
                activeGridModifier.onClick();
            }

            levelCreator.keyListener.setTextField(activeText);
        }

        void onLeftRelease()
        {
            leftClick = false;
            if (dragging)
            {
                if (holdType != 60)
                {
                    if (activeTile != null)
                    {
                        if (holdType != 24)
                        {
                            if (swapDrag)
                            {
                                if (activeTile.type != 60)
                                    prevTile.setType(activeTile.type);
                            }
                            activeTile.setType(holdType);
                            setTex();
                        }
                        else
                        {
                            if (canPlaceBase)
                            {
                                placeBase();
                                //activeTile.setType(holdType);
                                setTex();
                            }
                            else
                            {

                            }
                        }
                    }
                    holdType = 0;
                    swapDrag = false;
                }
            }
            dragging = false;
        }

        void checkBasePlace()
        {
            canPlaceBase = false;
            if (activeTile != null)
            {
                if (activeTile.coord.X > 0 && activeTile.coord.X < levelCreator.width - 1)
                {
                    if (activeTile.coord.Y > 0 && activeTile.coord.Y < levelCreator.height - 1)
                        canPlaceBase = true;
                }
            }
        }
        void placeBase()
        {
            levelCreator.tileArray[(int)activeTile.coord.Y - 1, (int)activeTile.coord.X - 1].setType(20);
            levelCreator.tileArray[(int)activeTile.coord.Y - 1, (int)activeTile.coord.X].setType(21);
            levelCreator.tileArray[(int)activeTile.coord.Y - 1, (int)activeTile.coord.X + 1].setType(22);

            levelCreator.tileArray[(int)activeTile.coord.Y, (int)activeTile.coord.X - 1].setType(23);
            activeTile.setType(24);
            levelCreator.tileArray[(int)activeTile.coord.Y, (int)activeTile.coord.X + 1].setType(25);

            levelCreator.tileArray[(int)activeTile.coord.Y + 1, (int)activeTile.coord.X - 1].setType(26);
            levelCreator.tileArray[(int)activeTile.coord.Y + 1, (int)activeTile.coord.X].setType(27);
            levelCreator.tileArray[(int)activeTile.coord.Y + 1, (int)activeTile.coord.X + 1].setType(28);
        }

        void clearBase()
        {
            levelCreator.tileArray[(int)activeTile.coord.Y - 1, (int)activeTile.coord.X - 1].setType(60);
            levelCreator.tileArray[(int)activeTile.coord.Y - 1, (int)activeTile.coord.X].setType(60);
            levelCreator.tileArray[(int)activeTile.coord.Y - 1, (int)activeTile.coord.X + 1].setType(60);

            levelCreator.tileArray[(int)activeTile.coord.Y, (int)activeTile.coord.X - 1].setType(60);
            activeTile.setType(60);
            levelCreator.tileArray[(int)activeTile.coord.Y, (int)activeTile.coord.X + 1].setType(60);

            levelCreator.tileArray[(int)activeTile.coord.Y + 1, (int)activeTile.coord.X - 1].setType(60);
            levelCreator.tileArray[(int)activeTile.coord.Y + 1, (int)activeTile.coord.X].setType(60);
            levelCreator.tileArray[(int)activeTile.coord.Y + 1, (int)activeTile.coord.X + 1].setType(60);
        }

        public void checkCollision()
        {
            bool collided = false;
            for (int i = 0; i < levelCreator.height; i++)
            {
                for (int j = 0; j < levelCreator.width; j++)
                {
                    LcTilePlace temp = levelCreator.tileArray[i,j];

                    if (temp.activeCol)
                    {
                        if (temp.bounds.checkPoint(pos))
                        {
                            collided = true;
                            if (temp != activeTile)
                            {
                                if (activeTile != null)
                                    activeTile.disable();
                                activeTile = temp;
                                activeTile.activate();
                            }
                        }
                    }
                }
            }
            if (activeTile != null)
            {
                if (!collided)
                {
                    activeTile.disable();
                    activeTile = null;
                }
            }

            collided = false;
            foreach(LcMenuTile m in levelCreator.menuTiles)
            {
                if (m.bounds.checkPoint(pos))
                {
                    collided = true;
                    if (m != activeMenuTile)
                    {
                        if (activeMenuTile != null)
                            activeMenuTile.disable();
                        activeMenuTile = m;
                        activeMenuTile.activate();
                    }
                }
            }
            if (activeMenuTile != null)
            {
                if (!collided)
                {
                    activeMenuTile.disable();
                    activeMenuTile = null;
                }
            }

            collided = false;
            foreach (LcGridModifier m in levelCreator.gridModifiers)
            {
                if (m.bounds.checkPoint(pos))
                {
                    collided = true;
                    if (m != activeGridModifier)
                    {
                        if (activeGridModifier != null)
                            activeGridModifier.disable();
                        activeGridModifier = m;
                        activeGridModifier.activate();
                    }
                }
            }
            if (activeGridModifier != null)
            {
                if (!collided)
                {
                    activeGridModifier.disable();
                    activeGridModifier = null;
                }
            }

            if(levelCreator.saveButton.bounds.checkPoint(pos))
            {
                levelCreator.saveButton.activate();

                if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                    levelCreator.saveButton.onClick();
            }
            else
            {
                levelCreator.saveButton.disable();
            }

            collided = false;
            foreach (LcTextField t in levelCreator.textFields)
            {
                if (t.bounds.checkPoint(pos))
                {
                    collided = true;
                        activeText = t;
                }
            }
            if (activeText != null)
            {
                if (!collided)
                {
                    activeText = null;
                }
            }
        }

        public void Draw(SpriteBatch sb)
        {
            if (holdType > 0)
            {
                if(holdType == 24 && !canPlaceBase)
                    sb.Draw(tex, new Vector2(pos.X - tex.Bounds.Width/2, pos.Y - tex.Bounds.Width/2), Color.Red * 0.5f);
                else
                    sb.Draw(tex, new Vector2(pos.X - tex.Bounds.Width / 2, pos.Y - tex.Bounds.Width / 2), Color.White * 0.5f);


            }
        }

    }
}
