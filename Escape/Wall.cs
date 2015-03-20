using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;

namespace Escape
{
    class Wall
    {
        public Texture2D sprite;
        public int XPosition;
        public int YPosition;
        public int Width;
        public int Height;
        public Rectangle HitBox;

        public Wall(int x, int y)
        {
            this.Width = 50;
            this.Height = 50;
            this.XPosition = x;
            this.YPosition = y;
            HitBox = new Rectangle(XPosition, YPosition, Width, Height);
        }

        public void LoadContent(ContentManager content)
        {
            sprite = content.Load<Texture2D>("wall.png");
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(sprite,
                    new Rectangle(XPosition, YPosition, Width, Height),
                    Color.White);
        }
    }
}
