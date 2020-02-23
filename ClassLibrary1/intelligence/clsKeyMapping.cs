using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using gameLogic;

namespace gameLogic
{
    public class clsKeyMapping
    {
        public Keys key { get; }
        public InputActionName actionName { get; }

        public clsKeyMapping(Keys key, InputActionName actionName)
        {
            this.key = key;
            this.actionName = actionName;
        }
    }
}
