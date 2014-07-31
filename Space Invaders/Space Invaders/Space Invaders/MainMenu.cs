using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace Space_Invaders
{
    /// <summary>
    /// Class defining the main menu component of the game.
    /// 
    /// Author Patrick Nicoll
    /// Version 31/07/2014 - v1.4
    /// </summary>
    public class MainMenu : Microsoft.Xna.Framework.DrawableGameComponent
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

        private Texture2D titleScreenImg;
        private SoundEffect titleScreenSong;
        private SoundEffectInstance titleSongInstance; //Will allow theme song to loop

        private bool songPlaying; //If the theme song is currently playing

        KeyboardState keyboard;
        KeyboardState prevKeyboard;
        SpriteFont font; //Menu font

        List<string> menuItems = new List<string>();
        int selected = 0; //Highlighted menu item

        private System.Timers.Timer timer = new System.Timers.Timer(500); //Create a timer to delay game start
        

        public MainMenu(Game1 game) : base(game)
        {
            this.game = game;
            graphics = game.getGraphicsDeviceManager();
            screenHeight = graphics.PreferredBackBufferHeight;
            screenWidth = graphics.PreferredBackBufferWidth;

            songPlaying = false;

            //Add each menu item to the list
            menuItems.Add("New Game");
            menuItems.Add("High Scores");
            menuItems.Add("Exit");

            //Initialize font color to white and flash timer to 0
            color = Color.White;
            colorFlashTimer = 0;
            
            //Padding between menu items
            padding = 3;


            // Hook up event to timer
            timer.Elapsed += new System.Timers.ElapsedEventHandler(OnTimedEvent);
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = game.Content.Load<SpriteFont>("menuFont");
            titleScreenImg = game.Content.Load<Texture2D>("title-screen");
            titleScreenSong = game.Content.Load<SoundEffect>("themeSong");
            titleSongInstance = titleScreenSong.CreateInstance();
            titleSongInstance.IsLooped = true;
            titleSongInstance.Volume = 0.8f;
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (game.GetGameState() == Game1.GameState.MainMenu)
            {
                if (!songPlaying)
                {
                    titleSongInstance.Play();
                    songPlaying = true;
                }

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
                if (CheckKeyboard(Keys.Enter))
                {
                    switch (selected)
                    {
                        case 0:
                            {
                                titleSongInstance.Stop();
                                timer.Start();
                                break;
                            }
                        case 1:
                            {
                                game.SetGameState(Game1.GameState.HighScores);
                                break;
                            }
                        case 2:
                            {
                                game.SetGameState(Game1.GameState.Exit);
                                break;
                            }
                    }
                }

                prevKeyboard = keyboard;
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (game.GetGameState() == Game1.GameState.MainMenu)
            {
                spriteBatch.Begin();
                spriteBatch.Draw(titleScreenImg, new Vector2(0, 0), Color.White);
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
                         ((screenHeight / 2) + 90) + (font.LineSpacing * menuItems.Count / 2) + ((font.LineSpacing + padding) * i)), color);
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
    }
}
