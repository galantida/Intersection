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
        public Vector2 location; // screen location
        //public float rotation; // texture rotation
        public Vector2 origin; // origin of texture rotation


        public clsSpriteTile(intTile tile, Dictionary<string, Texture2D>  textures)
        {
            this.tile = tile;

            location = new Vector2(0, 0);
            //rotation = 0f;
            //origin = new Vector2(0, 0);
            origin = new Vector2(32, 32);

            this.texture = textures[this.tile.textureName];
        }

        public void draw(clsDisplay display, SpriteBatch spriteBatch)
        {
            // adjusted rotation
            Vector2 adjustedLocation = new Vector2(this.location.X + 32, this.location.Y + 32);

            //this.location = display.screenLocation + (new Vector2(x * 64, y * 64)) * display.scale;
            spriteBatch.Draw(texture, adjustedLocation, null, Color.White, tile.textureRotation, origin, display.scale, SpriteEffects.None, 1);
        }
    }
}
