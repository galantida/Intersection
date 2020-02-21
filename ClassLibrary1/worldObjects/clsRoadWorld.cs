using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using tileWorld;
using Microsoft.Xna.Framework;


namespace gameLogic
{
    public enum CardinalDirection { East, Southeast, South, Southhwest, West, Northwest, North, Northeast }

    public class clsRoadWorld : clsWorld
    {
        public clsRoadWorld(long tilesWide, float tileSize): base(tileSize)
        {
            loadTiles(tilesWide);
            loadObjects();
            loadActors();
        }

        public float currentTime
        {
            get
            {
                return _currentTime.ElapsedMilliseconds;
            }
        }

        /*****************************************
                Creating the world
         *****************************************/
        public void loadTiles(long tilesWide)
        {
            tiles = new intTile[tilesWide, tilesWide];
            long x, y;

            // grass
            for (y = 0; y < tilesWide; y++)
            {
                for (x = 0; x < tilesWide; x++)
                {
                    this.addGrass(x, y);
                    //tiles[x, y] = new clsTile("grass", false, false, false, false);
                }
            }

            // east west road
            y = tilesWide / 2;
            for (x = 0; x < tilesWide; x++)
            {
                addRoad(x, y, CardinalDirection.East);
                addRoad(x, y - 1, CardinalDirection.West);
            }

            // north south road
            x = (tilesWide / 2);
            for (y = 0; y < tilesWide; y++)
            {
                addRoad(x, y, CardinalDirection.North);
                addRoad(x-1, y, CardinalDirection.South);
            }

            // intersection
            long center = tilesWide / 2;
            addIntersection(center - 1, center, true, false, false, true);
            addIntersection(center, center-1, false, true, true, false);
            addIntersection(center - 1, center - 1, false, true, false, true);
            addIntersection(center, center, true, false, true, false);
        }

        public void loadObjects()
        {
            worldObjects = new List<intObject>();

            // create entry points
            createEntry(new Vector2(6, 0), new Vector2(0, 1), "car", 20000);
            createEntry(new Vector2(7, 13), new Vector2(0, -1), "car", 20000);
            createEntry(new Vector2(13, 6), new Vector2(-1, 0), "car", 20000);
            createEntry(new Vector2(0, 7), new Vector2(1, 0), "car", 20000);

            // create exit points
            createExit(new Vector2(7, 0));
            createExit(new Vector2(6, 13));
            createExit(new Vector2(13, 7));
            createExit(new Vector2(0, 6));
        }

        public void loadActors()
        {
            actors = new List<intActor>();

            // spawn a human car
            actors.Add(spawnCarHuman(new Vector2(256, 256), new Vector2(1, 0), new Vector2(0, 0)));
        }

        /*****************************************
                Running the world
         *****************************************/
        public new void update()
        {
            // process each actor
            for (int t = 0; t < actors.Count; t++)
            {
                actors[t].update(currentTime);
            }

            // process each object
            for (int t = 0; t < worldObjects.Count; t++)
            {
                // update all objects
                worldObjects[t].update(currentTime);
            }

            // tiles are activated and read they are not processed

            base.update();
        }

        /*****************************************
                Instance Objects in the world
         *****************************************/

        // tiles[(tilesWide / 2) - 1, (tilesWide / 2)] = new clsTile("intersection", true, false, false, true); // southwest

        public void addGrass(long tilex, long tiley)
        {
            clsGrass grass = new clsGrass();
            base.addTile(grass, tilex, tiley);
        }

        public void addRoad(long tilex, long tiley, CardinalDirection direction)
        {
            switch (direction)
            {
                case CardinalDirection.East:
                    {
                        clsLane road = new clsLane(CardinalDirection.East);
                        base.addTile(road, tilex, tiley);
                        break;
                    }
                case CardinalDirection.West:
                    {
                        clsLane road = new clsLane(CardinalDirection.West);
                        base.addTile(road, tilex, tiley);
                        break;
                    }
                case CardinalDirection.North:
                    {
                        clsLane road = new clsLane(CardinalDirection.North);
                        base.addTile(road, tilex, tiley);
                        break;
                    }
                case CardinalDirection.South:
                    {
                        clsLane road = new clsLane(CardinalDirection.South);
                        base.addTile(road, tilex, tiley);
                        break;
                    }
            }
        }

        public void addIntersection(long tilex, long tiley, bool east, bool west, bool north, bool south)
        {
            clsIntersection intersection = new clsIntersection(east, west, north, south);
            base.addTile(intersection, tilex, tiley);
        }

        /*****************************************
                Instance Objects in the world
         *****************************************/
        public clsEntry createEntry(Vector2 squareCoordinate, Vector2 direction, string typeName, int maxSpawnTime)
        {
            clsEntry entry = new clsEntry("entry", tileCoordinateToWorldLocation(squareCoordinate), direction, this, typeName, maxSpawnTime);
            worldObjects.Add((intObject)entry);
            return entry;
        }

        public clsExit createExit(Vector2 squareCoordinate)
        {
            clsExit exit = new clsExit("exit", tileCoordinateToWorldLocation(squareCoordinate), new Vector2(0, 1));
            worldObjects.Add((intObject)exit);
            return exit;
        }

        public clsCar createCar(Vector2 worldLocation, Vector2 direction, Vector2 velocity)
        {
            clsCar car = new clsCar("car", worldLocation, direction, velocity);
            worldObjects.Add((intObject)car);
            return car;
        }

        /*****************************************
                Instance Game Intelegence hooks
         *****************************************/
        public clsDriverHuman createDriverHuman(clsCar car)
        {
            clsDriverHuman human = new clsDriverHuman(car); // assign human to it
            actors.Add((intActor)human);
            return human;
        }

        public clsDriverAI createDriverAI(clsCar car, Vector2 exitLocation)
        {
            clsDriverAI ai = new clsDriverAI(this, car, exitLocation);
            actors.Add((intActor)ai);
            return ai;
        }


        /*****************************************
                Instance Objects in the world
         *****************************************/
        public clsDriverHuman spawnCarHuman(Vector2 worldLocation, Vector2 direction, Vector2 velocity)
        {
            clsCar car = createCar(worldLocation, direction, velocity); // spanw car
            clsDriverHuman human = createDriverHuman(car); // create human for the car
            return human;
        }

        public clsDriverAI spawnCarAI(Vector2 worldLocation, Vector2 direction, Vector2 velocity, Vector2 destination)
        {
            clsCar car = createCar(worldLocation, direction, velocity); // spanw car
            clsDriverAI AI = createDriverAI(car, destination); // create AI for the car
            return AI;
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
            intRoad currentSquare = (intRoad)this.getTileFromTileCoordinate(currentWaypoint);

            // get all posible directions off of the current square
            foreach (Vector2 currentDirection in currentSquare.directions)
            {
                // get way point for this direction
                Vector2 newWaypoint = new Vector2(currentWaypoint.X, currentWaypoint.Y) + currentDirection;

                // is this a valid new waypoint
                if (this.inTileCoordinateBounds(newWaypoint)) // is it on the map
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
    }
    
}
