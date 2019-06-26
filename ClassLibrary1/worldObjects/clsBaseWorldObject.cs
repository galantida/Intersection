using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace gameLogic
{
    // the physical piece which is unaware of its location on the board

    public abstract class clsBaseWorldObject : clsNewtonObject
    {
        // interface fields
        public WorldObjectType worldObjectType { get; set; }
        public clsWorld world { get; set; }
        public Vector2 direction { get; set; }
        public bool collisionDetection { get; set; }
        public Dictionary<Color, Color> colorReplacements { get; set; }

        protected float lastUpdated { get; set; }

        public clsBaseWorldObject(clsWorld world, Vector2 location, Vector2 direction, Vector2 velocity, float mass = 1000.0f):base(location, velocity, mass)
        {
            this.world = world; // reference to the world it exists in
            this.direction = direction;

            colorReplacements = new Dictionary<Color, Color>();
        }

        public void update()
        {
            base.update(world.currentTime); // always process the objects physics
        }
    }
}
