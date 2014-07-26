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
    /// Version 12/12/2013 - v1.0
    /// </summary>
    class GameOverMenu : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private Game1 game;
        private GraphicsDeviceManager graphics;
        private int screenHeight;
        private int screenWidth;
        private SpriteBatch spriteBatch;

        private Color color;
        private Color selectedColor; // Selected menu item's color
        private int colorFlashTimer; // Used to time color flash effect on selected menu
        private int padding;

        KeyboardState keyboard;
        KeyboardState prevKeyboard;
        SpriteFont font; //Menu font

        List<string> menuItems = new List<string>();
        int selected = 0; //Highlighted menu item

        private int score;
        private int highScore;

        //NOTE: Keeping this incase of future need
        //private System.Timers.Timer timer = new System.Timers.Timer(250); //Create a timer to delay game start


        public GameOverMenu(Game1 game)
            : base(game)
        {
            this.game = game;
            graphics = game.getGraphicsDeviceManager();
            screenHeight = graphics.PreferredBackBufferHeight;
            screenWidth = graphics.PreferredBackBufferWidth;

            //Add each menu item to the list
            menuItems.Add("Restart");
            menuItems.Add("Exit");

            //Initialize font color to white
            color = Color.White;
            colorFlashTimer = 0;

            //Padding between menu items
            padding = 3;

            // Hook up event to timer
            //timer.Elapsed += new System.Timers.ElapsedEventHandler(OnTimedEvent);
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = game.Content.Load<SpriteFont>("scoreFont");
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (game.GetGameState() == Game1.GameState.GameOverMenu)
            {
                keyboard = Keyboard.GetState();

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
                        case 0:
                            {
                                //timer.Start();
                                break;
                            }
                        case 1:
                            game.SetGameState(Game1.GameState.Exit);
                            break;
                    }
                }

                prevKeyboard = keyboard;
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (game.GetGameState() == Game1.GameState.GameOverMenu)
            {
                score = game.GetGameScore();
                highScore = game.GetGameHighScore();
                spriteBatch.Begin();

                spriteBatch.DrawString(font, "GAME OVER", new Vector2((screenWidth / 2) - (font.MeasureString("GAME OVER").X / 2),
                    screenHeight / 4), Color.White);

                if (score <= highScore)
                {
                    spriteBatch.DrawString(font, "HIGH SCORE: " + highScore, new Vector2((screenWidth / 2) - (font.MeasureString("HIGH SCORE: " + highScore).X / 2),
                        screenHeight / 3), Color.White);
                    spriteBatch.DrawString(font, "YOUR SCORE: " + score, new Vector2((screenWidth / 2) - (font.MeasureString("YOUR SCORE: " + score).X / 2),
                        screenHeight / 3 - font.LineSpacing + padding), Color.White);
                }
                else
                {
                    spriteBatch.DrawString(font, "NEW HIGH SCORE: " + score, new Vector2((screenWidth / 2) - (font.MeasureString("NEW HIGH SCORE: " + score).X / 2),
                        (screenHeight / 3)), Color.White);
                }

                game.removeComponents();


                for (int i = 0; i < menuItems.Count; i++)
                {
                    if (i == selected)
                    {
                        FlashTimerControl();
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

        public bool CheckKeyboard(Keys key)
        {
            return (keyboard.IsKeyDown(key) && !prevKeyboard.IsKeyDown(key));
        }

        private void OnTimedEvent(object source, System.Timers.ElapsedEventArgs e)
        {
            game.SetGameState(Game1.GameState.Playing);
            //timer.Dispose();
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
    }
}
