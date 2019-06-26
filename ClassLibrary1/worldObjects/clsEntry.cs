using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using Microsoft.Xna.Framework;


namespace gameLogic
{
    public class clsGamePieceEntry : clsBaseWorldObject, intWorldObject
    {
        // object specific 
        public int maxSpawnTime { get; set; }
        public WorldObjectType spawnType { get; set; }
        public float nextSpawnTime { get; set; }

        public clsGamePieceEntry(clsWorld world, Vector2 location, Vector2 direction, WorldObjectType spawnType, int maxSpawnTime) : base(world, location, direction, new Vector2(0,0), 0)
        {
            this.worldObjectType = WorldObjectType.entry;
            base.collisionDetection = false;

            this.spawnType = spawnType;
            this.maxSpawnTime = maxSpawnTime;
            // add min spawn time later
            this.nextSpawnTime = world.random.Next(maxSpawnTime);
        }


        new public void update()
        {
            if (world.currentTime > nextSpawnTime) 
            {
                nextSpawnTime = world.currentTime + world.random.Next(maxSpawnTime);

                switch (this.spawnType)
                {
                    case WorldObjectType.car:
                        clsGamePieceExit exit = (clsGamePieceExit)world.getRandomWorldObject(WorldObjectType.exit);
                        Vector2 spawnLocation = randomizeVector(world, this.location); // randomizes car location slightly
                        world.spawnCarAI(spawnLocation, this.direction, new Vector2(0, 0), exit.location);
                        break;
                }
                base.addForce(new Vector2(0,0));
            }

            // always apply phypics
            base.update();
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
