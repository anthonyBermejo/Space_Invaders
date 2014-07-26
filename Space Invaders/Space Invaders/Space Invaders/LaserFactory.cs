using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Space_Invaders
{
    // Declaration of delegate types needed for event handlers

    //Alien collision delegate
    public delegate void AlienCollision(DrawableGameComponent killedAlien, int points);

    //Mothership collision delegate
    public delegate void MothershipCollision(DrawableGameComponent killedMothership, int points);

    /// <summary>
    /// A class derived from ProjectileFactory that is specialized in creating lasers of type Projectile
    /// that are launched by a Player object.
    /// 
    /// Authors - Anthony Bermejo, Venelin Koulaxazov, Patrick Nicoll
    /// Version - 26/07/2014 - v1.1
    /// </summary>
    public class LaserFactory : ProjectileFactory
    {
        // instance variable declarations
        private Game game;
        private Texture2D imageLaser;
        private ProjectileSprite projectile;
        private TimeSpan previousLaunchTime = new TimeSpan();
        private TimeSpan tolerance = TimeSpan.FromMilliseconds(360);
        //private TimeSpan tolerance = TimeSpan.FromMilliseconds(0);
        private AlienSquad alienSquad;
        private MothershipSprite mothership;
        public event AlienCollision AlienCollision;
        public event MothershipCollision MothershipCollision;

        // constructor
        public LaserFactory(Game1 game, int screenHeight) : base(game, screenHeight) 
        {
            bullets = new List<ProjectileSprite>();
            this.game = game;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            imageLaser = game.Content.Load<Texture2D>("laser1");
            base.LoadContent();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// Calls the Update method of each Projectile object in the list of lasers.
        /// Checks on each update if there was a collision between a laser and an Alien, removes it
        /// from the list if one was detected.
        /// Removes the laser from the list of Projectiles if it has reached the top of the screen.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        /// <summary>
        /// Checks if there was a collision between a laser and an alien by iterating through the AlienSquad
        /// array. Returns True if a collision was detected, False if there was no collision. Also checks for
        /// a collision with the mothership.
        /// </summary>
        /// <param name="laser">Laser that was launched by a player to be checked for collisions.</param>
        /// <returns>A boolean representing if a collision was detected</returns>
        protected override bool checkCollision(ProjectileSprite laser)
        {
            bool collision = false;
            int pts = 0;

            for (int row = 0; row < alienSquad.getAlienRowCount(); row++)
                for (int col = 0; col < alienSquad.getAlienColumnCount(); col++)
                    if (laser.GetBoundary().Intersects(alienSquad[row, col].GetBoundary()))
                        if (alienSquad[row, col].GetAlienState() == AlienState.ACTIVE)
                        {
                            // Only gives points if Alien dies, on higher difficulties multiple hits will not award points
                            if (alienSquad[row, col].GetHitPoints() == 1)
                                pts = 10 + (alienSquad.getAlienRowCount() - 1 - row) * 10;

                            onAlienCollision(alienSquad[row, col], pts);
                            collision = true;

                            //remove laser after it hits alien
                            laser.Dispose();
                            bullets.Remove(laser);

                            return collision;
                        }
            if (laser.GetBoundary().Intersects(mothership.GetBoundary()))
                if(mothership.GetAlienState() == AlienState.ACTIVE)
            {
                onMothershipCollision(mothership, 100);
                collision = true;

                // remove laser after it hits alien
                laser.Dispose();
                bullets.Remove(laser);

                return collision;
            }

            return collision;
        }

        /// <summary>
        /// Called when a Player is able to launch a laser. Sets the position of the laser according to the 
        /// Players's position then adds it to the list of Projectiles. Controls how rapidly a player
        /// can fire a laser through throttling.
        /// </summary>
        /// <param name="player">The border of the player object</param>
        /// <param name="gameTime">Provides a snapshot of timing values</param>
        public override void Launch(Rectangle player, GameTime gameTime)
        {
            int yCoordinate = player.Top - 1 - imageLaser.Height;
            int xCoordinate = player.X + (player.Width / 2 - 1);

            if (gameTime.TotalGameTime - previousLaunchTime > tolerance)
            {
                projectile = new ProjectileSprite(game, imageLaser, true, 8F);
                projectile.Initialize();
                projectile.SetPosition(xCoordinate, yCoordinate);
                bullets.Add(projectile);
                previousLaunchTime = gameTime.TotalGameTime;
            }
        }

        /// <summary>
        /// Protected method that fires the collision event.
        /// </summary>
        /// <param name="killedAlien">Alien that was hit by a laser.</param>
        /// <param name="points">Points received when alien is killed</param>
        protected void onAlienCollision(AlienSprite killedAlien, int points)
        {
            if (AlienCollision != null)
                AlienCollision(killedAlien, points);
        }

        /// <summary>
        /// Protected method that fires the collision event.
        /// </summary>
        /// <param name="killedMothership">Mothership that was hit by the laser</param>
        /// <param name="points">Points received when mothership is killed</param>
        protected void onMothershipCollision(MothershipSprite killedMothership, int points)
        {
            if (MothershipCollision != null)
                MothershipCollision(killedMothership, points);
        }

        /// <summary>
        /// Sets the AlienSquad instance variable to the object sent in as a parameter.
        /// </summary>
        /// <param name="alienSquad">An instance of an AlienSquad.</param>
        public void SetAlienSquad(AlienSquad alienSquad)
        {
            this.alienSquad = alienSquad;
        }

        /// <summary>
        /// Sets the Mothership instance variable to the object sent in as a parameter.
        /// </summary>
        /// <param name="mothership">An instance of a Mothership</param>
        public void SetMothership(MothershipSprite mothership)
        {
            this.mothership = mothership;
        }
    }
}
