using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TexturePackerLoader;

namespace Escape
{
    abstract class Enemy : Character
    {

        public float FreezeTimer = 0;
        public bool Frozen
        {
            get
            {
                return FreezeTimer > 0;
            }
        }
        public override float SpeedMult
        {
            get
            {
                return Frozen ? 0.1f : 1f;
            }
        }

        public override Color Tint
        {
            get
            {
                return Frozen ? Color.Cyan : base.Tint;
            }
        }

        public Enemy(ContentManager cm, SpriteRender sr, string spriteSheetName)
            : base(cm, sr, spriteSheetName)
        {

        }

        public override void Update(GameTime gt, Screen s)
        {
            if (FreezeTimer > 0)
            {
                FreezeTimer -= (float)gt.ElapsedGameTime.TotalSeconds;
            }
            base.Update(gt, s);
        }

    }
}
