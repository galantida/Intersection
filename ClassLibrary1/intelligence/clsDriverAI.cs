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
        // AI instructions
        private clsCarObject car { get; set; }
        public Vector2 destination { get; set; }
        public clsRoute route { get; set; }
        public float speedLimit { get; set; }

        // driver spped differences
        float speedRangePercentage; // top and bottom speeds, when to give more gas or break. (e.g. 5% under target or 5 over target)
        float speedComplianceVariationPercentage; // faster or slower than normal. (e.g. 10% slower than normal)

        public float lastUpdated { get; set; }


        public clsDriverAI(clsCarObject car, Vector2 destination)
        {
            // inputs
            this.car = car;
            this.destination = destination;
            this.route = null;
            speedLimit = 25f;

            // driver differences
            //speedRangePercentage = (car.world.random.Next(10) - 5) / 100.0f;  
            //speedComplianceVariationPercentage = (car.world.random.Next(10) - 5) / 100.0f;

            // start timer
            this.lastUpdated = car.world.currentTime;
        }

        public void update()
        {
            float deltaTime = car.world.currentTime - lastUpdated; // time since last updated
            if (deltaTime > 10)
            {
                lastUpdated = car.world.currentTime; // reset last updated

                if (route == null) calculateShortestRoute(car.location, destination);

                Vector2 wayPointWorldLocation = car.world.squareCoordinateToWorldLocation(this.route.currentWaypoint);

                // acceleration
                //var range = speedRangePercentage * speedLimit;
                //var compliance = speedComplianceVariationPercentage * speedLimit;
                //var maxSpeed = speedLimit + compliance + range;
                //var minSpeed = speedLimit + compliance - range;


                if (car.mph < speedLimit * 0.9f)
                {
                    // increase acceleration
                    car.shifter = ShifterPosition.drive;
                    car.acceleratorPedal += 0.1f;
                    car.breakPedal = 0;
                }
                else if (car.mph > speedLimit * 1.1f)
                {
                    // decrease acceleration
                    car.acceleratorPedal -= 0.1f;
                    car.breakPedal += 0;
                } else if (car.mph > speedLimit * 1.5f)
                {
                    // increase breaking
                    car.acceleratorPedal = 0.0f;
                    car.breakPedal += 0.1f;
                }
                else
                {
                    // no change in pedals
                    //car.acceleratorPedal = 0;
                    //car.breakPedal = 0;
                }
                

                // get desired direction
                Vector2 desiredDirection = getDirection(car, wayPointWorldLocation); // this is the correct vector to my waypoint

                // how much are we off
                float steering = VectorMath.angleBetween(desiredDirection, car.velocity);
                if (double.IsNaN(steering)) steering = 0;

                // redirect the velocity
                car.steeringWheel = steering / -45;
                //if ((steering > -5f) && (steering < 5f)) car.velocity = desiredDirection * car.velocity.Length();
                //else car.steering -= steering / 2;

                // last minute turn signals for fun
                if (car.steeringWheel < -0.5f) car.turnSignal = -1;
                else if (car.steeringWheel > 0.5f) car.turnSignal = 1;
                else car.turnSignal = 0;


                // reached the destination
                if (Vector2.Distance(car.location, destination) < 32)
                {
                    car.world.removeGamePiece(car);
                }
                


                // distance to way point
                float distance = Vector2.Distance(car.location, wayPointWorldLocation);

                if (distance < 75)
                {
                    try
                    {
                        // arrived at way point, advance to next one
                        this.route.advanceWaypoint();
                    }
                    catch(Exception ex)
                    {
                        // must be at exit remove car
                        car.world.removeGamePiece(car);
                    }
                }
                else if (distance > 0)
                {
                    // keep trying

                }

            }
        }


        public Vector2 getDirection(clsCarObject car, Vector2 destinationLocation)
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
            Vector2 carSquareCoordinate = car.world.worldLocationToSquareCoordinate(fromLocation);
            Vector2 exitSquareCoordinate = car.world.worldLocationToSquareCoordinate(toLocation);

            this.route = new clsRoute(car.world.findShortestPath(carSquareCoordinate, exitSquareCoordinate));
        }
    }
}
