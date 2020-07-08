using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using tileWorld;

namespace gameLogic
{
    public enum DeviceTypes { Keyboard, Mouse }

    // needs to convert keyboard input to
    // the aprropriate predefined commands
    public class clsInput
    {
        // devices
        private clsKeyboard keyboard;
        private clsMouse mouse;

        private List<clsActionMapping> actionMappings; // actions that are mapped to controls
        public List<InputActions> inputActions; // actions that are detected

        private float lastUpdated { get; set; }

        public clsInput()
        {
            actionMappings = new List<clsActionMapping>(); // reset action mapping

            // initialize input devices
            initializeKeyboard();
            initializeMouse();
        }

        public void update(float currentTime)
        {
            float deltaTime = currentTime - lastUpdated; // time since last updated
            if (deltaTime > 10)
            {
                lastUpdated = currentTime; // reset last updated

                // check all mapped actions for activity
                inputActions = new List<InputActions>(); // clear input actions
                foreach (clsActionMapping actionMapping in this.actionMappings)
                {
                    if (actionMapping.deviceType == DeviceTypes.Mouse)
                    {
                        // check mouse action mapping
                        if (mouse.isPressed(actionMapping.button)) inputActions.Add(actionMapping.actionName); // add to active actions list
                    }
                    else
                    {
                        // check keyboard action mapping
                        if (keyboard.isPressed(actionMapping.key)) inputActions.Add(actionMapping.actionName); // add to active actions list
                    }
                }
                keyboard.reset();
                mouse.reset();
            }
        }

        public bool isActivated(InputActions action)
        {
            // this is only a collection of the actions that are activated
            foreach (InputActions a in inputActions)
            {
                if (action == a) return true;
            }
            return false;
        }

        private void initializeKeyboard()
        {
            // load keyboard bindings and map to actions
            this.keyboard = new clsKeyboard();
            foreach (KeyValuePair<Keys, string> kvp in keyboard.bindings)
            {
                if (Enum.TryParse(kvp.Value, out InputActions inputAction))
                {
                    actionMappings.Add(new clsActionMapping(inputAction, kvp.Key));
                }
            }
        }

        private void initializeMouse()
        {
            this.mouse = new clsMouse();
            foreach (KeyValuePair<MouseButtons, string> kvp in mouse.bindings)
            {
                if (Enum.TryParse(kvp.Value, out InputActions inputAction))
                {
                    actionMappings.Add(new clsActionMapping(inputAction, kvp.Key));
                }
            }
        }
    }
}
