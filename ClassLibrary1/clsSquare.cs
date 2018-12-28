using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace gameLogic
{
    // square is the AI changer
    // direction
    // speed
    // stop , stop temp, yeild, lane chnage, no lane change

    public class clsSquare
    {
        public List<Vector2> directions;

        public clsSquare()
        {
            directions = new List<Vector2>();
        }
        public clsSquare(bool east, bool west, bool north, bool south)
        {
            directions = new List<Vector2>();
            if (east) directions.Add(new Vector2(1, 0));
            if (west) directions.Add(new Vector2(-1, 0));
            if (north) directions.Add(new Vector2(0, -1));
            if (south) directions.Add(new Vector2(0, 1));
        }
    }
}
