using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace tileWorld
{
    // route is a collection of square coordinates between two squares
    public class clsRoute
    {
        private List<Vector2> waypoints;
        private int currentWaypointIndex = 1;

        public clsRoute(List<Vector2> waypoints)
        {
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

        public void advanceWaypoint()
        {
            currentWaypointIndex++;
            if (currentWaypointIndex >= waypoints.Count()) currentWaypointIndex = waypoints.Count() - 1;
        }
    }
        
}
