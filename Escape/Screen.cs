using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Escape
{
    abstract class Screen
    {
        public Vector2 position;
        public Texture2D backgroundTexture;
        public int width;
        public int height;
        public Song backgroundMusic;

        public void Initialize(ContentManager cm);
        public void Update(GameTime gt);
        public void Draw(SpriteBatch sb);
    }
}
