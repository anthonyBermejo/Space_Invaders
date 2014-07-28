using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Space_Invaders
{

    /// <summary>
    /// Class defining the Game Over Menu of the game.
    /// 
    /// Author Patrick Nicoll
    /// Version 26/07/2014 - v1.2
    /// </summary>
    class GameOverMenu : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private Game1 game;
        private GraphicsDeviceManager graphics;
        private int screenHeight;
        private int screenWidth;
        private SpriteBatch spriteBatch;

        private Color color;
        private Color selectedColor;  //Selected menu item's color
        private int colorFlashTimer;  //Used to time color flash effect on selected menu item 
        private int padding;

        KeyboardState keyboard;
        KeyboardState prevKeyboard;
        SpriteFont font; //Menu font

        List<string> menuItems = new List<string>();
        int selected = 0; //Highlighted menu item

        private int score;
        private int highScore;
        private char[] playerInitials;
        private string initialString;
        private int initialCtr; //Position 0,1, or 2 of the char array containing initials
        private bool newHighScore;
        private bool lockControls; //If true, will override controls so that input goes towards initials, not game movements
        private bool releaseControls; //When initials are done being entered, will allow lockControls to switch states

        //NOTE: Keeping this incase of future need
        private System.Timers.Timer timer = new System.Timers.Timer(500); //Create a timer to delay game start


        public GameOverMenu(Game1 game)
            : base(game)
        {
            this.game = game;
            graphics = game.getGraphicsDeviceManager();
            screenHeight = graphics.PreferredBackBufferHeight;
            screenWidth = graphics.PreferredBackBufferWidth;

            //Add each menu item to the list
            menuItems.Add("Restart");
            menuItems.Add("Main Menu");
            menuItems.Add("Exit");

            //Initialize font color to white and flash timer to 0
            color = Color.White;
            colorFlashTimer = 0;

            //Padding between menu items
            padding = 3;

            // Hook up event to timer
            timer.Elapsed += new System.Timers.ElapsedEventHandler(OnTimedEvent);

            playerInitials = new char[3] {'_', '_', '_'};
            initialCtr = 0;
            newHighScore = false;
            lockControls = false;
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = game.Content.Load<SpriteFont>("menuFont");
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (game.GetGameState() == Game1.GameState.GameOverMenu)
            {
                keyboard = Keyboard.GetState();
    
                //Makes sure user does not need to enter initial for highscore, if so controls are overriden
                if (!lockControls)
                {

                    //Checks for input from user that represent going up
                    if (CheckKeyboard(Keys.Up) || CheckKeyboard(Keys.W))
                    {
                        //Will not go up further than top list item, wraps to bottom item
                        if (selected > 0)
                            selected--;
                        else if (selected == 0)
                            selected = menuItems.Count - 1;
                    }

                    //Checks for input from user that represent going down
                    if (CheckKeyboard(Keys.Down) || CheckKeyboard(Keys.S))
                    {
                        //Will not go lower than bottom list item, wraps to top item
                        if (selected < menuItems.Count - 1)
                            selected++;
                        else if (selected == (menuItems.Count - 1))
                            selected = 0;
                    }

                    //Checks for input from user that represents 'enter'
                    if (CheckKeyboard(Keys.Enter) || CheckKeyboard(Keys.Space))
                    {
                        switch (selected)
                        {
                            //Game restart case
                            case 0:
                                {
                                    timer.Start();
                                    initialCtr = 0;
                                    game.restartGame();
                                    break;
                                }
                            //Main menu case
                            case 1:
                                {
                                    //game.restartGame();
                                    //game.SetGameState(Game1.GameState.MainMenu);
                                    break;
                                }

                            //Exit case
                            case 2:
                                {
                                    game.SetGameState(Game1.GameState.Exit);
                                    break;
                                }
                        }
                    }
                }

                score = game.GetGameScore();
                highScore = game.GetGameHighScore();

               
                //Will not accept key input if game controls are active
                if (!releaseControls)
                {
                    acceptHighScoreInitials();
                } 
                
                prevKeyboard = keyboard;
            }

           

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (game.GetGameState() == Game1.GameState.GameOverMenu)
            {
                spriteBatch.Begin();

                spriteBatch.DrawString(font, "GAME OVER", new Vector2((screenWidth / 2) - (font.MeasureString("GAME OVER").X / 2),
                    screenHeight / 4), Color.White);

                if (score <= highScore)
                {
                    spriteBatch.DrawString(font, "HIGH SCORE: " + highScore, new Vector2((screenWidth / 2) - (font.MeasureString("HIGH SCORE: " + highScore).X / 2),
                        screenHeight / 3), Color.White);
                    spriteBatch.DrawString(font, "YOUR SCORE: " + score, new Vector2((screenWidth / 2) - (font.MeasureString("YOUR SCORE: " + score).X / 2),
                        screenHeight / 3 - font.LineSpacing + padding), Color.White);

                    newHighScore = false;
                }
                else
                {
                    spriteBatch.DrawString(font, "NEW HIGH SCORE: " + score, new Vector2((screenWidth / 2) - (font.MeasureString("NEW HIGH SCORE: " + score).X / 2),
                        (screenHeight / 3)), Color.White);

                    newHighScore = true;
                    if(!releaseControls)
                        lockControls = true;
                }

                //game.removeComponents();


                for (int i = 0; i < menuItems.Count; i++)
                {
                    if (i == selected)
                    {
                        FlashTimerControl();  //Calls method to produce the text flashing effect
                        color = selectedColor;
                    }
                    else
                        color = Color.White;
                    spriteBatch.DrawString(font, menuItems[i], new Vector2((screenWidth / 2) - (font.MeasureString(menuItems[i]).X / 2),
                         (screenHeight / 2) - (font.LineSpacing * menuItems.Count / 2) + ((font.LineSpacing + padding) * i)), color);
                }
                spriteBatch.End();
            }
            base.Draw(gameTime);
        }

        private void acceptHighScoreInitials()
        {
            if (CheckKeyboard(Keys.A))
            {
                playerInitials[initialCtr] = 'A';
                Console.WriteLine("A");
                Console.WriteLine(playerInitials);
                initialCtr++;
            }
            else if (CheckKeyboard(Keys.B))
            {
                playerInitials[initialCtr] = 'B';
                Console.WriteLine("B");
                Console.WriteLine(playerInitials);
                initialCtr++;
            }
            else if (CheckKeyboard(Keys.C))
            {
                playerInitials[initialCtr] = 'C';
                Console.WriteLine("C");
                Console.WriteLine(playerInitials);
                initialCtr++;
            }
            else if (CheckKeyboard(Keys.D))
            {
                playerInitials[initialCtr] = 'D';
                Console.WriteLine("D");
                Console.WriteLine(playerInitials);
                initialCtr++;
            }
            else if (CheckKeyboard(Keys.E))
            {
                playerInitials[initialCtr] = 'E';
                Console.WriteLine("E");
                Console.WriteLine(playerInitials);
                initialCtr++;
            }
            else if (CheckKeyboard(Keys.F))
            {
                playerInitials[initialCtr] = 'F';
                Console.WriteLine("F");
                Console.WriteLine(playerInitials);
                initialCtr++;
            }
            else if (CheckKeyboard(Keys.G))
            {
                playerInitials[initialCtr] = 'G';
                Console.WriteLine("G");
                Console.WriteLine(playerInitials);
                initialCtr++;
            }
            else if (CheckKeyboard(Keys.H))
            {
                playerInitials[initialCtr] = 'H';
                Console.WriteLine("H");
                Console.WriteLine(playerInitials);
                initialCtr++;
            }
            else if (CheckKeyboard(Keys.I))
            {
                playerInitials[initialCtr] = 'I';
                Console.WriteLine("I");
                Console.WriteLine(playerInitials);
                initialCtr++;
            }
            else if (CheckKeyboard(Keys.J))
            {
                playerInitials[initialCtr] = 'J';
                Console.WriteLine("J");
                Console.WriteLine(playerInitials);
                initialCtr++;
            }
            else if (CheckKeyboard(Keys.K))
            {
                playerInitials[initialCtr] = 'K';
                Console.WriteLine("K");
                Console.WriteLine(playerInitials);
                initialCtr++;
            }
            else if (CheckKeyboard(Keys.L))
            {
                playerInitials[initialCtr] = 'L';
                Console.WriteLine("L");
                Console.WriteLine(playerInitials);
                initialCtr++;
            }
            else if (CheckKeyboard(Keys.M))
            {
                playerInitials[initialCtr] = 'M';
                Console.WriteLine("M");
                Console.WriteLine(playerInitials);
                initialCtr++;
            }
            else if (CheckKeyboard(Keys.N))
            {
                playerInitials[initialCtr] = 'N';
                Console.WriteLine("N");
                Console.WriteLine(playerInitials);
                initialCtr++;
            }
            else if (CheckKeyboard(Keys.O))
            {
                playerInitials[initialCtr] = 'O';
                Console.WriteLine("O");
                Console.WriteLine(playerInitials);
                initialCtr++;
            }
            else if (CheckKeyboard(Keys.P))
            {
                playerInitials[initialCtr] = 'P';
                Console.WriteLine("P");
                Console.WriteLine(playerInitials);
                initialCtr++;
            }
            else if (CheckKeyboard(Keys.Q))
            {
                playerInitials[initialCtr] = 'Q';
                Console.WriteLine("Q");
                Console.WriteLine(playerInitials);
                initialCtr++;
            }
            else if (CheckKeyboard(Keys.R))
            {
                playerInitials[initialCtr] = 'R';
                Console.WriteLine("R");
                Console.WriteLine(playerInitials);
                initialCtr++;
            }
            else if (CheckKeyboard(Keys.S))
            {
                playerInitials[initialCtr] = 'S';
                Console.WriteLine("S");
                Console.WriteLine(playerInitials);
                initialCtr++;
            }
            else if (CheckKeyboard(Keys.T))
            {
                playerInitials[initialCtr] = 'T';
                Console.WriteLine("T");
                Console.WriteLine(playerInitials);
                initialCtr++;
            }
            else if (CheckKeyboard(Keys.U))
            {
                playerInitials[initialCtr] = 'U';
                Console.WriteLine("U");
                Console.WriteLine(playerInitials);
                initialCtr++;
            }
            else if (CheckKeyboard(Keys.V))
            {
                playerInitials[initialCtr] = 'V';
                Console.WriteLine("V");
                Console.WriteLine(playerInitials);
                initialCtr++;
            }
            else if (CheckKeyboard(Keys.W))
            {
                playerInitials[initialCtr] = 'W';
                Console.WriteLine("W");
                Console.WriteLine(playerInitials);
                initialCtr++;
            }
            else if (CheckKeyboard(Keys.X))
            {
                playerInitials[initialCtr] = 'X';
                Console.WriteLine("X");
                Console.WriteLine(playerInitials);
                initialCtr++;
            }
            else if (CheckKeyboard(Keys.Y))
            {
                playerInitials[initialCtr] = 'Y';
                Console.WriteLine("Y");
                Console.WriteLine(playerInitials);
                initialCtr++;
            }
            else if (CheckKeyboard(Keys.Z))
            {
                playerInitials[initialCtr] = 'Z';
                Console.WriteLine("Z");
                Console.WriteLine(playerInitials);
                initialCtr++;
            }
            else if (CheckKeyboard(Keys.D0) || CheckKeyboard(Keys.NumPad0))
            {
                playerInitials[initialCtr] = '0';
                Console.WriteLine("0");
                Console.WriteLine(playerInitials);
                initialCtr++;
            }
            else if (CheckKeyboard(Keys.D1) || CheckKeyboard(Keys.NumPad1))
            {
                playerInitials[initialCtr] = '1';
                Console.WriteLine("1");
                Console.WriteLine(playerInitials);
                initialCtr++;
            }
            else if (CheckKeyboard(Keys.D2) || CheckKeyboard(Keys.NumPad2))
            {
                playerInitials[initialCtr] = '2';
                Console.WriteLine("2");
                Console.WriteLine(playerInitials);
                initialCtr++;
            }
            else if (CheckKeyboard(Keys.D3) || CheckKeyboard(Keys.NumPad3))
            {
                playerInitials[initialCtr] = '3';
                Console.WriteLine("3");
                Console.WriteLine(playerInitials);
                initialCtr++;
            }
            else if (CheckKeyboard(Keys.D4) || CheckKeyboard(Keys.NumPad4))
            {
                playerInitials[initialCtr] = '4';
                Console.WriteLine("4");
                Console.WriteLine(playerInitials);
                initialCtr++;
            }
            else if (CheckKeyboard(Keys.D5) || CheckKeyboard(Keys.NumPad5))
            {
                playerInitials[initialCtr] = '5';
                Console.WriteLine("5");
                Console.WriteLine(playerInitials);
                initialCtr++;
            }
            else if (CheckKeyboard(Keys.D6) || CheckKeyboard(Keys.NumPad6))
            {
                playerInitials[initialCtr] = '6';
                Console.WriteLine("6");
                Console.WriteLine(playerInitials);
                initialCtr++;
            }
            else if (CheckKeyboard(Keys.D7) || CheckKeyboard(Keys.NumPad7))
            {
                playerInitials[initialCtr] = '7';
                Console.WriteLine("7");
                Console.WriteLine(playerInitials);
                initialCtr++;
            }
            else if (CheckKeyboard(Keys.D8) || CheckKeyboard(Keys.NumPad8))
            {
                playerInitials[initialCtr] = '8';
                Console.WriteLine("8");
                Console.WriteLine(playerInitials);
                initialCtr++;
            }
            else if (CheckKeyboard(Keys.D9) || CheckKeyboard(Keys.NumPad9))
            {
                playerInitials[initialCtr] = '9';
                Console.WriteLine("9");
                Console.WriteLine(playerInitials);
                initialCtr++;
            }
            else if (CheckKeyboard(Keys.OemMinus) || CheckKeyboard(Keys.Subtract))
            {
                playerInitials[initialCtr] = '-';
                Console.WriteLine("-");
                Console.WriteLine(playerInitials);
                initialCtr++;
            }
            else if (CheckKeyboard(Keys.Back))
            {
                Console.WriteLine("Back");
                switch (initialCtr)
                {
                    //Initial counter is in first position
                    case 0:
                        {
                            playerInitials[initialCtr] = '_';
                            break;
                        }
                    //Initial counter is in second position
                    case 1:
                        {
                            initialCtr--;
                            playerInitials[initialCtr] = '_';
                            break;
                        }
                    //Initial counter is in third position
                    case 2:
                        {
                            //If there is a third initial entered clear third initial
                            if (playerInitials[2] != '_')
                                playerInitials[initialCtr] = '_';
                            //If not, iterate back and clear second initial
                            else
                            {
                                initialCtr--;
                                playerInitials[initialCtr] = '_';
                            }
                            break;
                        }
                }
                Console.WriteLine(playerInitials);

            }
            else if (CheckKeyboard(Keys.Space))
            {
                playerInitials[initialCtr] = ' ';
                Console.WriteLine("Space");
                Console.WriteLine(playerInitials);
                initialCtr++;
            }
            else if (CheckKeyboard(Keys.Enter))
            {
                Console.WriteLine("Enter");
                initialString = ToString();
                initialString = initialString.Replace('_', ' ');
                game.writeHighScore(initialString, score);
                lockControls = false;
                releaseControls = true;
            }

            if (initialCtr > 2)
                initialCtr = 2;
        }

        public bool CheckKeyboard(Keys key)
        {
            return (keyboard.IsKeyDown(key) && !prevKeyboard.IsKeyDown(key));
        }

        /* KEEP INCASE NEEDED
        private static bool isKeyAChar(Keys key)
        {
            if (key >= Keys.A && key <= Keys.Z)
                return true;
            else
                return false;
        }
         */

        private void OnTimedEvent(object source, System.Timers.ElapsedEventArgs e)
        {
            //game.SetGameState(Game1.GameState.Playing);
            timer.Enabled = false;
        }

        private void FlashTimerControl()
        {
            colorFlashTimer++;

            // change color between yellow and white
            if (colorFlashTimer % 20 == 0)
            {
                if (selectedColor == Color.Yellow)
                    selectedColor = Color.White;
                else
                    selectedColor = Color.Yellow;

                colorFlashTimer = 0;
            }
        }

        public override string ToString()
        {
            string tempString = "";

            foreach (char c in playerInitials)
                tempString += c;

            return tempString;
        }
    }
}
