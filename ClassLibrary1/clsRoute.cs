using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace gameLogic
{
    public class clsRoute
    {
        public List<Vector2> waypoints;
        public clsWorld world;

        public clsRoute(clsWorld world)
        {
            this.world = world;
        }

        public List<Vector2> findShortestRoute(Vector2 fromSquareCoordinate, Vector2 toSquareCoordinate)
        {
            List<Vector2> startRoute = new List<Vector2>();
            startRoute.Add(fromSquareCoordinate);

            List<List<Vector2>> allRoutes = getAllRoutes(startRoute, toSquareCoordinate);

            List<Vector2> shortestRoute = null;
            foreach (List<Vector2> route in allRoutes)
            {
                if ((shortestRoute == null) || (route.Count() < shortestRoute.Count())) {
                    shortestRoute = route;
                }
            }
            return shortestRoute;
        }

        public List<List<Vector2>> getAllRoutes(List<Vector2> curRoute, Vector2 destinationSquareCoordinate)
        {
            List<List<Vector2>> routes = new List<List<Vector2>>();

            // get the last waypoint in the curRoute
            Vector2 currentSquareCoordinate = curRoute[curRoute.Count() - 1];

            // get the square at this waypoint to access its directions
            clsSquare curSquare = world.squares[(int)currentSquareCoordinate.X, (int)currentSquareCoordinate.Y];

            // get next direction route options
            foreach (Vector2 currentDirection in curSquare.directions)
            {
                // get new waypoint for this direction
                Vector2 newWaypoint = new Vector2(currentSquareCoordinate.X, currentSquareCoordinate.Y) + currentDirection;

                // is this a valid new waypoint
                if (!world.inWorldBounds(newWaypoint)) // is it on the map
                {
                    if (containsWaypoint(curRoute, newWaypoint)) // is it not an infinite loop
                    {
                        // fully copy existing route and add this new waypint
                        List<Vector2> newRouteOption = new List<Vector2>();
                        newRouteOption = copyRoute(curRoute);
                        newRouteOption.Add(newWaypoint);

                        // does this direction reach our destination?
                        if ((newWaypoint.X == destinationSquareCoordinate.X) && (newWaypoint.Y == destinationSquareCoordinate.Y))
                        {
                            // add the now completed route to the results
                            routes.Add(newRouteOption);
                        } 
                        else
                        {
                            // spin off further exploration of this new waypoints directions and add them to the resutls
                            List<List<Vector2>> newRoutes = getAllRoutes(newRouteOption, destinationSquareCoordinate);
                            foreach (List<Vector2> route in newRoutes)
                            {
                                routes.Add(route);
                            }
                        }
                    }
                }
            }

            // all processing is done
            return routes;
        }

        public static List<Vector2> copyRoute(List<Vector2> route)
        {
            List<Vector2> newRoute = new List<Vector2>();
            foreach (Vector2 waypoint in route)
            {
                newRoute.Add(new Vector2(waypoint.X, waypoint.Y));
            }
            return newRoute;
        }

        public static void appendRoute(List<Vector2> originalRoute, List<Vector2> routeToAppend) 
        {
            foreach (Vector2 waypoint in routeToAppend)
            {
                originalRoute.Add(new Vector2(waypoint.X, waypoint.Y));
            }
            //return originalRoute;
        }

        public static bool containsWaypoint(List<Vector2> waypoints, Vector2 waypoint)
        {
            foreach (Vector2 existingWaypoint in waypoints)
            {
                if ((existingWaypoint.X == waypoint.X) && (existingWaypoint.Y == waypoint.Y)) return true;
            }
            return false;
        }
    }
}
