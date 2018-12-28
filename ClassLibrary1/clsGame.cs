using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace gameLogic
{
    public class clsGame
    {
        public clsWorld world;

        public clsGame(clsInput input)
        {
            world = new clsWorld(14, 64, input);
        }

        public void update()
        {
            world.update();
        }
    }
}
