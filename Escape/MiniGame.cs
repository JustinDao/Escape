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
        private int timeRemaining;
        private int timeInterval;
        private int questionsRemaining;
        private Rectangle BackgroundBox { get; set; }
        private SpriteFont Font { get; set; }

        public MiniGame(MainGame mg, GraphicsDevice gd)
        {
            this.mg = mg;
            this.gd = gd;
            this.BackgroundBox = new Rectangle(0, 0, mg.GAME_WIDTH, mg.GAME_HEIGHT);
            this.currentQuestion = new Question();
            this.timeRemaining = 60;
            this.timeInterval = 0;
            this.Active = false;
        }

        public void Reinitialize()
        {
            this.timeInterval = 0;
            this.timeRemaining = 60;
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(BackgroundTexture, BackgroundBox, Color.White * 0.6f);
            sb.DrawString(Font, currentQuestion.QuestionText, new Vector2(mg.GAME_WIDTH / 2, 200), Color.Black);

            Color[] colors = { Color.Yellow, Color.Blue, Color.Red, Color.Green };

            for(int i = 0; i < currentQuestion.Options.Count; i++)
            {
                String option = currentQuestion.Options[i];
                sb.DrawString(Font, option, new Vector2(mg.GAME_WIDTH / 2, 300 + 50 * i), colors[i]);
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
            }

            if (controls.onPress(currentQuestion.CorrectKey, currentQuestion.CorrectButton)) 
            {
                this.Active = false;
                player.RegainControl();
            }            
        }
    }
}
