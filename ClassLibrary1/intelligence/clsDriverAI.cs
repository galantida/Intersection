using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using physicalWorld;
using tileWorld;
using Microsoft.Xna.Framework;


namespace gameLogic
{
    public class clsDriverAI : clsActor, intActor
    {
        // AI instructions
        public clsCar car { get; set; }
        public Vector2 destination { get; set; }
        public clsRoute route { get; set; }

        public clsRoadWorld world;

        // driver speed differences
        float speedRangePercentage; // top and bottom speeds, when to give more gas or break. (e.g. 5% under target or 5 over target)
        float speedComplianceVariationPercentage; // faster or slower than normal. (e.g. 10% slower than normal)

        public clsDriverAI(clsRoadWorld world, clsCar car, Vector2 destination)
        {
            // inputs
            this.car = car;
            this.destination = destination;
            this.route = null;
            this.world = world;

            // driver differences
            //speedRangePercentage = (car.world.random.Next(10) - 5) / 100.0f;  
            //speedComplianceVariationPercentage = (car.world.random.Next(10) - 5) / 100.0f;

            // start timer
            this.lastUpdated = world.currentTime;
        }

        public void update(float currentTime)
        {
            float deltaTime = world.currentTime - lastUpdated; // time since last updated
            if (deltaTime > 10)
            {
                lastUpdated = world.currentTime; // reset last updated

                // calculate route every time?
                if (this.route == null) calculateShortestRoute(car.location, destination);

                // route speed limit
                float speedLimit = 0.0f;
                if (this.route != null) speedLimit = this.world.getRoadWorldTileFromTileCoordinate(this.route.currentWaypoint).speedLimit;

                // drivers desired speed due to route conditions
                float driversSpeedLimit = speedLimit;
                if (this.route.distanceToNextTurn < 2) driversSpeedLimit = speedLimit * 0.5f; // half speed on turns. Shoudl be a driver setting

                // drivers desired speed due to obstructions
                if (this.route.distanceToNextObstruction < 4) driversSpeedLimit = speedLimit * 0.75f;
                if (this.route.distanceToNextObstruction < 3) driversSpeedLimit = speedLimit * 0.50f;
                if (this.route.distanceToNextObstruction < 2) 
                {
                    driversSpeedLimit = 0.0f; // stop to avoid collisions
                }


                // ********************************************
                //          speed Control
                if (car.mph < driversSpeedLimit * 0.7f)
                {
                    // heavy increase in acceleration
                    car.shifter = ShifterPosition.drive;
                    car.acceleratorPedal += 0.1f;
                    car.breakPedal = 0;
                }
                else if (car.mph < driversSpeedLimit * 0.8f)
                {
                    // slight increase in acceleration
                    car.acceleratorPedal += 0.01f;
                    car.breakPedal = 0;
                }
                else if (car.mph > driversSpeedLimit * 1.5f)
                {
                    // hard braking
                    car.acceleratorPedal = 0.0f;
                    car.breakPedal = 1.0f;
                }
                else if (car.mph > driversSpeedLimit * 1.4f)
                {
                    // heavy breaking
                    car.acceleratorPedal = 0.0f;
                    car.breakPedal = 0.1f;
                }
                else if (car.mph > driversSpeedLimit * 1.3f)
                {
                    // slight breaking
                    car.acceleratorPedal = 0.0f;
                    car.breakPedal = 0.01f;
                }
                else if (car.mph > driversSpeedLimit * 1.2f)
                {
                    // decrease acceleration
                    car.acceleratorPedal -= 0.1f;
                    car.breakPedal = 0;
                }

                // ********************************************
                //          steering control
                Vector2 wayPointWorldLocation = Vector2.Normalize(car.velocity);
                if (this.route != null) wayPointWorldLocation = world.tileCoordinateToWorldLocation(this.route.currentWaypoint);
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

                // ********************************************
                //      way point control

                // reached the destination
                if (Vector2.Distance(car.location, destination) < 64)
                {
                    world.remove(car);
                    world.remove(this);
                }

                // distance to way point
                float distance = Vector2.Distance(car.location, wayPointWorldLocation);
                if (distance < 32) // was 75
                {
                    // comming up to way point target the next one
                    this.route.advanceWaypoint();
                }
            }
        }


        public Vector2 getDirection(clsCar car, Vector2 destinationLocation)
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
            Vector2 carSquareCoordinate = world.worldLocationToTileCoordinate(fromLocation);
            Vector2 exitSquareCoordinate = world.worldLocationToTileCoordinate(toLocation);

            this.route = new clsRoute(world, world.findShortestPath(carSquareCoordinate, exitSquareCoordinate));
        }
    }
}
