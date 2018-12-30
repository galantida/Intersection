using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.Xna.Framework;


namespace gameLogic
{
    public enum ShifterPosition { reverse, neutral, drive }

    public class clsGamePieceCar : clsBaseGamePiece, intGamePiece
    {
        // specifications
        private float acceleration = 0.2f; // force to add in the direction of the transmissions
        private float breaking = 0.5f; // creates additional drag on the dirtion of force
        private float handling = 0.01f; // .05 was good
        private float weight = 3000; // weight in pounds
        private float topSpeed = .25f; // .2 was good
        private float rollingResistance = 0.01f; // default resistance caused by the wheels


        // inputs
        public float acceleratorPedal { get; set; }
        public float breakPedal { get; set; }
        public float steeringWheel { get; set; }
        public ShifterPosition shifter { get; set; }
        // driver either AI or HUman
        public intDriver driver;


        // car status
        public Vector2 direction { get; set; }
        public float speed
        {
            get
            {
                return base.velocity.Length();
            }
        }


        public clsGamePieceCar(intDriver driver, Vector2 location, Vector2 direction, Vector2 velocity) :base(location, velocity)
        {
            this.driver = driver;
            base.gamePieceType = GamePieceType.car;
            base.mass = this.weight / 3;
            this.direction = direction; // car direction
        }

        new public void update(clsWorld world)
        {
            float deltaTime = stopWatch.ElapsedMilliseconds; // using the base stopwatch
            if (deltaTime > 10)
            {
                // update AI
                driver.update(this);

                /**************************************** 
                        steering and direction
                ****************************************/

                // change facing direction based on steering wheel position
                handling = 0.002f;
                Vector2 existingDirection = new Vector2(direction.X, direction.Y); // copy existing direction
                float rotation = (handling * steeringWheel) * deltaTime; // calculate new car rotation based on handling steering wheel and time
                if (rotation != 0)
                {
                    direction = existingDirection.Rotate(rotation); // rotate car
                }

                // alter velocity direction based on cars new rotated facing direction
                Vector2 existingVelocity = new Vector2(velocity.X, velocity.Y); // get the existing velocity
                if (shifter == ShifterPosition.drive) velocity = direction * existingVelocity.Length(); // direction is forward
                else velocity = -direction * existingVelocity.Length(); // direction is backward


                /**************************************** 
                        acceleration and force
                ****************************************/
                // only accelerate if pedal ispressed
                if (acceleratorPedal > 0)
                {
                    // add force based on the shifter postion, acceleration amount and direction the car is facing
                    this.applyForce((direction * ((int)shifter - 1)) * acceleration, deltaTime);
                }
                

                /**************************************** 
                        breaking and resistance
                ****************************************/
                float totalResistance = rollingResistance; // standard rolling resistance

                // only break if the pedal is pressed
                if (breakPedal > 0)
                {
                    // add resistance direction adn transmission are irrelevant
                    totalResistance += breaking;
                }

                // resistance is always applied
                applyResistance(totalResistance, deltaTime);


                // went off the map
                //if (!world.inWorldBounds(this.location)) world.removeGamePiece(this);

                base.update(world);
            }
        }
    }


}
