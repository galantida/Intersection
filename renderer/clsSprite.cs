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

namespace renderer
{
    public class clsSprite
    {
        public Vector2 displayLocation { get; set; }
        public float rotation { get; set; }
        public float scale { get; set; }
        public Vector2 origin { get; set; } 

        public intObject worldObject { get; set; }
        public Texture2D baseTexture { get; set; }
        public Texture2D spriteTexture { get; set; }
        public Rectangle sourceTileArea { get; set; }

        public clsSprite(intObject worldObject, Texture2D texture)
        {
            // passed
            this.worldObject = worldObject;
            this.baseTexture = texture;
            this.spriteTexture = new Texture2D(this.baseTexture.GraphicsDevice, this.baseTexture.Width, this.baseTexture.Height);

            // initialized
            this.displayLocation = new Vector2(0, 0);
            this.scale = 1;
            this.origin = new Vector2(32, 32);
            this.rotation = 0;

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
