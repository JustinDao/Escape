﻿using Microsoft.Xna.Framework;
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
        ContentManager contentManager;

        public Vector2 Position { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }

        public List<Floor> Floors { get; set; }
        public List<Wall> Walls { get; set; }
        public List<Obstacle> Obstacles { get; set; }

        public List<Enemy> Enemies { get; set; }
        public List<Item> Items { get; set; }

        public Room(MainGame mg)
        {
            this.Width = 1000;
            this.Height = 600;

            this.Enemies = new List<Enemy>();
            Enemies.Add(new Enemy(mg, 200, 200));

            Floors = new List<Floor>();
            for (int i = 0; i < this.Width / 25; i++)
            {
                for (int j = 0; j < this.Height / 25; j++)
                {
                    Floors.Add(new Floor(25 * i, 25 * j));
                }
            }

            Walls = new List<Wall>();
			Obstacles = new List<Obstacle>();

            for (int i = 0; i < this.Width / 25; i++)
            {
                if (i == 0 || i == this.Width / 25 - 1)
                {
                    for (int j = 0; j < this.Height / 25; j++)
                    {
						if (j != this.Height / 2 / 25 && j != (this.Height / 2 / 25) - 1) 
						{
							Walls.Add(new Wall (25 * i, 25 * j));
						} 
						else 
						{
							Obstacles.Add(new Door (25 * i, 25 * j));
						}
                        
                    }
                }
				else if (i == this.Width / 2 / 25 || i == (this.Width / 2 / 25) - 1) 
				{
					Obstacles.Add(new Door(25 * i, 0));
					Obstacles.Add(new Door(25 * i, 25 * (this.Height / 25 - 1)));
                }
                else
                {
                    Walls.Add(new Wall(25 * i, 0));
                    Walls.Add(new Wall(25 * i, 25 * (this.Height / 25 - 1)));
                }

            }


			Obstacles.Add(new Hole(300, 300, 0));
			Obstacles.Add(new Hole(400, 400, 1));
			Obstacles.Add(new Hole(425, 400, 2));
			Obstacles.Add(new Hole(450, 400, 2));
			Obstacles.Add(new Hole(475, 400, 3));
			Obstacles.Add(new Hole(475, 425, 4));
			Obstacles.Add(new Hole(475, 450, 5));
			Obstacles.Add(new Hole(450, 450, 6));
			Obstacles.Add(new Hole(425, 450, 6));
			Obstacles.Add(new Hole(400, 450, 7));
			Obstacles.Add(new Hole(400, 425, 8));
			Obstacles.Add(new Hole(425, 425, 9));
			Obstacles.Add(new Hole(450, 425, 9));
        }

        public void Update(GameTime gameTime)
        {
            List<Obstacle> toRemove = new List<Obstacle>();

            foreach (Obstacle o in Obstacles)
            {
				if (!(o is Hole) && !(o is Door))
                {
                    o.Update(gameTime);

                    if (o is FireBall)
                    {
                        FireBall f = (FireBall)o;

                        if (f.Position.X < 0 || f.Position.X > Width)
                        {
                            toRemove.Add(f);
                        }
                        else if (f.Position.Y < 0 || f.Position.Y > Height)
                        {
                            toRemove.Add(f);
                        }
                    }
                }
            }

            Obstacles = Obstacles.Except(toRemove).ToList();

            foreach (Enemy e in Enemies)
            {
                e.Update(gameTime, this);
            }

            checkFireBallEnemyCollisions();
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

            foreach (Enemy e in Enemies)
            {
                e.Draw(sb);
            }
			
			foreach (Obstacle o in Obstacles)
			{
				o.Draw(sb);
			}

        }

        public void LoadContent(ContentManager cm)
        {
            contentManager = cm;

            foreach (Floor f in Floors)
            {
                f.LoadContent(cm);
            }

            foreach (Wall w in Walls)
            {
                w.LoadContent(cm);
            }

            foreach (Enemy e in Enemies)
            {
                e.LoadContent(cm);
            }

            foreach (Obstacle o in Obstacles)
			{
                o.LoadContent(cm);
            }
		}

        public void AddFireBall(Vector2 position, Direction dir)
        {
            FireBall f = new FireBall(position, dir);
            f.LoadContent(contentManager);
            Obstacles.Add(f);
        }

        private void checkFireBallEnemyCollisions()
        {
            List<Enemy> enemiesToRemove = new List<Enemy>();
            List<Obstacle> fireBallsToRemove = new List<Obstacle>();

            foreach(Obstacle o in Obstacles)
            {
                if(o is FireBall)
                {
                    FireBall f = (FireBall)o;

                    foreach(Enemy e in Enemies)
                    {
                        if (e.HitBox.Intersects(f.HitBox))
                        {
                            fireBallsToRemove.Add(f);
                            enemiesToRemove.Add(e);
                            break;
                        }
                    }
                }
            }

            Obstacles = Obstacles.Except(fireBallsToRemove).ToList();
            Enemies = Enemies.Except(enemiesToRemove).ToList();
        }
    }
}
