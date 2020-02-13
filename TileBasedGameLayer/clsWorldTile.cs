using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tileWorld;
using Microsoft.Xna.Framework;

namespace tileWorld
{
    public class clsWorldTile 
    {
        public string typeName { get; set; }
        public string textureName { get; set; }

        public List<Vector2> directions { get; set; }

        public clsWorldTile(bool east, bool west, bool north, bool south)
        {
            this.directions = new List<Vector2>();
            if (east) directions.Add(new Vector2(1, 0));
            if (west) directions.Add(new Vector2(-1, 0));
            if (north) directions.Add(new Vector2(0, -1));
            if (south) directions.Add(new Vector2(0, 1));

            setTexture();
        }
       
        public void setTexture()
        {
            if (directions.Count() == 0)
            {
                this.textureName = "grass";
            }
            else if (directions.Count() == 1)
            {
                // directional road
                this.textureName = "road";
            }
            else
            {
                // multidirectional road
                this.textureName = "intersection";
            }
        }

       
    }
}
