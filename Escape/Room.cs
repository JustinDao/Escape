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

        public List<Floor> Floors { get; set; }
        public List<Obstacle> Obstacles { get; set; }

        public List<Enemy> Enemies { get; set; }
        public List<Item> Items { get; set; }

        public Room()
        {
            Floors = new List<Floor>();
            for (int i = 0; i < 600 / 50; i++)
            {
                for (int j = 0; j < 600 / 50; j++)
                {
                    Floors.Add(new Floor(50 * i, 50*j));
                }
            }
        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch sb)
        {
            foreach (Floor f in Floors) 
            {
                f.Draw(sb);
            }
        }

        public void LoadContent(ContentManager cm)
        {
            foreach (Floor f in Floors)
            {
                f.LoadContent(cm);
            }
        }
    }
}
