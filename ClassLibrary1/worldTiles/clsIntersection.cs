using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using tileWorld;

namespace gameLogic
{
    class clsIntersection : clsTile, intTile, intRoad
    {
        public List<Vector2> directions { get; set; }

        public clsIntersection(bool east, bool west, bool north, bool south) : base("intersection", "intersection", true)
        {
            this.directions = new List<Vector2>();
            if (east) directions.Add(new Vector2(1, 0));
            if (west) directions.Add(new Vector2(-1, 0));
            if (north) directions.Add(new Vector2(0, -1));
            if (south) directions.Add(new Vector2(0, 1));
        }
    }
}
