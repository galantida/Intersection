using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace gameLogic
{

    // ussed to convert humnan input into car input
    public class clsDriverHuman : intDriver
    {
        // inputs and outputs
        private clsCarObject car;
        private KeyboardState keyboard;
        private MouseState mouse;

        public clsDriverHuman(clsCarObject car, Keyboard kb, Mouse mouse)
        {
            this.car = car;
        }

        public void update()
        {
            float worldSize = 14 * 64;

            // inbounds
            while (car.location.X < 0) car.location = new Vector2(car.location.X + worldSize, car.location.Y);
            while (car.location.X > worldSize) car.location = new Vector2(car.location.X - worldSize, car.location.Y);
            while (car.location.Y < 0) car.location = new Vector2(car.location.X, car.location.Y + worldSize);
            while (car.location.Y > worldSize) car.location = new Vector2(car.location.X, car.location.Y - worldSize);
        }
                

        // convert user input to gas and break petal control 
        void accelerator(float amount)
        {
            car.acceleratorPedal = amount;
        }

        void steeringWheel(float amount)
        {
            car.steeringWheel = amount;
        }

        void shifter(ShifterPosition shifterPosition)
        {
            car.shifter = shifterPosition;
        }

        /*

        // convert user input to gas and break petal control 
        void pedals_old(clsCarObject car)
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

        void steeringWheel_old(clsCarObject car)
        {
            car.steeringWheel = 0;
            if (input.left || input.right)
            {
                car.steeringWheel = -1;
                if (input.right) car.steeringWheel = 1;
                if (car.shifter == ShifterPosition.reverse) car.steeringWheel = -car.steeringWheel; // flip the left right keys if we are going backwards
            }
        }

    */
    }
}
