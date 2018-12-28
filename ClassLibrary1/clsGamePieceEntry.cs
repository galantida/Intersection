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
    public class clsGamePieceEntry : clsBaseGamePiece, intGamePiece
    {
        // object specific 
        public int maxSpawnTime { get; set; }
        public GamePieceType spawnType { get; set; }
        public long nextSpawnTime { get; set; }
        public Vector2 direction { get; set; }

        public clsGamePieceEntry(clsWorld world, Vector2 location, Vector2 direction, GamePieceType spawnType, int maxSpawnTime) : base(world, location,new Vector2(0,0), 0)
        {
            this.gamePieceType = GamePieceType.entry;
            this.direction = direction;

            this.spawnType = spawnType;
            this.maxSpawnTime = maxSpawnTime;
            this.nextSpawnTime = base.world.rnd.Next(maxSpawnTime);
        }


        new public void update()
        {
            if (stopWatch.ElapsedMilliseconds > nextSpawnTime) 
            {
                nextSpawnTime = base.world.rnd.Next(maxSpawnTime);

                switch (this.spawnType)
                {
                    case GamePieceType.car:
                        //Vector2 carVelocity = direction * (base.world.rnd.Next(5) * 0.03f);
                        clsGamePieceExit exit = randomExit();
                        clsGamePieceCar car = world.createCar(this.squareCoordinate, this.direction, new Vector2(0,0));
                        car.location = randomizeVector(car.location); // renadomizes car location slightly
                        clsDriverAI ai = new clsDriverAI(car, exit, world);
                        world.drivers.Add(ai);
                        break;
                }

                base.update();
            }
        }

        private clsGamePieceExit randomExit()
        {
            List<clsGamePieceExit> exits = new List<clsGamePieceExit>();
            foreach (intGamePiece gamePiece in world.gamePieces)
            {
                if (gamePiece.gamePieceType == GamePieceType.exit)
                {
                    if (Vector2.Distance(gamePiece.squareCoordinate, this.squareCoordinate) > 5)
                    {
                        exits.Add((clsGamePieceExit)gamePiece);
                    }
                }
            }

            return exits[base.world.rnd.Next(exits.Count())];
        }

        private Vector2 randomizeVector(Vector2 location)
        {
            int variant = 10;
            float modX = base.world.rnd.Next(variant * 2) -variant;
            float modY = base.world.rnd.Next(variant * 2) -variant;
            return new Vector2(location.X + modX, location.Y + modY);
        }
    }
}
