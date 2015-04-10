using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Escape
{
    class MacGuffin : SpriteEntity
    {
        MainGame mg;

        public MacGuffin(MainGame mg, ContentManager cm)
            : base(cm, "final.png")
        {
            this.mg = mg;
            Position = new Vector2(mg.GAME_WIDTH / 2 - this.HitBox.Width / 2, 100);
        }

        public override void Update(GameTime gt, Screen s)
        {
            // nothing!
        }
    }
}
