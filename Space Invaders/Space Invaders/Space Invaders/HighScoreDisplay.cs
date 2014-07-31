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
    /// Class defining the high score screen
    /// 
    /// Author - Anthony Bermejo, Patrick Nicoll
    /// Version - 31/07/2014 - v1.0
    /// </summary>
    class HighScoreDisplay : Microsoft.Xna.Framework.DrawableGameComponent
    {

        private Game1 game;
        private int screenHeight;
        private int screenWidth;
        private SpriteBatch spriteBatch;

        private Color color;
        private Color selectedColor;  //Selected menu item's color
        private int colorFlashTimer;  //Used to time color flash effect on selected menu item 

        private KeyboardState keyboard;
        private KeyboardState prevKeyboard;
        private SpriteFont font; //Menu font

        List<string> menuItems = new List<string>();

        public HighScoreDisplay(Game1 game, int screenHeight, int screenWidth) : base(game)
        {
            this.game = game;
            this.screenHeight = screenHeight;
            this.screenWidth = screenWidth;

            menuItems.Add("Back");

            //Initialize font color to white and flash timer to 0
            color = Color.White;
            colorFlashTimer = 0;
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

        public override void Draw(GameTime gameTime)
        {
            if (game.GetGameState() == Game1.GameState.HighScores)
            {
                spriteBatch.Begin();
                FlashTimerControl();  //Calls method to produce the text flashing effect
                color = selectedColor;

                spriteBatch.DrawString(font, menuItems[0], new Vector2((screenWidth / 2) - (font.MeasureString(menuItems[0]).X / 2),
                         (screenHeight / 4 * 3)), color);
                spriteBatch.End();
            }
            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            if (game.GetGameState() == Game1.GameState.HighScores)
            {
                keyboard = Keyboard.GetState();

                if (CheckKeyboard(Keys.Enter))
                    game.SetGameState(Game1.GameState.MainMenu);

                prevKeyboard = keyboard;

            }
            base.Update(gameTime);
        }

        public bool CheckKeyboard(Keys key)
        {
            return (keyboard.IsKeyDown(key) && !prevKeyboard.IsKeyDown(key));
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
