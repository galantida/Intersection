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
        private float acceleration = 0.02f; // force to add in the direction of the transmissions
        private float breaking = 1.0f; // creates additional kinetic friction coefficient
        private float handling = 0.002f; // .05 was good

        // control status
        private float _acceleratorPedal;
        private float _breakPedal;
        private float _steeringWheel;
        public ShifterPosition shifter { get; set; } // reverse, neutral, drive

        // car status
        public Vector2 direction { get; set; }

        public clsCarObject(clsWorld world, Vector2 location, Vector2 direction, Vector2 velocity) : base(world, location, velocity)
        {
            // newtonian physics
            base.mass = 1.0f;
            base.surfaceArea = 0.01f;
            base.kineticFrictionCoefficient.baseValue = 0.02f;
            base.staticFrictionCoefficient.baseValue = 0.03f;
            base.dragCoefficient.baseValue = 0.25f;

            // general
            base.gamePieceType = GamePieceType.car;

            // mechanical properties
            this.direction = direction; // car direction
            this.shifter = ShifterPosition.neutral;
        }

        // car inputs
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
                else if (_steeringWheel < 0) _steeringWheel = 0;
            }
        }

        
        public float speed
        {
            get
            {
                return base.velocity.Length(); // speed in mile per hour
            }
        }

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
                float addedFrictionCoefficient = 0;
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
                Vector2 existingDirection = new Vector2(direction.X, direction.Y); // copy existing direction
                float rotation = (handling * (steeringWheel/2)) * deltaTime; // calculate new car rotation based on handling steering wheel and time
                if (rotation != 0)
                {
                    direction = existingDirection.Rotate(rotation); // rotate car (this has to be updated for valiable rotation)
                }

                // alter velocity direction based on cars new rotated facing direction
                Vector2 existingVelocity = new Vector2(velocity.X, velocity.Y); // get the existing velocity
                if (shifter == ShifterPosition.drive) velocity = direction * existingVelocity.Length(); // direction is forward
                else velocity = -direction * existingVelocity.Length(); // direction is backward


                /**************************************** 
                        acceleration and force
                ****************************************/
                // only accelerate if pedal is pressed
                Vector2 addedForce = new Vector2(0, 0);
                if (_acceleratorPedal > 0)
                {
                    // add force based on the shifter postion, acceleration amount and direction the car is facing
                    addedForce = direction * ((int)shifter - 1) * acceleration;
                }

                // went off the map
                //if (!world.inWorldBounds(this.location)) world.removeGamePiece(this);

                base.applyForce(addedForce, addedFrictionCoefficient);
            }

            // always apply phypics
            base.update();
        }
    }


}
