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

        public string CorrectOption
        {
            get
            {
                return Options[correctOptionIndex];
            }
        }

        private int correctOptionIndex;

        private Dictionary<int, Keys> indexKeyMap = new Dictionary<int, Keys>
        {
            {0, Keys.D1},
            {1, Keys.D2},
            {2, Keys.D3},
            {3, Keys.D4},
        };

        private Dictionary<int, Buttons> indexButtonMap = new Dictionary<int, Buttons>
        {
            {0, Buttons.A},
            {1, Buttons.X},
            {2, Buttons.B},
            {3, Buttons.Y},
        };

        public Keys CorrectKey 
        { 
            get
            {
                return indexKeyMap[correctOptionIndex];
            }
        }

        public Buttons CorrectButton
        {
            get
            {
                return indexButtonMap[correctOptionIndex];
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

        public string GetButton(string option)
        {
            int optionIndex = Options.IndexOf(option);

            if (optionIndex < 0)
            {
                // didn't find option
                return null;
            }

            
            return indexButtonMap[optionIndex].ToString();
            
        }

        public void Initialize(ContentManager cm)
        {

        }

        public void Draw(SpriteBatch sb)
        {

        }
    }
}

