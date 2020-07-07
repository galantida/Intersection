using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using gameLogic;

namespace renderer
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class clsGame : Game
    {
        // global graphic opbjects
        clsWindow window;
        clsRoadWorld roadWorld;

        public clsGame()
        {
            roadWorld = new clsRoadWorld(28, 64); // 64 pixels is 15 feet
            window = new clsWindow(new GraphicsDeviceManager(this), new Vector2(1536, 1024), roadWorld);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // TODO: use this.Content to load your game content here
            // Create a new SpriteBatch, which can be used to draw textures.
            Content.RootDirectory = "Content";

            window.textures.Add("pixel", new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color));
            window.textures["pixel"].SetData(new[] { Color.White });

            window.textures.Add("grass", Content.Load<Texture2D>("grass"));
            window.textures.Add("road", Content.Load<Texture2D>("road"));
            window.textures.Add("shoulder", Content.Load<Texture2D>("shoulder"));
            window.textures.Add("intersection", Content.Load<Texture2D>("intersection"));
            window.textures.Add("car", Content.Load<Texture2D>("car"));
            window.textures.Add("gear", Content.Load<Texture2D>("gear"));
            window.textures.Add("entry", Content.Load<Texture2D>("entry"));
            window.textures.Add("exit", Content.Load<Texture2D>("exit"));

            window.fonts.Add("arial", Content.Load<SpriteFont>("File"));
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape)) Exit();

            float currentTime = gameTime.ElapsedGameTime.Milliseconds;

            roadWorld.update(currentTime); // base these updates on game time
            window.update(currentTime); // base these updates on game time

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            window.draw();
            base.Draw(gameTime);
        }
    }
}
