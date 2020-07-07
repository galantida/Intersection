using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using tileWorld;

namespace tileWorld
{
    public enum InputActionNames { ZoomIn, ZoomOut, ShiftDrive, ShiftReverse, ShiftNeutral, Accelerate, Decelerate, Break, SteerLeft, SteerRight, HeadLights, LeftTurnSignal, RightTurnSignal, Hazzards };

    // needs to convert keyboard input to
    // the aprropriate predefined commands
    public class clsInput
    {
        public List<InputActionNames> inputActions;

        private List<clsControlMapping> keyMappings;
        private clsKeyboard keyboard;
        private clsMouse mouse;

        private float lastUpdated { get; set; }

        public clsInput()
        {
            inputActions = new List<InputActionNames>();

            this.loadKeyMappings();
            keyboard = new clsKeyboard();
            mouse = new clsMouse();
        }

        public void update(float currentTime)
        {
            float deltaTime = currentTime - lastUpdated; // time since last updated
            if (deltaTime > 10)
            {
                lastUpdated = currentTime; // reset last updated

                // read inputs
                inputActions = new List<InputActionNames>();

                // keyboard inputs
                foreach (clsControlMapping keyMapping in this.keyMappings)
                {
                    if (keyMapping.controlType == ControlTypes.Mouse)
                    {
                        // mouse mapping
                        if (mouse.isPressed(keyMapping.button))
                        {
                            inputActions.Add(keyMapping.actionName);
                        }
                    }
                    else
                    {
                        // keyboard mapping
                        if (keyboard.isPressed(keyMapping.key)) inputActions.Add(keyMapping.actionName);
                    }
                }
                keyboard.reset();
                mouse.reset();
            }
        }

        public bool isActivated(InputActionNames action)
        {
            foreach (InputActionNames a in inputActions)
            {
                if (action == a) return true;
            }
            return false;
        }

        public void loadKeyMappings()
        {
            // key mappings
            keyMappings = new List<clsControlMapping>();

            // controls
            keyMappings.Add(new clsControlMapping(Keys.Up, InputActionNames.Accelerate));
            keyMappings.Add(new clsControlMapping(Keys.Down, InputActionNames.Decelerate));
            keyMappings.Add(new clsControlMapping(Keys.Left, InputActionNames.SteerLeft));
            keyMappings.Add(new clsControlMapping(Keys.Right, InputActionNames.SteerRight));
            keyMappings.Add(new clsControlMapping(Keys.B, InputActionNames.Break));
            keyMappings.Add(new clsControlMapping(Keys.D, InputActionNames.ShiftDrive));
            keyMappings.Add(new clsControlMapping(Keys.R, InputActionNames.ShiftReverse));
            keyMappings.Add(new clsControlMapping(Keys.N, InputActionNames.ShiftNeutral));

            // lights
            keyMappings.Add(new clsControlMapping(Keys.H, InputActionNames.HeadLights));
            keyMappings.Add(new clsControlMapping(Keys.E, InputActionNames.Hazzards));
            keyMappings.Add(new clsControlMapping(Keys.OemComma, InputActionNames.LeftTurnSignal));
            keyMappings.Add(new clsControlMapping(Keys.OemPeriod, InputActionNames.RightTurnSignal));

            // controls
            keyMappings.Add(new clsControlMapping(InputActionNames.ZoomIn, MouseButtons.ScrollUp));
            keyMappings.Add(new clsControlMapping(InputActionNames.ZoomOut, MouseButtons.ScrollDown));
        }

    }
}
