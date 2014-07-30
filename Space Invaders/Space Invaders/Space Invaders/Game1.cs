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
    /// Version - 28/07/2014 - v1.3
    /// </summary>
    /// 

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        //instance variable declaration
        private GraphicsDeviceManager graphics;
        public GamerServicesComponent GamerServices;
        private MainMenu mainMenu;
        private GameOverMenu gameOverMenu;
        private Starfield starfield;
        private SpriteBatch spriteBatch;
        private PlayerSprite playerSprite;
        private AlienSquad alienSquad;
        private MothershipSprite mothershipSprite;
        private SoundEffect pauseSound;
        private SoundEffectInstance pauseSoundInstance;
        private LaserFactory laser;
        private BombFactory bomb;
        private ScoreSprite score;
        private int screenWidth;
        private int screenHeight;
        private bool paused = false;
        private bool pauseKeyDown = false;
        private bool pausedForGuide = false;
        private SpriteFont font;

        public enum GameState
        {
            MainMenu,
            Paused,
            Playing,
            GameOverMenu,
            Exit
        }
        GameState currentGameState = GameState.MainMenu;

        //constructor
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 700;
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
            setWindowPosition();

            //Creates the main menu
            mainMenu = new MainMenu(this);

            //Creates the game over menu
            gameOverMenu = new GameOverMenu(this);

            //Generates the starfield
            starfield = new Starfield(this, screenHeight);

            laser = new LaserFactory(this, graphics.PreferredBackBufferHeight);
            playerSprite = new PlayerSprite(this, laser);
            bomb = new BombFactory(this, graphics.PreferredBackBufferHeight, playerSprite);
            mothershipSprite = new MothershipSprite(this, laser);
            alienSquad = new AlienSquad(this, screenWidth, screenHeight, bomb, laser, mothershipSprite, playerSprite);
            score = new ScoreSprite(this, bomb, laser, alienSquad);
            laser.SetAlienSquad(alienSquad);
            laser.SetMothership(mothershipSprite);

            Components.Add(new GamerServicesComponent(this));
            Components.Add(starfield);
            Components.Add(mainMenu);
            Components.Add(gameOverMenu);
            

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
            pauseSound = this.Content.Load<SoundEffect>("pauseSound");
            pauseSoundInstance = pauseSound.CreateInstance();
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || currentGameState == GameState.Exit)
                this.Exit();

            // Updating playing state
            switch (currentGameState)
            {
                case GameState.Playing:
                    {
                        
                        break;
                    }
                case GameState.MainMenu:
                    {
                        break;
                    }

                case GameState.Paused:
                    {
                        //alienSquad.setAlienSoundState(SoundState.Stopped);
                        break;
                    }

                case GameState.GameOverMenu: 
                    {
                        mothershipSprite.setMothershipSoundState(SoundState.Stopped);
                        alienSquad.setAlienSoundState(SoundState.Stopped);
                        score.setExtraLifeSoundState(SoundState.Stopped);
                        break; 
                    }

            }

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
        /// Acts as a handler to the GetHighScore method in ScoreSprite
        /// </summary>
        /// <returns>Highscore of the game</returns>
        public int GetGameHighScore()
        {
            return score.GetHighScore();
        }

        /// <summary>
        /// Acts as a handler to the GetScore method in ScoreSprite
        /// </summary>
        /// <returns>Score of the current game</returns>
        public int GetGameScore()
        {
            return score.GetScore();
        }

        /// <summary>
        /// Returns the gameState from a set of possible enums
        /// </summary>
        /// <returns>The current state of the game</returns>
        public GameState GetGameState()
        {
            return currentGameState; 
            }

        public void SetGameState(GameState gs)
        {
            currentGameState = gs;
        }

        public void writeHighScore(string initials, int gameScore)
        {
            score.writeHighScore(initials, gameScore);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            switch (currentGameState)
            {
                case GameState.Playing:
                    {
                        break;
                    }
                case GameState.MainMenu:
                    {
                        break;
                    }
            }

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
            SetGameState(GameState.Paused);
            pauseSoundInstance.Play();
            
            //Pauses sound effects
            if(mothershipSprite.getMothershipSoundState() == SoundState.Playing)
                mothershipSprite.setMothershipSoundState(SoundState.Playing);
            if (alienSquad.getAlienSoundState() == SoundState.Playing)
                alienSquad.setAlienSoundState(SoundState.Playing);
            if (score.getExtraLifeSoundState() == SoundState.Playing)
                score.setExtraLifeSoundState(SoundState.Playing);

            //removeComponents();
        }

        private void EndPause()
        {
            pausedForGuide = false;
            paused = false;
            pauseSoundInstance.Stop();

            //Resumes sound effects
            if (mothershipSprite.getMothershipSoundState() == SoundState.Paused)
                mothershipSprite.setMothershipSoundState(SoundState.Paused);
            if (alienSquad.getAlienSoundState() == SoundState.Paused)
                alienSquad.setAlienSoundState(SoundState.Paused);
            if (score.getExtraLifeSoundState() == SoundState.Paused)
                score.setExtraLifeSoundState(SoundState.Paused);

            SetGameState(GameState.Playing);
            //Components.Add(laser);
            //Components.Add(bomb);
            //Components.Add(playerSprite);
            //Components.Add(alienSquad);
            //Components.Add(mothershipSprite);
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

        public void restartGame()
        {
            score.resetGame();
            playerSprite.resetGame();
            mothershipSprite.resetGame();
            alienSquad.resetGame();
            laser.resetGame();
            SetGameState(GameState.Playing);
            
        }

        /// <summary>
        /// Set inital window position of game
        /// </summary>
        private void setWindowPosition()
        {
            int xPosition = 0;
            int yPosition = 0;

            // get width and height of screen/monitor
            int monitorWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            int monitorHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

            // center the game window 
            xPosition = (monitorWidth / 2) - (screenWidth / 2);
            yPosition = 50;

            // set new game window location
            System.Windows.Forms.Form form = (System.Windows.Forms.Form)System.Windows.Forms.Control.FromHandle(Window.Handle);
            form.Location = new System.Drawing.Point(xPosition, yPosition);
        }

    }
}
