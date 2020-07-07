using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace tileWorld
{
    public interface intActor
    {
        void update(float currentTime);
        clsRoute route { get; set; }
        clsObject worldObject { get; set; }
        bool yielding { get; set; }


    }
}
