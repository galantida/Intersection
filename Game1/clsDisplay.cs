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
        public Rectangle screenArea { get; set; }

        // display Objects
        List<clsSprite> sprites = new List<clsSprite>();
        clsSpriteTile[,] spriteTiles = new clsSpriteTile[0,0];
        public List<clsLine> lines { get; set; } // lines above the tiles but below the objects


        // content
        public Dictionary<string, Texture2D> textures;
        public Dictionary<string, SpriteFont> fonts;

        public clsDisplay(Rectangle screenArea, clsCamera camera, Dictionary<string, Texture2D> textures, Dictionary<string, SpriteFont> fonts)
        {
            this.screenArea = screenArea;
            this.camera = camera;
            
            this.textures = textures;
            this.fonts = fonts;
        }

        public void update()
        {
            spriteManagement();
            spriteTileManagement();
            lineManagement();

        }

        public Vector2 getDisplayCoordinate(Vector2 worldCoordinate)
        {
            Vector2 cameraCoordinate = this.camera.getCameraCoordinate(worldCoordinate);
            return cameraCoordinate * this.scale;
        }

        public float scale
        {
            get
            {
                return (float)this.screenArea.Width / (float)camera.visibleArea.Width;
            }
        }

        // **********************************************************************
        // drawing element management
        public void spriteTileManagement()
        {
            float spacing = this.camera.world.tileSize * this.scale;

            // cache and not calculate everytime
            Rectangle visibleTileArea = camera.visibleTileArea;

            // define array size for visible tiles
            spriteTiles = new clsSpriteTile[visibleTileArea.Right - visibleTileArea.Left, visibleTileArea.Bottom - visibleTileArea.Top];

            // loop through world tiles
            for (int x = visibleTileArea.Left; x <= visibleTileArea.Right; x++)
            {
                for (int y = visibleTileArea.Top; y <= visibleTileArea.Bottom; y++)
                {
                    // make sure this tile is in the generated world
                    if ((x>=0) && (x < camera.world.tiles.GetUpperBound(0)) && (y >= 0) && (y < camera.world.tiles.GetUpperBound(1)))
                    {
                        // convert world tile coordinates to zero index display coordinates
                        int spriteTileX = x - visibleTileArea.Left;
                        int spriteTileY = y - visibleTileArea.Top;

                        // make sure we are inside the array defined above
                        if ((spriteTileX < spriteTiles.GetUpperBound(0)) && spriteTileY < spriteTiles.GetUpperBound(1))
                        {
                            // populate display sprites from camera visible tiles in the world
                            spriteTiles[spriteTileX, spriteTileY] = new clsSpriteTile(camera.world.tiles[x, y], textures);
                            spriteTiles[spriteTileX, spriteTileY].displayLocation = new Vector2(this.screenArea.X, this.screenArea.Y) + new Vector2(spriteTileX * spacing, spriteTileY * spacing);
                        }
                    }
                }
            }
        }

        public void spriteManagement()
        {
            // get all the world objects in the display area
            List<intObject> displayedWorldObjects = new List<intObject>();
            foreach (intObject gp in camera.visibleObjects)
            {
                displayedWorldObjects.Add(gp);
            }

            // remove sprites that represent gamepieces nolonger in the display area
            for (int s = 0; s < sprites.Count; s++)
            {
                bool found = false;
                foreach (intObject worldObject in camera.visibleObjects)
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

        public void lineManagement()
        {
            lines = new List<clsLine>();
            foreach (intActor driver in this.camera.world.actors)
            {
                clsDriverAI driverAI = driver as clsDriverAI;   // Here is where I get typeof(AI)
                if (driverAI != null)
                {
                    if (driverAI.route != null)
                    {
                        Vector2 lastWaypointWorldLocation = driverAI.car.location;
                        for (int t = driverAI.route.currentWaypointIndex; t < driverAI.route.waypoints.Count; t++)
                        {
                            Vector2 thisWaypointWorldLocation = this.camera.world.tileCoordinateToWorldLocation(driverAI.route.waypoints[t]);
                            if (this.camera.isInVisibleArea(thisWaypointWorldLocation))
                            {
                                lines.Add(new clsLine(lastWaypointWorldLocation, thisWaypointWorldLocation, driverAI.car.color, 4));
                            }
                            lastWaypointWorldLocation = thisWaypointWorldLocation;
                        }
                    }
                }
            }
        }

        // **********************************************************************
        // complex drawing methods
        public void draw(SpriteBatch spriteBatch)
        {
            drawSpriteTiles(spriteBatch);
            drawLines(spriteBatch);
            drawSprites(spriteBatch);
            drawOverlay(spriteBatch);
            drawBorder(spriteBatch, textures["pixel"], new Rectangle((int)this.screenArea.X, (int)this.screenArea.Y, (int)this.screenArea.Width, (int)this.screenArea.Height), 4, Color.White);
        }
        
        private void drawSpriteTiles(SpriteBatch spriteBatch)
        {
            Vector2 displayLocation = new Vector2(this.screenArea.X, this.screenArea.Y);
            float adjustment = 32.0f * this.scale;
            Vector2 halfTile = new Vector2(adjustment, adjustment);
            foreach (clsSpriteTile spriteTile in spriteTiles)
            {
                if (spriteTile != null) // tile array is created based on camera coverage even if there is no world tile to see
                {
                    // get adjusted location
                    spriteTile.displayLocation = this.getDisplayCoordinate(spriteTile.tile.location) + halfTile;
                    spriteBatch.Draw(spriteTile.texture, displayLocation + spriteTile.displayLocation, null, Color.White, spriteTile.tile.textureRotation, spriteTile.origin, this.scale, SpriteEffects.None, 1);
                } 
            }
        }


        private void drawSprites(SpriteBatch spriteBatch)
        {
            // get top left of display
            Vector2 displayLocation = new Vector2(this.screenArea.X, this.screenArea.Y);

            for (int s = 0; s < sprites.Count; s++)
            {
                // manipulate sprite to represent world object properties
                sprites[s].displayLocation = this.getDisplayCoordinate(sprites[s].worldObject.location);
                sprites[s].rotation = clsGameMath.toRotation(sprites[s].worldObject.direction);
                float scale = this.scale * sprites[s].scale;

                // update custom colors
                if (sprites[s].worldObject.colorsUpdated) sprites[s].updateSpriteTexture();

                // draw
                spriteBatch.Draw(sprites[s].spriteTexture, displayLocation + sprites[s].displayLocation, new Rectangle(0, 0, sprites[s].spriteTexture.Width, sprites[s].spriteTexture.Height), Color.White, sprites[s].rotation, sprites[s].origin, scale, SpriteEffects.None, 1);

            }
        }

        

        private void drawLines(SpriteBatch spriteBatch)
        {
            // get top left of display
            Vector2 displayLocation = new Vector2(this.screenArea.X, this.screenArea.Y);
            foreach (clsLine line in this.lines)
            {
                // transform line from world coordinates to screen coordinates
                DrawLine(spriteBatch, displayLocation + this.getDisplayCoordinate(line.point1), displayLocation + this.getDisplayCoordinate(line.point2), line.color, line.thickness * this.scale);
            }
        }

        private void drawOverlay(SpriteBatch spriteBatch)
        {
            // screen over lay
            SpriteFont spriteFont = fonts["arial"];
            int y = 10;
            spriteBatch.DrawString(spriteFont, "Zoom Scale:" + this.scale, new Vector2(10, y), Color.White);
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
