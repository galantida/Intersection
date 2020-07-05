using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using tileWorld;

namespace gameLogic
{
    public class clsRoadWorldTile : clsTile, intTile, intRoadWorldTile
    {
        public float speedLimit { get; set; }

        public clsRoadWorldTile(List<Vector2>directions, int speedLimit) : base(null, true)
        {
            this.directions = directions;
            this.speedLimit = speedLimit;

            if (directions.Count > 1)
            {
                this.textureName = "intersection";
                this.rotation = 0;
                this.collisionDetection = true;
            }

            if (directions.Count == 0)
            {
                this.textureName = "grass";
                this.rotation = 0;
                this.collisionDetection = false;
            }

            if (directions.Count == 1)
            {
                this.textureName = "road";
                this.collisionDetection = true;
                // determine texture based on values
                // should only be one direction per lane
                if (directions[0].X == 1)
                {
                    rotation = 3.14f;
                    //origin = new Vector2(64, 64);
                }

                if (directions[0].X == -1)
                {
                    rotation = 0f;
                    //origin = new Vector2(0, 0);
                }

                if (directions[0].Y == -1)
                {
                    rotation = 1.57f;
                    //origin = new Vector2(0, 64);
                }

                if (directions[0].Y == 1)
                {
                    rotation = 4.71f;
                    //origin = new Vector2(64, 0);
                }
            }
        }

        public static List<Vector2> getDirections(bool east, bool west, bool north, bool south)
        {
            List<Vector2> directions = new List<Vector2>();
            if (east) directions.Add(new Vector2(1, 0));
            if (west) directions.Add(new Vector2(-1, 0));
            if (north) directions.Add(new Vector2(0, -1));
            if (south) directions.Add(new Vector2(0, 1));
            return directions;
        }
    }
}
