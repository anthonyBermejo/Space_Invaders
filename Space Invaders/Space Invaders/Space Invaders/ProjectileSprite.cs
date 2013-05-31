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
    /// Represents a ProjectileSprite that is a GUI wrapper class of a Projectile object.
    ///
    /// Authors - Anthony Bermejo, Venelin Koulaxazov, Patrick Nicoll
    /// Version - 29/03/2012
    /// </summary>
    public class ProjectileSprite : Microsoft.Xna.Framework.DrawableGameComponent
    {
        // instance variable declaration
        private Projectile projectile;
        private SpriteBatch spriteBatch;
        private Texture2D imageProjectile;
        private Game game;
        private bool laserOrBomb;
        private float speed;

        // constructor
        public ProjectileSprite(Game game, Texture2D imageProjectile, bool laserOrBomb, float speed)
            : base(game)
        {
            this.game = game;
            this.imageProjectile = imageProjectile;
            this.laserOrBomb = laserOrBomb;
            this.speed = speed;
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
            projectile = new Projectile(imageProjectile.Width, imageProjectile.Height, GraphicsDevice.Viewport.Height, GraphicsDevice.Viewport.Width, speed, laserOrBomb);
            base.LoadContent();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// Calls the Projectile class' move method on every Update call.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
           projectile.Move();
           base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values</param>
        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(imageProjectile, projectile.GetPosition(), Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
        }

        /// <summary>
        /// Calls the Projectile class' GetBoundary method.
        /// </summary>
        /// <returns>The boundary of the projectile.</returns>
        public Rectangle GetBoundary()
        {
            return projectile.GetBoundary();
        }

        /// <summary>
        /// Calls the Projectile class' GetPosition method.
        /// </summary>
        /// <returns>The position of the projectile.</returns>
        public Vector2 GetPosition()
        {
            return projectile.GetPosition();
        }

        /// <summary>
        /// Calls the Projectile class' SetPosition method.
        /// </summary>
        /// <param name="x">The X coordinate of the projectile to be set to</param>
        /// <param name="y">The Y coordinate of the projectile to be set to</param>
        public void SetPosition(int x, int y)
        {
            projectile.SetPosition(x, y);
        }
    }
}
