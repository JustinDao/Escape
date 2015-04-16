using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Escape
{
    class Text : Entity
    {
        public Color Color;
        public string Value;
        public SpriteFont Font;

        public override Rectangle HitBox
        {
            get
            {
                return new Rectangle(0, 0, 0, 0);
            }
        }

        public Color WHITE = Color.White;

        public Text(ContentManager cm, string value, Vector2 position, string fontName, Color? color = null)
        {
            this.Font = cm.Load<SpriteFont>(fontName);
            this.Value = value;
            this.Position = position;
            
            if (color == null)
            {
                this.Color = Color.White;
            }
            else
            {
                this.Color = (Color)color;
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.DrawString(Font, Value, Position, Color);
        }
        
        public override void Update(GameTime gt, Screen s)
        {
            // nothing!
        }
    }
}
