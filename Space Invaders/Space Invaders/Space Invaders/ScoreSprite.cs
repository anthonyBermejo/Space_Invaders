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
    /// This is a game component that implements IUpdateable and IDrawable
    /// 
    /// Authors - Anthony Bermejo, Venelin Koulaxazov, Patrick Nicoll
    /// Version - 12/12/2013 - v1.1
    /// </summary>
    public class ScoreSprite : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private int score;
        private int highScore;
        private int lives;
        private int extraLifeAccumulator; // Resets to 0 after reaching a certain point amount, giving the player an extra life
        private bool gameOver = false;
        private bool endOfGame = false; // When true the Game Over Menu will continue to show
        private SpriteFont font;
        private SpriteBatch spriteBatch;
        private Texture2D imagePlayer;
        private Game1 game;
        private AlienSquad alienSquad;
        private BombFactory bomb;
        private GraphicsDeviceManager graphics;
        private int screenHeight;
        private int screenWidth;
        private HighScore highScoreObject;

        //Constructor
        public ScoreSprite(Game1 game, BombFactory bomb, LaserFactory laser, AlienSquad alienSquad)
            : base(game)
        {
            this.game = game;
            this.bomb = bomb;
            graphics = game.getGraphicsDeviceManager();
            bomb.playerCollision += removeLives;
            laser.AlienCollision += setScore;
            laser.MothershipCollision += setScore;
            alienSquad.GameOver += setGameOver;
            screenHeight = graphics.PreferredBackBufferHeight;
            screenWidth = graphics.PreferredBackBufferWidth;
            lives = 3;
            extraLifeAccumulator = 0;
            this.alienSquad = alienSquad;
            highScoreObject = new HighScore();
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load all of the content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = game.Content.Load<SpriteFont>("scoreFont");
            imagePlayer = game.Content.Load<Texture2D>("playerLife");
            base.LoadContent();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            if (game.GetGameState() == Game1.GameState.Playing)
                ;
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw(GameTime gameTime)
        {
            int lifeSpacingX = screenWidth - 3 * (imagePlayer.Width + 5);
            highScore = highScoreObject.ReadHighScore();
            if (game.GetGameState() == Game1.GameState.Playing)
            {
                 spriteBatch.Begin();

           
                if (!gameOver)
                {
                    spriteBatch.DrawString(font, "SCORE: " + score, new Vector2(0, 0), Color.White);
                    spriteBatch.DrawString(font, "LIVES: ", new Vector2(screenWidth - 190, 0), Color.White);
                    spriteBatch.DrawString(font, "HIGH SCORE: " + highScore, new Vector2(310, 0), Color.White);
                    spriteBatch.DrawString(font, "LEVEL: " + alienSquad.getLevel(), new Vector2(0, 20), Color.White);

                    spriteBatch.Draw(imagePlayer, new Vector2(lifeSpacingX, 0), Color.White);

                    // If player has more than 3 lives the display changes to compress the display instead of having multiple little pictures
                    if (lives > 3)
                    {
                        spriteBatch.DrawString(font, " x " + lives, new Vector2(screenWidth - 85, 0), Color.White);
                    }
                    else
                    {

                        for (int i = 0; i < (lives - 1); i++)
                        {
                            spriteBatch.Draw(imagePlayer, new Vector2(lifeSpacingX + imagePlayer.Width + 5, 0), Color.White);
                            lifeSpacingX += imagePlayer.Width + 5;
                        }
                    }
                }
                else
                {
                    if (score > highScore && endOfGame)
                    {
                        highScoreObject.WriteHighScore(score);       
                    }
                    endOfGame = true;//So the Game Over Menu will show
                    game.SetGameState(Game1.GameState.GameOverMenu);
                }
                

                spriteBatch.End();
            }
            base.Draw(gameTime);
        }

        public bool GetGameOver()
        {
            return gameOver;
        }

        /// <summary>
        /// Sets the gameOver status to true.
        /// </summary>
        private void setGameOver()
        {
            gameOver = true;
        }

        /// <summary>
        /// Removes a life of the counter when the player dies.
        /// </summary>
        /// <param name="player">The player object</param>
        /// <param name="points">There are no points assigned in this method</param>
        private void removeLives(DrawableGameComponent player, int points)
        {
            lives -= 1;
            bomb.SpawnProtection();
            ((PlayerSprite)player).ResetPosition();

            if (lives == 0)
                gameOver = true;
        }

        /// <summary>
        /// Returns the highscore
        /// </summary>
        /// <returns>Returns the highscore</returns>
        public int GetHighScore()
        {
            return highScore;
        }

        /// <summary>
        /// Returns the socre of the current game.
        /// </summary>
        /// <returns>Score of the game</returns>
        public int GetScore()
        {
            return score;
        }

        /// <summary>
        /// Keeps track of the player's score. Also gives the player extra lives once they reach a certain amount of points.
        /// </summary>
        /// <param name="killedAlien">The alien that was killed</param>
        /// <param name="points">The value of the alien in points when it gets killed</param>
        private void setScore(DrawableGameComponent killedAlien, int points)
        {
            score += points;
            extraLifeAccumulator += points;

            if ((extraLifeAccumulator / 5000) >= 1)
            {
                extraLifeAccumulator -= 5000;
                lives++;
            }
        }
    }
}
