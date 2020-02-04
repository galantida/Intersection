using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using physicalWorld;
using Microsoft.Xna.Framework;


namespace tileWorld
{
    // the physical piece which is unaware of its location on the board
    

    public class clsTileObject : clsNewtonObject
    {
        // interface fields
        public Vector2 direction { get; set; }

        public CollisionType collisionType { get; set; }

        public Dictionary<Color, Color> colorReplacements { get; set; }

        protected float lastUpdated { get; set; }

        public clsTileObject(Vector2 location, Vector2 direction, Vector2 velocity, float mass = 1000.0f):base(location, velocity, mass)
        {
            this.direction = direction;
        }

        public new void update(float currentTime)
        {
            base.update(currentTime);
        }
    }
}
