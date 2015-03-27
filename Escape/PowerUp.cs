using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Escape
{
    class PowerUp : Obstacle
    {
        public string SpriteName;
        public float Scale = 0.5f;
        public Vector2 Position;
        public Texture2D Texture;
        public bool IsFire;
        public bool IsIce;

        public Rectangle HitBox
        {
            get
            {
                return new Rectangle(
                    (int)Position.X,
                    (int)Position.Y,
                    (int)(Texture.Width * Scale),
                    (int)(Texture.Height * Scale)
                );
            }
        }

        public PowerUp(Vector2 position, string spriteName, bool isFire, bool isIce)
        {
            Position = position;
            SpriteName = spriteName;
            this.IsFire = isFire;
            this.IsIce = isIce;
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
        }
        public override void LoadContent(ContentManager cm)
        {
            Texture = cm.Load<Texture2D>(SpriteName);
        }
    }
}
