using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using gameLogic;

namespace Game1
{
    public class clsSprite
    {
        public intWorldObject worldObject;
        public Texture2D texture;
        public Rectangle sourceTileArea;
        public Vector2 location = new Vector2(0, 0);
        public float rotation = 0;
        public float scale = 1;
        public Vector2 origin = new Vector2(32,32);
        


        public clsSprite(intWorldObject worldObject, Texture2D texture)
        {
            // passed
            this.worldObject = worldObject;
            this.texture = texture;

            // calculated
            this.sourceTileArea = new Rectangle(0, 0, this.texture.Width, this.texture.Height);
        }

        public void draw(clsDisplay display, SpriteBatch spriteBatch)
        {
            // mimic game piece
            Vector2 displayLocation = new Vector2(display.displayArea.X, display.displayArea.Y);

            location = (displayLocation + worldObject.location) * display.scale;
            rotation = clsGameMath.toRotation(worldObject.direction);
            float scale = display.scale * this.scale;

            // color replacements
            Color[] data = new Color[texture.Width * texture.Height];
            texture.GetData(data);
            for (int i = 0; i < data.Length; i++)
            {
                foreach (KeyValuePair<Color, Color> kvp in worldObject.colorReplacements) {
                    Color source = new Color(data[i].R, data[i].G, data[i].B);
                    if (source == kvp.Key)
                    {
                        data[i] = kvp.Value;
                    }
                }
            }

            // apply colors to new texture
            Texture2D updatedTexture = new Texture2D(this.texture.GraphicsDevice, this.texture.Width, this.texture.Height);
            updatedTexture.SetData<Color>(data);


            // draw
            spriteBatch.Draw(updatedTexture, this.location, new Rectangle(0 , 0, this.texture.Width, this.texture.Height), Color.White, this.rotation, this.origin, scale, SpriteEffects.None, 1);
        }
    }
}
