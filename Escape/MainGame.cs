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
        SpriteRender spriteRender;
        Screen currentScreen;
        Castle castle;
        StartMenu start;
        PauseMenu pause;
        MiniGame miniGame;
        public Controls Control;
        SubmissionBar submissionBar;
        TestThing t;

        public int GAME_WIDTH = 1000;
        public int GAME_HEIGHT = 600;

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
            Control = new Controls();
            start = new StartMenu(this, GraphicsDevice);
            castle = new Castle(this);
            pause = new PauseMenu(this, GraphicsDevice);
            miniGame = new MiniGame(this, GraphicsDevice);
            currentScreen = start;
            t = new TestThing(this);

            Song song = this.Content.Load<Song>("Songs\\rtr.wav");
            MediaPlayer.Play(song);
            MediaPlayer.IsRepeating = true;
            

            submissionBar = new SubmissionBar(50, 50, graphics);
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
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            spriteRender = new SpriteRender(this.spriteBatch);
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

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            //Up, down, left, right affect the coordinates of the sprite

            if (currentScreen == start)
            {
                start.Update(Control);
            }
            else if(currentScreen == pause)
            {
                pause.Update(Control);
            }
            else if (currentScreen == castle)
            {
                castle.Update(controls, gameTime);

                if (!miniGame.Active && !castle.Player.PlayerControl)
                {
                    miniGame.Reinitialize();
                    miniGame.Active = true;
                }

                submissionBar.Update(castle.Player, graphics);

                if (miniGame.Active)
                {
                    miniGame.Update(Control, gameTime, castle.Player);
                }            
            }      

            t.Update(gameTime, this.controls);

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

            if (currentScreen == pause)
            {
                castle.Draw(spriteBatch);
            }
            currentScreen.Draw(spriteBatch);

            if (currentScreen == castle) 
            {
            submissionBar.Draw(spriteBatch);
            }

            if (miniGame.Active)
            {
                miniGame.Draw(spriteBatch);
            }            

            this.t.Draw(spriteBatch, spriteRender);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        public void SwitchToCastle()
        {
            currentScreen = castle;
            MediaPlayer.Resume();
        }
        public void SwitchToPause()
        {
            currentScreen = pause;
            MediaPlayer.Pause();
        }
    }
}
