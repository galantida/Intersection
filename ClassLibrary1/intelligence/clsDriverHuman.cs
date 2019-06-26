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
        public float lastUpdated { get; set; }

        KeyboardState lastState;

        public clsDriverHuman(clsCarObject car)
        {
            this.car = car;
        }

        public void update()
        {
            float deltaTime = car.world.currentTime - lastUpdated; // time since last updated
            if (deltaTime > 10)
            {
                lastUpdated = car.world.currentTime; // reset last updated

                // read human input
                keyboardInput();
                mouseInput();

                // human car inbounds only
                float worldSize = 14 * 64;
                while (car.location.X < 0) car.location = new Vector2(car.location.X + worldSize, car.location.Y);
                while (car.location.X > worldSize) car.location = new Vector2(car.location.X - worldSize, car.location.Y);
                while (car.location.Y < 0) car.location = new Vector2(car.location.X, car.location.Y + worldSize);
                while (car.location.Y > worldSize) car.location = new Vector2(car.location.X, car.location.Y - worldSize);
            }
        }

        public void mouseInput()
        {
            MouseState state = Mouse.GetState();
        }

        public void keyboardInput()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                if (!lastState.IsKeyDown(Keys.Up)) car.acceleratorPedal += 0.1f;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                if (!lastState.IsKeyDown(Keys.Down)) car.acceleratorPedal -= 0.1f;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                if (!lastState.IsKeyDown(Keys.Right)) car.steeringWheel += 0.1f;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                if (!lastState.IsKeyDown(Keys.Left)) car.steeringWheel -= 0.1f;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.N))
            {
                car.shifter = ShifterPosition.neutral;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                car.shifter = ShifterPosition.drive;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.R))
            {
                car.shifter = ShifterPosition.reverse;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.B))
            {
                car.acceleratorPedal = 0;
                car.breakPedal += 0.1f;
            }
            else
            {
                car.breakPedal -= 0.1f;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.H))
            {
                if (!lastState.IsKeyDown(Keys.H))
                {
                    if (this.car.lights == 0) this.car.lights = 3;
                    else this.car.lights = 0;
                }
            }

            if (Keyboard.GetState().IsKeyDown(Keys.OemComma))
            {
                if (!lastState.IsKeyDown(Keys.OemComma)) car.turnSignal--;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.OemPeriod))
            {
                if (!lastState.IsKeyDown(Keys.OemPeriod)) car.turnSignal++;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.E))
            {
                if (!lastState.IsKeyDown(Keys.E))
                {
                    if (!this.car.hazzard) this.car.hazzard = true;
                    else this.car.hazzard = false;
                }
            }

            lastState = Keyboard.GetState();
        }
    }
}
