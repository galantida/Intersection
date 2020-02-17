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

    public class clsWorld 
    {
        public Random random;
        protected Stopwatch _currentTime = new Stopwatch();

        public intTile[,] tiles;
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
        public void addTile(string typeName, string textureName, long tilex, long tiley, bool passable)
        {
            tiles[tilex, tiley] = (intTile)new clsTile(typeName, textureName, passable);
        }

        public void addTile(intTile tile, long tilex, long tiley)
        {
            tiles[tilex, tiley] = tile;
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
        public intTile getSquareFromWorldLocation(Vector2 worldLocation)
        {
            return getSquareFromSquareCoordinate(this.worldLocationToSquareCoordinate(worldLocation));
        }

        public intTile getSquareFromSquareCoordinate(Vector2 squareCoordinate)
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
         *              Collision Detections
         *****************************************/
        public intObject closestColidableObject(Vector2 location)
        {
            intObject closestObject = null;
            float closestDistance = 1000000000;
            foreach (intObject worldObject in this.worldObjects)
            {
                if (worldObject.collisionType != CollisionType.None)
                {
                    if (worldObject.location != location)
                    {
                        float distance = (location - worldObject.location).Length();
                        if ((closestObject == null) || (distance < closestDistance))
                        {
                            closestDistance = distance;
                            closestObject = worldObject;
                        }
                    }
                }
            }
            return closestObject;
        }

        public bool collision(intObject firstObject, intObject secondObject = null)
        {
            // detect collision between two specific objects
            if (secondObject != null)
            {
                Vector2 distance = firstObject.location - secondObject.location;
                if (distance.Length() < 64) return true;
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
