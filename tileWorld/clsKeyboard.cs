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
    public class clsKeyboard
    {
        public Dictionary<Keys, string> bindings;
        private KeyboardState currentState;
        private KeyboardState lastState;

        public clsKeyboard()
        {
            // this is exe path 
            loadBindings("Keyboard", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"configuration\\inputBindings.json"));
        }

        public void reset()
        {
            lastState = currentState;
            currentState = Keyboard.GetState();
        }

        // test for a key
        public bool isPressed(Keys key)
        {
            if (currentState.IsKeyDown(key))
            {
                if (!lastState.IsKeyDown(key)) return true;
            }
            return false;
        }

        private void loadBindings(string sectionName, string configurationFilePath)
        {
            // clear bindings
            bindings = new Dictionary<Keys, string>();

            // read JSON directly from a file
            using (StreamReader file = File.OpenText(configurationFilePath))
            using (JsonTextReader reader = new JsonTextReader(file))
            {
                JObject bindingsJSON = (JObject)JToken.ReadFrom(reader); // parse binding file

                // load keyboard bindings
                JObject objKeyboard = (JObject)bindingsJSON[sectionName];
                foreach (var property in objKeyboard)
                {
                    if (Enum.TryParse(property.Key, out Keys key))
                    {
                        bindings.Add(key, (string)property.Value);
                    }
                }
            }
        }


    }
}
