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
        public List<String> Options { get; set; }
        private int correctOptionIndex;
        public Keys CorrectKey { get; set; }
        public Buttons CorrectButton { get; set; }

        public Question() 
        {
            this.QuestionText = "What is your favorite color?";
            this.Options = new List<String> { "Yellow", "Blue", "Red", "Green" };
            this.correctOptionIndex = 1;
            this.CorrectButton = Buttons.X;
            this.CorrectKey = Keys.X;
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

