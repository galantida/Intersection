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
        //private float keyMode = 1; // 1 for forward and -1 for reverse and 0 for neutral

        public clsDriverHuman(clsInput input)
        {
            this.input = input;
        }

        public void update(clsGamePieceCar car)
        {
            pedals(car);
            steeringWheel(car);
        }

        // convert user input to gas and break petal control 
        void pedals(clsGamePieceCar car)
        {
            float velocityMegnitude = car.velocity.Length();

            switch (car.shifter)
            {
                case ShifterPosition.neutral:
                    {
                        if (input.forward) car.shifter = ShifterPosition.drive; // drive
                        if (input.backward) car.shifter = ShifterPosition.reverse; // reverse
                        break;
                    }
                case ShifterPosition.drive:
                    {
                        if (input.forward) car.acceleratorPedal = 1;
                        else car.acceleratorPedal = 0;

                        if (input.backward) car.breakPedal = 1;
                        else car.breakPedal = 0;

                        if ((velocityMegnitude < 0.01f) && (!input.forward) && (!input.backward))
                        {
                            car.shifter = ShifterPosition.neutral; // neutral
                        }
                        break;
                    }
                case ShifterPosition.reverse:
                    {
                        if (input.forward) car.breakPedal = 1;
                        else car.breakPedal = 0;

                        if (input.backward) car.acceleratorPedal = 1;
                        else car.acceleratorPedal = 0;

                        if ((velocityMegnitude < 0.01f) && (!input.forward) && (!input.backward))
                        {
                            car.shifter = ShifterPosition.neutral; // neutral
                        }
                        break;
                    }
            }
        }

        void steeringWheel(clsGamePieceCar car)
        {
            car.steeringWheel = 0;
            if (input.left || input.right)
            {
                car.steeringWheel = -1;
                if (input.right) car.steeringWheel = 1;
                if (car.shifter == ShifterPosition.reverse) car.steeringWheel = -car.steeringWheel; // flip the left right keys if we are going backwards
            }
        }
    }
}
