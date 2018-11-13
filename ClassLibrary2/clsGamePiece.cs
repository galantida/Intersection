using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace gameEngine
{
    public class clsGamePiece
    {
        public Vector2 squareLocation;
        public string textureName;

        public clsGamePiece(Vector2 squareLocation, string textureName)
        {
            this.textureName = textureName;
            this.squareLocation = squareLocation;
        }
    }
}
