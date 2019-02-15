using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace AdventuresOfAlfloog
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public enum GameStates
        {
            InGame,
            MainMenu,
            HighScore,
            Credits,
            Exit
        }

        public static GameStates GameState = GameStates.MainMenu;
#if DEBUG
        public static SpriteFont DebugFont;
#endif

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            HighScore.Initilize();

            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            graphics.PreferMultiSampling = true;
            graphics.ApplyChanges();
            IsFixedTimeStep = false;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

#if DEBUG
            DebugFont = Content.Load<SpriteFont>(@"Fonts/Debug");
#endif
            Main.LoadContent(Content, GraphicsDevice.Viewport);
            InGame.LoadContent(Content, GraphicsDevice.Viewport);
            HighScore.LoadContent(Content, GraphicsDevice.Viewport);
            Credits.LoadContent(Content, GraphicsDevice.Viewport);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.End))
                this.Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.F))
            {
                graphics.IsFullScreen = !graphics.IsFullScreen;
                graphics.ApplyChanges();
            }

            switch (GameState)
            {
                case GameStates.InGame:
                    if(IsMouseVisible) IsMouseVisible = !IsMouseVisible; // set mouse not visible
                    InGame.Update(gameTime);
                    break;
                case GameStates.MainMenu:
                    if (!IsMouseVisible) IsMouseVisible = !IsMouseVisible; // set mouse visible
                    Main.Update();
                    break;
                case GameStates.HighScore:
                    if (!IsMouseVisible) IsMouseVisible = !IsMouseVisible; // set mouse visible
                    HighScore.Update();
                    break;
                case GameStates.Credits:
                    if (!IsMouseVisible) IsMouseVisible = !IsMouseVisible; // set mouse visible
                    Credits.Update(gameTime);
                    break;
                case GameStates.Exit:
                    this.Exit();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            InGame.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            
            GraphicsDevice.Clear(Color.Black);
            switch (GameState)
            {
                case GameStates.InGame:
                    InGame.Draw(spriteBatch);
                    break;
                case GameStates.MainMenu:
                    Main.Draw(spriteBatch);
                    break;
                case GameStates.HighScore:
                    HighScore.Draw(spriteBatch);
                    break;
                case GameStates.Credits:
                    Credits.Draw(spriteBatch);
                    break;
                case GameStates.Exit:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            base.Draw(gameTime);
        }
    }
}
