using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Game1
{
    public class clsLine
    {
        public Vector2 point1 { get; set; }
        public Vector2 point2 { get; set; }
        public Color color { get; set; }
        public int thickness { get; set; }

        public clsLine(Vector2 point1, Vector2 point2, Color color, int thickness)
        {
            this.point1 = point1;
            this.point2 = point2;
            this.color = color;
            this.thickness = thickness;
        }
    }
}
