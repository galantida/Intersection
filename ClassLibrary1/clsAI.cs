using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.Xna.Framework;


namespace gameLogic
{
    public class clsAI : intDriver
    {
        // inputs and outputs
        public clsCar car;


        protected Stopwatch stopWatch = new Stopwatch();

        // AI instructions
        public clsExit exit { get; set; }
        private clsWorld World { get; set; }

        // path
        public Vector2 waypoint { get; set; }
        public clsRoute route { get; set; }

        public clsAI(clsCar car, clsExit exit, clsWorld world)
        {
            this.car = car;
            this.exit = exit;
            this.World = world;
            this.waypoint = new Vector2((car.squareCoordinate.X * 64) + 32, (car.squareCoordinate.Y * 64) + 32);

            stopWatch.Start();
        }

        public void update()
        {
            float deltaTime = stopWatch.ElapsedMilliseconds; // using the base stopwatch
            if (deltaTime > 100)
            {

                // acceleration
                car.shifter = 1;
                car.pedals = 1;

                // get desired direction
                Vector2 desiredDirection = getDirection(waypoint); // this is the correct vector to my waypoint

                float steering = VectorMath.angleBetween(desiredDirection, car.velocity);

                if ((steering > -5f) && (steering < 5f)) car.velocity = desiredDirection * car.velocity.Length();
                else car.steering -= steering  / 2;
                

                


               

                // reached the detination
                if (Vector2.Distance(car.location, exit.location) < 32)
                {
                    exit.removeGamePiece(car);
                }



                // distance to way point
                float distance = Vector2.Distance(car.location, this.waypoint);

                if (distance < 32)
                {
                    try
                    {
                        // arrived at way point
                        this.waypoint = chooseNewWaypoint();
                    }
                    catch(Exception ex)
                    {
                        exit.removeGamePiece(car);
                    }
                }
                else if (distance > 0)
                {
                    // keep trying

                }

                stopWatch.Restart(); // reset update timer
            }
        }


        public Vector2 chooseNewWaypoint()
        {
            // use new square to determine next way point
            clsSquare carSquare = car.square;

            // chose new way point
            Vector2 bestWaypoint = this.waypoint;
            float bestDistance = 1000000000f;
            foreach (Vector2 newDirection in carSquare.directions)
            {
                Vector2 newWaypoint = waypoint + (newDirection * 64);
                float newDistance = Vector2.Distance(newWaypoint, exit.location);
                if (newDistance < bestDistance)
                {
                    // found better choice
                    bestDistance = newDistance;
                    bestWaypoint = newWaypoint;
                }
            }
            return bestWaypoint; // set way point
        }

        public Vector2 desiredDirection()
        {
            return getDirection(waypoint); // this is the correct vector to my waypoint
        }


        public Vector2 getDirection(Vector2 destination)
        {
            // get direction of way point from cars location
            //Vector2 b = car.location - destination;
            Vector2 b = destination - car.location;
            Vector2 destinationDirection = new Vector2(b.X, b.Y);
            destinationDirection.Normalize();
            return destinationDirection;
        }
    }
}
