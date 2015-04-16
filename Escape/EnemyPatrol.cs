using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TexturePackerLoader;

namespace Escape
{
    abstract class EnemyPatrol : Enemy
    {
        public Vector2[] PatrolPoints;
        int targetIndex = 0;
        float lastDelta;

        public EnemyPatrol(ContentManager cm, SpriteRender sr,
            string spriteSheetName, params Vector2[] patrolPoints)
            : base(cm, sr, spriteSheetName)
        {
            PatrolPoints = patrolPoints;
            Position = patrolPoints[0];
        }

        public override void Update(GameTime gt, Screen s)
        {
            float delta = (float) gt.ElapsedGameTime.TotalSeconds;
            lastDelta = delta;
            var target = PatrolPoints[targetIndex];
            var distance = (target - Position).Length();
            if (distance <= MaxSpeed * delta)
            {
                targetIndex = (targetIndex + 1) % PatrolPoints.Count();
            }
            base.Update(gt, s);
        }

        public override Vector2 CurrentVelocity
        {
            get
            {
                var target = PatrolPoints[targetIndex];
                var direction = target - Position;
                if (direction.Length() < MaxSpeed * lastDelta) return Vector2.Zero;
                direction.Normalize();
                return direction * MaxSpeed;
            }
        }
    }
}
