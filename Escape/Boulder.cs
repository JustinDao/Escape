using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Escape
{
    class Boulder : SpriteEntity
    {
        public Boulder(ContentManager cm, string spriteName, Vector2 position)
            : base(cm, spriteName)
        {
            Origin = new Vector2(0.5f);
            Position = position;
        }

        public override void Update(GameTime gt, Screen s)
        {

        }
    }
}
