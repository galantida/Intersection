using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace gameLogic
{
    // the physical piece which is unaware of its location on the board

    public abstract class clsBaseGamePiece
    {
        // interface fields
        public GamePieceType gamePieceType { get; set; }
        public Vector2 location { get; set; } // pixel location
        public float mass { get; set; }
        public Vector2 velocity { get; set; }
        public float friction { get; set; } // result of physical contact. (constant)
        public float resistance { get; set; } // result of wind resistance. (increases with velocity)

        // general object
        protected Stopwatch stopWatch = new Stopwatch();

        public clsBaseGamePiece(Vector2 location, Vector2 velocity, float mass = 1000.0f)
        {
            this.location = location;
            this.velocity = velocity;
            this.mass = mass;

            this.resistance = 0.001f;

            stopWatch.Start();
        }

        protected void update(clsWorld world)
        {
            float deltaTime = stopWatch.ElapsedMilliseconds;
            if (deltaTime > 10)
            {
                this.applyResistance(deltaTime);
                this.location += this.velocity * deltaTime; // calculate velocity to location in direction
                stopWatch.Restart(); // reset update timer
            }
        }

        public void applyResistance(float deltaTime)
        {
            if (velocity.Length() > .001)
            {
                Vector2 resistanceDirection = new Vector2(velocity.X, velocity.Y); // resistance is applied against the direction of movment
                resistanceDirection.Normalize();
                this.velocity -= (resistanceDirection * (this.resistance * deltaTime)); // reistance should be a percentage of velocity
            }
            else velocity = new Vector2(0, 0);
        }

        public void applyFriction(float friction, float deltaTime)
        {
            if (velocity.Length() > .001)
            {
                Vector2 frictionDirection = new Vector2(velocity.X, velocity.Y); // friction is applied against the direction of movment
                frictionDirection.Normalize();
                this.velocity -= frictionDirection * (friction / mass) * deltaTime;
            }
            else velocity = new Vector2(0, 0);
        }

        public void applyForce(Vector2 force, float deltaTime)
        {
            this.velocity += (force / mass) * deltaTime;
        }
    }
}
