using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using tileWorld;
using physicalWorld;

namespace tileWorld
{
    // route is a collection of square coordinates between two squares
    public class clsRoute
    {
        private clsWorld world;
        public List<Vector2> waypoints; // tile coordinates
        private int _currentWaypointIndex = 1;

        public int currentWaypointIndex
        {
            get
            {
                return _currentWaypointIndex; 
            }
        }

        public clsRoute(clsWorld world, List<Vector2> waypoints)
        {
            this.world = world;
            this.waypoints = waypoints;
        }

        public void advanceWaypoint()
        {
            _currentWaypointIndex++;
            if (currentWaypointIndex >= waypoints.Count()) _currentWaypointIndex = waypoints.Count() - 1;
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
                int nextIndex = _currentWaypointIndex + 1;
                if (nextIndex >= waypoints.Count) nextIndex = waypoints.Count() - 1;
                return waypoints[nextIndex];
            }
        }

        public Vector2 previousWaypoint
        {
            // returns the previous waypoint or the first one if we are still at the start
            get
            {
                int previousIndex = _currentWaypointIndex - 1;
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

        public int distanceToNextObstruction
        {
            get
            {
                // this can be used for breaking or turn signaling
                for (int t = _currentWaypointIndex; t < this.length; t++)
                {
                    intTile tile = world.getTileFromTileCoordinate(this.waypoints[t]);
                    foreach (intObject worldObject in tile.worldObjects)
                    {
                        if (worldObject.collisionDetection != CollisionType.None)
                        {
                            return t - this.currentWaypointIndex;
                        }
                    }
                    
                }
                return 1000;
            }
        }

        
    }
        
}
