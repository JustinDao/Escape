#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using Tao.Sdl;
using Microsoft.Xna.Framework.Media;
using TexturePackerLoader;
using Microsoft.Xna.Framework.Audio;
#endregion

namespace Escape
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class MainGame : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public SpriteRender SpriteRender;
        Screen currentScreen;
        Castle castle;
        StartMenu start;
        PauseMenu pause;
        bool isPaused = false;
        MiniGame miniGame;
        EndScreen endScreen;
        public Controls Control;
        SubmissionBar submissionBar;
        public SoundEffectInstance CurrentSong;
        public SoundEffectInstance SubmissionSong;
        public SoundEffectInstance TransitionSong;

        public int GAME_WIDTH = 1000;
        public int GAME_HEIGHT = 600;

        public bool PlayingPrelude = false;
        private float preludeCounter = 0;
        private int preludeLength = 2*60 + 23; // 2m23s song length

        public bool PlayedTransition = false;
        private float transitionCounter = 0;
        private int transitionLength = 1; // 2m23s song length

        public MainGame()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = GAME_WIDTH;
            graphics.PreferredBackBufferHeight = GAME_HEIGHT;
            graphics.ApplyChanges();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            SpriteRender = new SpriteRender(this.spriteBatch);
            Control = new Controls();

            start = new StartMenu(this, GraphicsDevice);
            castle = new Castle(this);
            pause = new PauseMenu(this, GraphicsDevice);
            miniGame = new MiniGame(this, GraphicsDevice, castle.Player);
            endScreen = new EndScreen(this);
            currentScreen = start;            

            submissionBar = new SubmissionBar(new Rectangle(20, 20, 200, 20), graphics);

            var song = Content.Load<SoundEffect>("Songs\\Submission");
            SubmissionSong = song.CreateInstance();
            SubmissionSong.IsLooped = true;

            song = Content.Load<SoundEffect>("Songs/Transition");
            TransitionSong = song.CreateInstance();

            base.Initialize();

            Joystick.Init();
            Console.WriteLine("Number of joysticks: " + Sdl.SDL_NumJoysticks());
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            start.LoadContent(this.Content);
            castle.LoadContent(this.Content);
            pause.LoadContent(this.Content);
            miniGame.LoadContent(this.Content);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (PlayingPrelude)
            {
                preludeCounter += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (preludeCounter > preludeLength)
                {
                    this.CurrentSong.Stop();
                    var song = Content.Load<SoundEffect>("Songs\\Main");
                    this.CurrentSong = song.CreateInstance();
                    this.PlayingPrelude = false;
                    this.CurrentSong.IsLooped = true;
                    this.CurrentSong.Play();
                }
            }

            //set our keyboardstate tracker update can change the gamestate on every cycle
            Control.Update();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            //Up, down, left, right affect the coordinates of the sprite

            if (isPaused)
            {
                pause.Update(Control);
            }
            else if (castle.Player.BeatTheGame)
            {
                currentScreen = endScreen;
                endScreen.Update(gameTime);
                var song = Content.Load<SoundEffect>("Songs\\Victory");
                CurrentSong.Stop();
                CurrentSong = song.CreateInstance();
                CurrentSong.Play();
            }
            else if (currentScreen == start)
            {
                start.Update(Control);
            }
            else if (currentScreen == castle)
            {
                castle.Update(Control, gameTime);

                if (!miniGame.Active && !castle.Player.PlayerControl)
                {
                    miniGame.Reinitialize();
                    miniGame.Active = true;
                    currentScreen = miniGame;
                    this.CurrentSong.Pause();
                    PlayedTransition = true;
                    this.TransitionSong.Play();
                }

                submissionBar.Update(castle.Player, graphics);
     
            } 
            else if (currentScreen == miniGame)
            {
                miniGame.Update(Control, gameTime, castle.Player);
                if (PlayedTransition)
                {
                    transitionCounter += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if(transitionCounter > transitionLength)
                    {
                        SubmissionSong.Play();
                        PlayedTransition = false;
                    }
                }
            }
            else if (currentScreen is CreditsScreen)
            {
                var cs = currentScreen as CreditsScreen;
                cs.Update(gameTime);
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkGray);

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            currentScreen.Draw(spriteBatch);

            if (currentScreen == castle) 
            {
                submissionBar.Draw(spriteBatch);
            }

            if (miniGame.Active)
            {
                miniGame.Draw(spriteBatch);
            }

            if (this.isPaused)
            {
                pause.Draw(spriteBatch);
            }  


            spriteBatch.End();

            base.Draw(gameTime);
        }

        public void Pause()
        {
            isPaused = true;
        }

        public void UnPause()
        {
            isPaused = false;
        }

        public void SwitchToCastle()
        {
            currentScreen = castle;
        }
    }
}
