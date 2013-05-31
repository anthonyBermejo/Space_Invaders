using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Space_Invaders
{
    /// <summary>
    /// Represents an instance of a Player class.
    /// 
    /// Authors - Anthony Bermejo, Venelin Koulaxazov, Patrick Nicoll
    /// Version - 29/03/2012
    /// </summary>
    class Player
    {
        //instance variable declaration
        private readonly float SPEED;
        private int screenWidth;
        private int screenHeight;
        private int playerHeight;
        private int playerWidth;
        private Vector2 position;
        private Rectangle boundary;

        // Constructor
        public Player(int playerHeight, int playerWidth, int screenWidth, int screenHeight, float speed)
        {
            this.playerHeight = playerHeight;
            this.playerWidth = playerWidth;
            this.screenWidth = screenWidth;
            this.screenHeight = screenHeight;
            this.SPEED = speed;

            position.X = (screenWidth / 2) - (playerWidth / 2);
            position.Y = screenHeight - playerHeight;
        }

        /// <summary>
        /// Returns the boundary of a ship, used for collisions.
        /// </summary>
        /// <returns>The boundary of the ship</returns>
        public Rectangle GetBoundary()
        {
            boundary = new Rectangle((int)position.X, (int)position.Y, playerWidth, playerHeight);
            return boundary;
        }

        /// <summary>
        /// Returns the position of the player controlled ship.
        /// </summary>
        /// <returns>The position of the player controlled ship</returns>
        public Vector2 GetPosition()
        {
            return position;
        }

        /// <summary>
        /// Moves the ship to the left without going off the screen using the specified speed.
        /// </summary>
        public void MoveLeft()
        {
            float movement = position.X - SPEED;

            if (movement <= 0)
            {
                position.X = 0;
                return;
            }
            position.X -= SPEED;
        }

        /// <summary>
        /// Moves the ship to the right without going off the screen using the specified speed.
        /// </summary>
        public void MoveRight()
        {
            float movement = position.X + SPEED;

            if (movement + playerWidth >= screenWidth)
            {
                position.X = (screenWidth - playerWidth);
                return;
            }
            position.X += SPEED;
        }

        /// <summary>
        /// Resets the player's position after death.
        /// </summary>
        public void ResetPosition()
        {
            position.X = (screenWidth / 2) - (playerWidth / 2);
            position.Y = screenHeight - playerHeight;
        }

    }
}

