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
        protected Stopwatch stopWatch = new Stopwatch();

        // AI instructions
        private clsWorld world { get; set; }
        public Vector2 destination { get; set; }
        public clsRoute route { get; set; }
        public float speedLimit { get; set; }

        // driver spped differences
        float speedRangePercentage; // top and bottom speeds, when to give more gas or break. (e.g. 5% under target or 5 over target)
        float speedComplianceVariationPercentage; // faster or slower than normal. (e.g. 10% slower than normal)


        public clsDriverAI(clsWorld world, Vector2 destination)
        {
            // inputs
            this.world = world;
            this.destination = destination;
            this.route = null;
            speedLimit = 25f;

            // driver differences
            speedRangePercentage = (world.random.Next(10) - 5) / 100.0f;  
            speedComplianceVariationPercentage = (world.random.Next(10) - 5) / 100.0f; 

            // start
            stopWatch.Start();
        }

        public void update(clsGamePieceCar car)
        {
            float deltaTime = stopWatch.ElapsedMilliseconds; // using the base stopwatch
            if (deltaTime > 100)
            {
                if (route == null) calculateShortestRoute(car.location, destination);

                Vector2 wayPointWorldLocation = world.squareCoordinateToWorldLocation(this.route.currentWaypoint);

                /*
                // acceleration
                var range = speedRangePercentage * speedLimit;
                var compliance = speedComplianceVariationPercentage * speedLimit;
                var maxSpeed = speedLimit + compliance + range;
                var minSpeed = speedLimit + compliance - range;


                if (car.speed < minSpeed)
                {
                    // increase acceleration
                    car.shifter = ShifterPosition.drive;
                    car.acceleratorPedal += 0.1f;
                    car.breakPedal = 0;
                }
                else if (car.speed > maxSpeed)
                {
                    // increase breaking
                    car.acceleratorPedal = 0;
                    car.breakPedal += 0.1f;
                } else
                {
                    // no change in pedals
                    car.acceleratorPedal = 0;
                    car.breakPedal = 0;
                }
                */

                car.shifter = ShifterPosition.drive;
                car.acceleratorPedal = 1; // this for debug 

                // get desired direction
                Vector2 desiredDirection = getDirection(car, wayPointWorldLocation); // this is the correct vector to my waypoint

                // how much are we off
                float steering = VectorMath.angleBetween(desiredDirection, car.velocity);
                if (double.IsNaN(steering)) steering = 0;

                // redirect the velocity
                car.steeringWheel = steering / -45;
                //if ((steering > -5f) && (steering < 5f)) car.velocity = desiredDirection * car.velocity.Length();
                //else car.steering -= steering / 2;


                // reached the destination
                if (Vector2.Distance(car.location, destination) < 32)
                {
                    world.removeGamePiece(car);
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
                        world.removeGamePiece(car);
                    }
                }
                else if (distance > 0)
                {
                    // keep trying

                }

                stopWatch.Restart(); // reset update timer
            }
        }


        public Vector2 getDirection(clsGamePieceCar car, Vector2 destinationLocation)
        {
            // get direction of way point from cars location
            //Vector2 b = car.location - destination;
            Vector2 b = destinationLocation - car.location;
            Vector2 destinationDirection = new Vector2(b.X, b.Y);
            destinationDirection.Normalize();
            return destinationDirection;
        }

        public void calculateShortestRoute(Vector2 fromLocation, Vector2 toLocation)
        {
            Vector2 carSquareCoordinate = world.worldLocationToSquareCoordinate(fromLocation);
            Vector2 exitSquareCoordinate = world.worldLocationToSquareCoordinate(toLocation);

            this.route = new clsRoute(world.findShortestPath(carSquareCoordinate, exitSquareCoordinate));
        }
    }
}
