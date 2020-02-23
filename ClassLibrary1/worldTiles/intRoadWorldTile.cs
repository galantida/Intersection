using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace gameLogic
{
    public interface intRoadWorldTile
    {
        List<Vector2> directions { get; set; }

        float speedLimit { get; set; }
    }
}
