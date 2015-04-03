using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Escape
{
    abstract class Screen
    {
        public Vector2 Position { get; set; }
        public Texture2D BackgroundTexture { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public Song BackgroundMusic { get; set; }

        abstract public void Draw(SpriteBatch sb);
        abstract public void LoadContent(ContentManager cm);
    }
}
