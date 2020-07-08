using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using tileWorld;

namespace gameLogic
{
    public enum InputActions { ZoomIn, ZoomOut, ShiftDrive, ShiftReverse, ShiftNeutral, Accelerate, Decelerate, Break, SteerLeft, SteerRight, HeadLights, LeftTurnSignal, RightTurnSignal, Hazzards };
    
    public class clsActionMapping
    {
        public DeviceTypes deviceType { get; }
        public InputActions actionName { get; }
        public Keys key { get; }
        public MouseButtons button {get; }

        public clsActionMapping(InputActions actionName, Keys key) 
        {
            this.actionName = actionName;
            this.deviceType = DeviceTypes.Keyboard;
            this.key = key;
        }

        public clsActionMapping(InputActions actionName, MouseButtons button)
        {
            this.actionName = actionName;
            this.deviceType = DeviceTypes.Mouse;
            this.button = button;
        }
    }
}
