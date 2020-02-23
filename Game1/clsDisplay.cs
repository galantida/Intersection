using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using gameLogic;
using tileWorld;
using physicalWorld;

namespace Game1
{
    public class clsDisplay
    {
        // display is a portlet used to display the contents of one or more cameras

        public clsCamera camera;
        public Rectangle displayArea { get; set; }
        public float scale { get; set; }

        // display Objects
        List<clsSprite> sprites = new List<clsSprite>();
        clsSpriteTile[,] spriteTiles = new clsSpriteTile[0,0];
        

        // content
        public Dictionary<string, Texture2D> textures;
        public Dictionary<string, SpriteFont> fonts;

        public clsDisplay(Rectangle displayArea, clsCamera camera, Dictionary<string, Texture2D> textures, Dictionary<string, SpriteFont> fonts)
        {
            this.displayArea = displayArea;
            this.camera = camera;
            
            this.textures = textures;
            this.fonts = fonts;

            // determine display scale up or down of camera content
            if (this.displayArea.Width > camera.visibleArea.Width) this.scale = camera.visibleArea.Width / this.displayArea.Width;
            else this.scale = this.displayArea.Width / camera.visibleArea.Width;
        }

        public void update()
        {
            spriteManagement();
            staticSpriteManagement();
        }

        // **********************************************************************
        // drawing element management
        public void staticSpriteManagement()
        {
            float spacing = 64.0f * this.scale;

            Rectangle visibleTileArea = camera.visibleTileArea;

            spriteTiles = new clsSpriteTile[visibleTileArea.Right - visibleTileArea.Left, visibleTileArea.Bottom - visibleTileArea.Top];
            for (int x = visibleTileArea.Left; x < visibleTileArea.Right; x++)
            {
                for (int y = visibleTileArea.Top; y < visibleTileArea.Bottom; y++)
                {
                    spriteTiles[x, y] = new clsSpriteTile(camera.world.tiles[x, y], textures);
                    spriteTiles[x, y].screenLocation = new Vector2(this.displayArea.X, this.displayArea.Y) + new Vector2(x * spacing, y * spacing);
                }
            }
        }

        public void spriteManagement()
        {
            // get all the world objects in the display area
            List<intObject> displayedWorldObjects = new List<intObject>();
            foreach (intObject gp in camera.visableObjects)
            {
                // could be logic here to determine what is in the displayable area
                displayedWorldObjects.Add(gp);
            }

            // remove sprites that represent gamepieces nolonger in the display area
            for (int s = 0; s < sprites.Count; s++)
            {
                bool found = false;
                foreach (intObject worldObject in camera.visableObjects)
                {
                    if (sprites[s].worldObject == worldObject)
                    {
                        found = true;
                        break;
                    }
                }

                // remove sprites whos pair world object is nolonger in the display area
                if (!found)
                {
                    sprites[s].dispose();
                    sprites.Remove(sprites[s]);
                }
            }


            // remove world objects already pair with sprites
            foreach (clsSprite sprite in sprites)
            {
                displayedWorldObjects.Remove(sprite.worldObject);
            }

            // the remaining world objects are not yet paired with a sprite and need to be
            clsSprite newSprite;
            foreach (intObject worldObject in displayedWorldObjects)
            {
                newSprite = new clsSprite(worldObject, textures[worldObject.textureName]);
                sprites.Add(newSprite);
            }
        }

        // **********************************************************************
        // complex drawing methods
        public void draw(SpriteBatch spriteBatch)
        {
            drawStaticSprites(spriteBatch);
            drawSprites(spriteBatch);
            drawLines(spriteBatch);
            drawOverlay(spriteBatch);
            drawBorder(spriteBatch, textures["pixel"], new Rectangle((int)this.displayArea.X, (int)this.displayArea.Y, (int)this.displayArea.Width, (int)this.displayArea.Height), 4, Color.White);
        }
        
        private void drawStaticSprites(SpriteBatch spriteBatch)
        {
            float halfTile = 32.0f * this.scale;
            foreach (clsSpriteTile spriteTile in spriteTiles)
            {
                // get adjusted location
                Vector2 adjustedLocation = new Vector2(spriteTile.screenLocation.X + halfTile, spriteTile.screenLocation.Y + halfTile);
                spriteBatch.Draw(spriteTile.texture, adjustedLocation, null, Color.White, spriteTile.tile.textureRotation, spriteTile.origin, this.scale, SpriteEffects.None, 1);
            }
        }


        private void drawSprites(SpriteBatch spriteBatch)
        {
            for (int s = 0; s < sprites.Count; s++)
            {
                // get top left of display
                Vector2 displayLocation = new Vector2(this.displayArea.X, this.displayArea.Y);

                // manipulate sprite to represent world object properties
                sprites[s].screenLocation = (displayLocation + sprites[s].worldObject.location) * this.scale;
                sprites[s].rotation = clsGameMath.toRotation(sprites[s].worldObject.direction);
                float scale = this.scale * sprites[s].scale;

                // update custom colors
                if (sprites[s].worldObject.colorsUpdated) sprites[s].updateSpriteTexture();

                // draw
                spriteBatch.Draw(sprites[s].spriteTexture, sprites[s].screenLocation, new Rectangle(0, 0, sprites[s].spriteTexture.Width, sprites[s].spriteTexture.Height), Color.White, sprites[s].rotation, sprites[s].origin, scale, SpriteEffects.None, 1);

            }
        }

        private void drawLines(SpriteBatch spriteBatch)
        {
            foreach (clsLine line in this.camera.lines)
            {
                DrawLine(spriteBatch, new Vector2(this.displayArea.X + line.point1.X * this.scale, this.displayArea.Y + line.point1.Y * this.scale), new Vector2(this.displayArea.X + line.point2.X * this.scale, this.displayArea.Y + line.point2.Y * this.scale), line.color, line.thickness);
            }
        }

        private void drawOverlay(SpriteBatch spriteBatch)
        {
            // screen over lay
            SpriteFont spriteFont = fonts["arial"];
            int y = 10;
            foreach (string textString in camera.text)
            {
                spriteBatch.DrawString(spriteFont, textString, new Vector2(10, y), Color.White);
                y += 16;
            }
        }

        private void drawBorder(SpriteBatch spriteBatch, Texture2D texture, Rectangle rectangleToDraw, int thicknessOfBorder, Color borderColor)
        {
            // Draw top, left, right then bottom lines
            spriteBatch.Draw(texture, new Rectangle(rectangleToDraw.X, rectangleToDraw.Y, rectangleToDraw.Width, thicknessOfBorder), borderColor);
            spriteBatch.Draw(texture, new Rectangle(rectangleToDraw.X, rectangleToDraw.Y, thicknessOfBorder, rectangleToDraw.Height), borderColor);
            spriteBatch.Draw(texture, new Rectangle((rectangleToDraw.X + rectangleToDraw.Width - thicknessOfBorder), rectangleToDraw.Y, thicknessOfBorder, rectangleToDraw.Height), borderColor);
            spriteBatch.Draw(texture, new Rectangle(rectangleToDraw.X, rectangleToDraw.Y + rectangleToDraw.Height - thicknessOfBorder, rectangleToDraw.Width, thicknessOfBorder), borderColor);
        }

        // **********************************************************************
        // line drawing methods
        private static Texture2D _texture;
        private static Texture2D GetTexture(SpriteBatch spriteBatch)
        {
            if (_texture == null)
            {
                _texture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
                _texture.SetData(new[] { Color.White });
            }

            return _texture;
        }

        public static void DrawLine(SpriteBatch spriteBatch, Vector2 point, float length, float angle, Color color, float thickness = 1f)
        {
            var origin = new Vector2(0f, 0.5f);
            var scale = new Vector2(length, thickness);
            spriteBatch.Draw(GetTexture(spriteBatch), point, null, color, angle, origin, scale, SpriteEffects.None, 0);
        }
        public static void DrawLine(SpriteBatch spriteBatch, Vector2 point1, Vector2 point2, Color color, float thickness = 1f)
        {
            float distance = Vector2.Distance(point1, point2);
            float angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
            DrawLine(spriteBatch, point1, distance, angle, color, thickness);
        }


    }
}
