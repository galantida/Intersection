﻿using System;
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

        public void update()
        {
            processCollisions();
            processTileLocations();
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

        /*********************************************************************************
         * Actor Functions
         *********************************************************************************/
        public void remove(intActor actorObject)
        {
            actors.Remove(actorObject);
        }

        /**************************************************
            coordinate conversion functions
        **************************************************/
        public Vector2 worldLocationToTileCoordinate(Vector2 worldCoordinate)
        {
            return new Vector2((int)(worldCoordinate.X / tileSize), (int)(worldCoordinate.Y / tileSize));
        }

        public Vector2 tileCoordinateToWorldLocation(Vector2 tileCoordinate)
        {
            return new Vector2(tileCoordinate.X * tileSize + (tileSize / 2), tileCoordinate.Y * tileSize + (tileSize / 2));
        }

        /**************************************************
            tile access shortcut
        **************************************************/
        public intTile getTileFromWorldLocation(Vector2 worldLocation)
        {
            return getTileFromTileCoordinate(this.worldLocationToTileCoordinate(worldLocation));
        }

        public intTile getTileFromTileCoordinate(Vector2 tileCoordinate)
        {
            return tiles[(int)tileCoordinate.X, (int)tileCoordinate.Y];
        }

        public bool inTileCoordinateBounds(Vector2 tileCoordinate)
        {
            bool result = true;
            if ((tileCoordinate.X < 0) || (tileCoordinate.X >= tiles.GetLength(0))) result = false; 
            if ((tileCoordinate.Y < 0) || (tileCoordinate.Y >= tiles.GetLength(1))) result = false; 
            return result;
        }

        public bool inWorldBounds(Vector2 tileCoordinate)
        {
            bool result = true;
            if ((tileCoordinate.X < 0) || (tileCoordinate.X >= tiles.GetLength(0))) result = false;
            if ((tileCoordinate.Y < 0) || (tileCoordinate.Y >= tiles.GetLength(1))) result = false;
            return result;
        }

        /*****************************************
        *              Collision Detections
        *****************************************/
        public void processCollisions()
        {
            // create a list of objects to process
            List<intObject> allCollisionObjects = new List<intObject>();
            List<intObject> unprocessedCollisionObjects = new List<intObject>();
            foreach (intObject worldObject in this.worldObjects)
            {
                // clear world object previous list of collisions
                worldObject.collisions = new List<intObject>();
                if (worldObject.collisionDetection != CollisionType.None)
                {
                    allCollisionObjects.Add(worldObject);
                    unprocessedCollisionObjects.Add(worldObject);
                }
            }

            // process each object
            foreach (intObject worldObject in allCollisionObjects)
            {
                for (int t=0; t < unprocessedCollisionObjects.Count; t++)
                {
                    // objects should compare themselves to themselves
                    if (worldObject != unprocessedCollisionObjects[t])
                    {
                        float distance = (worldObject.location - unprocessedCollisionObjects[t].location).Length();
                        if (distance < (worldObject.collisionRadius + unprocessedCollisionObjects[t].collisionRadius))
                        {
                            // we have a collsions
                            worldObject.collisions.Add(unprocessedCollisionObjects[t]);
                            unprocessedCollisionObjects[t].collisions.Add(worldObject);
                        }
                    }
                }

                // remove this object from unprocessed
                unprocessedCollisionObjects.Remove(worldObject);
            }
        }

        /*****************************************
       *              tile locations
       *****************************************/
       public void processTileLocations()
        {
            for (int x = 0; x < this.tiles.GetLength(0); x += 1)
            {
                for (int y = 0; y < this.tiles.GetLength(1); y += 1)
                {
                    this.tiles[x, y].worldObjects = new List<intObject>();
                }
            }

            foreach (intObject worldObject in this.worldObjects)
            {
                intTile tile = this.getTileFromWorldLocation(worldObject.location);
                tile.worldObjects.Add(worldObject);
            }
        }
    }

}
