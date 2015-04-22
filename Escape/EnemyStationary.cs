using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TexturePackerLoader;

namespace Escape
{
    abstract class EnemyStationary : Enemy
    {
        int targetIndex = 0;
        float lastDelta;

        public override float AnimationSpeed
        {
            get
            {
                return base.SpeedMult;
            }
        }

        public EnemyStationary(ContentManager cm, SpriteRender sr, string spriteSheetName, Vector2 position)
            : base(cm, sr, spriteSheetName)
        {
            this.Position = position;
        }

        public override void Update(GameTime gt, Screen s)
        {
            base.Update(gt, s);
        }

        public override Vector2 CurrentVelocity
        {
            get
            {
                return Vector2.Zero;
            }
        }
    }
}
