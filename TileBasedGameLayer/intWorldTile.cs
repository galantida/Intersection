using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace gameLogic
{
    public interface intWorldTile
    {
        string typeName { get; set; }
        string textureName { get; set; }

        List<Vector2> directions { get; set; }

        Dictionary<Color, Color> colorReplacements { get; set; }


    }
}
