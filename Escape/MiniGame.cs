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
        public Texture2D DoorTexture;
        public PlayerClip PlayerClip;
        public Texture2D PathTexture;
        public Rectangle PathRectangle;

        private Question currentQuestion;
        private List<Question> CurrentQuestions;
        private List<Question> AllQuestions;
        
        private Rectangle BackgroundBox { get; set; }
        private SpriteFont Font { get; set; }

        private float clipX = 800;
        private float clipY = 50;

        private bool answeredWrong = false;
        // How long the pentaly for a wrong answer is
        private float wrongInterval = 2f;
        private float wrongTime = 0;

        private bool answeredRight = false;
        private float rightInterval = 0.5f;
        private float rightTime = 0;

        private float timeRemaining;
        private int TOTAL_TIME = 30;

        private int neededCorrectAnswers = 1;
        private int currentCorrectAnswers = 0;

        private int numQuestions = 1;
        private int MAX_QUESTIONS = 5;

        private Color[] colors = 
        { 
            Color.Green, Color.Blue, Color.Red, Color.Yellow, 
        };

        private Vector2[] position 
        {
            get
            {
                return new Vector2[] 
                {
                    new Vector2(mg.GAME_WIDTH / 2, 400),
                    new Vector2(mg.GAME_WIDTH / 2 - 200, 350),   
                    new Vector2(mg.GAME_WIDTH / 2 + 200, 350),
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
            this.Active = false;
            this.AllQuestions = new List<Question>(player.Questions);
            this.PlayerClip = new PlayerClip(mg.Content, mg.SpriteRender);
            this.PlayerClip.Position = new Vector2(clipX, clipY);

            this.PathRectangle = new Rectangle(50, 50, (int)clipX - 50, 10);
            this.PathTexture = mg.Content.Load<Texture2D>("pixel.png");

            randomizeQuestions();
        }

        public void Reinitialize()
        {
            this.timeRemaining = TOTAL_TIME;
        }

        public override void LoadContent(ContentManager cm)
        {
            this.DoorTexture = cm.Load<Texture2D>("castle_door.png");
            this.BackgroundTexture = new Texture2D(this.gd, 1, 1);
            this.BackgroundTexture.SetData(new Color[] { Color.White });
            this.Font = cm.Load<SpriteFont>("QuestionFont");
        }

        public override void Draw(SpriteBatch sb)
        {
            // Background
            sb.Draw(BackgroundTexture, BackgroundBox, Color.Black);
            // Draw Path
            var percentLeft = ((float)timeRemaining / (float)TOTAL_TIME);
            var barColor = Color.Lerp(Color.Green, Color.Red, 1-percentLeft);
            sb.Draw(PathTexture, PathRectangle, null, barColor);
            // Draw Door
            sb.Draw(DoorTexture, new Vector2(0,0), Color.White);
            // Draw PlayerReference
            PlayerClip.Draw(sb);
            // Question
            sb.DrawString(Font, currentQuestion.QuestionText, new Vector2(mg.GAME_WIDTH / 2 - 50, 200), Color.White);
            // Time Remaining
            sb.DrawString(Font, ((int)this.timeRemaining).ToString(), new Vector2(mg.GAME_WIDTH - 50, 50), Color.White);

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
                else if (answeredRight)
                {
                    if (option == currentQuestion.CorrectOption)
                    {
                        sb.DrawString(Font, optionText, position[i], Color.Lime);
                    }
                    //else
                    //{
                    //    sb.DrawString(Font, optionText, position[i], Color.Black);
                    //}
                }
                else
                {
                    sb.DrawString(Font, optionText, position[i], colors[i]);
                }
                
            }
        }

        public void Update(Controls controls, GameTime gt, Player player)
        {
            PlayerClip.Update(gt, this);
            PlayerClip.Position.X = clipX * ((float)timeRemaining / (float)TOTAL_TIME);
            PathRectangle.Width = (int)(clipX * ((float)timeRemaining / (float)TOTAL_TIME)) - PathRectangle.X;

            var delta = (float)gt.ElapsedGameTime.TotalSeconds;

            wrongTime += delta;
            rightTime += delta;

            if (wrongTime > wrongInterval)
            {
                if (answeredWrong)
                {
                    moveToNextQuestion();
                }               

                answeredWrong = false;
            }

            this.timeRemaining -= delta;

            // If you run out of time, do something
            if (timeRemaining <= 0)
            {
                this.Active = false;
                player.RegainControl(0f);
            }
            

            if (!answeredWrong && !answeredRight)
            {
                if (controls.onPress(currentQuestion.CorrectKey, currentQuestion.CorrectButton))
                {
                    answeredRight = true;
                    rightTime = 0;
                    currentCorrectAnswers++;
                    CurrentQuestions.Remove(currentQuestion);
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

            if (answeredRight && rightTime > rightInterval)
            {
                answeredRight = false;

                if (currentCorrectAnswers >= neededCorrectAnswers)
                {
                    answeredRight = false;
                    this.Active = false;
                    player.RegainControl((float)timeRemaining / (float)TOTAL_TIME);
                    mg.SwitchToCastle();
                    randomizeQuestions();
                    neededCorrectAnswers++;
                    currentCorrectAnswers = 0;
                    if (neededCorrectAnswers > MAX_QUESTIONS)
                    {
                        neededCorrectAnswers = MAX_QUESTIONS;
                    }
                }
                else
                {
                    moveToNextQuestion();                                      
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

        private void moveToNextQuestion()
        {
            int index = CurrentQuestions.IndexOf(currentQuestion) + 1;
            if (index >= CurrentQuestions.Count())
            {
                index = 0;
            }

            currentQuestion = CurrentQuestions[index];
        }
    }
}
