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
    /// An instance of an alien squad, consisting of multiple AlienSprite instances.
    /// 
    /// Authors - Anthony Bermejo, Venelin Koulaxazov, Patrick Nicoll
    /// Version - 25/05/2013
    /// </summary>
    /// 

    public delegate void GameOver();

    public class AlienSquad : Microsoft.Xna.Framework.DrawableGameComponent
    {
        // instance variable declaration
        private Game game;
        private AlienSprite[,] alienSquad;
        private MothershipSprite mothershipSprite;
        private Direction dir;
        private Direction previousDir;
        private static Random random = new Random();
        private BombFactory bomb;
        private int fireCap = 0; // variable used for the squad's shooting.
        private float fireTime = 120; // time between squad's shots
        private int alienWidth;
        private int alienHeight;
        private int alienWidthSpacing;
        private int alienLevel; // the level on the screen that the aliens spawn at
        private int screenWidth;
        private int screenHeight;
        private int killedCount; //represents the killed aliens.
        private int level; //represents how many levels have been cleared. After every stage clear, the count is incremented. 
        private int difficulty; // Represents the difficulty stage
        public event GameOver GameOver;
        private LaserFactory aLaser;
        private int motionCtr; //Used to delay time between switching images for aliens
        private int currentListPos; //Keeps track of position in alien picture lists
        private Texture2D alienTexture1;
        private List<Texture2D> alienMotion2;
        private Texture2D alienTexture2;
        private Texture2D alienTexture3;

        // Constructor
        public AlienSquad(Game game, int screenWidth, int screenHeight, BombFactory bomb, LaserFactory laser, MothershipSprite mothershipSprite)
            : base(game)
        {
            this.game = game;
            this.screenWidth = screenWidth;
            this.screenHeight = screenHeight;
            this.bomb = bomb;
            this.mothershipSprite = mothershipSprite;
            dir = Direction.LEFT;
            alienWidth = game.Content.Load<Texture2D>("spaceship1").Width;
            alienHeight = game.Content.Load<Texture2D>("spaceship1").Height;
            alienSquad = new AlienSprite[3, 8];

            currentListPos = 0;
            motionCtr = 0;
            alienTexture1 = game.Content.Load<Texture2D>("spaceship1");
            //Set bug motion pictures
            alienMotion2 = new List<Texture2D>();
            for (int i = 0; i < 4; i++)
            {
                alienMotion2.Add(game.Content.Load<Texture2D>("bug" + (i + 1)));
            }
            alienTexture2 = alienMotion2[0];
            alienTexture3 = game.Content.Load<Texture2D>("flyingsaucer1");
            laser.AlienCollision += killAlien;
            killedCount = 0;
            difficulty = 1;
            level = 1;
            aLaser = laser;
            
        }

        /// <summary>
        /// Indexer to access each individual AlienSprite within the alien squad.
        /// </summary>
        /// <param name="row">The position in the row</param>
        /// <param name="col">The position in the column</param>
        /// <returns>The selected AlienSprite object</returns>
        public AlienSprite this[int row, int col]
        {
            get
            {
                return alienSquad[row, col];
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw(GameTime gameTime)
        {
            if (motionCtr == 30)
            {
                //Iterate through list
                currentListPos++;
                if (currentListPos > 3)
                    currentListPos = 0;

                //Set image for all aliens of this type
                alienTexture2 = alienMotion2[currentListPos];
                for (int ctr = 0; ctr < alienSquad.GetLength(1); ctr++)
                    alienSquad[1, ctr].SetTexture(alienTexture2);

                //Reset motion counter delay
                motionCtr = 0;
            }

            for (int ctr1 = 0; ctr1 < alienSquad.GetLength(0); ctr1++)
                for (int ctr2 = 0; ctr2 < alienSquad.GetLength(1); ctr2++)
                {
                    if (alienSquad[ctr1, ctr2].GetAlienState() == AlienState.ACTIVE)
                        alienSquad[ctr1, ctr2].Draw(gameTime);
                }
            motionCtr++;
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
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {         
            // Determines the graphical image that the Alien will take
            for (int ctr = 0; ctr < alienSquad.GetLength(1); ctr++)
            {
                alienSquad[0, ctr] = new AlienSprite(game, alienTexture1);
                alienSquad[0, ctr].Initialize();
                alienSquad[1, ctr] = new AlienSprite(game, alienTexture2);
                alienSquad[1, ctr].Initialize();
                alienSquad[2, ctr] = new AlienSprite(game, alienTexture3);
                alienSquad[2, ctr].Initialize();
            }

            drawAlienSquad(level);

            base.LoadContent();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// The AlienSquad will move to the edge of the screen, when reached will shift down 1 movement,
        /// then start moving back across the screen.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            fireTime = 120 / difficulty;

            for (int ctr1 = 0; ctr1 < alienSquad.GetLength(0); ctr1++)
                for (int ctr2 = 0; ctr2 < alienSquad.GetLength(1); ctr2++)
                {
                    switch (dir)
                    {
                        case Direction.LEFT:
                        {
                            if (alienSquad[ctr1, ctr2].GetAlienState() != AlienState.INACTIVE)
                                if (!alienSquad[ctr1, ctr2].TryMove(dir))
                                {
                                    dir = Direction.DOWN;
                                    previousDir = Direction.LEFT;
                                }
                            break;
                        }
                        case Direction.RIGHT:
                        {
                            if (alienSquad[ctr1, ctr2].GetAlienState() != AlienState.INACTIVE)
                                if (!alienSquad[ctr1, ctr2].TryMove(dir))
                                {
                                    dir = Direction.DOWN;
                                    previousDir = Direction.RIGHT;
                                }
                            break;
                        }
                    }
                }

            for (int ctr1 = 0; ctr1 < alienSquad.GetLength(0); ctr1++)
                for (int ctr2 = 0; ctr2 < alienSquad.GetLength(1); ctr2++)
                {
                    alienSquad[ctr1, ctr2].Move(dir);
                    if (alienSquad[ctr1, ctr2].GetAlienState() == AlienState.ACTIVE)
                        if (alienSquad[ctr1, ctr2].GetBoundary().Bottom >= screenHeight)
                            onGameOver();
                }

            if (dir == Direction.DOWN)
            {
                if (previousDir == Direction.LEFT)
                    dir = Direction.RIGHT;
                if (previousDir == Direction.RIGHT)
                    dir = Direction.LEFT;
            }
           

            if (fireCap % fireTime == 0)
            {
                int squadRow = random.Next(0, alienSquad.GetLength(0));
                int squadCol = random.Next(0, alienSquad.GetLength(1));

                if (alienSquad[squadRow, squadCol].GetAlienState() != AlienState.INACTIVE)
                    bomb.Launch(alienSquad[squadRow, squadCol].GetBoundary(), gameTime);
            }
            fireCap++;
            base.Update(gameTime);
        }

        // Custom methods

        private void drawAlienSquad(int level)
        {
            // Resets the alien squad spawn to the start, allowing for other difficulty modifiers to come into play at higher levels.
            if (level % 10 == 0)
            {
                alienLevel = 1;
                difficulty++;
            }

            // Calling the SetPosition method to set the position of each alien
            alienWidthSpacing = alienWidth * 3;
            alienSquad[0, 0].SetPosition(alienWidth, alienHeight * alienLevel);

            for (int ctr = 1; ctr < alienSquad.GetLength(1); ctr++)
            {
                alienSquad[0, ctr].SetPosition(alienWidthSpacing, alienHeight * alienLevel);
                alienWidthSpacing += alienWidth * 2;
            }

            alienWidthSpacing = alienWidth * 3;

            alienSquad[1, 0].SetPosition(alienWidth, alienSquad[0, 0].GetBoundary().Bottom + 5);

            for (int ctr = 1; ctr < alienSquad.GetLength(1); ctr++)
            {
                alienSquad[1, ctr].SetPosition(alienWidthSpacing, alienSquad[0, 0].GetBoundary().Bottom + 5);
                alienWidthSpacing += alienWidth * 2;
            }

            alienWidthSpacing = alienWidth * 3;

            alienSquad[2, 0].SetPosition(alienWidth, alienSquad[1, 0].GetBoundary().Bottom + 5);

            for (int ctr = 1; ctr < alienSquad.GetLength(1); ctr++)
            {
                alienSquad[2, ctr].SetPosition(alienWidthSpacing, alienSquad[1, 0].GetBoundary().Bottom + 5);
                alienWidthSpacing += alienWidth * 2;
            }
        }

        /// <summary>
        /// Returns the number of columns in the two-dimensional AlienSprite array.
        /// </summary>
        /// <returns>Number of columns in the AlienSprite array.</returns>
        public int getAlienColumnCount()
        {
            return alienSquad.GetLength(1);
        }

        /// <summary>
        /// Returns the number of rows in the two-dimensional AlienSprite array.
        /// </summary>
        /// <returns>Number of rows in the AlienSprite array.</returns>
        public int getAlienRowCount()
        {
            return alienSquad.GetLength(0);
        }

        public int getLevel()
        {
            return level;
        }

        /// <summary>
        /// Called when a collision is detected between an AlienSprite and a Projectile from the
        /// LaserFactory.
        /// </summary>
        /// <param name="killedAlien">The killed Alien from which the collision was detected on.</param>
        private void killAlien(DrawableGameComponent killedAlien, int points)
        {
            ((AlienSprite)killedAlien).SetAlienState(AlienState.INACTIVE);
            killedCount++;
            //Increases speed of squad once a certain number are killed
            if ((killedCount % 8) == 0)
                Alien.IncreaseSpeed();
            //Resets squad if all are killed
            if (killedCount == alienSquad.Length)
                resetAlienSquad();
            //Spawns mothership once a certain number are killed
            if (killedCount == (alienSquad.Length / 2))
                mothershipSprite.SetSpawnMother(true);

        }

        protected void onGameOver()
        {
            if (GameOver != null)
                GameOver();
        }

        private void resetAlienSquad()
        {
            Alien.speed = 0.5f;
            level++;
            alienLevel++;
            killedCount = 0;
            drawAlienSquad(level);
            setAlienSquadToActive();
            resetMothership();
        }

        /// <summary>
        /// Calls the MothershipSprite's 
        /// </summary>
        private void resetMothership()
        {
            mothershipSprite.RandomizeMothershipSpawn();
            mothershipSprite.SetAlienState(AlienState.ACTIVE);
            mothershipSprite.SetSpawnMother(false);
        }

        /// <summary>
        /// Iterates through the alienSquad to set each alien to ACTIVE.
        /// </summary>
        private void setAlienSquadToActive()
        {
            for (int ctr1 = 0; ctr1 < alienSquad.GetLength(0); ctr1++)
                for (int ctr2 = 0; ctr2 < alienSquad.GetLength(1); ctr2++)
                    alienSquad[ctr1, ctr2].SetAlienState(AlienState.ACTIVE);
        }
    }
}