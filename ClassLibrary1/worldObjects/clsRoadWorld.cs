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
            tiles = new clsTile[tilesWide, tilesWide];
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
                addEastboundLane(x, y);
                addWestboundLane(x, y - 1);
            }

            // north south road
            x = (tilesWide / 2);
            for (y = 0; y < tilesWide; y++)
            {
                addNorthboundLane(x, y);
                addSouthboundLane(x-1, y);
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
            createEntry(new Vector2(6, 0), new Vector2(0, 1), "car", 10000);
            createEntry(new Vector2(7, 13), new Vector2(0, -1), "car", 10000);
            createEntry(new Vector2(13, 6), new Vector2(-1, 0), "car", 10000);
            createEntry(new Vector2(0, 7), new Vector2(1, 0), "car", 10000);

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
        public void update()
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

        }

        /*****************************************
                Instance Objects in the world
         *****************************************/

        // tiles[(tilesWide / 2) - 1, (tilesWide / 2)] = new clsTile("intersection", true, false, false, true); // southwest

        public void addGrass(long tilex, long tiley)
        {
            base.addTile("grass", "grass", tilex, tiley, false, false, false, false);
        }

        public void addEastboundLane(long tilex, long tiley)
        {
            base.addTile("road", "road", tilex, tiley, true, false, false, false);
        }
        public void addWestboundLane(long tilex, long tiley)
        {
            base.addTile("road", "road", tilex, tiley, false, true, false, false);
        }

        public void addNorthboundLane(long tilex, long tiley)
        {
            base.addTile("road", "road", tilex, tiley, false, false, true, false);
        }
        public void addSouthboundLane(long tilex, long tiley)
        {
            base.addTile("road", "road", tilex, tiley, false, false, false, true);
        }
        public void addIntersection(long tilex, long tiley, bool east, bool west, bool north, bool south)
        {
            base.addTile("intersection", "intersection", tilex, tiley, east, west, north, south);
        }

        /*****************************************
                Instance Objects in the world
         *****************************************/
        public clsEntry createEntry(Vector2 squareCoordinate, Vector2 direction, string typeName, int maxSpawnTime)
        {
            clsEntry entry = new clsEntry("entry", squareCoordinateToWorldLocation(squareCoordinate), direction, this, typeName, maxSpawnTime);
            worldObjects.Add((intObject)entry);
            return entry;
        }

        public clsExit createExit(Vector2 squareCoordinate)
        {
            clsExit exit = new clsExit("exit", squareCoordinateToWorldLocation(squareCoordinate), new Vector2(0, 1));
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
    }
    
}
