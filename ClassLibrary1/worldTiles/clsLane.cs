using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tileWorld;
using Microsoft.Xna.Framework;

namespace gameLogic
{
    class clsLane : clsTile, intTile, intRoad
    {
        public List<Vector2> directions { get; set; }

        public clsLane(CardinalDirection direction) : base("road", "road", true)
        {
            this.directions = new List<Vector2>();
            if (direction == CardinalDirection.East) {
                directions.Add(new Vector2(1, 0));
                rotation = 3.14f;
                //origin = new Vector2(64, 64);
            }

            if (direction == CardinalDirection.West) {
                directions.Add(new Vector2(-1, 0));
                rotation = 0f;
                //origin = new Vector2(0, 0);
            }

            if (direction == CardinalDirection.North) {
                directions.Add(new Vector2(0, -1));
                rotation = 1.57f;
                //origin = new Vector2(0, 64);
            }

            if (direction == CardinalDirection.South) {
                directions.Add(new Vector2(0, 1));
                rotation = 4.71f;
                //origin = new Vector2(64, 0);
            }
        }
    }
}
