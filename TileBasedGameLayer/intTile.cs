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
        string typeName { get; set; }
        string textureName { get; set; }
        float textureRotation { get; }
        float rotation { get; set; }
        bool collisionDetection { get; set; }
        List<intObject> worldObjects { get; set; }
        

        Dictionary<Color, Color> colorReplacements { get; set; }
    }
}
