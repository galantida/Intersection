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
    class clsWindow
    {
        // settings
        Vector2 size;
        GraphicsDeviceManager graphics;

        List<clsCamera> cameras;
        List<clsDisplay> displays;

        public Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();
        public Dictionary<string, SpriteFont> fonts = new Dictionary<string, SpriteFont>();

        public clsWindow(GraphicsDeviceManager graphics, Vector2 size, int tileSize, clsRoadWorld world)
        {
            this.graphics = graphics;
            this.size = size;
           
            this.graphics.PreferredBackBufferWidth = (int)size.X;  // set this value to the desired width of your window
            this.graphics.PreferredBackBufferHeight = (int)size.Y;   // set this value to the desired height of your window
            this.graphics.ApplyChanges();

            cameras = new List<clsCamera>();
            clsCamera camera = new clsCamera(world, new Rectangle(0,0,1000,1000));
            cameras.Add(camera);

            displays = new List<clsDisplay>();
            //clsDisplay display = new clsDisplay(camera, new Rectangle(50, 50, (int)this.size.X-50, (int)this.size.Y-50), 2.0f, textures, fonts);
            clsDisplay display = new clsDisplay(camera, new Rectangle(0, 0, (int)this.size.X, (int)this.size.Y), textures, fonts);
            displays.Add(display);
        }

        public void update()
        {
            foreach (clsCamera camera in cameras)
            {
                camera.update();
            }

            foreach (clsDisplay display in displays)
            {
                display.update();
            }
        }

        public void draw()
        {
            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);
            SpriteBatch spriteBatch = new SpriteBatch(graphics.GraphicsDevice);
            spriteBatch.Begin();
            foreach (clsDisplay display in displays)
            {
                display.draw(spriteBatch);
            }
            spriteBatch.End();
        }

       
    }
}
