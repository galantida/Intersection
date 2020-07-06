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
        private KeyboardState currentState;
        private KeyboardState lastState;

        public clsKeyboard()
        {
            
        }

        public void reset()
        {
            lastState = currentState;
            currentState = Keyboard.GetState();
        }

        public bool isPressed(Keys key)
        {
            if (currentState.IsKeyDown(key))
            {
                if (!lastState.IsKeyDown(key)) return true;
            }

            return false;
        }

        
    }
}
