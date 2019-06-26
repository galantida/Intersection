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
    public class clsDisplay
    {
        public clsCamera camera;
        public Rectangle displayArea;
        public float scale;

        // camera views
        List<clsSprite> sprites = new List<clsSprite>();
        clsTile[,] tiles = new clsTile[0,0];

        // content
        public Dictionary<string, Texture2D> textures;
        public Dictionary<string, SpriteFont> fonts;

        public clsDisplay(clsCamera camera, Rectangle displayArea, Dictionary<string, Texture2D> textures, Dictionary<string, SpriteFont> fonts)
        {
            this.camera = camera;
            this.displayArea = displayArea;
            this.textures = textures;
            this.fonts = fonts;

            // determine scale up or down of camera content
            if (this.displayArea.Width > camera.visibleArea.Width) this.scale = this.displayArea.Width / camera.visibleArea.Width;
            else this.scale = camera.visibleArea.Width / this.displayArea.Width;
        }

        public void draw(SpriteBatch spriteBatch)
        {
            drawBorder(spriteBatch, textures["pixel"], displayArea, 1, Color.Gold);
            drawTiles(spriteBatch);
            drawSprites(spriteBatch);
            drawLines(spriteBatch);
            drawOverlay(spriteBatch);
        }

        public void update()
        {
            spriteManagement();
            tileManagement();
        }

        private void drawBorder(SpriteBatch spriteBatch, Texture2D texture, Rectangle rectangleToDraw, int thicknessOfBorder, Color borderColor)
        {
            // Draw top, left, right then bottom lines
            spriteBatch.Draw(texture, new Rectangle(rectangleToDraw.X, rectangleToDraw.Y, rectangleToDraw.Width, thicknessOfBorder), borderColor);
            spriteBatch.Draw(texture, new Rectangle(rectangleToDraw.X, rectangleToDraw.Y, thicknessOfBorder, rectangleToDraw.Height), borderColor);
            spriteBatch.Draw(texture, new Rectangle((rectangleToDraw.X + rectangleToDraw.Width - thicknessOfBorder), rectangleToDraw.Y, thicknessOfBorder, rectangleToDraw.Height), borderColor);
            spriteBatch.Draw(texture, new Rectangle(rectangleToDraw.X, rectangleToDraw.Y + rectangleToDraw.Height - thicknessOfBorder, rectangleToDraw.Width, thicknessOfBorder), borderColor);
        }

        private void drawSprites(SpriteBatch spriteBatch)
        {
            foreach (clsSprite sprite in sprites)
            {
                sprite.draw(this, spriteBatch);
            }
        }

        private void drawTiles(SpriteBatch spriteBatch)
        {
            foreach (clsTile tile in tiles)
            {
                tile.draw(this, spriteBatch);
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

        private void drawLines(SpriteBatch spriteBatch)
        {
            foreach (clsWaypointLine line in this.camera.lines)
            {
                DrawLine(spriteBatch, new Vector2(displayArea.Left + line.point1.X, displayArea.Top + line.point1.Y), new Vector2(displayArea.Left + line.point2.X, displayArea.Top + line.point2.Y), Color.AliceBlue, 2);
            }
        }

        public void spriteManagement()
        {
            // get all the game pieces in the display area
            List<intWorldObject> displayedWorldObjects = new List<intWorldObject>();
            foreach (intWorldObject gp in camera.worldObjects)
            {
                // could be logic here to determine what is in the displayable area
                displayedWorldObjects.Add(gp);
            }

            // remove sprites that represent gamepieces nolonger in the display area
            for (int s = 0; s < sprites.Count; s++)
            {
                bool found = false;
                foreach (intWorldObject worldObject in camera.worldObjects)
                {
                    if (sprites[s].worldObject == worldObject)
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    sprites.Remove(sprites[s]);
                }
            }


            // remove world objects from the list that are already paired with a sprite
            foreach (clsSprite sprite in sprites)
            {
                displayedWorldObjects.Remove(sprite.worldObject);
            }

            // the remaining game pieces are not yet paired and need to be
            clsSprite newSprite;
            foreach (intWorldObject worldObject in displayedWorldObjects)
            {
                switch (worldObject.worldObjectType)
                {
                    case WorldObjectType.car:
                        {
                            newSprite = new clsSprite(worldObject, textures["car"]);
                            sprites.Add(newSprite);
                            break;
                        }
                    case (WorldObjectType.entry):
                        {
                            newSprite = new clsSprite(worldObject, textures["entry"]);
                            sprites.Add(newSprite);
                            break;
                        }
                    case (WorldObjectType.exit):
                        {
                            newSprite = new clsSprite(worldObject, textures["exit"]);
                            sprites.Add(newSprite);
                            break;
                        }
                }
            }
        }


        public void tileManagement()
        {
            Vector2 displayLocation = new Vector2(this.displayArea.Left, this.displayArea.Top);
            float spacing = 64.0f * this.scale;

            tiles = new clsTile[camera.squares.GetLength(0), camera.squares.GetLength(1)];
            for (int x=0; x < camera.squares.GetLength(0); x++)
            {
                for (int y = 0; y < camera.squares.GetLength(0); y++)
                {
                    tiles[x, y] = new clsTile(camera.squares[x,y], textures);
                    tiles[x, y].location = displayLocation + new Vector2(x * spacing, y * spacing);
                }
            }
        }

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

        public static void DrawLine(SpriteBatch spriteBatch, Vector2 point1, Vector2 point2, Color color, float thickness = 1f)
        {
            float distance = Vector2.Distance(point1, point2);
            float angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
            DrawLine(spriteBatch, point1, distance, angle, color, thickness);
        }

        public static void DrawLine(SpriteBatch spriteBatch, Vector2 point, float length, float angle, Color color, float thickness = 1f)
        {
            var origin = new Vector2(0f, 0.5f);
            var scale = new Vector2(length, thickness);
            spriteBatch.Draw(GetTexture(spriteBatch), point, null, color, angle, origin, scale, SpriteEffects.None, 0);
        }

    }
}
