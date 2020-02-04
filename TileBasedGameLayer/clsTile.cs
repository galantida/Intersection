using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace tileWorld
{
    // square is the AI changer
    // direction
    // speed
    // stop , stop temp, yeild, lane chnage, no lane change

    public class clsTile
    {
        public List<Vector2> directions;

        public clsTile()
        {
            directions = new List<Vector2>();
        }
        public clsTile(bool east, bool west, bool north, bool south)
        {
            directions = new List<Vector2>();
            if (east) directions.Add(new Vector2(1, 0));
            if (west) directions.Add(new Vector2(-1, 0));
            if (north) directions.Add(new Vector2(0, -1));
            if (south) directions.Add(new Vector2(0, 1));
        }
    }
}
