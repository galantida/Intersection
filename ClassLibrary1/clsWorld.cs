using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace gameLogic
{
    public class clsWorld
    {
        public List<intGamePiece> gamePieces;
        public clsSquare[,] squares;

        public Random rnd;

        public clsInput input; // human input

        public float squareSize;


        public clsWorld(long squaresWide, float squareSize, clsInput input)
        {
            this.squareSize = squareSize;

            // randomize
            rnd = new Random();

            this.input = input;

            // create the map
            loadSquares(squaresWide);
            loadGamePieces();
        }

        public void update()
        {
            // process each game piece
            for (int t = 0; t < gamePieces.Count; t++)
            {
                gamePieces[t].update(this);
            }
        }

        /**************************************************
            coordinate conversion functions
        **************************************************/
        public Vector2 worldLocationToSquareCoordinate(Vector2 worldCoordinate)
        {
            return new Vector2((int)(worldCoordinate.X / squareSize), (int)(worldCoordinate.Y / squareSize));
        }

        public Vector2 squareCoordinateToWorldLocation(Vector2 squareCoordinate)
        {
            return new Vector2(squareCoordinate.X * squareSize + (squareSize / 2), squareCoordinate.Y * squareSize + (squareSize / 2));
        }

        /**************************************************
            square access shortcut
        **************************************************/
        public clsSquare getSquareFromWorldLocation(Vector2 worldLocation)
        {
            return getSquareFromSquareCoordinate(this.worldLocationToSquareCoordinate(worldLocation));
        }

        public clsSquare getSquareFromSquareCoordinate(Vector2 squareCoordinate)
        {
            return squares[(int)squareCoordinate.X, (int)squareCoordinate.Y];
        }

        /**************************************************
            initialization
        **************************************************/
        public void loadSquares(long squaresWide)
        {
            squares = new clsSquare[squaresWide, squaresWide];
            long x, y;

            // grass
            for (y = 0; y < squaresWide; y++)
            {
                for (x = 0; x < squaresWide; x++)
                {
                    squares[x, y] = new clsSquare(false, false, false, false);
                }
            }

            // east west road
            y = squaresWide / 2;
            for (x = 0; x < squaresWide; x++)
            {
                squares[x, y - 1] = new clsSquare(false, true, false, false);
                squares[x, y] = new clsSquare(true, false, false, false);
            }

            // north south road
            x = (squaresWide / 2);
            for (y = 0; y < squaresWide; y++)
            {
                squares[x, y] = new clsSquare(false, false, true, false);
                squares[x - 1, y] = new clsSquare(false, false, false, true);
            }

            // intersection
            squares[(squaresWide / 2) - 1, (squaresWide / 2)] = new clsSquare(true, false, false, true); // southwest
            squares[(squaresWide / 2), (squaresWide / 2) - 1] = new clsSquare(false, true, true, false); // northeast
            squares[(squaresWide / 2) - 1, (squaresWide / 2) - 1] = new clsSquare(false, true, false, true); // northwest
            squares[(squaresWide / 2), (squaresWide / 2)] = new clsSquare(true, false, true, false); // southeast
        }

        public void loadGamePieces()
        {
            // create container for game pieces
            gamePieces = new List<intGamePiece>();

            // create entry points
            createEntry(new Vector2(6, 0), new Vector2(0, 1), GamePieceType.car, 10000);
            createEntry(new Vector2(7, 13), new Vector2(0, -1), GamePieceType.car, 10000);
            createEntry(new Vector2(13, 6), new Vector2(-1, 0), GamePieceType.car, 10000);
            createEntry(new Vector2(0, 7), new Vector2(1, 0), GamePieceType.car, 10000);

            // create exit points
            createExit(new Vector2(7, 0));
            createExit(new Vector2(6, 13));
            createExit(new Vector2(13, 7));
            createExit(new Vector2(0, 6));

            // create human drivable car
            clsDriverHuman human = new clsDriverHuman(input);
            clsGamePieceCar car = createCar(human, new Vector2(4, 4), new Vector2(-1, 0), new Vector2(0, 0));
        }

        public void removeGamePiece(intGamePiece gamePiece)
        {
            gamePieces.Remove(gamePiece);
        }

        public bool inSquareCoordinateBounds(Vector2 squareCoordinate)
        {
            bool result = true;
            if ((squareCoordinate.X < 0) || (squareCoordinate.X >= squares.GetLength(0))) result = false; 
            if ((squareCoordinate.Y < 0) || (squareCoordinate.Y >= squares.GetLength(1))) result = false; 
            return result;
        }

        public bool inWorldBounds(Vector2 squareCoordinate)
        {
            bool result = true;
            if ((squareCoordinate.X < 0) || (squareCoordinate.X >= squares.GetLength(0))) result = false;
            if ((squareCoordinate.Y < 0) || (squareCoordinate.Y >= squares.GetLength(1))) result = false;
            return result;
        }


        /*****************************************
         *              Path Finding
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
            clsSquare currentSquare = this.getSquareFromSquareCoordinate(currentWaypoint);

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
                Instance Objects in the world
         *****************************************/
        public clsGamePieceEntry createEntry(Vector2 squareCoordinate, Vector2 direction, GamePieceType gamePieceType, int maxSpawnTime)
        {
            clsGamePieceEntry entry = new clsGamePieceEntry(this, squareCoordinateToWorldLocation(squareCoordinate), direction, gamePieceType, maxSpawnTime);
            gamePieces.Add(entry);
            return entry;
        }

        public clsGamePieceExit createExit(Vector2 squareCoordinate)
        {
            clsGamePieceExit exit = new clsGamePieceExit(this, squareCoordinateToWorldLocation(squareCoordinate), new Vector2(0, 1));
            gamePieces.Add(exit);
            return exit;
        }

        public clsGamePieceCar createCar(intDriver driver, Vector2 squareCoordinate, Vector2 direction, Vector2 velocity)
        {
            clsGamePieceCar car = new clsGamePieceCar(driver, squareCoordinateToWorldLocation(squareCoordinate), direction, velocity);
            gamePieces.Add(car);
            return car;
        }

        public List<intGamePiece> getGamePieces(GamePieceType gamePieceType)
        {
            List<intGamePiece> gamePieces = new List<intGamePiece>();
            foreach (intGamePiece gamePiece in this.gamePieces)
            {
                if (gamePiece.gamePieceType == gamePieceType) gamePieces.Add(gamePiece);
            }
            return gamePieces;
        }

        public intGamePiece getRandomGamePiece(GamePieceType gamePieceType)
        {
            List<intGamePiece> gamePieces = getGamePieces(gamePieceType);
            return gamePieces[this.rnd.Next(gamePieces.Count())];
        }
    }
    
}
