using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Escape
{
    class MiniGame : Screen
    {
        public MainGame mg { get; set; }
        public GraphicsDevice gd { get; set; }
        public bool Active { get; set; }

        private Question currentQuestion;
        private List<Question> CurrentQuestions;
        private List<Question> AllQuestions;
        private int timeRemaining;
        private float timeInterval;
        private Rectangle BackgroundBox { get; set; }
        private SpriteFont Font { get; set; }

        private bool answeredWrong = false;
        // How long the pentaly for a wrong answer is
        private float wrongInterval = 2f;
        private float wrongTime = 0;

        private int TOTAL_TIME = 60;

        private int numQuestions = 1;
        private int MAX_QUESTIONS = 5;

        private Color[] colors = 
        { 
            Color.Green, Color.Red, Color.Blue, Color.Yellow, 
        };

        private Vector2[] position 
        {
            get
            {
                return new Vector2[] 
                {
                    new Vector2(mg.GAME_WIDTH / 2, 400),
                    new Vector2(mg.GAME_WIDTH / 2 + 200, 350),
                    new Vector2(mg.GAME_WIDTH / 2 - 200, 350),                
                    new Vector2(mg.GAME_WIDTH / 2, 300),
                };
            }
        }                             

        public MiniGame(MainGame mg, GraphicsDevice gd, Player player)
        {
            this.mg = mg;
            this.gd = gd;
            this.BackgroundBox = new Rectangle(0, 0, mg.GAME_WIDTH, mg.GAME_HEIGHT);
            this.timeRemaining = TOTAL_TIME;
            this.timeInterval = 0;
            this.Active = false;
            this.AllQuestions = new List<Question>(player.Questions);

            randomizeQuestions();
        }

        public void Reinitialize()
        {
            this.timeInterval = 0;
            this.timeRemaining = TOTAL_TIME;
        }

        public override void LoadContent(ContentManager cm)
        {
            this.BackgroundTexture = new Texture2D(this.gd, 1, 1);
            this.BackgroundTexture.SetData(new Color[] { Color.White });
            this.Font = cm.Load<SpriteFont>("QuestionFont");
        }

        public override void Draw(SpriteBatch sb)
        {
            // Background
            sb.Draw(BackgroundTexture, BackgroundBox, Color.White * 0.6f);
            // Question
            sb.DrawString(Font, currentQuestion.QuestionText, new Vector2(mg.GAME_WIDTH / 2, 200), Color.Black);
            // Time Remaining
            sb.DrawString(Font, this.timeRemaining.ToString(), new Vector2(mg.GAME_WIDTH - 50, 50), Color.Black);

            var middle = mg.GAME_WIDTH / 2;

            for(int i = 0; i < currentQuestion.Options.Count; i++)
            {
                string option = currentQuestion.Options[i];
                string optionText = currentQuestion.GetButton(option) + ": " + option;

                if (answeredWrong)
                {
                    if (option == currentQuestion.CorrectOption)
                    {
                        sb.DrawString(Font, optionText, position[i], Color.Lime);
                    }
                    else
                    {
                        sb.DrawString(Font, optionText, position[i], Color.DarkRed);
                    }                    
                }
                else
                {
                    sb.DrawString(Font, optionText, position[i], colors[i]);
                }
                
            }
        }

        public void Update(Controls controls, GameTime gt, Player player)
        {
            var delta = (float)gt.ElapsedGameTime.TotalSeconds;
            this.timeInterval += delta;

            wrongTime += delta;

            if (wrongTime > wrongInterval)
            {
                answeredWrong = false;
            }

            if (this.timeInterval > 1f)
            {
                this.timeRemaining -= 1;
                this.timeInterval = 0;

                // If you run out of time, do something
                if (timeRemaining == 0)
                {
                    this.Active = false;
                    player.RegainControl();
                }
            }

            if (!answeredWrong)
            {
                if (controls.onPress(currentQuestion.CorrectKey, currentQuestion.CorrectButton))
                {
                    if (currentQuestion == CurrentQuestions.Last())
                    {
                        this.Active = false;
                        player.RegainControl();
                        randomizeQuestions();
                    }
                    else
                    {
                        currentQuestion = CurrentQuestions[CurrentQuestions.IndexOf(currentQuestion) + 1];
                    }
                }
                else
                {
                    foreach (Keys key in Controls.ValidInput.Keys)
                    {
                        if (currentQuestion.CorrectButton == Controls.ValidInput[key]) continue;

                        if (controls.onPress(key, Controls.ValidInput[key]))
                        {
                            answeredWrong = true;
                            wrongTime = 0;
                            break;
                        }
                    }
                }
            }            
        }

        private void randomizeQuestions()
        {
            //http://stackoverflow.com/questions/5383498
            var rnd = new Random();
            CurrentQuestions = AllQuestions.OrderBy(item => rnd.Next()).ToList().Take(numQuestions++).ToList();
            if (numQuestions > MAX_QUESTIONS) numQuestions = MAX_QUESTIONS;
            this.currentQuestion = CurrentQuestions.First();
        }
    }
}
