using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MoonCow
{
    public class LcKeyboardListener
    {
        char pressedKey;
        bool pressed;
        LevelCreator lc;
        Game1 game;

        public LcTextField activeField;

        public LcKeyboardListener(Game1 game, LevelCreator lc)
        {
            this.game = game;
            this.lc = lc;
            pressed = false;
        }

        public void setTextField(LcTextField t)
        {
            if (t != activeField)
            {
                if(activeField != null)
                    activeField.disable();
                activeField = t;
                if (activeField != null)
                    activeField.activate();
            }
        }

        public void checkKeys()
        {
            if(Keyboard.GetState().GetPressedKeys().Count() == 0)
            {
                if(pressed)
                {
                    pressed = false;
                }
            }
            else
            {
                if (!pressed)
                {
                    pressed = true;

                    KeyboardState s = Keyboard.GetState();
                    if (Keyboard.GetState().IsKeyDown(Keys.Back) || Keyboard.GetState().IsKeyDown(Keys.Delete))
                    {
                        if(activeField != null)
                            activeField.deleteChar();
                    }
                    else
                    {
                        if (s.IsKeyDown(Keys.Q))
                        {
                            pressedKey = 'q';
                        }
                        else if (s.IsKeyDown(Keys.W))
                        {
                            pressedKey = 'w';
                        }
                        else if (s.IsKeyDown(Keys.E))
                        {
                            pressedKey = 'e';
                        }
                        else if (s.IsKeyDown(Keys.R))
                        {
                            pressedKey = 'r';
                        }
                        else if (s.IsKeyDown(Keys.T))
                        {
                            pressedKey = 't';
                        }
                        else if (s.IsKeyDown(Keys.Y))
                        {
                            pressedKey = 'y';
                        }
                        else if (s.IsKeyDown(Keys.U))
                        {
                            pressedKey = 'u';
                        }
                        else if (s.IsKeyDown(Keys.I))
                        {
                            pressedKey = 'i';
                        }
                        else if (s.IsKeyDown(Keys.O))
                        {
                            pressedKey = 'o';
                        }
                        else if (s.IsKeyDown(Keys.P))
                        {
                            pressedKey = 'p';
                        }
                        else if (s.IsKeyDown(Keys.A))
                        {
                            pressedKey = 'a';
                        }
                        else if (s.IsKeyDown(Keys.S))
                        {
                            pressedKey = 's';
                        }
                        else if (s.IsKeyDown(Keys.D))
                        {
                            pressedKey = 'd';
                        }
                        else if (s.IsKeyDown(Keys.F))
                        {
                            pressedKey = 'f';
                        }
                        else if (s.IsKeyDown(Keys.G))
                        {
                            pressedKey = 'g';
                        }
                        else if (s.IsKeyDown(Keys.H))
                        {
                            pressedKey = 'h';
                        }
                        else if (s.IsKeyDown(Keys.J))
                        {
                            pressedKey = 'j';
                        }
                        else if (s.IsKeyDown(Keys.K))
                        {
                            pressedKey = 'k';
                        }
                        else if (s.IsKeyDown(Keys.L))
                        {
                            pressedKey = 'l';
                        }
                        else if (s.IsKeyDown(Keys.Z))
                        {
                            pressedKey = 'z';
                        }
                        else if (s.IsKeyDown(Keys.X))
                        {
                            pressedKey = 'x';
                        }
                        else if (s.IsKeyDown(Keys.C))
                        {
                            pressedKey = 'c';
                        }
                        else if (s.IsKeyDown(Keys.V))
                        {
                            pressedKey = 'v';
                        }
                        else if (s.IsKeyDown(Keys.B))
                        {
                            pressedKey = 'b';
                        }
                        else if (s.IsKeyDown(Keys.N))
                        {
                            pressedKey = 'n';
                        }
                        else if (s.IsKeyDown(Keys.M))
                        {
                            pressedKey = 'm';
                        }
                        else if (s.IsKeyDown(Keys.Space))
                        {
                            pressedKey = ' ';
                        }
                        else if (s.IsKeyDown(Keys.D1))
                        {
                            pressedKey = '1';
                        }
                        else if (s.IsKeyDown(Keys.D2))
                        {
                            pressedKey = '2';
                        }
                        else if (s.IsKeyDown(Keys.D3))
                        {
                            pressedKey = '3';
                        }
                        else if (s.IsKeyDown(Keys.D4))
                        {
                            pressedKey = '4';
                        }
                        else if (s.IsKeyDown(Keys.D5))
                        {
                            pressedKey = '5';
                        }
                        else if (s.IsKeyDown(Keys.D6))
                        {
                            pressedKey = '6';
                        }
                        else if (s.IsKeyDown(Keys.D7))
                        {
                            pressedKey = '7';
                        }
                        else if (s.IsKeyDown(Keys.D8))
                        {
                            pressedKey = '8';
                        }
                        else if (s.IsKeyDown(Keys.D9))
                        {
                            pressedKey = '9';
                        }
                        else if (s.IsKeyDown(Keys.D0))
                        {
                            pressedKey = '0';
                        }
                        else if (s.IsKeyDown(Keys.OemMinus))
                        {
                            if (s.IsKeyDown(Keys.LeftShift) || s.IsKeyDown(Keys.RightShift))
                                pressedKey = '_';
                            else
                                pressedKey = '-';
                        }
                        if (activeField != null)
                        {
                            activeField.addChar(pressedKey);
                        }
                    }
                }
            }
        }
    }
}
