using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using tileWorld;
using physicalWorld;

namespace gameLogic
{
    // route is a collection of square coordinates between two squares
    public class clsRoute
    {
        private clsRoadWorld world;
        private List<Vector2> waypoints;
        private int currentWaypointIndex = 1;

        public clsRoute(clsRoadWorld world, List<Vector2> waypoints)
        {
            this.world = world;
            this.waypoints = waypoints;
        }

        public Vector2 currentWaypoint
        {
            get
            {
                return waypoints[currentWaypointIndex];
            }
        }

        public Vector2 nextWaypoint
        {
            // returns the next waypoint or the last one if we are already on the last one
            get
            {
                int nextIndex = currentWaypointIndex + 1;
                if (nextIndex >= waypoints.Count) nextIndex = waypoints.Count() - 1;
                return waypoints[nextIndex];
            }
        }

        public Vector2 previousWaypoint
        {
            // returns the previous waypoint or the first one if we are still at the start
            get
            {
                int previousIndex = currentWaypointIndex - 1;
                if (previousIndex < 0) previousIndex = 0;
                return waypoints[previousIndex];
            }
        }

        public int length
        {
            get { return waypoints.Count; }
        }

        public int distanceToNextTurn
        {
            get
            {
                // this can be used for breaking or turn signaling
                int distance = 0;
                Vector2[] testPoints = new Vector2[3];

                for (int t = this.currentWaypointIndex-2; t < this.length; t++)
                {
                    for (int l = 0; l <= 2; l++)
                    {
                        int index = t + l;
                        if (index >= this.length) index = this.length-1;
                        if (index < 0) index = 0;
                        testPoints[l] = waypoints[index];
                    }

                    if (clsGameMath.areaOfTriangle(testPoints[0], testPoints[1], testPoints[2]) != 0)
                    {
                        return distance; // early exit on turn
                    }
                    distance++;
                }
                return distance; // no turns. Maybe should return -1?
            }
        }

        public int distanceToNextCollision
        {
            get
            {
                for (int t = this.currentWaypointIndex; t < this.length; t++)
                {
                    Vector2 wayPointWorldLocation = world.squareCoordinateToWorldLocation(waypoints[t]);
                    intObject closestObject = world.closestColidableObject(wayPointWorldLocation);
                    Vector2 distance = closestObject.location - wayPointWorldLocation;
                    if ((distance.Length() > 0) && (distance.Length() < 64))
                    {
                        return t;
                    }
                }
                return 100000; // no turns. Maybe should return -1?
            }
        }

        public void advanceWaypoint()
        {
            currentWaypointIndex++;
            if (currentWaypointIndex >= waypoints.Count()) currentWaypointIndex = waypoints.Count() - 1;
        }
    }
        
}
