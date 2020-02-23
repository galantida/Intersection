using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace tileWorld
{
    public class clsKeyboard
    {
        private KeyboardState lastState;

        public clsKeyboard()
        {
            // this will have to read from settings file
        }

        public void update()
        {
            lastState = Keyboard.GetState();
        }

        public bool isPressed(Keys key)
        {
            if (Keyboard.GetState().IsKeyDown(key))
            {
                if (!lastState.IsKeyDown(key)) return true;
            }

            return false;
        }

        
    }
}
