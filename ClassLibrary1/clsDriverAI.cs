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
    public class clsDriverAI : intDriver
    {
        // inputs and outputs
        public clsGamePieceCar car;


        protected Stopwatch stopWatch = new Stopwatch();

        // AI instructions
        public clsGamePieceExit exit { get; set; }
        private clsWorld world { get; set; }

        // path
        public clsRoute route { get; set; }

        public clsDriverAI(clsGamePieceCar car, clsGamePieceExit exit, clsWorld world)
        {
            this.car = car;
            this.exit = exit;
            this.world = world;
            this.route = new clsRoute(world.findShortestPath(car.squareCoordinate, exit.squareCoordinate));
            stopWatch.Start();
        }

        public void update()
        {
            float deltaTime = stopWatch.ElapsedMilliseconds; // using the base stopwatch
            if (deltaTime > 100)
            {
                Vector2 wayPointWorldLocation = world.squareCoordinateToWorldLocation(this.route.currentWaypoint);

                // acceleration
                car.shifter = 1;
                car.pedals = 1;

                // get desired direction
                Vector2 desiredDirection = getDirection(wayPointWorldLocation); // this is the correct vector to my waypoint

                // how much are we off
                float steering = VectorMath.angleBetween(desiredDirection, car.velocity);
                if (double.IsNaN(steering)) steering = 0;

                // redirect the velocity
                car.steering = steering / -45;
                //if ((steering > -5f) && (steering < 5f)) car.velocity = desiredDirection * car.velocity.Length();
                //else car.steering -= steering / 2;


                // reached the destination
                if (Vector2.Distance(car.location, exit.location) < 32)
                {
                    exit.removeGamePiece(car);
                }
                


                // distance to way point
                float distance = Vector2.Distance(car.location, wayPointWorldLocation);

                if (distance < 64)
                {
                    try
                    {
                        // arrived at way point, advance to next one
                        this.route.advanceWaypoint();
                    }
                    catch(Exception ex)
                    {
                        // must be at exit remove car
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


        public Vector2 getDirection(Vector2 destinationLocation)
        {
            // get direction of way point from cars location
            //Vector2 b = car.location - destination;
            Vector2 b = destinationLocation - car.location;
            Vector2 destinationDirection = new Vector2(b.X, b.Y);
            destinationDirection.Normalize();
            return destinationDirection;
        }
    }
}
