using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Space_Invaders
{
    /// <summary>
    /// Represents an Projectile instance
    /// 
    /// Authors - Anthony Bermejo, Venelin Koulaxazov, Patrick Nicoll
    /// Version - 29/03/2012
    /// </summary>
    class Projectile
    {
        // instance variable declaration
        private readonly float SPEED;
        private int projectileWidth;
        private int projectileHeight;
        private int screenHeight;
        private int screenWidth;
        private Vector2 position;
        private Rectangle boundary;
        private bool laserOrBomb; //if true, projectile is a laser, otherwise it is a bomb. Helper variable to determine the projectile direction

        // constructor
        public Projectile(int projectileWidth, int projectileHeight, int screenHeight, int screenWidth, float speed, bool laserOrBomb)
        {
            this.projectileWidth = projectileWidth;
            this.projectileHeight = projectileHeight;
            this.screenHeight = screenHeight;
            this.screenWidth = screenWidth;
            this.SPEED = speed;
            this.laserOrBomb = laserOrBomb;
        }

        /// <summary>
        /// Returns the boundary of a projectile, used for collisions.
        /// </summary>
        /// <returns>The boundary of the projectile.</returns>
        public Rectangle GetBoundary()
        {
            boundary = new Rectangle((int)position.X, (int)position.Y, projectileWidth, projectileHeight);
            return boundary;
        }

        /// <summary>
        /// Returns the position of the projectile.
        /// </summary>
        /// <returns>The position of the projectile.</returns>
        public Vector2 GetPosition()
        {
            return position;
        }

        /// <summary>
        /// Moves the projectiles according to the specified speed. If laserOrBomb instance variable is True,
        /// the projectile is a laser and moves upwards along a straight path. If variable is false,
        /// projectile is a bomb and moves downwards along a straight path.
        /// </summary>
        public void Move()
        {
            if (laserOrBomb)
                position.Y -= SPEED;
            else
                position.Y += SPEED;
        }

        /// <summary>
        /// Sets the position of a projectile to the specified X and Y coordinates
        /// </summary>
        /// <param name="x">X coordinate of the projectile to be set to</param>
        /// <param name="y">Y coordinate of the projectile to be set to</param>
        public void SetPosition(int x, int y)
        {
            this.position.X = x;
            this.position.Y = y;
        }
    }
}
