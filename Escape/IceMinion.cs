using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TexturePackerLoader;

namespace Escape
{
    class IceMinion : Ghost
    {

        public IceMinion(ContentManager cm, SpriteRender sr, Entity target, Vector2 position, int hp = 3)
            : base(cm, sr, target, position)
        {
            this.hp = hp;
        }
        public override float TouchFreezeTimer
        {
            get
            {
                return 0.5f;
            }
        }

        public override Color Tint
        {
            get
            {
                if (Health > 0)
                {
                    return Color.Lerp(Color.Cyan, base.Tint, 0.5f);
                }
                else
                {
                    return base.Tint;
                }
            }
        }
    }
}
