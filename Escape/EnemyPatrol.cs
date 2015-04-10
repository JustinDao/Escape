using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TexturePackerLoader;

namespace Escape
{
    abstract class EnemyPatrol : Character
    {
        public Vector2[] PatrolPoints;
        int targetIndex = 0;

        public EnemyPatrol(ContentManager cm, SpriteRender sr,
            string spriteSheetName, Vector2[] patrolPoints)
            : base(cm, sr, spriteSheetName)
        {
            PatrolPoints = patrolPoints;
            Position = patrolPoints[0];
        }

        public override void Update(GameTime gt, Screen s)
        {
            var delta = gt.ElapsedGameTime.TotalSeconds;
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
                direction.Normalize();
                return direction * MaxSpeed;
            }
        }
    }
}
