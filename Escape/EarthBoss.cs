using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TexturePackerLoader;

namespace Escape
{
    class EarthBoss : EnemyStationary
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

                    TexturePackerMonoGameDefinitions.EarthBossSprites.Front1,
                    TexturePackerMonoGameDefinitions.EarthBossSprites.Front2,
                    TexturePackerMonoGameDefinitions.EarthBossSprites.Front3,
                    TexturePackerMonoGameDefinitions.EarthBossSprites.Front4,
                    TexturePackerMonoGameDefinitions.EarthBossSprites.Left1,
                    TexturePackerMonoGameDefinitions.EarthBossSprites.Left2,
                    TexturePackerMonoGameDefinitions.EarthBossSprites.Left3,
                    TexturePackerMonoGameDefinitions.EarthBossSprites.Left4,
                    TexturePackerMonoGameDefinitions.EarthBossSprites.Left5,
                    TexturePackerMonoGameDefinitions.EarthBossSprites.Left6,
                    TexturePackerMonoGameDefinitions.EarthBossSprites.Left7,
                    TexturePackerMonoGameDefinitions.EarthBossSprites.Left8,
                    TexturePackerMonoGameDefinitions.EarthBossSprites.Left9,
                    TexturePackerMonoGameDefinitions.EarthBossSprites.Front1,
                    TexturePackerMonoGameDefinitions.EarthBossSprites.Front2,
                    TexturePackerMonoGameDefinitions.EarthBossSprites.Front3,
                    TexturePackerMonoGameDefinitions.EarthBossSprites.Front4,
                    TexturePackerMonoGameDefinitions.EarthBossSprites.Right1,
                    TexturePackerMonoGameDefinitions.EarthBossSprites.Right2,
                    TexturePackerMonoGameDefinitions.EarthBossSprites.Right3,
                    TexturePackerMonoGameDefinitions.EarthBossSprites.Right4,
                    TexturePackerMonoGameDefinitions.EarthBossSprites.Right5,
                    TexturePackerMonoGameDefinitions.EarthBossSprites.Right6,
                    TexturePackerMonoGameDefinitions.EarthBossSprites.Right7,
                    TexturePackerMonoGameDefinitions.EarthBossSprites.Right8,
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
        public override float AnimationSpeed
        {
            get
            {
                return base.AnimationSpeed * (float)Health / (float)MaxHealth;
            }
        }

        public override int Damage
        {
            get
            {
                return base.Damage * 2;
            }
        }

        private bool recharging = true;
        private int boulderIndex = 0;
        private float boulderTimer = 0;
        private Vector2 boulderTarget;

        const float BOULDER_SPEED = 600f;
        const float BOULDER_INTERVAL = 0.5f;
        const float BOULDER_CHASE_TIME = 5;
        const uint BOULDER_COUNT = 5;
        private Boulder[] boulders = new Boulder[BOULDER_COUNT];

        public EarthBoss(ContentManager cm, SpriteRender sr, Room room, Vector2 location)
            : base(cm, sr, "earth_boss_sprite_sheet.png", location)
        {
            ignoreHoles = true;
            ignoreWater = true;
            Scale = 1.5f;
            Drop = new PowerUp(cm, Vector2.Zero, "farore.png", isStrength: true);
            for (int i = 0; i < boulders.Count(); i++)
            {
                boulders[i] = new Boulder(cm, Position, 1f);
                boulders[i].Center = Center;
                float angle = 2 * (float)Math.PI * (float)i / (float)boulders.Count();
                angle -= (float)Math.PI / 2f;
                float dist = 100;
                boulders[i].Position = Position + new Vector2(
                    dist * (float)Math.Cos(angle),
                    dist * (float)Math.Sin(angle)
                );
            }
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch sb)
        {
            base.Draw(sb);
            if (Health <= 0) return;
            foreach (var boulder in boulders)
            {
                boulder.Draw(sb);
            }
        }

        public override void Update(GameTime gt, Screen s)
        {

            var delta = (float)gt.ElapsedGameTime.TotalSeconds;
            var castle = s as Castle;
            if (castle == null) {
                return;
            }

            var player = castle.Player;
            boulderTimer += delta;
            if (recharging)
            {
                if (boulderTimer >= BOULDER_INTERVAL)
                {
                    recharging = false;
                    boulderTimer = 0;
                    boulderTarget = player.Center;
                }
            }
            else
            {
                var boulder = boulders[boulderIndex];
                var diff = boulderTarget - boulder.Position;
                if (diff.Length() < BOULDER_SPEED * delta || boulderTimer >= BOULDER_CHASE_TIME)
                {
                    boulderIndex = (boulderIndex + 1) % boulders.Count();
                    recharging = true;
                    boulderTimer = 0;
                }
                else
                {
                    diff.Normalize();
                    boulder.Center += diff * BOULDER_SPEED * delta;
                    if (boulder.HitBox.Intersects(player.CollisionBox))
                    {
                        player.OnEnemyCollision(this);
                    }
                }
            }

            foreach (var boulder in boulders) {
                player.CollideObstacle(boulder.HitBox);
            }

            base.Update(gt, s);
        }

        public override void OnDeath(Room r)
        {
            foreach (var boulder in boulders) {
                r.Boulders.Add(boulder);
            }
            base.OnDeath(r);
        }

    }
}
