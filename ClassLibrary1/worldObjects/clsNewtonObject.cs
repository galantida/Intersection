using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace gameLogic
{
    /*
     * All the physical properties and laws that would apply to any object in the real world
     * their defaults as well as their current modified status
     * 
     */
    public class clsNewtonObject
    {
        public Vector2 location { get; set; } // current position
        public Vector2 velocity { get; set; } // direction and magnitude
        public float mass { get; set; } // for acceleration and friction calucations
        public float surfaceArea { get; set; } // for wind resistance calulcations
        public clsNewtonianProperty staticFrictionCoefficient { get; }
        public clsNewtonianProperty kineticFrictionCoefficient { get; }
        public clsNewtonianProperty dragCoefficient { get; }

        public float lastProcessed { get; set; }

        public clsNewtonObject(Vector2 location, Vector2 velocity, float mass, float surfaceArea = 50.0f, float kineticFrictionCoefficient = 0.8f /* rubber on asphalt */, float staticFrictionCoefficient = 0.9f /* rubber on asphalt */, float dragCoefficient = 0.25f /* .25-.45 car in air at sea level */)
        {
            this.location = location;
            this.velocity = velocity;
            this.mass = mass;
            this.surfaceArea = surfaceArea;
            this.kineticFrictionCoefficient = new clsNewtonianProperty(kineticFrictionCoefficient);
            this.staticFrictionCoefficient = new clsNewtonianProperty(staticFrictionCoefficient);
            this.dragCoefficient = new clsNewtonianProperty(dragCoefficient);
        }

        public void update(float currentTime)
        {
            float deltaTime = currentTime - lastProcessed; // detemine how long since the last processing
            lastProcessed = currentTime; // set new last processing time

            // process world forces (possibly only if it is moving)
            if (velocity.Length() > 0)
            {
                decelerate(airResistance() * deltaTime); // friction that increases with speed
                decelerate(kineticFrictionResistance(kineticFrictionCoefficient.totalValue * deltaTime)); // friction that is basically constant
            }

            // calculate changes
            location += velocity * deltaTime; // process base on time since last processing.
        }


        /**************************************** 
            Object Modifier Functions
        ****************************************/
        # region Object Modifier Functions
        // apply force in any direction
        private void accelerate(Vector2 force)
        {
            velocity += Vector2.Divide(force, mass);
        }

        // apply force, opposite direction of travel
        private void decelerate(float force)
        {
            Vector2 direction = new Vector2(velocity.X, velocity.Y);
            direction.Normalize();
            direction *= force;
            velocity += Vector2.Divide(-direction, mass);
        }
        # endregion


        public void applyForce(Vector2 addedForce)
        {
            float weight = this.weight();
            bool moving = true;

            // if its not moving
            if (velocity.Length() == 0)
            {
                // did we apply enough force to get it moving
                if (addedForce.Length() <= staticFrictionResistance())
                {
                    moving = false;
                }
            }

            if (moving)
            {
                accelerate(addedForce);
            }
        }

        public float weight(float gravity = 1) // assume earths gravity
        {
            // mass - kg
            return mass * gravity;
        }


        /**************************************** 
            Calculation Functions
        ****************************************/
        # region Calculation Functions
        private float airResistance(float density = 1.225f)
        {
            // denisty of material - 1.225 kg air at sea level
            // surfaceArea = units?
            // dragCoeffecient - cube:1.05, sphere:0.47, half sphere:0.42, cone:0.50, steamline:0.04;
            float drag = (velocity.Length() * velocity.Length()) * ((density * surfaceArea * dragCoefficient.totalValue) / 2);
            return drag;
        }

        private float staticFrictionResistance(float gravity = 1)
        {
            // ice & wood (.05)
            // ice & steel (.03)
            // rubber on concrete (.60-.85)
            // rubber on wet concrete (.45-.75)
            float staticFriction = weight(1) * this.staticFrictionCoefficient.totalValue;
            return staticFriction;
        }

        private float kineticFrictionResistance(float gravity = 1)
        {
            // rubber on ice (.15)
            float kineticFriction = velocity.Length() * (weight(1) * kineticFrictionCoefficient.totalValue);
            return kineticFriction;
        }
        # endregion
    }
}
