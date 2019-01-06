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
        // car specifications
        private float acceleration = 0.2f; // force to add in the direction of the transmissions
        private float breaking = 0.5f; // creates additional drag on the dirtion of force
        private float handling = 0.01f; // .05 was good
        private float weight = 3000; // weight in pounds
        private float rollingResistance = 0.01f; // default resistance caused by the wheels

        // car inputs
        public float acceleratorPedal {
            get
            {
                return _acceleratorPedal;
            }
            set
            {
                _acceleratorPedal = value;
                if (_acceleratorPedal > 1) _acceleratorPedal = 1;
                else if (_acceleratorPedal < 0) _acceleratorPedal = 0;
            }
        }
        private float _acceleratorPedal;

        public float breakPedal
        {
            get
            {
                return _breakPedal;
            }
            set
            {
                _breakPedal = value;
                if (_breakPedal > 1) _breakPedal = 1;
                else if (_breakPedal < 0) _breakPedal = 0;
            }
        }
        private float _breakPedal;

        public float steeringWheel { get; set; }
        public ShifterPosition shifter { get; set; }
        public intDriver driver { get; set; } // driver either AI or HUman

        // car status
        public Vector2 direction { get; set; }
        public float speed
        {
            get
            {
                return base.velocity.Length() * 100; // speed in mile per hour
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
                // update this car base on the drivers input
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
                if (_acceleratorPedal > 0)
                {
                    // add force based on the shifter postion, acceleration amount and direction the car is facing
                    this.applyForce((direction * ((int)shifter - 1)) * acceleration, deltaTime);
                }
                

                /**************************************** 
                        breaking and resistance
                ****************************************/
                float totalResistance = rollingResistance; // standard rolling resistance

                // only break if the pedal is pressed
                if (_breakPedal > 0)
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
