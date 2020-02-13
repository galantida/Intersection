using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using tileWorld;

namespace tileWorld
{
    // square is the AI changer
    // direction
    // speed
    // stop , stop temp, yeild, lane chnage, no lane change

    public class clsTile_old
    {
        public List<Vector2> directions { get; set; }

        public clsTile_old()
        {
            directions = new List<Vector2>();
        }
        public clsTile_old(bool east, bool west, bool north, bool south)
        {
            directions = new List<Vector2>();
            if (east) directions.Add(new Vector2(1, 0));
            if (west) directions.Add(new Vector2(-1, 0));
            if (north) directions.Add(new Vector2(0, -1));
            if (south) directions.Add(new Vector2(0, 1));
        }

        public string textureName
        {
            get
            {
                if (directions.Count() == 0)
                {
                    return "grass";
                }
                else if (directions.Count() == 1)
                {
                    // directional road
                    return "road";
                }
                else
                {
                    // multidirectional road
                    return "intersection";
                }
            }
        }
    }
}
