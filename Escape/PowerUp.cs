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
		public bool IsStrength;
		public PowerUp(ContentManager cm, Vector2 pos, string spriteName, bool isFire, bool isIce, bool isStrength)
            : base(cm, spriteName)
        {
            Scale = 0.5f;
            Position = pos;
            IsFire = isFire;
            IsIce = isIce;
			IsStrength = isStrength;
        }
        public override void Update(GameTime gt, Screen s)
        {
        }
    }
}
