using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Escape
{
    class PowerUp : SpriteEntity
    {
        public bool IsFire;
        public bool IsIce;
        public PowerUp(ContentManager cm, Vector2 pos, string spriteName, bool isFire, bool isIce)
            : base(cm, spriteName)
        {
            Position = pos;
            IsFire = isFire;
            IsIce = isIce;
        }
        public override void Update(GameTime gt, Screen s)
        {
            throw new NotImplementedException("No Update() for PowerUp");
        }
    }
}
