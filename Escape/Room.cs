﻿using Microsoft.Xna.Framework;
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
        MainGame mg;
        ContentManager contentManager;
        Castle castle;

        public Vector2 Position { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }

        public List<Floor> Floors { get; set; }
        public List<Wall> Walls { get; set; }
        public List<Entity> Obstacles { get; set; }
        public List<Projectile> Projectiles = new List<Projectile>();
        public List<PowerUp> PowerUps = new List<PowerUp>();
		public List<Boulder> Boulders = new List<Boulder>();

        public List<Enemy> Enemies = new List<Enemy>();
        public Dictionary<Enemy, float> DyingEnemies = new Dictionary<Enemy, float>();
        public List<Item> Items { get; set; }

        public Dictionary<Direction, Room> Neighbors { get; set; }

        public Random Rng = new Random();

        public Room LeftRoom
        {
            get
            {
                return this.Neighbors[Direction.LEFT];
            }
            set
            {
                this.Neighbors.Add(Direction.LEFT, value);
                this.Doors.Add(Direction.LEFT, new Door(contentManager, 0, 11 * 25, true));

            }
        }

        public Room RightRoom
        {
            get
            {
                return this.Neighbors[Direction.RIGHT];
            }
            set
            {
                this.Neighbors.Add(Direction.RIGHT, value);
                this.Doors.Add(Direction.RIGHT, new Door(contentManager, this.mg.GAME_WIDTH - 25, 11 * 25, true));

            }
        }

        public Room UpRoom
        {
            get
            {
                return this.Neighbors[Direction.UP];
            }
            set
            {
                this.Neighbors.Add(Direction.UP, value);
                this.Doors.Add(Direction.UP, new Door(contentManager, 19 * 25, 0, false));

            }
        }

        public Room DownRoom
        {
            get
            {
                return this.Neighbors[Direction.DOWN];
            }
            set
            {
                this.Neighbors.Add(Direction.DOWN, value);
                this.Doors.Add(Direction.DOWN, new Door(contentManager, 19 * 25, this.mg.GAME_HEIGHT - 25, false));

            }
        }

        public Dictionary<Direction, Door> Doors { get; set; }

        // Methods

        public Room(MainGame mg, Castle castle)
        {
            contentManager = mg.Content;
            this.castle = castle;
            this.mg = mg;
            this.Width = mg.GAME_WIDTH;
            this.Height = mg.GAME_HEIGHT;

            Enemies.Add(new Ghost(mg.Content, mg.SpriteRender, castle.Player, new Vector2(500, 500)));

            Floors = new List<Floor>();
            for (int i = 0; i < this.Width / 25; i++)
            {
                for (int j = 0; j < this.Height / 25; j++)
                {
                    Floors.Add(new Floor(contentManager, 25 * i, 25 * j));
                }
            }

            Walls = new List<Wall>();
            Obstacles = new List<Entity>();

            for (int i = 0; i < this.Width / 25; i++)
            {
                if (i == 0 || i == this.Width / 25 - 1)
                {
                    for (int j = 0; j < this.Height / 25; j++)
                    {
                        if (j != this.Height / 2 / 25 && j != (this.Height / 2 / 25) - 1)
                        {
                            Walls.Add(new Wall(contentManager, 25 * i, 25 * j));
                        }
                    }
                }
                else if (i == this.Width / 2 / 25 || i == (this.Width / 2 / 25) - 1)
                {
                    // Skip
                }
                else
                {
                    Walls.Add(new Wall(contentManager, 25 * i, 0));
                    Walls.Add(new Wall(contentManager, 25 * i, 25 * (this.Height / 25 - 1)));
                }

            }

            Doors = new Dictionary<Direction, Door>();
            Neighbors = new Dictionary<Direction, Room>();

            //Doors.Add(Direction.LEFT, new Door(0, 11 * 25, true));
            //Doors.Add(Direction.RIGHT, new Door(mg.GAME_WIDTH - 25, 11 * 25, true));
            //Doors.Add(Direction.UP, new Door(19 * 25, 0, false));
            //Doors.Add(Direction.DOWN, new Door(19 * 25, mg.GAME_HEIGHT - 25, false));

            Obstacles.Add(new Hole(contentManager, 300, 300, 0));
            Obstacles.Add(new Hole(contentManager, 400, 400, 1));
            Obstacles.Add(new Hole(contentManager, 425, 400, 2));
            Obstacles.Add(new Hole(contentManager, 450, 400, 2));
            Obstacles.Add(new Hole(contentManager, 475, 400, 3));
            Obstacles.Add(new Hole(contentManager, 475, 425, 4));
            Obstacles.Add(new Hole(contentManager, 475, 450, 5));
            Obstacles.Add(new Hole(contentManager, 450, 450, 6));
            Obstacles.Add(new Hole(contentManager, 425, 450, 6));
            Obstacles.Add(new Hole(contentManager, 400, 450, 7));
            Obstacles.Add(new Hole(contentManager, 400, 425, 8));
            Obstacles.Add(new Hole(contentManager, 425, 425, 9));
            Obstacles.Add(new Hole(contentManager, 450, 425, 9));

			Boulders.Add(new Boulder(contentManager, new Vector2(175, 300), castle.Player));

            PowerUps.Add(new PowerUp(contentManager, new Vector2(200, 300), "yellow.png", false, false, false, true));
            PowerUps.Add(new PowerUp(contentManager, new Vector2(500, 500), "naryu.png", false, true, false, false));
            PowerUps.Add(new PowerUp(contentManager, new Vector2(700, 500), "farore.png", false, false, true, false));
        }

        public Room(MainGame mg, Castle castle, String csvName)
        {
            contentManager = mg.Content;
            this.mg = mg;
            this.castle = castle;
            this.Width = mg.GAME_WIDTH;
            this.Height = mg.GAME_HEIGHT;

            Floors = new List<Floor>();
            Walls = new List<Wall>();
            Obstacles = new List<Entity>();
            Doors = new Dictionary<Direction, Door>();
            Neighbors = new Dictionary<Direction, Room>();

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
                                Floors.Add(new Floor(contentManager, 25 * x_count, 25 * y_count));
                                break;
                            case 1: // c
                                Walls.Add(new Wall(contentManager, 25 * x_count, 25 * y_count));
                                break;
                            // 2 - 11 are Holes
                            case 12:
                                Floors.Add(new Floor(contentManager, 25 * x_count, 25 * y_count, sprite: "Water\\Layer_1.png"));
                                break;
                            case 13:
                                Floors.Add(new Floor(contentManager, 25 * x_count, 25 * y_count, sprite: "Water\\Layer_2.png"));
                                break;
                            case 14:
                                Floors.Add(new Floor(contentManager, 25 * x_count, 25 * y_count, sprite: "Water\\Layer_3.png"));
                                break;
                            case 15:
                                Floors.Add(new Floor(contentManager, 25 * x_count, 25 * y_count, sprite: "Water\\Layer_4.png"));
                                break;
                            case 16:
                                Floors.Add(new Floor(contentManager, 25 * x_count, 25 * y_count, sprite: "Water\\Layer_5.png"));
                                break;
                            case 17:
                                Floors.Add(new Floor(contentManager, 25 * x_count, 25 * y_count, sprite: "Water\\Layer_6.png"));
                                break;
                            case 18:
                                Floors.Add(new Floor(contentManager, 25 * x_count, 25 * y_count, sprite: "Water\\Layer_7.png"));
                                break;
                            case 19:
                                Floors.Add(new Floor(contentManager, 25 * x_count, 25 * y_count, sprite: "Water\\Layer_8.png"));
                                break;
                            case 20:
                                Floors.Add(new Floor(contentManager, 25 * x_count, 25 * y_count, sprite: "Water\\Layer_9.png"));
                                break;
                            case 21:
                                Floors.Add(new Floor(contentManager, 25 * x_count, 25 * y_count, sprite: "Water\\Layer_10.png"));
                                break;
                            case 22:
                                Floors.Add(new Floor(contentManager, 25 * x_count, 25 * y_count, sprite: "Water\\Layer_11.png"));
                                break;
                            case 23:
                                Floors.Add(new Floor(contentManager, 25 * x_count, 25 * y_count, sprite: "StartRoomEntities\\Door.png"));
                                break;
                            case 24:
                                Floors.Add(new Floor(contentManager, 25 * x_count, 25 * y_count, sprite: "StartRoomEntities\\Layer_1.png"));
                                break;
                            case 25:
                                Floors.Add(new Floor(contentManager, 25 * x_count, 25 * y_count, sprite: "StartRoomEntities\\Layer_2.png"));
                                break;
                            case 26:
                                Floors.Add(new Floor(contentManager, 25 * x_count, 25 * y_count, sprite: "StartRoomEntities\\Layer_3.png"));
                                break;
                            case 27:
                                Floors.Add(new Floor(contentManager, 25 * x_count, 25 * y_count, sprite: "StartRoomEntities\\Layer_4.png"));
                                break;
                            case 28:
                                Floors.Add(new Floor(contentManager, 25 * x_count, 25 * y_count, sprite: "StartRoomEntities\\Layer_5.png"));
                                break;
                            case 29:
                                Floors.Add(new Floor(contentManager, 25 * x_count, 25 * y_count, sprite: "Candle\\Layer_1-002.png"));
                                break;
                            case 30:
                                Floors.Add(new Floor(contentManager, 25 * x_count, 25 * y_count, sprite: "Candle\\Layer_2-002.png"));
                                break;
                            case 31:
                                Floors.Add(new Floor(contentManager, 25 * x_count, 25 * y_count, sprite: "Candle\\Layer_3-002.png"));
                                break;
							case 32:
								Floors.Add(new Floor(contentManager, 25 * x_count, 25 * y_count));
							    Boulders.Add(new Boulder(contentManager, new Vector2((25 * x_count), (25 * y_count)), castle.Player));
								break;
                            default: // default to a hole
                                if (int.Parse(cells[x_count]) > 11) break;
                                Obstacles.Add(new Hole(contentManager, 25 * x_count, 25 * y_count, int.Parse(cells[x_count]) - 2));
                                break;
                        }
                    }

                    y_count++;
                }
            }
        }

        public void Update(GameTime gameTime, Screen s)
        {
            float delta = (float) gameTime.ElapsedGameTime.TotalSeconds;
            List<Projectile> outProjectiles = new List<Projectile>();

            foreach (Projectile p in Projectiles)
            {
                p.Update(gameTime, s);
                Rectangle bounds = new Rectangle(0, 0, Width, Height);
                bool outOfBounds = !bounds.Intersects(p.HitBox);
                bool outOfRange = p.Range > 0 && ((p.Position - p.StartPosition).Length() > p.Range);
                if (outOfBounds || outOfRange)
                {
                    outProjectiles.Add(p);
                }
            }

			foreach (Entity o in Obstacles)
			{
				o.Update(gameTime, s);
			}

			for (int i = 0; i < Boulders.Count(); i++)
			{
				var b = Boulders[i];
				b.Update(gameTime, s);
				if (b.Removed)
				{
					i--;
				}
			}

            Projectiles = Projectiles.Except(outProjectiles).ToList();

            foreach (Enemy e in Enemies)
            {
                e.Update(gameTime, this.castle);
            }

            var dying = DyingEnemies.Keys.ToList();
            foreach (Enemy e in dying)
            {
                var t = DyingEnemies[e] - delta;
                if (t <= 0)
                {
                    DyingEnemies.Remove(e);
                }
                else
                {
                    DyingEnemies[e] = t;
                }
            }

            checkProjectileCollisions();
            checkMeleeAttacks();
            removeDeadEnemies();
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

            foreach (Door d in Doors.Values)
            {
                if (d != null)
                {
                    d.Draw(sb);
                }
            }

            foreach (Entity o in Obstacles)
            {
                o.Draw(sb);
            }

            foreach (PowerUp p in PowerUps)
            {
                p.Draw(sb);
            }

			foreach (Boulder b in Boulders)
			{
				b.Draw(sb);
			}

            foreach (Enemy e in Enemies)
            {
                e.Draw(sb);
            }

            foreach (var pair in DyingEnemies)
            {
                Enemy e = pair.Key;
                float timeLeft = pair.Value;
                var percentLeft = timeLeft / e.DeathFadeTime;
                e.OverrideTint = new Color(percentLeft, 0, 0, percentLeft);
                e.Draw(sb);
            }

            foreach (Projectile p in Projectiles)
            {
                p.Draw(sb);
            }

        }

        public void AddSnowflakes(Vector2 position)
        {
            int numFlakes = 16;
            // var rand = (float)Rng.NextDouble();
            // var angleOffset = rand * 2 * (float)Math.PI;
            for (int i = 0; i < numFlakes; i++)
            {
                float angle = (float)(2 * Math.PI * ((float)i / (float)numFlakes)) /* + angleOffset */;
                float speed = 400;
                Vector2 vel = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * speed;
                Projectile anna = Projectile.CreateSnowflake(contentManager, position, vel, 100);
                Projectiles.Add(anna);
            }
        }

        public void AddFireBall(Vector2 position, Vector2 dir, Enemy evil = null)
        {
            Projectiles.Add(Projectile.CreateFireball(contentManager, position, dir * 500, evil));
        }
        public void AddBoulder(Vector2 position, Vector2 dir, Enemy owner = null)
        {
            Projectiles.Add(Projectile.CreateBoulder(contentManager, position, dir * 500, owner));
        }

        public Door LeftDoor()
        {
            return Doors.ContainsKey(Direction.LEFT) ? Doors[Direction.LEFT] : null;
        }

        public Door RightDoor()
        {
            return Doors.ContainsKey(Direction.RIGHT) ? Doors[Direction.RIGHT] : null;
        }

        public Door UpDoor()
        {
            return Doors.ContainsKey(Direction.UP) ? Doors[Direction.UP] : null;
        }

        public Door DownDoor()
        {
            return Doors.ContainsKey(Direction.DOWN) ? Doors[Direction.DOWN] : null;
        }

        private void checkMeleeAttacks()
        {
            var p = castle.Player;
            if (p.AttackArea.HasValue)
            {
                var area = p.AttackArea.Value;
                foreach (var baddie in Enemies)
                {
                    if (!baddie.HitBox.Intersects(area))
                    {
                        baddie.BeingAttacked = false;
                    }
                    else if (!baddie.BeingAttacked)
                    {
                        baddie.BeingAttacked = true;
                        baddie.Health--;
                    }
                }
            }
            else
            {
                foreach (var baddie in Enemies)
                {
                    baddie.BeingAttacked = false;
                }
            }
        }

        private void checkProjectileCollisions()
        {
            List<Projectile> projectilesToRemove = new List<Projectile>();
            // TODO move to enemy
            // TODO skip enemies already hit
            foreach (var p in Projectiles)
            {
                if (p.Evil)
                {
                    var player = castle.Player;
                    if (p.HitBox.Intersects(player.CollisionBox))
                    {
                        player.OnProjectileCollision(p);
                        projectilesToRemove.Add(p);
                    }
                    continue;
                }
                // else
                foreach (Enemy g in Enemies)
                {

                    if (g.HitBox.Intersects(p.HitBox))
                    {
                        switch (p.Type)
                        {
                            case ProjectileType.SNOWFLAKE:
                                g.FreezeTimer = 3;
                                projectilesToRemove.Add(p);
                                break;
                            case ProjectileType.FIREBALL:
                                if (g.Frozen)
                                {
                                    g.FreezeTimer = 0;
                                    projectilesToRemove.Add(p);
                                }
                                else
                                {
                                    g.Health--;
                                    projectilesToRemove.Add(p);
                                }
                                break;
                        }
                    }
                }
            }

            Projectiles = Projectiles.Except(projectilesToRemove).ToList();
        }

        private void removeDeadEnemies()
        {
            for (int i = 0; i < Enemies.Count(); i++)
            {
                var e = Enemies[i];
                if (e.Health <= 0)
                {
                    DyingEnemies.Add(e, e.DeathFadeTime);
                    Enemies.RemoveAt(i);
                    i--;
                    if (e.Drop != null)
                    {
                        var drop = e.Drop;
                        PowerUps.Add(drop);
                        drop.Position = new Vector2(e.HitBox.Center.X, e.HitBox.Center.Y);
                    }
                }
            }
        }
    }
}
