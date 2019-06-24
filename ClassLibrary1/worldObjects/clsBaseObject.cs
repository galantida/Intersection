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

    public abstract class clsBaseGameObject : clsNewtonObject
    {
        // interface fields
        public GamePieceType gamePieceType { get; set; }
        public clsWorld world { get; set; }
        protected float lastUpdated { get; set; }

        public clsBaseGameObject(clsWorld world, Vector2 location, Vector2 velocity, float mass = 1000.0f):base(location, velocity, mass)
        {
            this.world = world; // reference to the world it exists in
        }

        public void update()
        {
            base.update(world.currentTime); // always process the objects physics
        }
    }
}
