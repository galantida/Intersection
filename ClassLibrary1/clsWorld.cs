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

    public class clsWorld : clsTileWorld
    {
        public List<intWorldObject> worldObjects;
        private List<intActor> actors;
        private Stopwatch _currentTime = new Stopwatch();
        public Random random;

        public clsWorld(long tilesWide, float tileSize): base(tileSize)
        {
            random = new Random(); // randomize seed the world
            _currentTime.Start(); // start processing clock

            loadActors();
            loadTiles(tilesWide);
            loadObjects();
            
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
                    tiles[x, y] = new clsTile(false, false, false, false);
                }
            }

            // east west road
            y = tilesWide / 2;
            for (x = 0; x < tilesWide; x++)
            {
                tiles[x, y - 1] = new clsTile(false, true, false, false);
                tiles[x, y] = new clsTile(true, false, false, false);
            }

            // north south road
            x = (tilesWide / 2);
            for (y = 0; y < tilesWide; y++)
            {
                tiles[x, y] = new clsTile(false, false, true, false);
                tiles[x - 1, y] = new clsTile(false, false, false, true);
            }

            // intersection
            tiles[(tilesWide / 2) - 1, (tilesWide / 2)] = new clsTile(true, false, false, true); // southwest
            tiles[(tilesWide / 2), (tilesWide / 2) - 1] = new clsTile(false, true, true, false); // northeast
            tiles[(tilesWide / 2) - 1, (tilesWide / 2) - 1] = new clsTile(false, true, false, true); // northwest
            tiles[(tilesWide / 2), (tilesWide / 2)] = new clsTile(true, false, true, false); // southeast
        }

        public void loadObjects()
        {
            worldObjects = new List<intWorldObject>();

            // create entry points
            createEntry(new Vector2(6, 0), new Vector2(0, 1), WorldObjectType.car, 10000);
            createEntry(new Vector2(7, 13), new Vector2(0, -1), WorldObjectType.car, 10000);
            createEntry(new Vector2(13, 6), new Vector2(-1, 0), WorldObjectType.car, 10000);
            createEntry(new Vector2(0, 7), new Vector2(1, 0), WorldObjectType.car, 10000);

            // create exit points
            createExit(new Vector2(7, 0));
            createExit(new Vector2(6, 13));
            createExit(new Vector2(13, 7));
            createExit(new Vector2(0, 6));

            // spawn a human car
            spawnCarHuman(new Vector2(256, 256), new Vector2(1, 0), new Vector2(0, 0));
        }

        public void loadActors()
        {
            actors = new List<intActor>();
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
        }

        /*****************************************
                Instance Objects in the world
         *****************************************/
        public void remove(intWorldObject gameObject)
        {
            worldObjects.Remove(gameObject);
        }

        public clsEntry createEntry(Vector2 squareCoordinate, Vector2 direction, WorldObjectType gameObjectType, int maxSpawnTime)
        {
            clsEntry entry = new clsEntry(squareCoordinateToWorldLocation(squareCoordinate), direction, this, gameObjectType, maxSpawnTime);
            worldObjects.Add((intWorldObject)entry);
            return entry;
        }

        public clsExit createExit(Vector2 squareCoordinate)
        {
            clsExit exit = new clsExit(squareCoordinateToWorldLocation(squareCoordinate), new Vector2(0, 1));
            worldObjects.Add((intWorldObject)exit);
            return exit;
        }

        public clsCar createCar(Vector2 worldLocation, Vector2 direction, Vector2 velocity)
        {
            clsCar car = new clsCar(worldLocation, direction, velocity);
            worldObjects.Add((intWorldObject)car);
            return car;
        }

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

        public List<intWorldObject> getWorldObjects(WorldObjectType worldObjectType)
        {
            List<intWorldObject> filteredWorldObjects = new List<intWorldObject>();
            foreach (intWorldObject worldObject in this.worldObjects)
            {
                if (worldObject.worldObjectType == worldObjectType) filteredWorldObjects.Add(worldObject);
            }
            return filteredWorldObjects;
        }

        public intWorldObject getRandomWorldObject(WorldObjectType worldObjectType)
        {
            List<intWorldObject> filteredWorldObjects = getWorldObjects(worldObjectType);
            return filteredWorldObjects[this.random.Next(filteredWorldObjects.Count())];
        }

        public bool collision(clsWorldObject firstObject, clsWorldObject secondObject = null)
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
                foreach (clsWorldObject worldObject in this.worldObjects)
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
