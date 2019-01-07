using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace gameLogic
{
    public class clsNewton
    {
        public Vector2 location { get; set; }
        public Vector2 velocity { get; set; }
        public float mass { get; set; } // for acceleration and friction calucations
        public float surfaceArea { get; set; } // for wind resistance calulcations
        public float kineticFrictionCoefficient { get; set; }
        public float kineticFriction { get; set; }
        public float staticFrictionCoefficient { get; set; }
        public float staticFriction { get; set; }
        public float dragCoefficient { get; set; }
        public float drag { get; set; }

        protected Stopwatch stopWatch = new Stopwatch();

        public clsNewton(Vector2 location, Vector2 velocity, float mass, float surfaceArea = 1)
        {
            this.location = location;
            this.velocity = velocity;
            this.mass = mass;
            this.surfaceArea = surfaceArea;
            this.kineticFrictionCoefficient = 0.4f; // rubber on dry pavment
            this.staticFrictionCoefficient = 0.5f; // rubber on dry pavment
            this.dragCoefficient = 0.47f; // air a sea level
        }

        public void update(Vector2 addedForce, float addedDragCoefficent = 0, float addedFrictionCoefficient = 0)
        {
            float deltaTime = stopWatch.ElapsedMilliseconds;
            if (deltaTime > 10)
            {
                float weight = this.weight();
                bool moving = true;

                // is it already moving
                if (velocity.Length() == 0)
                {
                    // did we apply enough force to get it moving
                    if (addedForce.Length() <= staticFrictionResistance(addedFrictionCoefficient)) moving = false;
                }

                if (moving)
                {
                    accelerate(addedForce);
                    decelerate(airResistance(addedDragCoefficent));
                    decelerate(kineticFrictionResistance(addedFrictionCoefficient));


                    location += velocity * deltaTime;
                }
                stopWatch.Restart(); // reset update timer
            }
        }

        public float weight(float gravity = 1) // assume earth
        {
            // mass - kg
            return mass * gravity;
        }

        public void accelerate(Vector2 force)
        {
            velocity += Vector2.Divide(force, mass);
        }

        public void decelerate(float force)
        {
            Vector2 direction = new Vector2(velocity.X, velocity.Y);
            direction.Normalize();
            direction *= force;
            velocity += Vector2.Divide(-direction, mass);
        }

        public float airResistance(float density = 1.225f, float addedDragoefficient = 0)
        {
            // denisty of material - 1.225 kg air at sea level
            // surfaceArea = units?
            // dragCoeffecient - cube:1.05, sphere:0.47, half sphere:0.42, cone:0.50, steamline:0.04;
            drag = (velocity.Length() * velocity.Length()) * ((density * surfaceArea * dragCoefficient) / 2);
            return drag;
        }

        public float staticFrictionResistance(float gravity = 1, float addedFrictionCoefficient = 0)
        {
            // ice & wood (.05)
            // ice & steel (.03)
            // rubber on concrete (.60-.85)
            // rubber on wet concrete (.45-.75)
            staticFriction = weight(1) * (this.staticFrictionCoefficient + addedFrictionCoefficient);
            return staticFriction;
        }

        public float kineticFrictionResistance(float gravity = 1, float addedFrictionCoefficient = 0)
        {
            // rubber on ice (.15)
            kineticFriction = velocity.Length() * (weight(1) * (kineticFrictionCoefficient + addedFrictionCoefficient));
            return kineticFriction;
        }
    }
}
