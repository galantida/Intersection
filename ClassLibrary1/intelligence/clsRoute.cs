using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace gameLogic
{
    // route is a collection of square coordinates between two squares
    public class clsRoute
    {
        public List<Vector2> waypoints;
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

        public Vector2 previousWaypoint
        {
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
