using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Audio;

namespace Escape
{   
    abstract class SpriteSheet
    {
        protected int spriteX, spriteY;
        protected int spriteWidth, spriteHeight;
        protected Texture2D spriteSheet;

        public SpriteSheet()
        {
        }
    }
}