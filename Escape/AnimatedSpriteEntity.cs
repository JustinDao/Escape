using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TexturePackerLoader;

namespace Escape
{
    abstract class AnimatedSpriteEntity : Entity
    {
        // transform stuff
        public float Scale = 1f;
        public Vector2 Position;
        public Vector2 Origin;
        public float Rotation = 0f; // DEGREES CCW :(
        public Color Tint = Color.White;
        public float Depth = 0f;

        public TPSpriteSheet SpriteSheet;
        public SpriteRender SpriteRender;

        public abstract string SpriteString { get; }

        public Rectangle SourceRect
        {
            get
            {
                return this.SpriteSheet.Sprite(SpriteString).SourceRectangle;
            }
        }

        public AnimatedSpriteEntity(ContentManager cm, SpriteRender sr, 
            string spriteSheetName, Rectangle? sourceRect = null)
        {
            this.SpriteRender = sr;
            var spriteSheetLoader = new SpriteSheetLoader(cm);
            this.SpriteSheet = spriteSheetLoader.Load(spriteSheetName);
            
        }

        public override void Draw(SpriteBatch sb)
        {
            this.SpriteRender.Draw(
                this.SpriteSheet.Sprite(
                    SpriteString
                ),
                Position,
                Tint,
                Rotation,
                Scale,
                SpriteEffects.None
            );
    
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
