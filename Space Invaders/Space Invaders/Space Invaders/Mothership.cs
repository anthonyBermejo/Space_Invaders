using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Space_Invaders
{
    /// <summary>
    /// Represents the mothership instance
    /// 
    /// Author - Patrick Nicoll
    /// Version - 26/07/2014 - v1.1
    /// </summary>
    class Mothership
    {
        // instance variable declaration
        private int motherWidth;
        private int motherHeight;
        private int screenHeight;
        private int screenWidth;
        private Vector2 position;
        private Direction dir;
        private Rectangle boundary;
        private readonly float SPEED;
        private AlienState alienState;

        public Mothership(int motherWidth, int motherHeight, int screenHeight, int screenWidth, float speed)
        {
            this.motherHeight = motherHeight;
            this.motherWidth = motherWidth;
            this.screenHeight = screenHeight;
            this.screenWidth = screenWidth;
            this.SPEED = speed;
            alienState = AlienState.ACTIVE;

            RandomizeMothershipSpawn();

            //Remains the same regardless of which side it spawns on.
            position.Y = motherHeight * 2;
            
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
        /// Returns the boundary of the mothership image that will later be used to determine collisions.
        /// </summary>
        /// <returns>The boundary of the mothership</returns>
        public Rectangle GetBoundary()
        {
            boundary = new Rectangle((int)position.X, (int)position.Y, motherWidth, motherHeight);
            return boundary;
        }

        /// <summary>
        ///  Returns the position of the mothership.
        /// </summary>
        /// <returns>The position of the mothership</returns>
        public Vector2 GetPosition()
        {
            return position;
        }

        /// <summary>
        /// Moves the mothership in the specified direction by one width according to the direction.
        /// </summary>
        /// <param name="dir">an enum representing one of 2 possible mothership directions of movement </param>
        public void Move()
        {
            switch (dir)
            {
                case Direction.LEFT:
                    {
                        position.X -= SPEED;
                        if (GetPosition().X + motherWidth <= 0)
                            SetAlienState(AlienState.INACTIVE);
                        break;
                    }
                case Direction.RIGHT:
                    position.X += SPEED;
                    if (GetPosition().X >= screenWidth)
                        SetAlienState(AlienState.INACTIVE);
                    break;
            }
        }

        /// <summary>
        /// Randomly decides which side of the screen the mothership will spawn on, thus also determining its travel direction
        /// </summary>
        public void RandomizeMothershipSpawn()
        {
            Random random = new Random();

            //Starts on left, moves right
            if (random.Next(0, 2) == 0)
            {
                position.X = 0 - motherWidth;
                dir = Direction.RIGHT;
            }
            else
            {
                position.X = screenWidth;
                dir = Direction.LEFT;
            }
        }

        /// <summary>
        /// Calls the Mothership class' SetAlienState method.
        /// </summary>
        /// <param name="state">The alien state.</param>
        public void SetAlienState(AlienState state)
        {
            alienState = state;
        }

        /// <summary>
        /// Sets the position of the mothership image on the screen.
        /// </summary>
        /// <param name="x">x coordinate of the mothership</param>
        /// <param name="y">y coordinate of the mothership</param>
        public void SetPosition(int x, int y)
        {
            this.position.X = x;
            this.position.Y = y;
        }
        }
    }
