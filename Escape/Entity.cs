using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Escape
{
    abstract class Entity
    {
        abstract public Rectangle HitBox
        {
            get;
        }
        abstract public void Draw(SpriteBatch sb);
        abstract public void Update(GameTime gt, Screen s);
    }
}

