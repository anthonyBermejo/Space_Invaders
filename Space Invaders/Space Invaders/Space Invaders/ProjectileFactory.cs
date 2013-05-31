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
    /// Abstract class that represents factories which will specialize in creating and managing projectiles.
    /// 
    /// Authors - Anthony Bermejo, Venelin Koulaxazov, Patrick Nicoll
    /// Version - 29/03/2012
    /// </summary>
    public abstract class ProjectileFactory : Microsoft.Xna.Framework.DrawableGameComponent
    {
        // instance variable declaration
        protected List<ProjectileSprite> bullets;
        protected int screenHeight;

        //constructor
        public ProjectileFactory(Game game, int screenHeight)
            : base(game)
        {
            this.screenHeight = screenHeight;
        }

                /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw(GameTime gameTime)
        {
            foreach (var item in bullets)
            {
                item.Draw(gameTime);
            }
            base.Draw(gameTime);
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
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            for (int ctr = 0; ctr < bullets.Count; ctr++)
            {
                bullets[ctr].Update(gameTime);
                if (checkCollision(bullets[ctr]))
                    //bullets.Remove(bullets[ctr]);
                    ;
                else if (bullets[ctr].GetPosition().Y > screenHeight || bullets[ctr].GetPosition().Y < 0)
                {
                    bullets[ctr].Dispose();
                    bullets.Remove(bullets[ctr]);
                }
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// Abstract Launch method that each derived class must override
        /// </summary>
        /// <param name="shooter">The object that will shoot a projectile.</param>
        /// <param name="gameTime">Provides a snapshot of timing values</param>
        public abstract void Launch(Rectangle shooter, GameTime gameTime);

        /// <summary>
        /// Abstract checkCollision method that each derived class must override
        /// </summary>
        /// <param name="projectile">The projectile object</param>
        /// <returns>Whether a collision occured or not</returns>
        protected abstract bool checkCollision(ProjectileSprite projectile);
    }
}
