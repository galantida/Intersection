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
    public class clsRoadWorld : clsWorld
    {
        public clsInput input; // input for wordl actions like scroll in and out

        public clsRoadWorld(long tilesWide, float tileSize): base(tileSize)
        {
            this.input = new clsInput(); // input devices
            loadTiles(tilesWide);
            loadObjects(tilesWide);
            loadActors();
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

        public void loadObjects(long tilesWide)
        {
            worldObjects = new List<intObject>();

            float roadx = tilesWide / 2;
            float roady = tilesWide / 2;

            // create exit points
            createExit(new Vector2(roadx, 0));
            createExit(new Vector2(roadx - 1, tilesWide - 1));
            createExit(new Vector2(tilesWide - 1, roady));
            createExit(new Vector2(0, roady - 1));

            // create entry points
            createEntry(new Vector2(roadx - 1, 0), new Vector2(0, 1), "car", 15000);
            createEntry(new Vector2(roadx, tilesWide-1), new Vector2(0, -1), "car", 15000);
            createEntry(new Vector2(tilesWide - 1, roady-1), new Vector2(-1, 0), "car", 15000);
            createEntry(new Vector2(0, roady), new Vector2(1, 0), "car", 15000);
        }

        public void loadActors()
        {
            actors = new List<intActor>();

            // spawn a human car
            spawnCarLocalHuman(this, new Vector2(256, 256), new Vector2(1, 0), new Vector2(0, 0));
        }

        /*****************************************
                Running the world
         *****************************************/
        public new void update()
        {
            base.update();
        }

        /*****************************************
                Instance Objects in the world
         *****************************************/

        public void addGrass(long tilex, long tiley)
        {
            clsRoadWorldTile grass = new clsRoadWorldTile(new List<Vector2>(), 0);
            base.addTile(grass, tilex, tiley);
        }

        public void addRoad(long tilex, long tiley, CardinalDirection direction)
        {
            switch (direction)
            {
                case CardinalDirection.East:
                    {
                        clsRoadWorldTile road = new clsRoadWorldTile(clsRoadWorldTile.getDirections(true, false, false, false), 55);
                        base.addTile(road, tilex, tiley);
                        break;
                    }
                case CardinalDirection.West:
                    {
                        clsRoadWorldTile road = new clsRoadWorldTile(clsRoadWorldTile.getDirections(false, true, false, false), 55);
                        base.addTile(road, tilex, tiley);
                        break;
                    }
                case CardinalDirection.North:
                    {
                        clsRoadWorldTile road = new clsRoadWorldTile(clsRoadWorldTile.getDirections(false, false, true, false), 35);
                        base.addTile(road, tilex, tiley);
                        break;
                    }
                case CardinalDirection.South:
                    {
                        clsRoadWorldTile road = new clsRoadWorldTile(clsRoadWorldTile.getDirections(false, false, false, true), 35);
                        base.addTile(road, tilex, tiley);
                        break;
                    }
            }
        }

        public void addIntersection(long tilex, long tiley, bool east, bool west, bool north, bool south)
        {
            clsRoadWorldTile intersection = new clsRoadWorldTile(clsRoadWorldTile.getDirections(east, west, north, south), 35);
            base.addTile(intersection, tilex, tiley);
        }

        /*****************************************
                Instance Objects in the world
         *****************************************/
        public clsEntry createEntry(Vector2 tileCoordinate, Vector2 direction, string typeName, int maxSpawnTime)
        {
            clsEntry entry = new clsEntry("entry", tileCoordinateToWorldLocation(tileCoordinate), direction, this, typeName, maxSpawnTime);
            worldObjects.Add((intObject)entry);
            return entry;
        }

        public clsExit createExit(Vector2 tileCoordinate)
        {
            clsExit exit = new clsExit("exit", tileCoordinateToWorldLocation(tileCoordinate), new Vector2(0, 1));
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
        public clsDriverHuman createDriverHuman(clsWorld world, clsCar car, clsInput input)
        {
            clsDriverHuman human = new clsDriverHuman(world, car, input); // assign human to it
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
        public clsDriverHuman spawnCarLocalHuman(clsWorld world, Vector2 worldLocation, Vector2 direction, Vector2 velocity)
        {
            clsCar car = createCar(worldLocation, direction, velocity); // spanw car
            clsDriverHuman human = createDriverHuman(world, car, this.input); // create human for the car
            return human;
        }

        public clsDriverAI spawnCarAI(Vector2 worldLocation, Vector2 direction, Vector2 velocity, Vector2 destination)
        {
            clsCar car = createCar(worldLocation, direction, velocity); // spanw car
            clsDriverAI AI = createDriverAI(car, destination); // create AI for the car
            return AI;
        }

        public intRoadWorldTile getRoadWorldTileFromTileCoordinate(Vector2 tileCoordinate)
        {
            return (intRoadWorldTile)this.getTileFromTileCoordinate(tileCoordinate);
        }
    }
    
}
