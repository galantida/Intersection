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
    class clsScreen
    {
        // settings
        Vector2 size;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        List<clsCamera> cameras;
        List<clsDisplay> displays;

        public Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();
        public Dictionary<string, SpriteFont> fonts = new Dictionary<string, SpriteFont>();

        public clsScreen(GraphicsDeviceManager graphics, Vector2 size, int tileSize, clsWorld world)
        {
            this.graphics = graphics;
            this.size = size;
           
            this.graphics.PreferredBackBufferWidth = (int)size.X;  // set this value to the desired width of your window
            this.graphics.PreferredBackBufferHeight = (int)size.Y;   // set this value to the desired height of your window
            this.graphics.ApplyChanges();
            spriteBatch = new SpriteBatch(graphics.GraphicsDevice);

            cameras = new List<clsCamera>();
            clsCamera camera = new clsCamera(world, new Vector2(0, 0), new Vector2(100,100));
            //cameras.Add(new clsCamera(game, new Vector2(0, 0), new Vector2(100, 600), 0.5f));
            cameras.Add(camera);

            displays = new List<clsDisplay>();
            clsDisplay display = new clsDisplay(camera, new Rectangle(50, 50, 800, 800), 1, textures, fonts);
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
            spriteBatch.Begin();
            foreach (clsDisplay display in displays)
            {
                display.draw(spriteBatch);
            }
            spriteBatch.End();
        }

       
    }
}
