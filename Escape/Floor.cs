using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Escape
{
    class Floor : Obstacle
    {
        public Vector2 Position { get; set; }
        public Texture2D FloorTexture { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public Floor(int x, int y)
        {
            Width = 50;
            Height = 50;
            Position = new Vector2(x, y);
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(FloorTexture, new Rectangle((int)Position.X, (int)Position.Y, Width, Height), Color.White);
        }

        public void LoadContent(ContentManager cm)
        {
            FloorTexture = cm.Load<Texture2D>("tile_50_50.png");
        }

    }
}
