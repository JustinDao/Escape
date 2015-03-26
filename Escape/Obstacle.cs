using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Escape
{
    abstract class Obstacle
    {
        abstract public void Draw(SpriteBatch sb);
        abstract public void Update(GameTime gt);
        abstract public void LoadContent(ContentManager cm);
    }
}

