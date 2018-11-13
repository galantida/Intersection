using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gameEngine
{
    

    public class clsSquare
    {
        public bool paved;
        public Direction flowDirection;

        public clsSquare(Direction flowDirection, bool paved)
        {
            this.flowDirection = flowDirection;
            this.paved = paved;
        }
    }
}
