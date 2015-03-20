using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;

namespace Escape
{
    abstract class Bar
    {
        public Rectangle Box;
        public Texture2D Texture;
        public Vector2 Coor;
        public int XPosition;
        public int YPosition;
        public int Width = 100;
        public int Height = 20;
        public Color BoxColor;
    }
}
