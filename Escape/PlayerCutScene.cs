using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TexturePackerLoader;

namespace Escape
{
    class PlayerCutScene : Character
    {
        public override string[] UpSprites
        {
            get
            {
                return new string[] {
                    TexturePackerMonoGameDefinitions.SoldierSprites.Backward_Layer_1,
                    TexturePackerMonoGameDefinitions.SoldierSprites.Backward_Layer_2,
                    TexturePackerMonoGameDefinitions.SoldierSprites.Backward_Layer_3,
                    TexturePackerMonoGameDefinitions.SoldierSprites.Backward_Layer_4,
                    TexturePackerMonoGameDefinitions.SoldierSprites.Backward_Layer_5,
                    TexturePackerMonoGameDefinitions.SoldierSprites.Backward_Layer_6,
                    TexturePackerMonoGameDefinitions.SoldierSprites.Backward_Layer_7,
                    TexturePackerMonoGameDefinitions.SoldierSprites.Backward_Layer_8,
                    TexturePackerMonoGameDefinitions.SoldierSprites.Backward_Layer_9,
                };
            }
        }
        public override string[] DownSprites
        {
            get
            {
                return new string[] {
                    TexturePackerMonoGameDefinitions.SoldierSprites.Forward_Layer_19,
                    TexturePackerMonoGameDefinitions.SoldierSprites.Forward_Layer_20,
                    TexturePackerMonoGameDefinitions.SoldierSprites.Forward_Layer_21,
                    TexturePackerMonoGameDefinitions.SoldierSprites.Forward_Layer_22,
                    TexturePackerMonoGameDefinitions.SoldierSprites.Forward_Layer_23,
                    TexturePackerMonoGameDefinitions.SoldierSprites.Forward_Layer_24,
                    TexturePackerMonoGameDefinitions.SoldierSprites.Forward_Layer_25,
                    TexturePackerMonoGameDefinitions.SoldierSprites.Forward_Layer_26,
                    TexturePackerMonoGameDefinitions.SoldierSprites.Forward_Layer_27,
                };
            }
        }
        public override string[] LeftSprites
        {
            get
            {
                return new string[] {
                    TexturePackerMonoGameDefinitions.SoldierSprites.Left_Layer_10,
                    TexturePackerMonoGameDefinitions.SoldierSprites.Left_Layer_11,
                    TexturePackerMonoGameDefinitions.SoldierSprites.Left_Layer_12,
                    TexturePackerMonoGameDefinitions.SoldierSprites.Left_Layer_13,
                    TexturePackerMonoGameDefinitions.SoldierSprites.Left_Layer_14,
                    TexturePackerMonoGameDefinitions.SoldierSprites.Left_Layer_15,
                    TexturePackerMonoGameDefinitions.SoldierSprites.Left_Layer_16,
                    TexturePackerMonoGameDefinitions.SoldierSprites.Left_Layer_17,
                    TexturePackerMonoGameDefinitions.SoldierSprites.Left_Layer_18,
                };
            }
        }
        public override string[] RightSprites
        {
            get
            {
                return new string[] {
                    TexturePackerMonoGameDefinitions.SoldierSprites.Right_Layer_28,
                    TexturePackerMonoGameDefinitions.SoldierSprites.Right_Layer_29,
                    TexturePackerMonoGameDefinitions.SoldierSprites.Right_Layer_30,
                    TexturePackerMonoGameDefinitions.SoldierSprites.Right_Layer_31,
                    TexturePackerMonoGameDefinitions.SoldierSprites.Right_Layer_32,
                    TexturePackerMonoGameDefinitions.SoldierSprites.Right_Layer_33,
                    TexturePackerMonoGameDefinitions.SoldierSprites.Right_Layer_34,
                    TexturePackerMonoGameDefinitions.SoldierSprites.Right_Layer_35,
                    TexturePackerMonoGameDefinitions.SoldierSprites.Right_Layer_36,
                };
            }
        }

        public override int SpriteInterval
        {
            get { return 100; }
        }
        public override float MaxSpeed
        {
            get { return 100; }
        }

        public Vector2 Target;

        public bool reached = false;

        public override Vector2 CurrentVelocity
        {
            get
            {
                if (Target != null)
                {
                    var dist = Target - Position;
                    dist.Normalize();

                    return dist * MaxSpeed;
                }
                else
                {
                    return Vector2.Zero;
                }
            }
        }

        public PlayerCutScene(ContentManager cm, SpriteRender sr, Vector2 target)
            : base(cm, sr, "soldier_sprite_sheet.png")
        {
            Target = target;
            CurrentSprites = UpSprites;
        }

        public override void Update(GameTime gt, Screen s)
        {
            var delta = (float)gt.ElapsedGameTime.TotalSeconds;

            if (CurrentVelocity.LengthSquared() * delta >= (Target - Position).LengthSquared())
            {
                if (!(s is EndScreen)) return;

                if(!reached)
                {
                    reached = true;

                    var e = s as EndScreen;

                    if (!e.DrawAmulet && !e.DrawPortal && !e.EndAll)
                        e.DrawAmulet = true;
                    else if (!e.BackAway)
                        e.BackAway = true;
                    else if (!e.DrawPortal)
                    {
                        e.DrawPortal = true;
                        e.DrawAmulet = false;
                        e.WalkBack = true;
                    }
                    else if(!e.EndAll)
                    {
                        e.EndAll = true;
                    }
                }               

                return;
            }
            else
            {
                reached = false;
            }

            base.Update(gt, s);
        }
    }
}
