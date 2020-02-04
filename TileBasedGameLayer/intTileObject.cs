using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace tileWorld
{
    public interface intTileObject
    {
        Dictionary<Color, Color> colorReplacements { get; set; }

        void update(float currentTime);



    }
}
