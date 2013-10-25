
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
    /// This is a game component that implements IUpdateable and IDrawable.
    /// Represents an AlienSprite which is a wrapper class of the Alien class.
    /// 
    /// Authors - Anthony Bermejo, Venelin Koulaxazov, Patrick Nicoll
    /// Version - 25/10/2012 - v1.01
    /// </summary>
    public class AlienSprite : Microsoft.Xna.Framework.DrawableGameComponent
    {
        // instance variable declaration
        private Alien alien;
        private int difficulty; // Difficulty level
        private int hitPoints; // Alien hitpoints based on difficulty level
        private AlienType alienType; // Type of Alien based on row number in AlienSquad array
        private SpriteBatch spriteBatch;
        private Texture2D imageAlien;
        private Game game;

        // Constructor
        public AlienSprite(Game game, Texture2D imageAlien, int difficulty)
            : base(game)
        {
            this.game = game;
            this.imageAlien = imageAlien;
            this.difficulty = difficulty;
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
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            hitPoints = difficulty; //Assigned for clarity
            alien = new Alien(imageAlien.Width, imageAlien.Height, GraphicsDevice.Viewport.Height, GraphicsDevice.Viewport.Width, hitPoints);
            base.LoadContent();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(imageAlien, alien.GetPosition(), Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
        }

        // Custom methods.

        /// <summary>
        /// Calls the Alien class' GetAlienState method.
        /// </summary>
        /// <returns>an enum representing one of two possible alien states</returns>
        public AlienState GetAlienState()
        {
            return alien.GetAlienState();
        }

        /// <summary>
        /// Returns the type of Alien using an enum
        /// </summary>
        /// <returns>Alien type enum</returns>
        public AlienType GetAlienType()
        {
            return alienType;
        }

        public void SetAlienType(AlienType type)
        {
            alienType = type;
        }

        /// <summary>
        /// Calls the Alien class' GetBoundary method.
        /// </summary>
        /// <returns>The boundary of the Alien.</returns>
        public Rectangle GetBoundary()
        {
            return alien.GetBoundary();
        }

        /// <summary>
        /// Calls the Alien class' GetHitpoints method
        /// </summary>
        /// <returns>Alien's current hitpoints</returns>
        public int GetHitPoints()
        {
            return alien.GetHitPoints();
        }

        public Vector2 GetPosition()
        {
            return alien.GetPosition();
        }

        /// <summary>
        /// Calls the Alien class' Move method
        /// </summary>
        /// <param name="dir">an enum representing one of 3 possible alien directions of movement.</param>
        public void Move(Direction dir)
        {
            alien.Move(dir);
        }

        /// <summary>
        /// Calls the Alien class' SetAlienState method.
        /// </summary>
        /// <param name="state">The alien state.</param>
        public void SetAlienState(AlienState state)
        {
            alien.SetAlienState(state);
        }

        /// <summary>
        /// Calls the Alien class' SetHitpoints method.
        /// </summary>
        /// <param name="hp"></param>
        public void SetHitPoints(int hp)
        {
            alien.SetHitPoints(hp);
        }

        /// <summary>
        /// Calls the Alien class' SetPosition method
        /// </summary>
        /// <param name="x">x coordinate of the Alien instance</param>
        /// <param name="y">y coordinate of the Alien instance</param>
        public void SetPosition(int x, int y)
        {
            alien.SetPosition(x, y);
        }

        /// <summary>
        /// Allows for the rotation of images for the alien, simulating movement
        /// </summary>
        /// <param name="texture">The new image</param>
        public void SetTexture(Texture2D texture)
        {
            imageAlien = texture;
        }

        /// <summary>>
        /// Calls the Alien class' TryMove method.
        /// </summary
        /// 
        ///<param name="dir">an enum representing one of 3 possible alien directions of movement</param>
        ///<returns>True if the alien can move , otherwise returns false</returns>
        public bool TryMove(Direction dir)
        {
            return alien.TryMove(dir);
        }
    }
}
