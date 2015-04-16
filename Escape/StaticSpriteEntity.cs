using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Escape
{
    class StaticSpriteEntity : SpriteEntity
    {
        public StaticSpriteEntity(ContentManager cm, string spriteName, Rectangle? sourceRect = null)
            : base(cm, spriteName, sourceRect)
        {
            // empty
        }

        public override void Update(GameTime gt, Screen s)
        {
            // empty
        }
    }
}
