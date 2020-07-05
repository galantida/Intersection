using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace tileWorld
{
    public interface intTile
    {
        // visible properties
        string textureName { get; set; }
        Vector2 location { get; set; }
        float rotation { get; set; }
        float textureRotation { get; }
        Dictionary<Color, Color> colorReplacements { get; set; }
        List<Vector2> directions { get; set; }

        // processing properties
        bool collisionDetection { get; set; }
        List<intObject> worldObjects { get; set; }
        

        
    }
}
