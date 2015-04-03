using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Escape
{

    class Projectile : SpriteEntity
    {
        public float Range;
        public ProjectileType Type;
        public Vector2 StartPosition;
        public Vector2 Velocity;
        public Projectile(ContentManager cm, string spriteName, ProjectileType type,
            Vector2 position, Vector2 velocity, float range = 0)
            : base(cm, spriteName)
        {
            Origin = new Vector2(0.5f);
            Position = position;
            StartPosition = position;
            Velocity = velocity;
            Type = type;
            Range = range;
        }
        public override void Update(GameTime gt, Screen s)
        {
            float delta = (float)gt.ElapsedGameTime.TotalSeconds;
            Position += Velocity * delta;
        }

        public static Projectile CreateSnowflake(ContentManager cm,
            Vector2 pos, Vector2 vel, float range)
        {
            var snow = new Projectile(cm, "snowflake.png", ProjectileType.SNOWFLAKE,
                pos, vel, range);
            snow.Scale = 0.5f;
            return snow;
        }
        public static Projectile CreateFireball(ContentManager cm,
            Vector2 pos, Vector2 vel)
        {
            var fire = new Projectile(cm, "fireball.png", ProjectileType.FIREBALL,
                pos, vel);
            fire.Scale = 0.15f;
            return fire;
        }
    }
}

