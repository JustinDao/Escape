using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TexturePackerLoader;

namespace Escape
{
    class EarthBoss : EnemyPatrol
    {
        public override int MaxHealth
        {
            get
            {
                return 10;
            }
        }
        public override string[] DownSprites
        {
            get
            {
                return new string[] {

                    TexturePackerMonoGameDefinitions.EarthBossSprites.Layer_1,
                    TexturePackerMonoGameDefinitions.EarthBossSprites.Layer_10,
                    TexturePackerMonoGameDefinitions.EarthBossSprites.Layer_11,
                    TexturePackerMonoGameDefinitions.EarthBossSprites.Layer_12,
                    TexturePackerMonoGameDefinitions.EarthBossSprites.Layer_13,
                    TexturePackerMonoGameDefinitions.EarthBossSprites.Layer_15,
                    TexturePackerMonoGameDefinitions.EarthBossSprites.Layer_16,
                    TexturePackerMonoGameDefinitions.EarthBossSprites.Layer_17,
                    TexturePackerMonoGameDefinitions.EarthBossSprites.Layer_18,
                    TexturePackerMonoGameDefinitions.EarthBossSprites.Layer_19,
                    TexturePackerMonoGameDefinitions.EarthBossSprites.Layer_2,
                    TexturePackerMonoGameDefinitions.EarthBossSprites.Layer_20,
                    TexturePackerMonoGameDefinitions.EarthBossSprites.Layer_21,
                    TexturePackerMonoGameDefinitions.EarthBossSprites.Layer_22,
                    TexturePackerMonoGameDefinitions.EarthBossSprites.Layer_23,
                    TexturePackerMonoGameDefinitions.EarthBossSprites.Layer_3,
                    TexturePackerMonoGameDefinitions.EarthBossSprites.Layer_4,
                    TexturePackerMonoGameDefinitions.EarthBossSprites.Layer_5,
                    TexturePackerMonoGameDefinitions.EarthBossSprites.Layer_6,
                    TexturePackerMonoGameDefinitions.EarthBossSprites.Layer_7,
                    TexturePackerMonoGameDefinitions.EarthBossSprites.Layer_8,
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
            get { return 200; }
        }


        private float currentBoulderInterval = 0;
        private float boulderInterval = 1;

        public EarthBoss(ContentManager cm, SpriteRender sr, Vector2[] patrolPoints)
            : base(cm, sr, "earthboss_sprite_sheet.png", patrolPoints)
        {
            ignoreHoles = true;
            Scale = 1.5f;
            Drop = new PowerUp(cm, Vector2.Zero, "farore.png", isStrength: true);
        }

        public override void Update(GameTime gt, Screen s)
        {
            currentBoulderInterval += (float)gt.ElapsedGameTime.TotalSeconds;

            if (currentBoulderInterval > boulderInterval)
            {
                if (s is Castle)
                {
                    var castle = s as Castle;
                    var player = castle.Player;

                    ShootBoulder(castle, player);
                }
                currentBoulderInterval -= boulderInterval;
            }

            base.Update(gt, s);
        }

        public void ShootBoulder(Castle castle, Player player)
        {
            var direction = player.Position - this.Position;
            direction.Normalize();
            castle.CurrentRoom.AddBoulder(this.Position, direction, evil: true);
        }

    }
}
