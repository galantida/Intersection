using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace gameLogic
{
    class clsNewton
    {
        public clsNewton()
        {

        }

        public static Vector2 velocity(Vector2 force, Vector2 velocity, float mass, float surfaceArea)
        {
            Vector2 newVelocity = new Vector2(0,0);
            float weight = clsNewton.weight(mass);
            bool moving = true;

            // is it already moving
            if (velocity.Length() == 0)
            {
                // has enough force been applied to get it moving
                if (force.Length() <= staticFrictionResistance(weight)) moving = false;
            }

            if (moving)
            {
                newVelocity = (velocity + acceleration(mass, force)) - airResistance(newVelocity, surfaceArea) - kineticFrictionResistance(newVelocity, weight); 
            }

            return newVelocity;
        }

        public static float weight(float mass, float gravity = 1) // assume earth
        {
            // mass - kg
            return mass * gravity;
        }

        public static Vector2 acceleration(float mass, Vector2 force)
        {
            return Vector2.Divide(force, mass);
        }

        public static Vector2 airResistance(Vector2 velocity, float surfaceArea, float density = 1.225f, float dragCoefficent = 0.47f)
        {
            // denisty of material - 1.225 kg air at sea level
            // surfaceArea = units?
            // dragCoeffecient - cube:1.05, sphere:0.47, half sphere:0.42, cone:0.50, steamline:0.04;
            return (velocity * velocity) * ((density * surfaceArea * dragCoefficent) / 2);
        }

        public static float staticFrictionResistance(float weight, float staitcFrictionCoefficient = 0.5f)
        {
            // ice & wood (.05)
            // ice & steel (.03)
            // rubber on concrete (.60-.85)
            // rubber on wet concrete (.45-.75)
            return weight * staitcFrictionCoefficient;
        }

        public static Vector2 kineticFrictionResistance(Vector2 velocity, float weight, float kineticFrictionCoefficient = 0.4f)
        {
            // rubber on ice (.15)
            return velocity * (weight * kineticFrictionCoefficient);
        }
    }
}
