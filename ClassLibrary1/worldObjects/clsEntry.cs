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
    public class clsGamePieceEntry : clsBaseGameObject, intGamePiece
    {
        // object specific 
        public int maxSpawnTime { get; set; }
        public GamePieceType spawnType { get; set; }
        public float nextSpawnTime { get; set; }
        public Vector2 heading { get; set; }

        public clsGamePieceEntry(clsWorld world, Vector2 location, Vector2 heading, GamePieceType spawnType, int maxSpawnTime) : base(world, location,new Vector2(0,0), 0)
        {
            this.gamePieceType = GamePieceType.entry;
            this.heading = heading;

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
                    case GamePieceType.car:
                        clsGamePieceExit exit = (clsGamePieceExit)world.getRandomGamePiece(GamePieceType.exit);
                        Vector2 spawnLocation = randomizeVector(world, this.location); // randomizes car location slightly
                        world.spawnCarAI(world.worldLocationToSquareCoordinate(spawnLocation), this.heading, new Vector2(0, 0), exit.location);
                        break;
                }
                base.applyForce(new Vector2(0,0));
            }

            // always apply phypics
            base.update();
        }

        private Vector2 randomizeVector(clsWorld world, Vector2 location)
        {
            int variant = 10;
            float modX = world.random.Next(variant * 2) -variant;
            float modY = world.random.Next(variant * 2) -variant;
            return new Vector2(location.X + modX, location.Y + modY);
        }
    }
}
