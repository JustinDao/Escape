using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TexturePackerLoader;

namespace Escape
{
    abstract class Enemy : Character
    {
        public Enemy Spawn = null;
        public virtual int Damage
        {
            get
            {
                return 500;
            }
        }
        public PowerUp Drop = null;
        const float FLASH_TIME = 0.75f;
        float flashTimer = 0;
        public bool BeingAttacked = false;
        public virtual int MaxHealth
        {
            get
            {
                return 1;
            }
        }
        protected int hp;
        public int Health
        {
            get
            {
                return hp;
            }
            set
            {
                if (value < hp)
                {
                    flashTimer = FLASH_TIME;
                    OnDamage(hp - value);
                }
                hp = value;
            }
        }
        public virtual float TouchFreezeTimer
        {
            get
            {
                return 0;
            }
        }

        public virtual float DeathFadeTime
        {
            get
            {
                return 2;
            }
        }

        public Color? OverrideTint = null;
        public override Color Tint
        {
            get
            {
                if (OverrideTint.HasValue)
                {
                    return OverrideTint.Value;
                }
                else if (flashTimer > 0)
                {
                    var percent = 1 - (flashTimer / FLASH_TIME);
                    var col = new Color(1,percent,percent);
                    return Frozen ? Color.Lerp(Color.Red, Color.Cyan, percent) : col;
                }
                else if (Frozen)
                {
                    return Color.Cyan;
                }
                else return base.Tint;
            }
        }

        public Enemy(ContentManager cm, SpriteRender sr, string spriteSheetName)
            : base(cm, sr, spriteSheetName)
        {
            Health = MaxHealth;
        }

        public virtual void OnDamage(int dmg)
        {
            // do nothing
        }

        public override void Update(GameTime gt, Screen s)
        {
            float delta = (float)gt.ElapsedGameTime.TotalSeconds;
            if (flashTimer > 0)
            {
                flashTimer -= delta;
            }
            base.Update(gt, s);
        }

        public virtual void OnDeath(Room r)
        {
            // do nothing by default
        }
    }
}
