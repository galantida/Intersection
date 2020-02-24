using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace tileWorld
{
    public enum MouseButtons { ScrollUp, ScrollDown } 

    public class clsMouse
    {
        MouseState lastState;
        MouseState currentState;

        public clsMouse()
        {

        }

        public void reset()
        {
            lastState = currentState;
            currentState = Mouse.GetState();
        }

        public bool isPressed(MouseButtons button)
        {
            /*
            if (currentState.IsKeyDown(key))
            {
                if (!lastState.IsKeyDown(key)) return true;
            }
            */

            if (button == MouseButtons.ScrollUp)
            {
                if (currentState.ScrollWheelValue > lastState.ScrollWheelValue) return true;
            }
            else if (button == MouseButtons.ScrollDown)
            {
                if (currentState.ScrollWheelValue < lastState.ScrollWheelValue) return true;
            }

            return false;
        }
    }
}
