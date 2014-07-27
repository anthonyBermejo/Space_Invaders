using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace Space_Invaders
{
    /// <summary>
    /// This is a game component that implements IUpdateable and IDrawable
    /// Represents a PlayerSprite which is a wrapper class of the Player class.
    /// 
    /// Authors - Anthony Bermejo, Venelin Koulaxazov, Patrick Nicoll
    /// Version - 26/07/2014 - v1.1
    /// </summary>
    public class PlayerSprite : Microsoft.Xna.Framework.DrawableGameComponent
    {
        // instance variable declaration
        private Player player;
        private SpriteBatch spriteBatch;
        private Texture2D imagePlayer;
        private Game1 game;
        private LaserFactory laser;
        private KeyboardState oldState;
        private TimeSpan previousKeyTime = new TimeSpan();
        private TimeSpan tolerance = TimeSpan.FromMilliseconds(50);

        // Constructor
        public PlayerSprite(Game1 game, LaserFactory laser)
            : base(game)
        {
            this.laser = laser;
            this.game = game;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            oldState = Keyboard.GetState();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load all of the content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            imagePlayer = game.Content.Load<Texture2D>("player");
            player = new Player(imagePlayer.Height, imagePlayer.Width, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, 5F);
            base.LoadContent();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            if (game.GetGameState() == Game1.GameState.Playing)
            {
                checkInput(gameTime);    
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values</param>
        public override void Draw(GameTime gameTime)
        {
            if (game.GetGameState() == Game1.GameState.Playing)
            {
                spriteBatch.Begin();
                spriteBatch.Draw(imagePlayer, player.GetPosition(), Color.White);
                spriteBatch.End();
            }
            base.Draw(gameTime);
        }

        // Custom methods.

        /// <summary>
        /// The method takes care of the key input and will be used for the movement of the ship as well as launching the lasers.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values</param>
        public void checkInput(GameTime gameTime)
        {
            KeyboardState newState = Keyboard.GetState();

            if (newState.IsKeyDown(Keys.Right) || newState.IsKeyDown(Keys.D))
            {
                if (!oldState.IsKeyDown(Keys.Right))
                {
                    player.MoveRight();
                    previousKeyTime = gameTime.TotalGameTime;
                }
                else
                    if (gameTime.TotalGameTime - previousKeyTime > tolerance)
                        player.MoveRight();
            }
            else if (newState.IsKeyDown(Keys.Left) || newState.IsKeyDown(Keys.A))
            {
                if (!oldState.IsKeyDown(Keys.Left))
                {
                    player.MoveLeft();
                    previousKeyTime = gameTime.TotalGameTime;
                }
                else
                    if (gameTime.TotalGameTime - previousKeyTime > tolerance)
                        player.MoveLeft();
            }

            if (newState.IsKeyDown(Keys.Space))
            {
                laser.Launch(player.GetBoundary(), gameTime);
            }
        }

        /// <summary>
        /// Calls the Player class' GetBoundary method.
        /// </summary>
        /// <returns>The boundary of the ship</returns>
        public Rectangle GetBoundary()
        {
            return player.GetBoundary();
        }

        /// <summary>
        /// Calls the Player class' MoveLeft method.
        /// </summary>
        public void MoveLeft()
        {
            player.MoveLeft();
        }

        /// <summary>
        /// Calls the Player class' MoveRight method.
        /// </summary>
        public void MoveRight()
        {
            player.MoveRight();
        }

        /// <summary>
        /// Calls the Player class' ResetPosition method.
        /// </summary>
        public void ResetPosition()
        {
            player.ResetPosition();
        }

        public void resetGame()
        {
            ResetPosition();
        }
    }
}