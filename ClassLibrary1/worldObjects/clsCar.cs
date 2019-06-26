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

    public class clsCarObject : clsBaseWorldObject, intWorldObject
    {
        // car specifications
        private float acceleration; // force to add in the direction of the transmissions
        private float breaking; // creates additional kinetic friction coefficient
        private float handling; // .05 was good

        // control status
        public ShifterPosition shifter { get; set; } // reverse, neutral, drive
        private float _acceleratorPedal;
        private float _breakPedal;
        private float _steeringWheel;
        private int _turnSignal; // (-1 left, 0, 1 right)
        private int _lights;
        private bool _hazzard;

        // indicators
        private Stopwatch flasher;


        public clsCarObject(clsWorld world, Vector2 location, Vector2 direction, Vector2 velocity) : base(world, location, direction, velocity)
        {
            // general
            base.worldObjectType = WorldObjectType.car;

            // newtonian properties
            base.mass = 10.0f; // weight (10 = 4000 lbs.)
            base.kineticFrictionCoefficient.baseValue = 0.002f; // drag while moving
            base.staticFrictionCoefficient.baseValue = 0.001f; // drag on starting to move
            base.surfaceArea = 0.02f; // surface area facing wind
            base.dragCoefficient.baseValue = 0.25f; // wind resistance
            base.collisionDetection = true;

            // Car properties
            this.acceleration = 0.02f; // force to add in the direction of the transmissions
            this.breaking = 0.05f; // creates additional kinetic friction coefficient
            this.handling = 1.0f; // im pact of steering

            // mechanical status
            this.shifter = ShifterPosition.neutral;

            // flasher circuit
            flasher = new Stopwatch();
            flasher.Start();
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
                        car specific physics
                ****************************************/
                if (this.velocity.Length() < 0.001f) this.velocity = new Vector2(0, 0); // to slow to keep moving


                /**************************************** 
                       acceleration and force
               ****************************************/
                // only accelerate if pedal is pressed
                Vector2 addedForce = new Vector2(0, 0);
                if (_acceleratorPedal > 0)
                {
                    // add force based on the shifter postion, acceleration amount and direction the car is facing
                    addedForce = direction * ((int)shifter - 1) * acceleration * acceleratorPedal;
                    base.addForce(addedForce);
                }

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
                Vector2 existingDirection = new Vector2(direction.X, direction.Y); // copy existing direction
                if (velocity.Length() > 0)
                {
                    float rotation = ((handling * steeringWheel)/50) * deltaTime * velocity.Length(); // calculate new car rotation based on handling steering wheel and time
                    direction = existingDirection.Rotate(rotation); // rotate car (this has to be updated for valiable rotation)
                }

                // alter velocity direction based on cars new rotated facing direction
                Vector2 existingVelocity = new Vector2(velocity.X, velocity.Y); // get the existing velocity
                if (shifter == ShifterPosition.drive) velocity = direction * existingVelocity.Length(); // direction is forward
                else velocity = -direction * existingVelocity.Length(); // direction is backward


                /**************************************** 
                   model display
               ****************************************/
                base.colorReplacements = new Dictionary<Color, Color>();
                if (this.breakLights)
                {
                    Color source = new Color(128, 0, 0);
                    Color destination = new Color(255, 0, 0);
                    colorReplacements.Add(source, destination);
                }

                if (this.reverseLights)
                {
                    Color source = new Color(128, 128, 129);
                    Color destination = new Color(255, 255, 255);
                    colorReplacements.Add(source, destination);
                }

                if ((this.leftTurnSignal) || (this.hazzards))
                {
                    Color source = new Color(215, 90, 0);
                    Color destination = new Color(255, 192, 0);
                    colorReplacements.Add(source, destination);
                }

                if ((this.rightTurnSignal) || (this.hazzards))
                {
                    Color source = new Color(215, 90, 1);
                    Color destination = new Color(255, 192, 0);
                    colorReplacements.Add(source, destination);
                }

                if (this.headLights)
                {
                    Color source = new Color(128, 128, 128);
                    Color destination = new Color(255, 255, 255);
                    colorReplacements.Add(source, destination);
                }
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

        public int lights
        {
            set
            {
                // 0-off, 1 parking lights, 2 running lights, 3 high beams
                int tmp = value;
                if (tmp < 0) tmp = 0;
                else if (tmp > 3) tmp = 3;
                _lights = tmp;
            }
            get
            {
                return _lights;
            }

        }

        public int turnSignal
        {
            get
            {
                return _turnSignal;
            }
            set
            {
                if (value == 0)
                {
                    _turnSignal = 0;
                }
                else
                {
                    if (value > 0) _turnSignal = 1;
                    else if (value < 0) _turnSignal = -1;
                }
            }
        }

        public bool hazzard
        {
            get
            {
                return _hazzard;
            }
            set
            {
                if (value)
                {
                    _hazzard = true;
                }
                else
                {
                    _hazzard = false;
                }
            }
        }
        #endregion


        /**************************************** 
             outputs
        ****************************************/
        #region Output Functions

        public float mph
        {
            get
            {
                // 64 pixels = 15 feet
                // mile = 5280 feet
                // 22528 pixels in a mile
                // pixel = .234375 feet

                float pixelsPerMilisecond = base.velocity.Length();
                float pixelsPerHour = (((pixelsPerMilisecond * 1000) * 60) * 60);
                float mph = pixelsPerHour / 22528;

                return mph; // (pixels in a mile / pixels per miliseond) / (60 * 1000)
            }
        }

        public CardinalDirection cardinalDirection
        {
            get
            {
                double angle = Math.Atan2(direction.Y, direction.X);
                int octant = (int)Math.Round(8 * angle / (2 * Math.PI) + 8) % 8;
                return (CardinalDirection)octant;
            }
        }

        public bool headLights
        {
            get
            {
                if (this.lights >= 2) return true;
                else return false;
            }
        }

        public bool breakLights
        {
            get
            {
                if (this.breakPedal > 0) return true;
                else return false;
            }
        }

        public bool reverseLights
        {
            get
            {
                if (this.shifter == ShifterPosition.reverse) return true;
                else return false;
            }
        }

        public bool leftTurnSignal
        {
            get
            {
                if(this.turnSignal == -1)
                {
                    if ((flasher.Elapsed.Milliseconds % 500) < 250)
                    {
                        return true;
                    }

                }
                return false;
            }
        }

        public bool rightTurnSignal
        {
            get
            {
                if (this.turnSignal == 1)
                {
                    if ((flasher.Elapsed.Milliseconds % 500) < 250)
                    {
                        return true;
                    }

                }
                return false;
            }
        }

        public bool hazzards
        {
            get
            {
                if (this.hazzard == true)
                {
                    if ((flasher.Elapsed.Milliseconds % 500) < 250)
                    {
                        return true;
                    }

                }
                return false;
            }
        }

        public float lbs()
        {
            return (base.weight() / 2.5f);
        }
        #endregion
    }
}
