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
        public bool IsSpeed;
        public PowerUp(ContentManager cm, Vector2 pos, string spriteName, bool isFire, bool isIce, bool isSpeed)
            : base(cm, spriteName)
        {
            Scale = 0.5f;
            Position = pos;
            IsFire = isFire;
            IsIce = isIce;
            IsSpeed = isSpeed;
        }
        public override void Update(GameTime gt, Screen s)
        {
            throw new NotImplementedException("No Update() for PowerUp");
        }
    }
}
