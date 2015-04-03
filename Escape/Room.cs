using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
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

        public Room LeftRoom { get; set; }
        public Room RightRoom { get; set; }
        public Room UpRoom { get; set; }
        public Room DownRoom { get; set; }

        public Door LeftDoor { get; set; }
        public Door RightDoor { get; set; }
        public Door UpDoor { get; set; }
        public Door DownDoor { get; set; }

        public Room(MainGame mg)
        {
            this.Width = mg.GAME_WIDTH;
            this.Height = mg.GAME_HEIGHT;

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
                            Walls.Add(new Wall(25 * i, 25 * j));
                        }
                    }
                }
                else if (i == this.Width / 2 / 25 || i == (this.Width / 2 / 25) - 1)
                {
                    // Skip
                }
                else
                {
                    Walls.Add(new Wall(25 * i, 0));
                    Walls.Add(new Wall(25 * i, 25 * (this.Height / 25 - 1)));
                }

            }

            LeftDoor = new Door(0, 11 * 25, true);
            RightDoor = new Door(mg.GAME_WIDTH - 25, 11 * 25, true);
            UpDoor = new Door(19 * 25, 0, false);
            DownDoor = new Door(19 * 25, mg.GAME_HEIGHT - 25, false);

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

        public Room(MainGame mg, String csvName)
        {
            this.Width = mg.GAME_WIDTH;
            this.Height = mg.GAME_HEIGHT;

            Floors = new List<Floor>();
            Walls = new List<Wall>();
            Obstacles = new List<Obstacle>();
            Enemies = new List<Enemy>();

            //http://stackoverflow.com/questions/25331714
            var path = @"Content\Rooms\" + csvName;

            using (var stream = TitleContainer.OpenStream(path))
            using (var reader = new StreamReader(stream))
            {
                int y_count = 0;

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();

                    string[] cells = line.Split(',');

                    for (int x_count = 0; x_count < cells.Length; x_count++)
                    {
                        switch (int.Parse(cells[x_count]))
                        {
                            case 0: // floor
                                Floors.Add(new Floor(25 * x_count, 25 * y_count));
                                break;
                            case 1: // wall
                                Walls.Add(new Wall(25 * x_count, 25 * y_count));
                                break;
                        }
                    }

                    y_count++;
                }
            }
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
                }
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

            List<Door> doors = new List<Door>();
            doors.Add(LeftDoor);
            doors.Add(RightDoor);
            doors.Add(UpDoor);
            doors.Add(DownDoor);

            foreach (Door d in doors)
            {
                if (d != null)
                {
                    d.Draw(sb);
                }
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

            List<Door> doors = new List<Door>();
            doors.Add(LeftDoor);
            doors.Add(RightDoor);
            doors.Add(UpDoor);
            doors.Add(DownDoor);

            foreach(Door d in doors)
            {
                if(d != null)
                {
                    d.LoadContent(cm);
                }
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

        public void AddSnowflakes(Vector2 position)
        {
            int numFlakes = 16;
            for (int i = 0; i < numFlakes; i++)
            {
                float angle = (float)(2 * Math.PI * ((float)i / (float)numFlakes));
                float speed = 400;
                Vector2 vel = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * speed;
                Snowflake anna = new Snowflake(position, vel, 100);
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
            foreach (Obstacle o in Obstacles)
            {
                var s = o as Snowflake;
                if (s == null) continue;
                foreach (Enemy e in Enemies)
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

            foreach (Obstacle o in Obstacles)
            {
                if (o is FireBall)
                {
                    FireBall f = (FireBall)o;

                    foreach (Enemy e in Enemies)
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
