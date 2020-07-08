using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;

namespace tileWorld
{
    public enum MouseButtons { Left, Middle, Right, ScrollUp, ScrollDown } 

    public class clsMouse
    {
        public Dictionary<MouseButtons, string> bindings;
        MouseState lastState;
        MouseState currentState;

        public clsMouse()
        {
            loadBindings("Mouse", AppDomain.CurrentDomain.BaseDirectory + "\\configuration\\inputBindings.json");
        }

        public void reset()
        {
            lastState = currentState;
            currentState = Mouse.GetState();
        }

        // test for amouse button
        public bool isPressed(MouseButtons button)
        {
            switch (button)
            {
                case MouseButtons.Left:
                    if (currentState.LeftButton == ButtonState.Pressed) return true;
                    break;
                case MouseButtons.Middle:
                    if (currentState.MiddleButton == ButtonState.Pressed) return true;
                    break;
                case MouseButtons.Right:
                    if (currentState.RightButton == ButtonState.Pressed) return true;
                    break;
                case MouseButtons.ScrollUp:
                    if (currentState.ScrollWheelValue > lastState.ScrollWheelValue) return true;
                    break;
                case MouseButtons.ScrollDown:
                    if (currentState.ScrollWheelValue < lastState.ScrollWheelValue) return true;
                    break;
            }
            return false;
        }

        private void loadBindings(string sectionName, string configurationFilePath)
        {
            // clear bindings
            bindings = new Dictionary<MouseButtons, string>();

            // read JSON directly from a file
            using (StreamReader file = File.OpenText(configurationFilePath))
            using (JsonTextReader reader = new JsonTextReader(file))
            {
                JObject bindingsJSON = (JObject)JToken.ReadFrom(reader); // parse binding file

                // load keyboard bindings
                JObject objKeyboard = (JObject)bindingsJSON[sectionName];
                foreach (var property in objKeyboard)
                {
                    if (Enum.TryParse(property.Key, out MouseButtons button))
                    {
                        bindings.Add(button, (string)property.Value);
                    }
                }
            }
        }
    }
}
