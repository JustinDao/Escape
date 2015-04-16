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
        public Vector2 Position;
        abstract public Rectangle HitBox
        {
            get;
        }
        public Vector2 Center
        {
            get
            {
                return new Vector2(HitBox.Center.X, HitBox.Center.Y);
            }
            set
            {
                var diff = value - Center;
                Position += diff;
            }
        }
        abstract public void Draw(SpriteBatch sb);
        abstract public void Update(GameTime gt, Screen s);
    }
}

