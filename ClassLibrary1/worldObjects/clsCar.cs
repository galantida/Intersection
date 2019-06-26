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

    public class clsCarObject : clsBaseGameObject, intGamePiece
    {
        // car specifications
        private float acceleration; // force to add in the direction of the transmissions
        private float breaking; // creates additional kinetic friction coefficient
        private float handling; // .05 was good

        // control status
        private float _acceleratorPedal;
        private float _breakPedal;
        private float _steeringWheel;
        public ShifterPosition shifter { get; set; } // reverse, neutral, drive

        // car status
        public Vector2 heading { get; set; }

        public clsCarObject(clsWorld world, Vector2 location, Vector2 heading, Vector2 velocity) : base(world, location, velocity)
        {
            // general
            base.gamePieceType = GamePieceType.car;

            // newtonian properties
            base.mass = 10.0f; // weight (10 = 4000 lbs.)
            base.kineticFrictionCoefficient.baseValue = 0.001f; // drag while moving
            base.staticFrictionCoefficient.baseValue = 0.002f; // drag on starting to move
            base.surfaceArea = 0.01f; // surface area facing wind
            base.dragCoefficient.baseValue = 0.25f; // wind resistance

            // Car properties
            this.acceleration = 0.02f; // force to add in the direction of the transmissions
            this.breaking = 1.0f; // creates additional kinetic friction coefficient
            this.handling = 0.002f; // im pact of steering

            // mechanical status
            this.heading = heading; // car direction
            this.shifter = ShifterPosition.neutral;
        }


        /**************************************** 
            Processing Functions
        ****************************************/
        #region Processing Functions
        new public void update()
        {
            float deltaTime = world.currentTime - lastUpdated; // time since last updated
            if (deltaTime > 10)
            {
                lastUpdated = world.currentTime; // reset last updated


                /**************************************** 
                       breaking and resistance
               ****************************************/
                // only break if the pedal is pressed
                if (_breakPedal > 0)
                {
                    // add resistance steering direction and transmission are irrelevant
                    kineticFrictionCoefficient.addedValue = breaking;
                }
                else
                {
                    // remove resistance
                    kineticFrictionCoefficient.addedValue = 0;
                }


                /**************************************** 
                        steering and direction
                ****************************************/
                // change facing direction based on steering wheel position
                Vector2 existingDirection = new Vector2(heading.X, heading.Y); // copy existing direction
                float rotation = (handling * (steeringWheel / 2)) * deltaTime; // calculate new car rotation based on handling steering wheel and time
                if (rotation != 0)
                {
                    heading = existingDirection.Rotate(rotation); // rotate car (this has to be updated for valiable rotation)
                }

                // alter velocity direction based on cars new rotated facing direction
                Vector2 existingVelocity = new Vector2(velocity.X, velocity.Y); // get the existing velocity
                if (shifter == ShifterPosition.drive) velocity = heading * existingVelocity.Length(); // direction is forward
                else velocity = -heading * existingVelocity.Length(); // direction is backward


                /**************************************** 
                        acceleration and force
                ****************************************/
                // only accelerate if pedal is pressed
                Vector2 addedForce = new Vector2(0, 0);
                if (_acceleratorPedal > 0)
                {
                    // add force based on the shifter postion, acceleration amount and direction the car is facing
                    addedForce = heading * ((int)shifter - 1) * acceleration * acceleratorPedal;
                }

                // went off the map
                //if (!world.inWorldBounds(this.location)) world.removeGamePiece(this);

                base.applyForce(addedForce);
            }

            // always apply phypics
            base.update();
        }
        #endregion


        /**************************************** 
             inputs
        ****************************************/
        #region Input Functions
        public float acceleratorPedal
        {
            // accelerator pedal value 0 - 1
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
        

        public float breakPedal
        {
            // break pedal value 0 - 1
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
        

        public float steeringWheel
        {
            // -1 to 1 representing steeringwheel position
            get
            {
                return _steeringWheel;
            }
            set
            {
                _steeringWheel = value;
                if (_steeringWheel > 1) _steeringWheel = 1;
                else if (_steeringWheel < -1) _steeringWheel = -1;
            }
        }
        #endregion


        /**************************************** 
             outputs
        ****************************************/
        #region Output Functions

        public float speed
        {
            get
            {
                // 64 pixels = 15 feet
                // mile = 5280 feet
                // 22528 pixels in a mile
                // pixel = .234375 feet

                float pixelsPerMilisecond = base.velocity.Length();
                float pixelsPerHour = (((pixelsPerMilisecond * 1000) * 60) * 60);
                float MPH = pixelsPerHour / 22528;

                return MPH; // (pixels in a mile / pixels per miliseond) / (60 * 1000)
            }
        }

        public CardinalDirection cardinalDirection
        {
            get
            {
                double angle = Math.Atan2(heading.Y, heading.X);
                int octant = (int)Math.Round(8 * angle / (2 * Math.PI) + 8) % 8;
                return (CardinalDirection)octant;
            }
        }

        public float weightPounds()
        {
            return (base.weight() / 2.5f);
        }
        #endregion
    }
}
