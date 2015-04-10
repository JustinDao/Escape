using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Escape
{
    class Question
    {
        public String QuestionText { get; set; }
        public List<string> Options { get; set; }
        private int correctOptionIndex;
        public Keys CorrectKey 
        { 
            get
            {
                switch (correctOptionIndex)
                {
                    case 0:
                        return Keys.D1;
                    case 1:
                        return Keys.D2;
                    case 2:
                        return Keys.D3;
                    case 3:
                        return Keys.D4;
                    default:
                        return Keys.D0;
                }
            }
        }
        public Buttons CorrectButton
        {
            get
            {
                switch(correctOptionIndex)
                {
                    case 0:
                        return Buttons.Y;
                    case 1:
                        return Buttons.X;
                    case 2:
                        return Buttons.B;
                    case 3:
                        return Buttons.A;
                    default:
                        // what button is this
                        return Buttons.BigButton;
                }
            }
        }

        public Question() 
        {
            this.QuestionText = "What is your favorite color?";
            this.Options = new List<string> { "Yellow", "Blue", "Red", "Green" };
            this.correctOptionIndex = 1;
        }

        public Question(string question, List<string> answers, int correctOptionIndex)
        {
            this.QuestionText = question;
            this.Options = answers;
            this.correctOptionIndex = correctOptionIndex;
        }

        public bool IsCorrectOption(int answerId)
        {
            return answerId == correctOptionIndex;
        }

        public void Initialize(ContentManager cm)
        {

        }

        public void Draw(SpriteBatch sb)
        {

        }
    }
}

