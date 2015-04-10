using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Escape
{

    class Projectile : SpriteEntity
    {
        public Enemy Owner;
        public bool Evil
        {
            get
            {
                return Owner != null;
            }
        }
        public float Range;
        public ProjectileType Type;
        public Vector2 StartPosition;
        public Vector2 Velocity;
        public Projectile(ContentManager cm, string spriteName, ProjectileType type,
            Vector2 position, Vector2 velocity, Enemy owner = null, float range = 0)
            : base(cm, spriteName)
        {
            Origin = new Vector2(0.5f, 0.5f);
            Position = position;
            StartPosition = position;
            Velocity = velocity;
            Type = type;
            Range = range;
            Owner = owner;
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
                pos, vel, range: range);
            snow.Scale = 0.5f;
            return snow;
        }
        public static Projectile CreateFireball(ContentManager cm,
            Vector2 pos, Vector2 vel, Enemy owner = null)
        {
            var fire = new Projectile(cm, "fireball.png", ProjectileType.FIREBALL,
                pos, vel, owner: owner);
            fire.Scale = 0.15f;
            return fire;
        }
        public static Projectile CreateBoulder(ContentManager cm,
            Vector2 pos, Vector2 vel, bool evil = false)
        {
            var boulder = new Projectile(cm, "boulder.png", ProjectileType.BOULDER,
                pos, vel, evil);
            boulder.Scale = 0.5f;
            return boulder;
        }
    }
}

