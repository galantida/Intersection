using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tileWorld;
using Microsoft.Xna.Framework;

namespace tileWorld
{
    public class clsTile 
    {
        public string typeName { get; set; }
        public string textureName { get; set; }

        public List<Vector2> directions { get; set; }

        // other than direction what properties would a tile have
        // height? impead movment? slippery? temperature?

        public clsTile(string typeName, string textureName, bool east, bool west, bool north, bool south)
        {
            this.typeName = typeName;
            this.textureName = textureName;

            this.directions = new List<Vector2>();
            if (east) directions.Add(new Vector2(1, 0));
            if (west) directions.Add(new Vector2(-1, 0));
            if (north) directions.Add(new Vector2(0, -1));
            if (south) directions.Add(new Vector2(0, 1));

            
        }
    }
}
