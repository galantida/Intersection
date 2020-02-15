using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using physicalWorld;
using Microsoft.Xna.Framework;


namespace tileWorld
{
    public enum CardinalDirection { East, Southeast, South, Southhwest, West, Northwest, North, Northeast }
    public enum CollisionType { None, Spherical }

    public class clsWorld 
    {
        public Random random;
        protected Stopwatch _currentTime = new Stopwatch();

        public clsTile[,] tiles;
        public List<intObject> worldObjects;
        public List<intActor> actors;

        public float tileSize { get; set; }

        public clsWorld(float tileSize)
        {
            random = new Random(); // randomize seed the world
            _currentTime.Start(); // start processing clock

            this.tileSize = tileSize;
        }

        /*********************************************************************************
         * Tile Functions
         *********************************************************************************/
        public void addTile(string typeName, string textureName, long tilex, long tiley, bool east, bool west, bool north, bool south)
        {
            tiles[tilex, tiley] = new clsTile(typeName, textureName, east, west, north, south);
        }

        /*********************************************************************************
         * Object Functions
         *********************************************************************************/

        public void remove(intObject gameObject)
        {
            worldObjects.Remove(gameObject);
        }

        public List<intObject> getWorldObjects(string typeName)
        {
            List<intObject> filteredWorldObjects = new List<intObject>();
            foreach (intObject worldObject in this.worldObjects)
            {
                if (worldObject.typeName == typeName) filteredWorldObjects.Add(worldObject);
            }
            return filteredWorldObjects;
        }

        public intObject getRandomWorldObject(string typeName)
        {
            List<intObject> filteredWorldObjects = getWorldObjects(typeName);
            return filteredWorldObjects[this.random.Next(filteredWorldObjects.Count())];
        }

        /*
        public clsTileObject addTileObject(Vector2 location, Vector2 direction, Vector2 velocity, float mass = 1000.0f)
        {
            clsTileObject newTileObject = new clsTileObject(location, direction, velocity, mass);
            tileWorldObjects.Add((intTileObject)newTileObject);
            return newTileObject;
        }

        public void removeTileObject(intTileObject tileObject)
        {
            tileWorldObjects.Remove(tileObject); // remove from
        }
        */

        /**************************************************
            coordinate conversion functions
        **************************************************/
        public Vector2 worldLocationToSquareCoordinate(Vector2 worldCoordinate)
        {
            return new Vector2((int)(worldCoordinate.X / tileSize), (int)(worldCoordinate.Y / tileSize));
        }

        public Vector2 squareCoordinateToWorldLocation(Vector2 squareCoordinate)
        {
            return new Vector2(squareCoordinate.X * tileSize + (tileSize / 2), squareCoordinate.Y * tileSize + (tileSize / 2));
        }

        /**************************************************
            square access shortcut
        **************************************************/
        public clsTile getSquareFromWorldLocation(Vector2 worldLocation)
        {
            return getSquareFromSquareCoordinate(this.worldLocationToSquareCoordinate(worldLocation));
        }

        public clsTile getSquareFromSquareCoordinate(Vector2 squareCoordinate)
        {
            return tiles[(int)squareCoordinate.X, (int)squareCoordinate.Y];
        }

        public bool inSquareCoordinateBounds(Vector2 squareCoordinate)
        {
            bool result = true;
            if ((squareCoordinate.X < 0) || (squareCoordinate.X >= tiles.GetLength(0))) result = false; 
            if ((squareCoordinate.Y < 0) || (squareCoordinate.Y >= tiles.GetLength(1))) result = false; 
            return result;
        }

        public bool inWorldBounds(Vector2 squareCoordinate)
        {
            bool result = true;
            if ((squareCoordinate.X < 0) || (squareCoordinate.X >= tiles.GetLength(0))) result = false;
            if ((squareCoordinate.Y < 0) || (squareCoordinate.Y >= tiles.GetLength(1))) result = false;
            return result;
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
            Vector2 currentWaypoint = previousWaypoints[previousWaypoints.Count()-1];
            clsTile currentSquare = this.getSquareFromSquareCoordinate(currentWaypoint);

            // get all posible directions off of the current square
            foreach (Vector2 currentDirection in currentSquare.directions)
            {
                // get way point for this direction
                Vector2 newWaypoint = new Vector2(currentWaypoint.X, currentWaypoint.Y) + currentDirection;

                // is this a valid new waypoint
                if (this.inSquareCoordinateBounds(newWaypoint)) // is it on the map
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


        /*****************************************
         *              Collision Detections
         *****************************************/

        public bool collision(clsObject firstObject, clsObject secondObject = null)
        {
            // detect collision between two specific objects
            if (secondObject != null)
            {
                Vector2 distance = firstObject.location - secondObject.location;
                if (distance.Length() < 50) return true;
                else return false;
            }
            else
            {
                // detect collision with anyting
                foreach (clsObject worldObject in this.worldObjects)
                {

                    if (worldObject.collisionType != CollisionType.None)
                    {
                        if (worldObject != firstObject)
                        {
                            Vector2 distance = firstObject.location - worldObject.location;
                            if (distance.Length() < 50) return true;
                        }
                    }
                }
                return false;
            }
        }
    }
    
}
