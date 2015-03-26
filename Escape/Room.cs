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
        public List<Wall> Walls { get; set; }
        public List<Obstacle> Obstacles { get; set; }

        public List<Enemy> Enemies { get; set; }
        public List<Item> Items { get; set; }

        public Room()
        {
            this.Width = 1000;
            this.Height = 600;

            Floors = new List<Floor>();
            for (int i = 1; i < this.Width / 25; i++)
            {
                for (int j = 1; j < this.Height / 25; j++)
                {
                    Floors.Add(new Floor(25 * i, 25 * j));
                }
            }

            Walls = new List<Wall>();
            for (int i = 0; i < this.Width / 25; i++)
            {
                if (i == 0 || i == this.Width / 25 - 1)
                {
                    for (int j = 0; j < this.Height / 25; j++)
                    {
                        Walls.Add(new Wall(25 * i, 25 * j));
                    }
                }
                else
                {
                    Walls.Add(new Wall(25 * i, 0));
                    Walls.Add(new Wall(25 * i, 25 * (this.Height / 25 - 1)));
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

            foreach (Wall w in Walls)
            {
                w.Draw(sb);
            }
        }

        public void LoadContent(ContentManager cm)
        {
            foreach (Floor f in Floors)
            {
                f.LoadContent(cm);
            }

            foreach (Wall w in Walls)
            {
                w.LoadContent(cm);
            }
        }
    }
}
