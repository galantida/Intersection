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
    public enum CollisionType { None, Spherical }

    public class clsObject : clsNewtonObject
    {
        public string typeName { get; set; }
        public string textureName { get; set; }

        // interface fields
        public Vector2 direction { get; set; }

        public CollisionType collisionType { get; set; }

        private Dictionary<Color, Color> _colorReplacements { get; set; }

        public bool colorsUpdated { get; set; }

        protected float lastUpdated { get; set; }

        public clsObject(string textureName, Vector2 location, Vector2 direction, Vector2 velocity, float mass = 1000.0f) : base(location, velocity, mass)
        {
            this.textureName = textureName;
            this.direction = direction;
            _colorReplacements = new Dictionary<Color, Color>();
            this.colorsUpdated = false;
        }

        public void colorReplace(Color originalColor, Color? newColor = null)
        {
            this.colorsUpdated = true;
            if (_colorReplacements.TryGetValue(originalColor, out Color currentColor)) // Returns true.
            {
                if (newColor == null)
                {
                    // remove existing replacement
                    _colorReplacements.Remove(originalColor);
                }
                else
                {
                    // update exsiting replacement
                    currentColor = (Color)newColor;
                }
            }
            else
            {
                // add new replacement
                if (newColor != null) _colorReplacements.Add(originalColor, (Color)newColor);
            }
        }

        public Dictionary<Color, Color> colorReplacements
        {
            get
            {
                this.colorsUpdated = false;
                return _colorReplacements;
            }
        }
    }
}
