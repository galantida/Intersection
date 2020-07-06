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
    // current waypoint is always list item zero
    // destination waypoint is always the last list item
    public class clsRoute
    {
        private clsWorld world;
        private List<Vector2> _waypoints; // tile coordinates

        public clsObject nextObstruction;
        public int distanceToNextObstruction = 1000;
        public clsObject nextCollision;
        public int distanceToNextCollision = 1000;
        

        private float lastUpdated { get; set; }

        public clsRoute(clsWorld world, List<Vector2> waypoints)
        {
            this.world = world;
            _waypoints = waypoints;
        }

        public clsRoute(clsWorld world, Vector2 start, Vector2 destination)
        {
            this.world = world;
            _waypoints = this.findShortestPath(start, destination);
        }

        public void advanceWaypoint()
        {
            _waypoints.Remove(_waypoints[0]);
        }

        public Vector2? currentWaypoint 
        {
            get
            {
                return this.getWaypoint(0);
            }
        }

        public Vector2? nextWaypoint
        {
            // returns the next waypoint or the last one if we are already on the last one
            get
            {
                return this.getWaypoint(1);
            }
        }

        public Vector2? getWaypoint(int waypointIndex)
        {
            if (waypointIndex < _waypoints.Count) return _waypoints[waypointIndex];
            else return null;
        }

        public int waypoints
        {
            get
            {
                return _waypoints.Count;
            }
        }

        public void update()
        {
            this.distanceToNextObstruction = 1000;
            this.distanceToNextCollision = 1000;


            // calculate next obstruction & future collsisions
            for (int t = 0; t < _waypoints.Count; t++)
            {
                // get the tile in questions
                intTile tile = world.getTileFromTileCoordinate(_waypoints[t]);

                // look for next obstruction
                if (this.distanceToNextObstruction == 1000)
                {
                    foreach (intObject worldObject in tile.worldObjects)
                    {
                        if (worldObject.collisionDetection != CollisionType.None) // make sure collision detection is on
                        {
                            // we have to add check it it has collision with self
                            this.nextObstruction = (clsObject)worldObject;
                            this.distanceToNextObstruction = t;
                            break;
                        }

                    }
                }

                // look for next future collision
                if (this.distanceToNextCollision == 1000)
                {
                    foreach (intActor worldActor in this.world.actors)
                    {
                        if (worldActor.worldObject.collisionDetection != CollisionType.None) // only process where collsisions are on
                        {
                            if (worldActor.yield == false) // are they already yielding to us
                            {
                                if (worldActor.route != null) // make sure its not null
                                {
                                    if (worldActor.route != this) // don't compare to your self.
                                    {
                                        Vector2? waypoint = worldActor.route.getWaypoint(t); // get the same route waypoint for this world actor as we are checking for ours
                                        if ((waypoint != null) && (waypoint.Equals(_waypoints[t])))
                                        {
                                            this.nextCollision = worldActor.worldObject;
                                            this.distanceToNextCollision = t;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        /*****************************************
        *              Path Finding (Tiles)
        *****************************************/
        public List<Vector2> findShortestPath(Vector2 fromWaypoint, Vector2 toWaypoint)
        {
            // create a new path with the start point as the first way point
            List<Vector2> startPath = new List<Vector2>();
            startPath.Add(fromWaypoint);

            // get all paths to destination
            List<List<Vector2>> allPaths = this.getAllPaths(startPath, toWaypoint);

            // get shorts path from that list
            List<Vector2> shortestPath = null;
            foreach (List<Vector2> path in allPaths)
            {
                if ((shortestPath == null) || (path.Count() < shortestPath.Count()))
                {
                    shortestPath = path;
                }
            }
            return shortestPath;
        }


        // get all the valid routes between two points
        // the only rule is that they can not overlap themselves
        public List<List<Vector2>> getAllPaths(List<Vector2> previousWaypoints, Vector2 destinationSquareCoordinate)
        {
            // collection of all possible paths
            List<List<Vector2>> paths = new List<List<Vector2>>();


            // set the current square to the starting location
            Vector2 currentWaypoint = previousWaypoints[previousWaypoints.Count() - 1];
            intTile currentSquare = (intTile)this.world.getTileFromTileCoordinate(currentWaypoint);

            // get all posible directions off of the current square
            foreach (Vector2 currentDirection in currentSquare.directions)
            {
                // get way point for this direction
                Vector2 newWaypoint = new Vector2(currentWaypoint.X, currentWaypoint.Y) + currentDirection;

                // is this a valid new waypoint
                if (this.world.inTileCoordinateBounds(newWaypoint)) // is it on the map
                {
                    if (!containsWaypoint(previousWaypoints, newWaypoint)) // is it not an infinite loop
                    {
                        // fully copy existing path to a new path and add the new waypoint direction
                        List<Vector2> newPath = copyWaypoints(previousWaypoints);
                        newPath.Add(newWaypoint);

                        // does this new path reach our destination?
                        if ((newWaypoint.X == destinationSquareCoordinate.X) && (newWaypoint.Y == destinationSquareCoordinate.Y))
                        {
                            // add the now completed path to the collection of paths
                            paths.Add(newPath);
                        }
                        else
                        {
                            // do further exploration of this path and all its possibilities
                            List<List<Vector2>> newPaths = getAllPaths(newPath, destinationSquareCoordinate);
                            foreach (List<Vector2> newSubPath in newPaths)
                            {
                                paths.Add(newSubPath);
                            }
                        }
                    }
                }
            }

            // all processing is done
            return paths;
        }

        public static List<Vector2> copyWaypoints(List<Vector2> waypoints)
        {
            List<Vector2> newWaypoints = new List<Vector2>();
            foreach (Vector2 waypoint in waypoints)
            {
                newWaypoints.Add(new Vector2(waypoint.X, waypoint.Y));
            }
            return newWaypoints;
        }

        public static void appendWaypoints(List<Vector2> originalWaypoints, List<Vector2> waypointsToAppend)
        {
            foreach (Vector2 waypoint in waypointsToAppend)
            {
                originalWaypoints.Add(new Vector2(waypoint.X, waypoint.Y));
            }
        }

        public static bool containsWaypoint(List<Vector2> waypoints, Vector2 waypoint)
        {
            foreach (Vector2 existingWaypoint in waypoints)
            {
                if ((existingWaypoint.X == waypoint.X) && (existingWaypoint.Y == waypoint.Y)) return true;
            }
            return false;
        }

        public int distanceToNextTurn
        {
            get
            {
                // this can be used for breaking or turn signaling
                int distance = 0;
                Vector2[] testPoints = new Vector2[3];

                for (int t = 0; t < _waypoints.Count; t++)
                {
                    for (int l = 0; l <= 2; l++)
                    {
                        int index = t + l;
                        if (index >= _waypoints.Count) index = _waypoints.Count - 1;
                        if (index < 0) index = 0;
                        testPoints[l] = _waypoints[index];
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

        

        
    }
        
}
