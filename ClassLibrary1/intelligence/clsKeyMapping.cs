using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using gameLogic;
using tileWorld;

namespace gameLogic
{
    public enum ControlTypes { Keyboard, Mouse }

    public class clsControlMapping
    {
        public ControlTypes controlType { get; }
        public InputActionNames actionName { get; }
        public Keys key { get; }
        public MouseButtons button {get; }

        public clsControlMapping(Keys key, InputActionNames actionName) // swap these at some point. Action should be first
        {
            this.actionName = actionName;
            this.controlType = ControlTypes.Keyboard;
            this.key = key;
        }

        public clsControlMapping(InputActionNames actionName, MouseButtons button)
        {
            this.actionName = actionName;
            this.controlType = ControlTypes.Mouse;
            this.button = button;
        }
    }
}
