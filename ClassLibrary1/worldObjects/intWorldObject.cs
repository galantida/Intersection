using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace gameLogic
{
    public enum WorldObjectType { entry, exit, car };

    public interface intWorldObject
    {
        WorldObjectType worldObjectType { get; set; }
        Vector2 location { get; set; }
        //Vector2 squareCoordinate { get; }
        Vector2 velocity { get; set; }
        float mass { get; set; }
        Vector2 direction { get; set; }
        bool collisionDetection { get; set; }
        Dictionary<Color, Color> colorReplacements { get; set; }

        void update();
    }
}
