using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Escape
{
    class Sprite
    {
        public Texture2D Texture;
        public Rectangle SourceRect;
        public Sprite(ContentManager cm, string texName, Rectangle? rect = null)
            : this(cm, cm.Load<Texture2D>(texName), rect)
        {
            // this constructor intentionally left blank
        }
        public Sprite(ContentManager cm, Texture2D tex, Rectangle? rect = null)
        {
            Texture = tex;
            SourceRect = rect.GetValueOrDefault(new Rectangle(0, 0, Texture.Width, Texture.Height));
        }
    }
}
