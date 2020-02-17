using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace tileWorld
{
    public interface intObject
    {
        string typeName { get; set; }
        string textureName { get; set; }

        Vector2 location { get; set; }
        Vector2 direction { get; set; }
        Dictionary<Color, Color> colorReplacements { get; }
        bool colorsUpdated { get; }

        CollisionType collisionType { get; set; }

        void update(float currentTime);
    }
}
