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

namespace Space_Invaders
{
    /// <summary>
    /// This is the main type for your game
    /// 
    /// Authors - Anthony Bermejo, Venelin Koulaxazov, Patrick Nicoll
    /// Version - 25/05/2013
    /// </summary>
    /// 

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        //instance variable declaration
        private GraphicsDeviceManager graphics;
        public GamerServicesComponent GamerServices;
        private SpriteBatch spriteBatch;
        private PlayerSprite playerSprite;
        private AlienSquad alienSquad;
        private MothershipSprite mothershipSprite;
        private LaserFactory laser;
        private BombFactory bomb;
        private ScoreSprite score;
        private int screenWidth;
        private int screenHeight;
        private bool paused = false;
        private bool pauseKeyDown = false;
        private bool pausedForGuide = false;
        private SpriteFont font;

        //constructor
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 630;
            this.screenWidth = graphics.PreferredBackBufferWidth;
            this.screenHeight = graphics.PreferredBackBufferHeight;
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
            laser = new LaserFactory(this, graphics.PreferredBackBufferHeight);
            playerSprite = new PlayerSprite(this, laser);
            bomb = new BombFactory(this, graphics.PreferredBackBufferHeight, playerSprite);
            mothershipSprite = new MothershipSprite(this, laser);
            alienSquad = new AlienSquad(this, screenWidth, screenHeight, bomb, laser, mothershipSprite);
            score = new ScoreSprite(this, bomb, laser, alienSquad);
            laser.SetAlienSquad(alienSquad);
            laser.SetMothership(mothershipSprite);

            Components.Add(new GamerServicesComponent(this));
            Components.Add(laser);
            Components.Add(bomb);
            Components.Add(playerSprite);
            Components.Add(mothershipSprite);
            Components.Add(alienSquad);
            Components.Add(score);

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
            font = this.Content.Load<SpriteFont>("scoreFont");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
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
            KeyboardState keyboardState = Keyboard.GetState();

            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // Check to see if the user has paused or unpaused
            if (!score.GetGameOver())
            {
                checkPauseKey(keyboardState);
                checkPauseGuide();
            }

            // If the user hasn't paused, Update normally
            if (!paused)
            {
                base.Update(gameTime);
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            if (paused)
            {
                spriteBatch.Begin();
                spriteBatch.DrawString(font, "||   PAUSED   ||", new Vector2((screenWidth / 2) - 85, screenHeight / 2 - 10), Color.White);
                spriteBatch.End();
            }

            base.Draw(gameTime);
        }

        private void BeginPause(bool UserInitiated)
        {
            paused = true;
            pausedForGuide = !UserInitiated;
            removeComponents();
        }

        private void EndPause()
        {
            pausedForGuide = false;
            paused = false;
            Components.Add(laser);
            Components.Add(bomb);
            Components.Add(playerSprite);
            Components.Add(alienSquad);
            Components.Add(mothershipSprite);
        }

        private void checkPauseKey(KeyboardState keyboardState)
        {
            bool pauseKeyDownThisFrame = keyboardState.IsKeyDown(Keys.P);
            // If key was not down before, but is down now, we toggle the
            // pause setting
            if (!pauseKeyDown && pauseKeyDownThisFrame)
            {
                if (!paused)
                    BeginPause(true);
                else
                    EndPause();
            }
            pauseKeyDown = pauseKeyDownThisFrame;
        }

        private void checkPauseGuide()
        {
            // Pause if the Guide is up
            if (!paused && Guide.IsVisible)
                BeginPause(false);
            // If we paused for the guide, unpause if the guide
            // went away
            else if (paused && pausedForGuide && !Guide.IsVisible)
                EndPause();
        }

        /// <summary>
        /// Returns graphics 
        /// </summary>
        /// <returns></returns>
        public GraphicsDeviceManager getGraphicsDeviceManager()
        {
            return graphics;
        }

        /// <summary>
        /// Removes components
        /// </summary>
        public void removeComponents()
        {
            Components.Remove(laser);
            Components.Remove(bomb);
            Components.Remove(playerSprite);
            Components.Remove(alienSquad);
            Components.Remove(mothershipSprite);
        }
    }
}