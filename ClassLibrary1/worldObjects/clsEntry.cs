﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using tileWorld;
using physicalWorld;
using Microsoft.Xna.Framework;


namespace gameLogic
{
    public class clsEntry : clsObject, intObject
    {
        // object specific 
        public clsRoadWorld worldToSpawnIn;
        public string spawnTypeName { get; set; }
        public int maxSpawnTime { get; set; }
        public float nextSpawnTime { get; set; }

        public clsEntry(string textureName, Vector2 location, Vector2 direction, clsRoadWorld worldToSpawnIn, string spawnTypeName, int maxSpawnTime) : base(textureName, location, direction, new Vector2(0,0))
        {
            base.typeName = "entry";
            base.collisionDetection = CollisionType.None;
            this.spawnTypeName = spawnTypeName;
            this.maxSpawnTime = maxSpawnTime;
            this.worldToSpawnIn = worldToSpawnIn;

            // add min spawn time later
            this.nextSpawnTime = worldToSpawnIn.random.Next(maxSpawnTime);
        }


        new public void update(float currentTime)
        {
            if (currentTime > nextSpawnTime) 
            {
                // is it clear to spawn?
                if (this.worldToSpawnIn.getTileFromWorldLocation(this.location).worldObjects.Count <= 1)
                {
                    // get the next spawn time
                    nextSpawnTime = currentTime + worldToSpawnIn.random.Next(maxSpawnTime);

                    switch (this.spawnTypeName)
                    {
                        case "car":
                            Vector2 spawnLocation = randomizeVector(worldToSpawnIn, this.location); // randomizes car location slightly

                            clsExit exit = null;
                            while (exit == null)
                            {
                                exit = (clsExit)worldToSpawnIn.getRandomWorldObject("exit");
                                if ((spawnLocation - exit.location).Length() < 300) exit = null;
                            }

                            clsDriverAI driver = worldToSpawnIn.spawnCarAI(spawnLocation, this.direction, new Vector2(0, 0), exit.location);
                            driver.car.color = new Color(worldToSpawnIn.random.Next(0, 255), worldToSpawnIn.random.Next(0, 255), worldToSpawnIn.random.Next(0, 255));
                            break;
                    }
                    base.addForce(new Vector2(0, 0)); // not sure why we add no force
                }

            }

            // always apply physpics
            base.update(currentTime);
        }

        private Vector2 randomizeVector(clsRoadWorld world, Vector2 location)
        {
            int variant = 16;
            float modX = world.random.Next(variant * 2) -variant;
            float modY = world.random.Next(variant * 2) -variant;
            return new Vector2(location.X + modX, location.Y + modY);
        }
    }
}
