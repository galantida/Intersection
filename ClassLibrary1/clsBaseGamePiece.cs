using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace gameLogic
{
    // the physical piece which is unaware of its location on the board

    public abstract class clsBaseGamePiece : clsNewton
    {
        // interface fields
        public GamePieceType gamePieceType { get; set; }

        public clsBaseGamePiece(Vector2 location, Vector2 velocity, float mass = 1000.0f):base(location, velocity, mass)
        {
            stopWatch.Start();
        }
    }
}
