using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
namespace MoonCow
{
    class MgKey
    {
        Buttons dPad;
        //Vector2 leftStick;
        float stickAxis;
        float stickThresh;
        Keys wasd;
        Keys arrow;

        bool dTrig;
        bool sTrig;
        bool wTrig;
        bool aTrig;

        public bool pressed;

        public MgKey(Buttons dPad, Keys w, Keys arrow, float axis, float thresh)
        {
            this.dPad = dPad;
            this.wasd = w;
            this.arrow = arrow;
            stickAxis = axis;
            stickThresh = thresh;
        }

        public void update()
        {
            pressed = false;
            if (GamePad.GetState(PlayerIndex.One).IsButtonDown(dPad))
            {
                if (!dTrig)
                    pressed = true;
                dTrig = true;
            }
            else
                dTrig = false;

            if (Keyboard.GetState().IsKeyDown(wasd))
            {
                if (!wTrig)
                    pressed = true;
                wTrig = true;
            }
            else
                wTrig = false;

            if (Keyboard.GetState().IsKeyDown(arrow))
            {
                if (!aTrig)
                    pressed = true;
                aTrig = true;
            }
            else
                aTrig = false;
        }
    }
}
