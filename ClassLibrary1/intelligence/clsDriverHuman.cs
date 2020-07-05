using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using tileWorld;
using Microsoft.Xna.Framework.Input;

namespace gameLogic
{

    // ussed to convert humnan input into car input
    public class clsDriverHuman : clsActor, intActor
    {
        // inputs and outputs
        public clsCar car;
        private clsInput input;

        public clsDriverHuman(clsWorld world, clsCar car, clsInput input) : base(world, car)
        {
            this.car = car;
            this.input = input;
        }

        public void update(float currentTime)
        {
            float deltaTime = currentTime - lastUpdated; // time since last updated
            if (deltaTime > 10)
            {
                lastUpdated = currentTime; // reset last updated

                // read human input
                readUserInput();

                /*
                // human car inbounds only
                float worldSize = 14 * 64;
                while (car.location.X < 0) car.location = new Vector2(car.location.X + worldSize, car.location.Y);
                while (car.location.X > worldSize) car.location = new Vector2(car.location.X - worldSize, car.location.Y);
                while (car.location.Y < 0) car.location = new Vector2(car.location.X, car.location.Y + worldSize);
                while (car.location.Y > worldSize) car.location = new Vector2(car.location.X, car.location.Y - worldSize);
                */

                base.update();
            }
        }

        public void readUserInput()
        {
            // accelerator
            if (input.isActivated(InputActionNames.Accelerate)) car.acceleratorPedal += 0.1f;
            else if (input.isActivated(InputActionNames.Decelerate)) car.acceleratorPedal -= 0.1f;
            //else car.acceleratorPedal = 0.0f;

            // breaking
            if (input.isActivated(InputActionNames.Break)) car.breakPedal += 0.1f;
            else car.breakPedal = 0.0f;

            if (input.isActivated(InputActionNames.SteerLeft)) car.steeringWheel -= 0.1f;
            if (input.isActivated(InputActionNames.SteerRight)) car.steeringWheel += 0.1f;

            if (input.isActivated(InputActionNames.ShiftNeutral)) car.shifter = ShifterPosition.neutral;
            if (input.isActivated(InputActionNames.ShiftDrive)) car.shifter = ShifterPosition.drive;
            if (input.isActivated(InputActionNames.ShiftReverse)) car.shifter = ShifterPosition.reverse;



            if (input.isActivated(InputActionNames.LeftTurnSignal)) car.turnSignal--;
            if (input.isActivated(InputActionNames.RightTurnSignal)) car.turnSignal++;

            if (input.isActivated(InputActionNames.HeadLights))
            {
                if (this.car.lights == 0) this.car.lights = 3;
                else this.car.lights = 0;
            }

            if (input.isActivated(InputActionNames.Hazzards))
            {
                if (!this.car.hazzard) this.car.hazzard = true;
                else this.car.hazzard = false;
            }
        }
    }
}
