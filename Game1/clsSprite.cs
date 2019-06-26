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
        public intGamePiece gamePiece;
        public Texture2D texture;
        public Rectangle sourceTileArea;
        public Vector2 location = new Vector2(0, 0);
        public float rotation = 0;
        public float scale = 1;
        public Vector2 origin = new Vector2(32,32);


        public clsSprite(intGamePiece gamePiece, Texture2D texture)
        {
            // passed
            this.gamePiece = gamePiece;
            this.texture = texture;

            // calculated
            this.sourceTileArea = new Rectangle(0, 0, this.texture.Width, this.texture.Height);
        }

        public void draw(clsDisplay display, SpriteBatch spriteBatch)
        {
            // mimic game piece
            Vector2 displayLocation = new Vector2(display.displayArea.X, display.displayArea.Y);

            location = (displayLocation + gamePiece.location) * display.scale;
            rotation = clsGameMath.toRotation(gamePiece.heading);
            float scale = display.scale * this.scale;

            // draw
            spriteBatch.Draw(this.texture, this.location, new Rectangle(0 , 0, this.texture.Width, this.texture.Height), Color.White, this.rotation, this.origin, scale, SpriteEffects.None, 1);
        }
    }
}
