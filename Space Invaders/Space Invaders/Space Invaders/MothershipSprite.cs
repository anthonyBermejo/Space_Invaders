using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// Represents a MothershipSprite which is a wrapper class of the Mothership class.
    /// 
    /// Authors - Patrick Nicoll
    /// Version - 28/07/2014 - v1.3
    /// </summary>
    public class MothershipSprite : Microsoft.Xna.Framework.DrawableGameComponent
    {
        //instance variable declaration
        private Mothership mothership;
        private SpriteBatch spriteBatch;
        private Texture2D imageMother;
        private SoundEffect mothershipSound;
        private SoundEffectInstance mothershipSoundInstance; // Will allow more control over the mothership sound.
        private SoundEffect mothershipKillSound;
        private Game1 game;
        private bool spawnMother;

        //Constructor
        public MothershipSprite(Game1 game, LaserFactory laser)
            : base(game)
        {
            this.game = game;
            laser.MothershipCollision += killMothership;
            spawnMother = false; //For clarity
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
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            imageMother = game.Content.Load<Texture2D>("mothership");
            mothershipSound = game.Content.Load<SoundEffect>("mothershipSound");
            mothershipSoundInstance = mothershipSound.CreateInstance();
            mothershipSoundInstance.Volume = 0.3f;
            mothershipKillSound = game.Content.Load<SoundEffect>("mothershipKillSound");
            mothership = new Mothership(imageMother.Width, imageMother.Height, GraphicsDevice.Viewport.Height, GraphicsDevice.Viewport.Width, 2F);
            base.LoadContent();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            if(game.GetGameState() == Game1.GameState.Playing)
            {
                if (spawnMother)
                {
                    if (mothership.GetAlienState() != AlienState.INACTIVE)
                    {
                        Move();
                        if (mothershipSoundInstance.State != SoundState.Playing)
                            mothershipSoundInstance.Play();
                    } 
                    else if (mothership.GetAlienState() == AlienState.INACTIVE)
                        mothershipSoundInstance.Stop();
                }
               
            }

                base.Update(gameTime);
         }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw(GameTime gameTime)
        {
            if (game.GetGameState() == Game1.GameState.Playing)
            {
                spriteBatch.Begin();
                if (mothership.GetAlienState() == AlienState.ACTIVE)
                    spriteBatch.Draw(imageMother, mothership.GetPosition(), Color.White);
                spriteBatch.End();
            }
            base.Draw(gameTime);
        }

        /// <summary>
        /// Calls the Mothership class' GetAlienState method.
        /// </summary>
        /// <returns>an enum representing one of two possible alien states</returns>
        public AlienState GetAlienState()
        {
            return mothership.GetAlienState();
        }

        /// <summary>
        /// Calls the Mothership class' GetBoundary method.
        /// </summary>
        /// <returns>The boundary of the Mothership.</returns>
        public Rectangle GetBoundary()
        {
            return mothership.GetBoundary();
        } 
        
        public SoundState getMothershipSoundState()
        {
            return mothershipSoundInstance.State;
        }

        /// <summary>
        /// Calls the Mothership class' GetPosition method.
        /// </summary>
        /// <returns>The position of the mothership</returns>
        public Vector2 GetPosition()
        {
            return mothership.GetPosition();
        }

        /// <summary>
        /// Gets the bool value representing whether or not the mothership should spawn
        /// </summary>
        /// <returns>If the mothership should spawn</returns>
        public bool GetSpawnMother()
        {
            return spawnMother;
        }

       

        /// <summary>
        /// Calls the Mothership class' Move method
        /// </summary>
        /// <param name="dir">an enum representing one of 2 possible mothership directions of movement.</param>
        public void Move()
        {
            mothership.Move();
        }

        

        /// <summary>
        /// Calls the Mothership class' RandomizeMothershipSpawn
        /// </summary>
        public void RandomizeMothershipSpawn()
        {
            mothership.RandomizeMothershipSpawn();
        }

        public void resetGame()
        {
            spawnMother = false;
            SetAlienState(AlienState.ACTIVE);
        }

        /// <summary>
        /// Calls the Mothership class' SetAlienState method.
        /// </summary>
        /// <param name="state">The alien state.</param>
        public void SetAlienState(AlienState state)
        {
            mothership.SetAlienState(state);
        }
        
        public void setMothershipSoundState(SoundState state)
        {
            if (state == SoundState.Playing)
                mothershipSoundInstance.Pause();
            else if (state == SoundState.Paused)
                mothershipSoundInstance.Resume();
            else
                mothershipSoundInstance.Stop();
        }

        /// <summary>
        /// Calls the Mothership class' SetPosition method
        /// </summary>
        /// <param name="x">x coordinate of the Mothership instance</param>
        /// <param name="y">y coordinate of the Mothership instance</param>
        public void SetPosition(int x, int y)
        {
            mothership.SetPosition(x, y);
        }

        /// <summary>
        /// Sets whether or not the mothership should spawn
        /// </summary>
        /// <param name="spawnMother">True if yes, false if it shouldn't yet</param>
        public void SetSpawnMother(bool spawnMother)
        {
            this.spawnMother = spawnMother;
        }

        /// <summary>
        /// Called when a collision is detected between an MothershipSprite and a Projectile from the
        /// LaserFactory.
        /// </summary>
        /// <param name="killedMothership">The killed Mothership from which the collision was detected on.</param>
        private void killMothership(DrawableGameComponent killedMothership, int points)
        {
            ((MothershipSprite)killedMothership).SetAlienState(AlienState.INACTIVE);
             mothershipKillSound.Play();
        }

    }
}
