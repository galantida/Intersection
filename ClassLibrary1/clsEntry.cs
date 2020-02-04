using System;
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
    public class clsEntry : clsWorldObject, intWorldObject
    {
        // object specific 
        public clsWorld worldToSpawnIn;
        public WorldObjectType spawnType { get; set; }
        public int maxSpawnTime { get; set; }
        public float nextSpawnTime { get; set; }

        public clsEntry(Vector2 location, Vector2 direction, clsWorld worldToSpawnIn, WorldObjectType spawnType, int maxSpawnTime) : base(location, direction, new Vector2(0,0))
        {
            this.worldObjectType = WorldObjectType.entry;
            base.collisionType = CollisionType.None;
            this.spawnType = spawnType;
            this.maxSpawnTime = maxSpawnTime;
            this.worldToSpawnIn = worldToSpawnIn;

            // add min spawn time later
            this.nextSpawnTime = worldToSpawnIn.random.Next(maxSpawnTime);
        }


        new public void update(float currentTime)
        {
            if (currentTime > nextSpawnTime) 
            {
                nextSpawnTime = currentTime + worldToSpawnIn.random.Next(maxSpawnTime);

                switch (this.spawnType)
                {
                    case WorldObjectType.car:
                        clsExit exit = (clsExit)worldToSpawnIn.getRandomWorldObject(WorldObjectType.exit);
                        Vector2 spawnLocation = randomizeVector(worldToSpawnIn, this.location); // randomizes car location slightly
                        worldToSpawnIn.spawnCarAI(spawnLocation, this.direction, new Vector2(0, 0), exit.location);
                        break;
                }
                base.addForce(new Vector2(0,0));
            }

            // always apply phypics
            base.update(currentTime);
        }

        private Vector2 randomizeVector(clsWorld world, Vector2 location)
        {
            int variant = 16;
            float modX = world.random.Next(variant * 2) -variant;
            float modY = world.random.Next(variant * 2) -variant;
            return new Vector2(location.X + modX, location.Y + modY);
        }
    }
}
