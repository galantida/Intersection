using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace physicalWorld
{
    public static class VectorMath
    {
        public static float toRotation(Vector2 direction)
        {
            // shortest roation off of X axis
            return (float)Math.Atan2(direction.Y, direction.X); // not used yet
        }

        public static float crossProduct(Vector2 vector1, Vector2 vector2)
        {
            return vector1.X * vector2.Y - vector1.Y * vector2.X;
        }

        public static float angleBetween(Vector2 vectorA, Vector2 vectorB)
        {
            Vector vector1 = new Vector(vectorA.X, vectorA.Y);
            Vector vector2 = new Vector(vectorB.X, vectorB.Y);
            float angleBetween = (float)Vector.AngleBetween(vector1, vector2);
            return angleBetween;
        }
    }
}
