using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace gameLogic
{
    public abstract class clsBaseGamePiece
    {
        // interface fields
        public GamePieceType gamePieceType { get; set; }
        public Vector2 location { get; set; }
        public float mass { get; set; }
        public Vector2 velocity { get; set; }

        // general object
        protected clsWorld world;
        protected Stopwatch stopWatch = new Stopwatch();

        public clsBaseGamePiece(clsWorld world, Vector2 location, Vector2 velocity, float mass = 1000.0f)
        {
            this.world = world;
            this.location = location;
            this.velocity = velocity;
            this.mass = mass;

            stopWatch.Start();
        }

        public void update()
        {
            float deltaTime = stopWatch.ElapsedMilliseconds;
            if (deltaTime > 10)
            {
                this.location += this.velocity * deltaTime; // apply velocity to location in direction
                stopWatch.Restart(); // reset update timer
            }
        }

        public void applyForce(Vector2 force, float deltaTime)
        {
            this.velocity += (force / mass) * deltaTime;
        }

        public void applyResistance(float resistance, float deltaTime)
        {
            if (velocity.Length() > .001)
            {
                Vector2 resistanceDirection = new Vector2(velocity.X, velocity.Y);
                resistanceDirection.Normalize();
                this.velocity -= resistanceDirection * (resistance / mass) * deltaTime;
            }
            else velocity = new Vector2(0, 0);
        }

        public Vector2 squareCoordinate
        {
            get
            {
                int x = (int)(location.X / 64);
                int y = (int)(location.Y / 64);
                return new Vector2(x,y);
            }
            set
            {
                location = (value * 64) + new Vector2(32,32);
            }
        }

        public clsSquare square
        {
            get
            {
                Vector2 squareCoordinate = this.squareCoordinate;
                return world.squares[(int)squareCoordinate.X, (int)squareCoordinate.Y];
            }
        }
    }
}
