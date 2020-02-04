using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace gameLogic
{
    public interface intWorldObject
    {
        WorldObjectType worldObjectType { get; set; }
        void update(float currentTime);

        Vector2 location { get; set; }
        Vector2 direction { get; set; }
        Dictionary<Color, Color> colorReplacements { get; set; }
    }
}
