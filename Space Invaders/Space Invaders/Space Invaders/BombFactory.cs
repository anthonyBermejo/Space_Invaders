using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using Microsoft.Xna.Framework.Audio;

namespace Space_Invaders
{
    // Declaration of delegate type needed for event handlers
    public delegate void PlayerCollision(DrawableGameComponent player, int points);

    /// <summary>
    /// A class derived from ProjectileFactory that is specialized in creating bombs of type Projectile
    /// that are launched by an AlienSquad object.
    /// 
    /// Authors - Anthony Bermejo, Venelin Koulaxazov, Patrick Nicoll
    /// Version - 26/07/2014 - v1.2
    /// </summary>
    public class BombFactory : ProjectileFactory
    {
        // instance variable declarations
        private Game game;
        private Texture2D bombImage;
        private SoundEffect playerDeathSound;
        private PlayerSprite player;
        public event PlayerCollision playerCollision;

        // constructor
        public BombFactory(Game1 game, int screenHeight, PlayerSprite player) : base(game, screenHeight)
        {
            bullets = new List<ProjectileSprite>();
            this.game = game;
            this.screenHeight = screenHeight;
            this.player = player;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            bombImage = game.Content.Load<Texture2D>("bomb");
            playerDeathSound = game.Content.Load<SoundEffect>("playerDeathSound");
            base.LoadContent();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// Calls the Update method of each Projectile object in the list of bombs.
        /// Checks on each update if there was a collision between a bomb and the Player.
        /// Removes the bomb from the list of Projectiles if it has reached the top of the screen.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        /// <summary>
        /// Checks if there was a collision between a bomb and a player.
        /// </summary>
        /// <param name="bomb">Bomb that was launched by an Alien to be checked for collisions.</param>
        protected override bool checkCollision(ProjectileSprite bomb)
        {
            bool collision = false;

            if (bomb.GetBoundary().Intersects(player.GetBoundary()))
            {
                onPlayerCollision(player, 0);
                collision = true;
                playerDeathSound.Play();
            }

            return collision;
        }

        /// <summary>
        /// Called when an Alien is able to launch a bomb. Sets the position of the bomb according to the 
        /// alien's position then adds it to the list of Projectiles.
        /// </summary>
        /// <param name="alien">The Alien that will launch the bomb.</param>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Launch(Rectangle alien, GameTime gameTime)
        {

            int xCoordinate = alien.X + (alien.Width / 2);
            int yCoordinate = alien.Bottom + 1;
            ProjectileSprite projectile = new ProjectileSprite(game, bombImage, false, 6F);
            projectile.Initialize();
            projectile.SetPosition(xCoordinate, yCoordinate);
            bullets.Add(projectile);
        }

        /// <summary>
        /// Protected method that fires the collision event.
        /// </summary>
        /// <param name="player">Player which was hit by the bomb</param>
        /// <param name="points">Points scored when the player is hit by certain bombs</param>
        protected void onPlayerCollision(DrawableGameComponent player, int points)
        {
            if (playerCollision != null)
                playerCollision(player, points);
        }

        /// <summary>
        /// Removes all bombs from the screen. Thus allowing the player to have a moment of spawn protection after respawning.
        /// </summary>
        public void SpawnProtection()
        {
            bullets.RemoveRange(0, bullets.Count);
        }
    }
}
