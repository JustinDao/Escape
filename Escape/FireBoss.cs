using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TexturePackerLoader;

namespace Escape
{
    class FireBoss : EnemyPatrol
    {
        public override string[] DownSprites
        {
            get
            {
                return new string[] {
                    TexturePackerMonoGameDefinitions.FireBossSprites.Layer_1,
                    TexturePackerMonoGameDefinitions.FireBossSprites.Layer_2,
                    TexturePackerMonoGameDefinitions.FireBossSprites.Layer_3,
                    TexturePackerMonoGameDefinitions.FireBossSprites.Layer_4,
                    TexturePackerMonoGameDefinitions.FireBossSprites.Layer_5,
                    TexturePackerMonoGameDefinitions.FireBossSprites.Layer_6,
                    TexturePackerMonoGameDefinitions.FireBossSprites.Layer_7,
                    TexturePackerMonoGameDefinitions.FireBossSprites.Layer_8,
                };
            }
        }
        public override string[] UpSprites
        {
            get
            {
                return DownSprites;
            }
        }
        public override string[] LeftSprites
        {
            get
            {
                return DownSprites;
            }
        }
        public override string[] RightSprites
        {
            get
            {
                return DownSprites;
            }
        }

        public override int SpriteInterval
        {
            get { return 100; }
        }

        public override float MaxSpeed
        {
            get { return 400; }
        }


        private float currentFireballInterval = 0;
        private float fireballInterval = 1;

        public FireBoss(ContentManager cm, SpriteRender sr, Vector2[] patrolPoints)
            : base(cm, sr, "fireboss_sprite_sheet.png", patrolPoints)
        {
            Scale = 1.5f;
        }

        public override void CollideObstacles(Room room)
        {
            foreach (var wall in room.Walls)
            {
                collideObstacle(wall);
            }
            foreach (var e in room.Obstacles)
            {
                if (e is Hole) continue;

                collideObstacle(e);
            }
        }

        public override void Update(GameTime gt, Screen s)
        {
            Console.WriteLine("FireBoss update()");
            currentFireballInterval += (float)gt.ElapsedGameTime.TotalSeconds;

            if (currentFireballInterval > fireballInterval)
            {
                Console.WriteLine("Time to boom!");
                if (s is Castle)
                {
                    Console.WriteLine("It's a castle!!!");
                    var castle = s as Castle;
                    var player = castle.Player;

                    ShootFireBall(castle, player);
                }
                currentFireballInterval -= fireballInterval;
            }

            base.Update(gt, s);
        }

        public void ShootFireBall(Castle castle, Player player)
        {
            var direction = player.Position - this.Position;
            direction.Normalize();
            Console.WriteLine("BOOM!!!");
            castle.CurrentRoom.AddFireBall(this.Position, direction, evil: true);
        }

    }
}
