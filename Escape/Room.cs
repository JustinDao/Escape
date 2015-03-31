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

            Obstacles.Add(new PowerUp(new Vector2(175, 75), "din.png", true, false));
            Obstacles.Add(new PowerUp(new Vector2(500, 500), "naryu.png", false, true));
		}

		public void Update(GameTime gameTime)
		{
			List<Obstacle> toRemove = new List<Obstacle>();

			foreach (Obstacle o in Obstacles)
			{                
				if (o is FireBall)
				{
					o.Update(gameTime);

					FireBall f = (FireBall)o;

					if (f.Position.X < 0 || f.Position.X > Width)
					{
						toRemove.Add(f);
					}
					else if (f.Position.Y < 0 || f.Position.Y > Height)
					{
						toRemove.Add(f);
					}
                } // fuck visual studio >:(
                else if (o is Snowflake)
                {
                    Snowflake s = o as Snowflake;
                    s.Update(gameTime);
                    Rectangle bounds = new Rectangle(0, 0, Width, Height);
                    if (!bounds.Contains(s.Position) || (s.Position - s.StartPosition).Length() > s.Range)
                    {
                        toRemove.Add(s);
                    }
                }
			}

			Obstacles = Obstacles.Except(toRemove).ToList();

			foreach (Enemy e in Enemies)
			{
				e.Update(gameTime, this);
			}

			checkFireBallEnemyCollisions();
            checkSnowflakeEnemyCollisions();
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

        public void Elsa(Vector2 position)
        {
            int numFlakes = 16;
            for (int i = 0; i < numFlakes; i++)
            {
                float angle = (float)(2 * Math.PI * ((float)i / (float)numFlakes));
                float speed = 400;
                Vector2 vel = new Vector2((float)Math.Cos(angle),(float)Math.Sin(angle)) * speed;
                Snowflake anna = new Snowflake(position, vel, 100); // love = open door
                anna.LoadContent(contentManager);
                Obstacles.Add(anna);
            }
        }

		public void AddFireBall(Vector2 position, Vector2 dir)
		{
			FireBall f = new FireBall(position, dir);
			f.LoadContent(contentManager);
			Obstacles.Add(f);
		}

        private void checkSnowflakeEnemyCollisions()
        {
            foreach(Obstacle o in Obstacles)
            {
                var s = o as Snowflake;
                if (s == null) continue;
                foreach(Enemy e in Enemies)
                {
                    if (e.HitBox.Intersects(s.HitBox))
                    {
                        e.Frozen = true;
                    }
                }
            }
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
                            if (e.Frozen)
                            {
                                e.Frozen = false;
                            }
                            else
                            {
                                enemiesToRemove.Add(e);
                            }
                            fireBallsToRemove.Add(f);
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
