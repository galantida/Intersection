using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using gameLogic;

namespace Game1
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class clsGame : Game
    {
        // global graphic opbjects
        clsScreen screen;
        clsHuman human;
        public clsWorld world;

        public clsGame()
        {
            human = new clsHuman();
            world = new clsWorld(14, 64, human);
            screen = new clsScreen(new GraphicsDeviceManager(this), new Vector2(1024, 1024), 64, world);
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

            screen.textures.Add("pixel", new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color));
            screen.textures["pixel"].SetData(new[] { Color.White });

            screen.textures.Add("grass", Content.Load<Texture2D>("grass"));
            screen.textures.Add("road", Content.Load<Texture2D>("road"));
            screen.textures.Add("shoulder", Content.Load<Texture2D>("shoulder"));
            screen.textures.Add("intersection", Content.Load<Texture2D>("intersection"));
            screen.textures.Add("car", Content.Load<Texture2D>("car"));
            screen.textures.Add("gear", Content.Load<Texture2D>("gear"));
            screen.textures.Add("entry", Content.Load<Texture2D>("entry"));
            screen.textures.Add("exit", Content.Load<Texture2D>("exit"));

            screen.fonts.Add("arial", Content.Load<SpriteFont>("File"));
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
            KeyBoardInput();
            world.update();
            screen.update();
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            screen.draw();
            base.Draw(gameTime);
        }

        private float lastMouseX = 0;
        private float lastMouseY = 0;


        public clsInput KeyBoardInput()
        {
            MouseState state = Mouse.GetState();

            float curMouseX = state.X;
            input.x = lastMouseX - state.X;
            lastMouseX = curMouseX;

            float curMouseY = state.Y;
            input.y = lastMouseY - state.Y;
            lastMouseY = curMouseY;

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape)) Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.Up)) input.forward = true;
            else input.forward = false;


            if (Keyboard.GetState().IsKeyDown(Keys.Down)) input.backward = true;
            else input.backward = false;


            if (Keyboard.GetState().IsKeyDown(Keys.Left)) input.left = true;
            else input.left = false;


            if (Keyboard.GetState().IsKeyDown(Keys.Right)) input.right = true;
            else input.right = false;

            if (Keyboard.GetState().IsKeyDown(Keys.N)) input.n = true;
            else input.n = false;

            return input;
        }

        
    }
}
