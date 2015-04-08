using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Escape
{
    abstract class SpriteEntity : Entity
    {
        // transform stuff
        public float Scale = 1f;
        public Vector2 Position;
        public Vector2 Origin;
        public float Rotation = 0f; // DEGREES CCW :(
        public Color Tint = Color.White;
        public float Depth = 0f;

        public Sprite CurrentSprite;
        public Texture2D Texture
        {
            get
            {
                return CurrentSprite.Texture;
            }
        }
        public Rectangle SourceRect
        {
            get
            {
                return CurrentSprite.SourceRect;
            }
        }
        public SpriteEntity(ContentManager cm, string spriteName, Rectangle? sourceRect = null)
        {
            CurrentSprite = new Sprite(cm, spriteName, sourceRect);
        }
        public override void Draw(SpriteBatch batch)
        {
            var tempOrigin = new Vector2(Origin.X * SourceRect.Width, Origin.Y * SourceRect.Height) * Scale;
            batch.Draw(Texture, Position, SourceRect, Tint, Rotation, tempOrigin, Scale, SpriteEffects.None, Depth);
        }
        public override Rectangle HitBox
        {
            // TODO lol how do i rotation?
            get
            {
                int x = (int)(Position.X - SourceRect.Width * Scale * Origin.X);
                int y = (int)(Position.Y - SourceRect.Height * Scale * Origin.Y);
                int w = (int)(SourceRect.Width * Scale);
                int h = (int)(SourceRect.Height * Scale);
                return new Rectangle(x, y, w, h);
            }
        }
    }
}
