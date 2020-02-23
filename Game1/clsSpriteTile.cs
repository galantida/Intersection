using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using tileWorld;
using gameLogic;


namespace Game1
{
    public class clsSpriteTile
    {
        public intTile tile;
        public Texture2D texture;
        public Vector2 screenLocation; // screen location
        public Vector2 origin; // origin of texture rotation


        public clsSpriteTile(intTile tile, Dictionary<string, Texture2D>  textures)
        {
            this.tile = tile;
            screenLocation = new Vector2(0, 0);
            origin = new Vector2(32, 32);
            this.texture = textures[this.tile.textureName];
        }
    }
}
