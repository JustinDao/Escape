using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TexturePackerLoader;

namespace Escape
{
    abstract class EnemyFollow : Character
    {
        public AnimatedSpriteEntity Target;

        public EnemyFollow(ContentManager cm, SpriteRender sr, AnimatedSpriteEntity target,
            string spriteSheetName)
            : base(cm, sr, spriteSheetName)
        {
            Target = target;
        }

        public override Vector2 CurrentVelocity
        {
            get
            {
                if (Target != null)
                {
                    var dist = Target.Position - Position;
                    dist.Normalize();
                    return dist * MaxSpeed;
                }
                else
                {
                    return Vector2.Zero;
                }
            }
        }
    }
}
