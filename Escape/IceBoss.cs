using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TexturePackerLoader;

namespace Escape
{
    class IceBoss : EnemyPatrol
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

                    TexturePackerMonoGameDefinitions.IceBossSprites.Layer_1,
                    TexturePackerMonoGameDefinitions.IceBossSprites.Layer_2,
                    TexturePackerMonoGameDefinitions.IceBossSprites.Layer_3,
                    TexturePackerMonoGameDefinitions.IceBossSprites.Layer_4,
                    TexturePackerMonoGameDefinitions.IceBossSprites.Layer_5,
                    TexturePackerMonoGameDefinitions.IceBossSprites.Layer_6,
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
                return 1;
                // return (float)Health/(float)MaxHealth;
            }
        }

        public override int Damage
        {
            get
            {
                return base.Damage * 3;
            }
        }

        private bool recharging = true;
        private float iceTimer = 0;

        const float ICE_INTERVAL = 0.5f;

        public IceBoss(ContentManager cm, SpriteRender sr, Room room, Vector2 location)
            : base(cm, sr, "ice_boss_sprite_sheet.png", location)
        {
            ignoreHoles = true;
            Scale = 1.5f;
            Drop = new PowerUp(cm, Vector2.Zero, "naryu.png", isIce: true);
        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);
            if (Health <= 0) return;
        }

        public override void Update(GameTime gt, Screen s)
        {

            var delta = (float)gt.ElapsedGameTime.TotalSeconds;
            var castle = s as Castle;
            if (castle == null)
            {
                return;
            }

            var player = castle.Player;
            iceTimer += delta;
            if (recharging)
            {
                if (iceTimer >= ICE_INTERVAL)
                {
                    recharging = false;
                    iceTimer = 0;
                }
            }
            else
            {
                // shoot ice
            }

            base.Update(gt, s);
        }

        public override void OnDeath(Room r)
        {
            
            base.OnDeath(r);
        }

    }
}
