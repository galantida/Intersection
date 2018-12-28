using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace gameLogic
{

    // ussed to convert humnan input into car input
    public class clsDriverHuman : intDriver
    {
        // inputs and outputs
        private clsInput input;
        public clsGamePieceCar car;

        public clsDriverHuman(clsGamePieceCar car, clsInput input)
        {
            this.car = car;
            this.input = input;
        }

        public void update()
        {
            pedals(car.velocity.Length());
            steeringWheel();
        }

        // convert user input to gas and break petal control 
        void pedals(float velocityMegnitude)
        {
            // has the car stopped and they let up on gas and break pedal put it in neutral
            if ((velocityMegnitude < 0.01f) && (!input.forward) && (!input.backward))
            {
                car.shifter = 0;
            }

            // one or more pedals are pressed
            car.pedals = 0;
            switch ((int)car.shifter)
            {
                case 1: // forward
                    {
                        if (input.forward) car.pedals = 1; // accelerate forward
                        else if (input.backward) car.pedals = -1; // stop
                        break;
                    }
                case 0: // stationary
                    {
                        if (input.forward) car.shifter = 1; // drive
                        if (input.backward) car.shifter = -1; // reverse
                        break;
                    }
                case -1: // reverse
                    {
                        if (input.forward) car.pedals = -1; // accelerate backwards
                        else if (input.backward) car.pedals = 1; // stop
                        break;
                    }
            }

        }

        void steeringWheel()
        {
            car.steering = 0;
            if (input.left || input.right)
            {
                car.steering = -1;
                if (input.right) car.steering = 1;
                if (car.shifter == -1) car.steering = -car.steering;
            }
        }
    }
}
