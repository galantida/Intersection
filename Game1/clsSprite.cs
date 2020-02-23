using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using gameLogic;
using physicalWorld;
using tileWorld;

namespace Game1
{
    public class clsSprite
    {
        public Vector2 screenLocation = new Vector2(0, 0);
        public float rotation = 0;
        public float scale = 1;
        public Vector2 origin = new Vector2(32, 32);

        public intObject worldObject;
        public Texture2D baseTexture;
        public Texture2D spriteTexture;
        public Rectangle sourceTileArea;
        

        public clsSprite(intObject worldObject, Texture2D texture)
        {
            // passed
            this.worldObject = worldObject;
            this.baseTexture = texture;
            this.spriteTexture = new Texture2D(this.baseTexture.GraphicsDevice, this.baseTexture.Width, this.baseTexture.Height);

            // calculated
            this.sourceTileArea = new Rectangle(0, 0, this.baseTexture.Width, this.baseTexture.Height);
        }

        public void updateSpriteTexture()
        {
            // get texture as a strip of color values
            Color[] data = new Color[baseTexture.Width * baseTexture.Height];
            baseTexture.GetData(data);
            for (int i = 0; i < data.Length; i++)
            {
                // loop over color replacements
                foreach (KeyValuePair<Color, Color> kvp in worldObject.colorReplacements)
                {
                    // review each pixel and replace based on color replacements
                    Color source = new Color(data[i].R, data[i].G, data[i].B);
                    if (source == kvp.Key)
                    {
                        data[i] = kvp.Value;
                    }
                }
            }

            // apply colors to new texture
            this.spriteTexture.SetData<Color>(data);
        }

        public void dispose()
        {
            this.spriteTexture.Dispose();
        }
    }
}
