using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Escape
{
    class Floor : SpriteEntity
    {
        public Floor(ContentManager cm, int x, int y) : base(cm, "tile_50_50.png")
        {
            Scale = 0.5f;
            Position = new Vector2(x, y);
        }
        public override void Update(GameTime gt, Screen s)
        {
            throw new NotImplementedException("No Update() for Floor");
        }
    }
}
