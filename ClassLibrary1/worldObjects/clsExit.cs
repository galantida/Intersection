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
    public class clsGamePieceExit : clsBaseGameObject, intGamePiece
    {
        public Vector2 direction { get; set; }
        
        public clsGamePieceExit(clsWorld world, Vector2 location, Vector2 direction) : base(world, location, new Vector2(0, 0), 0)
        {
            this.gamePieceType = GamePieceType.exit;
            this.direction = direction;
        }


        new public void update()
        {
            float deltaTime = this.world.currentTime - lastUpdated; // time since last updated
            if (deltaTime > 10)
            {
                this.lastUpdated = this.world.currentTime; // reset last updated

                // maybe remove cars
                // maybe calculate score

                base.applyForce(new Vector2(0,0));
            }

            // always apply phypics
            base.update();
        }

        public void removeGamePiece(intGamePiece gamePiece)
        {
            world.removeGamePiece(gamePiece);
        }
    }
}
