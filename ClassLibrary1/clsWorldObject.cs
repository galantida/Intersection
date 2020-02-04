using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tileWorld;
using Microsoft.Xna.Framework;

namespace gameLogic
{
    public enum WorldObjectType { entry, exit, car };

    public class clsWorldObject : clsTileObject
    {
        public WorldObjectType worldObjectType { get; set; }
        

        public clsWorldObject(Vector2 location, Vector2 direction, Vector2 velocity) : base(location, direction, velocity)
        {
            colorReplacements = new Dictionary<Color, Color>();
        }

        

    }
}
