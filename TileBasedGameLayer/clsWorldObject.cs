using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tileWorld;
using Microsoft.Xna.Framework;
using physicalWorld;

namespace tileWorld
{
    //public enum WorldObjectType { entry, exit, car };

    public class clsWorldObject : clsNewtonObject
    {
        public string typeName { get; set; }
        public string textureName { get; set; }

        // interface fields
        public Vector2 direction { get; set; }

        public CollisionType collisionType { get; set; }

        public Dictionary<Color, Color> colorReplacements { get; set; }

        protected float lastUpdated { get; set; }

        public clsWorldObject(string textureName, Vector2 location, Vector2 direction, Vector2 velocity, float mass = 1000.0f) : base(location, velocity, mass)
        {
            this.textureName = textureName;
            this.direction = direction;
            colorReplacements = new Dictionary<Color, Color>();
        }

        

    }
}
