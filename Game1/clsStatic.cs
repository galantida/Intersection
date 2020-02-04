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
    public class clsStaticSprite
    {
        public clsTile square;
        public Texture2D texture;
        public Vector2 location = new Vector2(0, 0); // screen location
        public float rotation = 0;
        public Vector2 origin = new Vector2(0, 0); // origin of rotation


        public clsStaticSprite(clsTile square, Dictionary<string, Texture2D>  textures)
        {
            this.square = square;

            rotation = 0f;

            // determine origin and rotation
            if (square.directions.Count == 1)
            {
                // simple road
                if (square.directions[0].X == 1)
                {
                    // east
                    rotation = 3.14f;
                    origin = new Vector2(64, 64);
                }
                else if (square.directions[0].X == -1)
                {
                    // west
                    rotation = 0f;
                    origin = new Vector2(0, 0);
                }
                else if (square.directions[0].Y == 1)
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

            // static section
            if (square.directions.Count() == 0)
            {
                this.texture = textures["grass"];
            }
            else if (square.directions.Count() == 1)
            {
                // directional statics
                this.texture = textures["road"];
            }
            else
            {
                // multidirectional statics
                this.texture = textures["intersection"];
            }
        }

        public void draw(clsDisplay display, SpriteBatch spriteBatch)
        {
            //this.location = display.screenLocation + (new Vector2(x * 64, y * 64)) * display.scale;
            spriteBatch.Draw(texture, this.location, null, Color.White, rotation, origin, display.scale, SpriteEffects.None, 1);
        }
    }
}
