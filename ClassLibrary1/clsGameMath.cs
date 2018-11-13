using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace gameLogic
{
    public static class clsGameMath
    {
        public static Vector2 toVector(float rotation)
        {
            return new Vector2((float)Math.Sin(rotation), -(float)Math.Cos(rotation));
        }

        public static float toRotation(Vector2 direction)
        {
            return (float)(Math.Atan2(direction.Y, direction.X) + 1.5708f);
        }

        public static Vector2 Rotate(this Vector2 v, float radians)
        {
            double sin = Math.Sin(radians);
            double cos = Math.Cos(radians);

            double tx = v.X;
            double ty = v.Y;

            return new Vector2((float)(cos * tx - sin * ty), (float)(sin * tx + cos * ty));
        }
    }
}
