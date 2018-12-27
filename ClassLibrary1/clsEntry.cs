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
    public class clsEntry : clsBaseGamePiece, intGamePiece
    {
        // object specific 
        public int maxSpawnTime { get; set; }
        public GamePieceType spawnType { get; set; }
        public long nextSpawnTime { get; set; }
        public Vector2 direction { get; set; }

        public clsEntry(clsWorld world, Vector2 location, Vector2 direction, GamePieceType spawnType, int maxSpawnTime) : base(world, location,new Vector2(0,0), 0)
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
                        clsExit exit = randomExit();
                        clsCar car = world.createCar(randomSpawnLocation(), this.direction, new Vector2(0,0));
                        clsAI ai = new clsAI(car, exit, world);
                        world.drivers.Add(ai);
                        break;
                }

                base.update();
            }
        }

        private clsExit randomExit()
        {
            List<clsExit> exits = new List<clsExit>();
            foreach (intGamePiece gamePiece in world.gamePieces)
            {
                if (gamePiece.gamePieceType == GamePieceType.exit)
                {
                    if (Vector2.Distance(gamePiece.squareCoordinate, this.squareCoordinate) > 5)
                    {
                        exits.Add((clsExit)gamePiece);
                    }
                }
            }

            return exits[base.world.rnd.Next(exits.Count())];
        }

        private Vector2 randomSpawnLocation()
        {
            int variant = 10;
            float modX = base.world.rnd.Next(variant * 2) -variant;
            float modY = base.world.rnd.Next(variant * 2) -variant;
            return new Vector2(location.X + modX, location.Y + modY);
        }
    }
}
