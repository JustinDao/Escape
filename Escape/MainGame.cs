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
        CreditsScreen credits;
        GameOverScreen gameOver;
        public Controls Control;
        SubmissionBar submissionBar;
        public SoundEffectInstance CurrentSong;
        public SoundEffectInstance PreludeSong;
        public SoundEffectInstance SubmissionSong;
        public SoundEffectInstance TransitionSong;
        public SoundEffectInstance EndingSong;
        public SoundEffectInstance CreditsSong;

        public Texture2D EndingBackground;
        public Rectangle EndingBackgroundBox;

        public int GAME_WIDTH = 1000;
        public int GAME_HEIGHT = 600;

        public bool PlayingPrelude = false;
        private float preludeCounter = 0;
        private float preludeLength = 2 * 60 + 23.30f; // 2m23s song time

        public bool PlayedTransition = false;
        private float transitionCounter = 0;
        private float transitionLength = 1.15f;

        public bool Fading = false;
        public bool Faded = false;
        public bool SwitchedToCredits = false;
        public float FadeOpacity = 0f;
        private float fadeCounter = 0;
        private float fadeInterval = 0.05f;

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
            credits = new CreditsScreen(Content, this);
            gameOver = new GameOverScreen(Content, this);
            currentScreen = start;            

            submissionBar = new SubmissionBar(new Rectangle(0, 0, 200, 20), graphics);

            EndingBackground = new Texture2D(GraphicsDevice, 1, 1);
            EndingBackground.SetData(new Color[] { Color.Black });
            EndingBackgroundBox = new Rectangle(0, 0, GAME_WIDTH, GAME_HEIGHT);

            var song = Content.Load<SoundEffect>("Songs/Submission");
            SubmissionSong = song.CreateInstance();
            SubmissionSong.IsLooped = true;

            song = Content.Load<SoundEffect>("Songs/Prelude");
            PreludeSong = song.CreateInstance();

            song = Content.Load<SoundEffect>("Songs/Transition");
            TransitionSong = song.CreateInstance();

            song = Content.Load<SoundEffect>("Songs/Victory");
            EndingSong = song.CreateInstance();

            song = Content.Load<SoundEffect>("Songs/Credits");
            CreditsSong = song.CreateInstance();

            PlayingPrelude = false;
            preludeCounter = 0;
            PlayedTransition = false;
            transitionCounter = 0;
            Fading = false;
            Faded = false;
            SwitchedToCredits = false;
            FadeOpacity = 0f;
            fadeCounter = 0;

            base.Initialize();

            Joystick.Init();
            Console.WriteLine("Number of joysticks: " + Sdl.SDL_NumJoysticks());
        }

        public void ReInitialize()
        {
            this.Initialize();
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
            //set our keyboardstate tracker update can change the gamestate on every cycle
            Control.Update();

            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            //    Exit();

            // TODO: Add your update logic here
            //Up, down, left, right affect the coordinates of the sprite

            if (isPaused)
            {
                pause.Update(Control);
            }
            else if (currentScreen == credits)
            {
                if(!SwitchedToCredits) SwitchedToCredits = true;
                credits.Update(gameTime);
            }
            else if (castle.Player.BeatTheGame)
            {
                currentScreen = endScreen;
                endScreen.Update(gameTime);
                CurrentSong.Stop();
                EndingSong.Play();

                if (Fading)
                {
                    if (Faded)
                    {
                        currentScreen = credits;
                        EndingSong.Stop();
                        CreditsSong.Play();
                    }

                    fadeCounter += (float)gameTime.ElapsedGameTime.TotalSeconds;

                    if (fadeCounter > fadeInterval)
                    {
                        FadeOpacity += 0.01f;

                        if (FadeOpacity >= 1f)
                        {
                            Faded = true;
                        }
                    }
                }
                
            }
            else if (currentScreen == start)
            {
                start.Update(Control);
            }
            else if (currentScreen == castle)
            {
                if (PlayingPrelude)
                {
                    preludeCounter += (float)gameTime.ElapsedGameTime.TotalSeconds;

                    if (preludeCounter > preludeLength)
                    {
                        this.CurrentSong.Stop();
                        var song = Content.Load<SoundEffect>("Songs/Main");
                        this.CurrentSong = song.CreateInstance();
                        this.PlayingPrelude = false;
                        this.CurrentSong.IsLooped = true;
                        this.CurrentSong.Play();
                    }
                }

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
            else if (currentScreen == gameOver)
            {
                gameOver.Update(gameTime);
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

            if (Faded) spriteBatch.Draw(EndingBackground, EndingBackgroundBox, null, Color.Black * FadeOpacity);

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

            if (endScreen.EndAll && !SwitchedToCredits)
            {
                if (!Fading) Fading = true;
                spriteBatch.Draw(EndingBackground, EndingBackgroundBox, null, Color.Black * FadeOpacity);
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
            transitionCounter = 0;
            currentScreen = castle;
        }

        public void SwitchToGameOver()
        {
            currentScreen = gameOver;
        }
    }
}
