using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Escape
{
    class Room
    {

        public Vector2 Position { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public List<Obstacle> Obstacles { get; set; }
        public List<Enemy> Enemies { get; set; }
        public List<Item> Items { get; set; }

        public void Initialize(ContentManager cm)
        {
        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch sb)
        {

        }

        public void LoadContent(ContentManager cm)
        {

        }
    }
}
