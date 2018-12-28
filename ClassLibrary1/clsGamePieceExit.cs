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
    public class clsGamePieceExit : clsBaseGamePiece, intGamePiece
    {
        public Vector2 direction { get; set; }
        public clsWorld world;

        public clsGamePieceExit(clsWorld world, Vector2 location, Vector2 direction) : base(location, new Vector2(0, 0), 0)
        {
            this.gamePieceType = GamePieceType.exit;
            this.direction = direction;
        }


        new public void update(clsWorld world)
        {
            if (stopWatch.ElapsedMilliseconds > 1000) 
            {
                // maybe remove cars
                // maybe calculate score

                base.update(world);
            }
        }

        public void removeGamePiece(intGamePiece gamePiece)
        {
            world.removeGamePiece(gamePiece);
        }
    }
}
