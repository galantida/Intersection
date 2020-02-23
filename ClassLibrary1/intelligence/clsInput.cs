using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using tileWorld;

namespace gameLogic
{
    public enum InputActionName { ShiftDrive, ShiftReverse, ShiftNeutral, Accelerate, Decelerate, Break, SteerLeft, SteerRight, HeadLights, LeftTurnSignal, RightTurnSignal, Hazzards };

    // needs to convert keyboard input to
    // the aprropriate predefined commands
    public class clsInput
    {
        public List<InputActionName> inputActions;

        private List<clsKeyMapping> keyMappings;
        private clsKeyboard keyboard;

        private float lastUpdated { get; set; }

        public clsInput()
        {
            inputActions = new List<InputActionName>();

            this.loadKeyMappings();
            keyboard = new clsKeyboard();
        }

        public void update(float currentTime)
        {
            float deltaTime = currentTime - lastUpdated; // time since last updated
            if (deltaTime > 10)
            {
                lastUpdated = currentTime; // reset last updated

                // read inputs
                keyboard.update();
                inputActions = new List<InputActionName>();
                foreach (clsKeyMapping keyMapping in this.keyMappings)
                {
                    if (keyboard.isPressed(keyMapping.key))
                    {
                        inputActions.Add(keyMapping.actionName);
                    }
                }
            }
        }

        public bool isActivated(InputActionName action)
        {
            foreach (InputActionName a in inputActions)
            {
                if (action == a) return true;
            }
            return false;
        }

        public void loadKeyMappings()
        {
            // key mappings
            keyMappings = new List<clsKeyMapping>();

            // controls
            keyMappings.Add(new clsKeyMapping(Keys.Up, InputActionName.Accelerate));
            keyMappings.Add(new clsKeyMapping(Keys.Down, InputActionName.Decelerate));
            keyMappings.Add(new clsKeyMapping(Keys.Left, InputActionName.SteerLeft));
            keyMappings.Add(new clsKeyMapping(Keys.Right, InputActionName.SteerRight));
            keyMappings.Add(new clsKeyMapping(Keys.B, InputActionName.Break));
            keyMappings.Add(new clsKeyMapping(Keys.D, InputActionName.ShiftDrive));
            keyMappings.Add(new clsKeyMapping(Keys.R, InputActionName.ShiftReverse));
            keyMappings.Add(new clsKeyMapping(Keys.N, InputActionName.ShiftNeutral));

            // lights
            keyMappings.Add(new clsKeyMapping(Keys.H, InputActionName.HeadLights));
            keyMappings.Add(new clsKeyMapping(Keys.E, InputActionName.Hazzards));
            keyMappings.Add(new clsKeyMapping(Keys.OemComma, InputActionName.LeftTurnSignal));
            keyMappings.Add(new clsKeyMapping(Keys.OemPeriod, InputActionName.RightTurnSignal));
        }

    }
}
