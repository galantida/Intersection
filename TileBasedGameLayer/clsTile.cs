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
        public string typeName { get; set; }
        public string textureName { get; set; }
        public float rotation { get; set; } // actual rotation of the tile from its default
        private float _defaultTextureRotation; // if tile rotation is zero what should the default texture roation be?
        public bool collisionDetection { get; set; }
        public List<intObject> worldObjects { get; set; }

        public Dictionary<Color, Color> colorReplacements { get; set; }

        // other than direction what properties would a tile have
        // height? impead movment? slippery? temperature?

        public clsTile(string typeName, string textureName, bool collisionDetection, float defaultTextureRotation = 0)
        {
            this.typeName = typeName;
            this.textureName = textureName;
            this.collisionDetection = collisionDetection;
            _defaultTextureRotation = defaultTextureRotation;
        }

        public float textureRotation
        {
            get
            {
                return (rotation + _defaultTextureRotation);
            }
        }
    }
}
