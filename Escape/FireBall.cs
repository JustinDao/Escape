using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Escape
{
	class FireBall : Obstacle
	{
        public Vector2 Position { get; set; }
        public Texture2D Texture { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public Direction Dir { get; set; }
        public int Speed { get; set; }
        public Rectangle HitBox
        {
            get { return new Rectangle((int)Position.X, (int)Position.Y, Width, Height); }
        }

		public FireBall (Vector2 position, Direction dir)
		{
            Position = new Vector2(position.X, position.Y);
            Dir = dir;
            Speed = 10;
            Width = 23;
            Height = 32;
		}

        override public void Update(GameTime gt)
        {
            switch (Dir) 
            {
                case Direction.N:
                    Position += new Vector2(0, -Speed);
                    break;
                case Direction.NE:
                    Position += new Vector2(Speed, -Speed);
                    break;
                case Direction.E:
                    Position += new Vector2(Speed, 0);
                    break;
                case Direction.SE:
                    Position += new Vector2(Speed, Speed);
                    break;
                case Direction.S:
                    Position += new Vector2(0, Speed);
                    break;
                case Direction.SW:
                    Position += new Vector2(-Speed, Speed);
                    break;
                case Direction.W:
                    Position += new Vector2(-Speed, 0);
                    break;
                case Direction.NW:
                    Position += new Vector2(-Speed, -Speed);
                    break;
            }

        }

        override public void Draw(SpriteBatch sb)
        {
            sb.Draw(Texture, new Rectangle((int)Position.X, (int)Position.Y, Width, Height), Color.White);
        }

        override public void LoadContent(ContentManager cm)
        {
            Texture = cm.Load<Texture2D>("fireball.png");
        }




	}
}

