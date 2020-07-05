using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tileWorld;
using Microsoft.Xna.Framework;

namespace tileWorld
{
    public class clsTile
    {
        // visible properties
        public string textureName { get; set; }
        public Vector2 location { get; set;  } // world location
        public float rotation { get; set; } // actual rotation of the tile from its default
        public Dictionary<Color, Color> colorReplacements { get; set; }
        public List<Vector2> directions { get; set; }

        // processing properties
        public bool collisionDetection { get; set; } // will store a reference to objects that are on this tile
        public List<intObject> worldObjects { get; set; } // objects on this tile
        
        // privates 
        private float _defaultTextureRotation; // if tile rotation is zero what should the default texture roation be?

        // Notes
        // other than direction what properties would a tile have
        // height? impead movment? slippery? temperature?

        public float textureRotation
        {
            get
            {
                return (rotation + _defaultTextureRotation);
            }
        }


        public clsTile(string textureName, bool collisionDetection, float defaultTextureRotation = 0)
        {
            this.textureName = textureName;
            this.collisionDetection = collisionDetection;
            _defaultTextureRotation = defaultTextureRotation;
        }

        
    }
}
