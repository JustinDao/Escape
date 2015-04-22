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
        private int timeInterval;
        private Rectangle BackgroundBox { get; set; }
        private SpriteFont Font { get; set; }

        private int TOTAL_TIME = 60;

        private int numQuestions = 1;
        private int MAX_QUESTIONS = 5;

        private Dictionary<Keys, Buttons> ValidInput = new Dictionary<Keys, Buttons>();

        public MiniGame(MainGame mg, GraphicsDevice gd, Player player)
        {
            this.mg = mg;
            this.gd = gd;
            this.BackgroundBox = new Rectangle(0, 0, mg.GAME_WIDTH, mg.GAME_HEIGHT);
            this.timeRemaining = TOTAL_TIME;
            this.timeInterval = 0;
            this.Active = false;
            this.AllQuestions = new List<Question>(player.Questions);
            this.ValidInput.Add(Keys.A, Buttons.A);
            this.ValidInput.Add(Keys.B, Buttons.B);
            this.ValidInput.Add(Keys.X, Buttons.X);
            this.ValidInput.Add(Keys.Y, Buttons.Y);

            randomizeQuestions();
        }

        public void Reinitialize()
        {
            this.timeInterval = 0;
            this.timeRemaining = TOTAL_TIME;
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(BackgroundTexture, BackgroundBox, Color.White * 0.6f);
            sb.DrawString(Font, currentQuestion.QuestionText, new Vector2(mg.GAME_WIDTH / 2, 200), Color.Black);

            var middle = mg.GAME_WIDTH / 2;
            Color[] colors = { Color.Yellow, Color.Blue, Color.Red, Color.Green };
            Vector2[] position = {
                                     new Vector2(middle, 300),
                                     new Vector2(middle - 200, 350),
                                     new Vector2(middle + 200, 350),
                                     new Vector2(middle, 400),
                                 };

            for(int i = 0; i < currentQuestion.Options.Count; i++)
            {
                String option = currentQuestion.Options[i];
                sb.DrawString(Font, option, position[i], /*new Vector2(mg.GAME_WIDTH / 2, 300 + 50 * i)*/ colors[i]);
            }

            sb.DrawString(Font, this.timeRemaining.ToString(), new Vector2(mg.GAME_WIDTH - 50, 50), Color.Black);
        }

        public override void LoadContent(ContentManager cm)
        {
            this.BackgroundTexture = new Texture2D(this.gd, 1, 1);
            this.BackgroundTexture.SetData(new Color[] { Color.White });
            this.Font = cm.Load<SpriteFont>("QuestionFont");
        }

        public void Update(Controls controls, GameTime gt, Player player)
        {
            this.timeInterval += (int)gt.ElapsedGameTime.TotalMilliseconds;

            if (this.timeInterval > 1000)
            {
                this.timeRemaining -= 1;
                this.timeInterval = 0;

                if (timeRemaining == 0)
                {
                    this.Active = false;
                    player.RegainControl();
                }
            }

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
                foreach (Keys key in ValidInput.Keys)
                {
                    if (currentQuestion.CorrectButton == ValidInput[key]) continue;

                    if (controls.onPress(key, ValidInput[key]))
                    {
                        // Show wrong answer
                        break;
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
