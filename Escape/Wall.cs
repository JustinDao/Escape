using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;

namespace Escape
{
    class Wall : SpriteEntity
    {
        public Wall(ContentManager cm, int x, int y) : base(cm, "wall_50_50.png")
        {
            Scale = 0.5f;
            Position = new Vector2(x, y);
        }
        public override void Update(GameTime gt, Screen s)
        {
            throw new NotImplementedException("No Update() for Wall");
        }
    }
}
