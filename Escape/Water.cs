using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Escape
{
    class Water : StaticSpriteEntity
    {
        public bool IsFrozen = false;

        public override Color Tint
        {
            get
            {
                if (this.IsFrozen)
                {
                    return Color.Cyan;
                }

                return base.Tint;
            }
        }

        public Water(ContentManager cm, string spriteName, int x, int y)
            : base(cm, spriteName)
        {
            Scale = 0.5f;
            Position = new Vector2(x, y);
        }
        public override void Update(GameTime gt, Screen s)
        {
            // Nothing
        }

        public void Freeze()
        {
            this.IsFrozen = true;
        }
    }
}
