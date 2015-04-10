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
        public bool BeingAttacked = false;
        public virtual int MaxHealth
        {
            get
            {
                return 1;
            }
        }
        private int hp;
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
                    OnDamage(hp - value);
                }
                hp = value;
            }
        }
        public float FreezeTimer = 0;
        public bool Frozen
        {
            get
            {
                return FreezeTimer > 0;
            }
        }
        public override float SpeedMult
        {
            get
            {
                return Frozen ? 0.1f : 1f;
            }
        }
        public virtual float DeathFadeTime
        {
            get
            {
                return 1;
            }
        }

        public Color? OverrideTint = null;
        public override Color Tint
        {
            get
            {
                var hit = BeingAttacked || Health == 0;
                if (OverrideTint.HasValue)
                {
                    return OverrideTint.Value;
                }
                else if (Frozen && hit)
                {
                    return Color.Purple;
                }
                else if (Frozen)
                {
                    return Color.Cyan;
                }
                else if (hit)
                {
                    return Color.Red;
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
            if (FreezeTimer > 0)
            {
                FreezeTimer -= (float)gt.ElapsedGameTime.TotalSeconds;
            }
            base.Update(gt, s);
        }

    }
}
