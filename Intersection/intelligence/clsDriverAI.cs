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

        // driver speed differences
        float speedRangePercentage; // top and bottom speeds, when to give more gas or break. (e.g. 5% under target or 5 over target)
        float speedComplianceVariationPercentage; // faster or slower than normal. (e.g. 10% slower than normal)

        public clsDriverAI(clsRoadWorld world, clsCar car, Vector2 destination) : base(world, car)
        {
            // inputs
            this.car = car;
            this.destination = destination;
            this.yield = false;

            // calculate route
            calculateShortestRoute(car.location, destination);

            // driver differences
            //speedRangePercentage = (car.world.random.Next(10) - 5) / 100.0f;  
            //speedComplianceVariationPercentage = (car.world.random.Next(10) - 5) / 100.0f;

            // start timer
            this.lastUpdated = world.currentTime;
        }

        public clsRoadWorld roadWorld
        {
            get
            {
                // promise that any world will be a road world.
                return (clsRoadWorld)this.world;
            }
        }


        public void update(float currentTime)
        {
            float deltaTime = world.currentTime - lastUpdated; // time since last updated
            if (deltaTime > 10)
            {
                lastUpdated = world.currentTime; // reset last updated

                // are we going anywhere?
                Vector2? currentWaypoint = this.route.currentWaypoint;
                if (currentWaypoint != null)
                {
                    // route speed limit
                    float speedLimit = 0.0f;
                    speedLimit = this.roadWorld.getRoadWorldTileFromTileCoordinate((Vector2)currentWaypoint).speedLimit;

                    // drivers desired speed due to situation
                    float driversSpeedLimit = speedLimit;

                    // yielding to another object
                    if (this.yield) driversSpeedLimit = 0.0f;

                    // scale driver caution based on car speed
                    float caution = 10.0f + (this.car.mph / 50); // @10mph is 2 and @50 is 10

                    // slow down for turns
                    if (this.route.distanceToNextTurn < 2) driversSpeedLimit = speedLimit * 0.5f; // half speed on turns. Should be a driver setting

                    // drivers desired speed due to obstructions and predicted collisions. (Number represents tiles out at 50 mph)
                    if ((this.route.distanceToNextCollision <= (caution *.1)) || (this.route.distanceToNextObstruction <= (caution * 0)))
                    {
                        // pending doom
                        driversSpeedLimit = 0.0f; // stop to avoid collisions
                    }
                    else if ((this.route.distanceToNextCollision <= (caution * .2)) || (this.route.distanceToNextObstruction <= (caution * .1)))
                    {
                        // reasonable reaction
                        driversSpeedLimit = speedLimit * 0.50f;
                    }
                    else if ((this.route.distanceToNextCollision <= (caution * .3)) || (this.route.distanceToNextObstruction <= (caution * .2)))
                    {
                        // caution
                        driversSpeedLimit = speedLimit * 0.75f;
                    }
                    else
                    {
                        driversSpeedLimit = speedLimit;
                    }
                    
                    


                    // ********************************************
                    //          speed Control
                    if (driversSpeedLimit == 0.0f)
                    {
                        // hard braking
                        car.acceleratorPedal = 0.0f;
                        car.breakPedal = 1.0f;
                    }
                    else if (car.mph < driversSpeedLimit * 0.7f)
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
                    else if (car.mph < driversSpeedLimit * 1.1f)
                    {
                        // slight decrease in acceleration
                        car.acceleratorPedal -= 0.01f;
                        car.breakPedal = 0;
                    }
                    else if (car.mph < driversSpeedLimit * 1.2f)
                    {
                        // heavy decrease in acceleration
                        car.acceleratorPedal -= 0.1f;
                        car.breakPedal = 0;
                    }
                    else if (car.mph < driversSpeedLimit * 1.3f)
                    {
                        // slight breaking
                        car.acceleratorPedal = 0.0f;
                        car.breakPedal = 0.01f;
                    }
                    else if (car.mph < driversSpeedLimit * 1.4f)
                    {
                        // heavy breaking
                        car.acceleratorPedal = 0.0f;
                        car.breakPedal = 0.1f;
                    }
                    else 
                    {
                        // hard braking
                        car.acceleratorPedal = 0.0f;
                        car.breakPedal = 1.0f;
                    }

                    // ********************************************
                    //          signaling control




                    // ********************************************
                    //          steering & Signaling control
                    Vector2 wayPointWorldLocation = Vector2.Normalize(car.velocity); // this is the current vector
                    float nextTurn = 0;
                    if (this.route != null)
                    {
                        wayPointWorldLocation = world.tileCoordinateToWorldLocation((Vector2)currentWaypoint);
                        if (this.route.nextWaypoint != null)
                        {
                            Vector2? signalingWayPointWorldLocation = world.tileCoordinateToWorldLocation((Vector2)this.route.nextWaypoint);
                            if (signalingWayPointWorldLocation != null)
                            {
                                // how much are we off our next turn
                                nextTurn = VectorMath.angleBetween((Vector2)signalingWayPointWorldLocation, car.velocity);
                                if (double.IsNaN(nextTurn)) nextTurn = 0;
                                nextTurn = nextTurn / -45;

                                
                            }
                        }
                        
                    }

                    // signal
                    if (nextTurn > 1) car.turnSignal = -1;
                    else if (nextTurn < -1) car.turnSignal = 1;
                    else car.turnSignal = 0;

                    Vector2 desiredDirection = getDirection(car, wayPointWorldLocation); // this is the correct vector to the next waypoint

                    // how much are we off
                    float steering = VectorMath.angleBetween(desiredDirection, car.velocity);
                    if (double.IsNaN(steering)) steering = 0;

                    // redirect the velocity
                    car.steeringWheel = steering / -45;
                    //if ((steering > -5f) && (steering < 5f)) car.velocity = desiredDirection * car.velocity.Length();
                    //else car.steering -= steering / 2;


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

                base.update();
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

        
    }
}
