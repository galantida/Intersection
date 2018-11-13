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

    public class clsCar : clsBaseGamePiece, intGamePiece
    {
        // specifications
        public float acceleration = 0.2f; // force to add in the direction of the transmissions
        public float breaking = 0.5f; // creates additional drag on the dirtion of force
        public float handling = 0.01f; // .05 was good
        public float weight = 1000;
        public float topSpeed = 0.1f; // .2 was good
        public float rollingResistance = 0.01f; // default resistance caused by the wheels

        // inputs
        public float pedals { get; set; }
        public float steering { get; set; }
        public float shifter { get; set; }

        // car status
        public Vector2 direction { get; set; }


        public clsCar(clsWorld world, Vector2 location, Vector2 direction, Vector2 velocity) :base(world, location, velocity)
        {
            base.gamePieceType = GamePieceType.car;
            base.mass = this.weight;
            this.direction = direction; // car direction
        }

        new public void update()
        {
            float deltaTime = stopWatch.ElapsedMilliseconds; // using the base stopwatch
            if (deltaTime > 10)
            {
                // change facing direction based on steering wheel position
                Vector2 existingDirection = new Vector2(direction.X, direction.Y);

                handling = 0.002f;
                float rotation = (handling * steering) * deltaTime;

                //if (float.IsNaN(rotation)) rotation = 0;
                //if (float.IsPositiveInfinity(rotation) || float.IsNegativeInfinity(rotation)) rotation = 0;
                if (rotation != 0)
                {
                    direction = existingDirection.Rotate(rotation);
                }

                


                // acceleration
                if (pedals == 1)
                {
                    if (this.velocity.Length() < this.topSpeed)
                    {
                        // add force based on acceleration and transmission direction
                        this.applyForce((direction * shifter) * acceleration, deltaTime);
                    }
                    else 
                    {
                        // top speed
                        Vector2 newVelocity = velocity;
                        newVelocity.Normalize();
                        newVelocity *= topSpeed;
                        velocity = newVelocity;
                    }
                }

                // Resistance
                float totalResistance = rollingResistance;
                if (pedals == -1) totalResistance += breaking;
                applyResistance(totalResistance, deltaTime);


                // alter velocity direction based on facing direction
                Vector2 existingVelocity = new Vector2(velocity.X, velocity.Y); // get the existing momentium
                if (shifter == 1) velocity = direction * existingVelocity.Length();
                else velocity = -direction * existingVelocity.Length();


                // went off the map
                if (!world.inWorldBounds(squareCoordinate)) world.removeGamePiece(this);


                base.update();
            }
        }
    }


}
