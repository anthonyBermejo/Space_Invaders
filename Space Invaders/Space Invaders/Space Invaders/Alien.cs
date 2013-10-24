using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Space_Invaders
{
    /// <summary>
    /// Represents an Alien instance
    /// 
    /// Authors - Anthony Bermejo, Venelin Koulaxazov, Patrick Nicoll
    /// Version - 23/10/2013
    /// </summary>
    public class Alien
    {
        // instance variable declaration
        private int alienWidth;
        private int alienHeight;
        private int screenHeight;
        private int screenWidth;
        private int hitPoints;
        private Vector2 position;
        private Rectangle boundary;
        private AlienState alienState;
        public static float speed = 0.5f;

        // constructor
        public Alien(int alienWidth, int alienHeight, int screenHeight, int screenWidth, int hitPoints)
        {
            this.alienWidth = alienWidth;
            this.alienHeight = alienHeight;
            this.screenHeight = screenHeight;
            this.screenWidth = screenWidth;
            this.hitPoints = hitPoints;
            alienState = AlienState.ACTIVE;
        }

        /// <summary>
        /// Speed property.
        /// </summary>
        public static float Speed
        {
            get;
            set;
        }

        /// <summary>
        /// Returns the current alien state. Either active, inactive.
        /// </summary>
        /// <returns>The state of the alien</returns>
        public AlienState GetAlienState()
        {
            return alienState;
        }

        /// <summary>
        /// Returns the boundary of the alien image that will later be used to determine collisions.
        /// </summary>
        /// <returns>The boundary of the alien ship</returns>
        public Rectangle GetBoundary()
        {
            boundary = new Rectangle((int)position.X, (int)position.Y, alienWidth, alienHeight);
            return boundary;
        }

        /// <summary>
        ///  Returns the position of the alien.
        /// </summary>
        /// <returns>The position of the alien</returns>
        public Vector2 GetPosition()
        {
            return position;
        }

        /// <summary>
        /// Returns the current hitpoints of the Alien
        /// </summary>
        /// <returns>Alien hitpoints</returns>
        public int GetHitPoints()
        {
            return hitPoints;
        }

        /// <summary>
        /// Increases the speed of alienSquad (by 0.5).
        /// </summary>
        public static void IncreaseSpeed()
        {
            speed += 0.5f;
        }

        /// <summary>
        /// Moves the alien in the specified direction by one width or height according to the direction.
        /// </summary>
        /// <param name="dir">an enum representing one of 3 possible alien directions of movement </param>
        public void Move(Direction dir)
        {
            switch (dir)
            {
                case Direction.LEFT:
                    position.X -= speed;
                    break;
                case Direction.RIGHT:
                    position.X += speed;
                    break;
                case Direction.DOWN:
                    position.Y += alienWidth;
                    break;
            }
        }

        /// <summary>
        /// Sets the alien state.
        /// </summary>
        /// <param name="state">The alien state</param>
        public void SetAlienState(AlienState state)
        {
            this.alienState = state;
        }

        public void SetHitPoints(int hp)
        {
            this.hitPoints = hp;
        }

        /// <summary>
        /// Sets the position of the alien image on the screen.
        /// </summary>
        /// <param name="x">x coordinate of the alien ship</param>
        /// <param name="y">y coordinate of the alien ship</param>
        public void SetPosition(int x, int y)
        {
            this.position.X = x;
            this.position.Y = y;
        }

        /// <summary>
        /// Is called before Move to determine whether the next movement will not make the alien's image go off the screen.
        /// Is also used for determining when to make the alien slide down one row by setting the direction to DOWN
        /// </summary>
        /// <param name="dir">an enum representing one of 3 possible alien directions of movement </param>
        /// <returns>True if the alien ship can move, otherwise returns false</returns>
        public bool TryMove(Direction dir)
        {
            switch (dir)
            {
                case Direction.LEFT:
                        if (position.X <= alienWidth)
                        {
                            return false;
                        }
                        return true;
                case Direction.RIGHT:
                        if (position.X + alienWidth >= screenWidth - alienWidth)
                        {
                            return false;
                        }
                        return true;
                case Direction.DOWN:
                        if (position.Y + alienHeight >= screenHeight)
                            return false;
                        return true;
                default:
                    return true;
            }
        }
    }
}
