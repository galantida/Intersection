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
        public clsWorldTile tile;
        public Texture2D texture;
        public Vector2 location; // screen location
        public float rotation; // texture rotation
        public Vector2 origin; // origin of texture rotation


        public clsSpriteTile(clsWorldTile tile, Dictionary<string, Texture2D>  textures)
        {
            this.tile = tile;

            location = new Vector2(0, 0);
            rotation = 0f;
            origin = new Vector2(0, 0);

            // determine origin and rotation
            if (tile.directions.Count == 1)
            {
                // simple road
                if (tile.directions[0].X == 1)
                {
                    // east
                    rotation = 3.14f;
                    origin = new Vector2(64, 64);
                }
                else if (tile.directions[0].X == -1)
                {
                    // west
                    rotation = 0f;
                    origin = new Vector2(0, 0);
                }
                else if (tile.directions[0].Y == 1)
                {
                    // south
                    rotation = 4.71f;
                    origin = new Vector2(64, 0);
                }
                else
                {
                    // north
                    rotation = 1.57f;
                    origin = new Vector2(0, 64);
                }

            }

            this.texture = textures[this.tile.textureName];
        }

        public void draw(clsDisplay display, SpriteBatch spriteBatch)
        {
            //this.location = display.screenLocation + (new Vector2(x * 64, y * 64)) * display.scale;
            spriteBatch.Draw(texture, this.location, null, Color.White, rotation, origin, display.scale, SpriteEffects.None, 1);
        }
    }
}
