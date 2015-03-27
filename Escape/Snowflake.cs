using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Escape
{
    class Snowflake : Obstacle
    {
        public float Scale = 0.5f;
        public float Range;
        public Vector2 Velocity;
        public Vector2 Position;
        public Vector2 StartPosition;
        public Texture2D Texture;
        public Rectangle HitBox
        {
            get
            {
                return new Rectangle(
                    (int)Position.X,
                    (int)Position.Y,
                    (int)(Texture.Width*Scale),
                    (int)(Texture.Height*Scale)
                );
            }
        }

        public Snowflake(Vector2 position, Vector2 notAnEnum, float range)
        {
            StartPosition = position;
            Position = position;
            Velocity = notAnEnum; // for Justin
            Range = range;
        }

        public override void Draw(SpriteBatch sb)
        {
            Rectangle r = new Rectangle(
                (int)Position.X,
                (int)Position.Y,
                (int)(Texture.Width * Scale),
                (int)(Texture.Height * Scale)
            );
            sb.Draw(Texture, r, Color.White);
        }
        public override void Update(GameTime gt)
        {
            Position += Velocity * (float)gt.ElapsedGameTime.TotalSeconds;
        }
        public override void LoadContent(ContentManager cm)
        {
            Texture = cm.Load<Texture2D>("snowflake.png");
        }
    }
}
