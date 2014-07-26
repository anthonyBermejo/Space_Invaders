using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Space_Invaders 
{
    /// <summary>
    /// Class defining the background of the game
    /// 
    /// Authors - Anthony Bermejo, Patrick Nicoll
    /// Version - 26/07/2014 - v1.0
    /// </summary>
    /// 

    class Starfield : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private Game1 game;
        private int screenHeight;
        private SpriteBatch spriteBatch;
        private Texture2D sf;
        private Vector2 background1;
        private Vector2 background2;
        private float speed = 2;

        //Constructor
        public Starfield(Game1 game, int screenHeight) : base(game)
        {
            this.game = game;
            this.screenHeight = screenHeight;

            background1 = new Vector2(0,0);
            background2 = new Vector2(0,0 - screenHeight);
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            sf = game.Content.Load<Texture2D>("star_background");
            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            //Draws the same background twice, one above the other outside the screen pane
            if (game.GetGameState() == Game1.GameState.Playing)
            {
                spriteBatch.Begin();
                spriteBatch.Draw(sf, background1, Color.White);
                spriteBatch.Draw(sf, background2, Color.White);
                spriteBatch.End();
            }
            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            //Both backgrounds move down the screen at the same speed to remain seemless
            if (game.GetGameState() == Game1.GameState.Playing)
            {
                background1.Y += speed;
                background2.Y += speed;

                //Background will loop to the top to give the impression of endless scrolling
                if (background1.Y >= screenHeight)
                {
                    background1.Y = 0;
                    background2.Y = 0 - screenHeight;
                }
            }
            base.Update(gameTime);
        }
    }
}
