using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TexturePackerLoader;

namespace Escape
{
    class IceMinion : Ghost
    {

        public override string[] UpSprites
        {
            get
            {
                return new string[] {
                    TexturePackerMonoGameDefinitions.IceMinionSprites.Up1,
                    TexturePackerMonoGameDefinitions.IceMinionSprites.Up2,
                    TexturePackerMonoGameDefinitions.IceMinionSprites.Up3,
                    TexturePackerMonoGameDefinitions.IceMinionSprites.Up2,
                };
            }
        }

        public override string[] DownSprites
        {
            get
            {
                return new string[] {
                    TexturePackerMonoGameDefinitions.IceMinionSprites.Down1,
                    TexturePackerMonoGameDefinitions.IceMinionSprites.Down2,
                    TexturePackerMonoGameDefinitions.IceMinionSprites.Down3,
                    TexturePackerMonoGameDefinitions.IceMinionSprites.Down2,
                };
            }
        }

        public override string[] LeftSprites
        {
            get
            {
                return new string[] {
                    TexturePackerMonoGameDefinitions.IceMinionSprites.Left1,
                    TexturePackerMonoGameDefinitions.IceMinionSprites.Left2,
                    TexturePackerMonoGameDefinitions.IceMinionSprites.Left3,
                    TexturePackerMonoGameDefinitions.IceMinionSprites.Left2,
                };
            }
        }

        public override string[] RightSprites
        {
            get
            {
                return new string[] {
                    TexturePackerMonoGameDefinitions.IceMinionSprites.Right1,
                    TexturePackerMonoGameDefinitions.IceMinionSprites.Right2,
                    TexturePackerMonoGameDefinitions.IceMinionSprites.Right3,
                    TexturePackerMonoGameDefinitions.IceMinionSprites.Right2,
                };
            }
        }

        public IceMinion(ContentManager cm, SpriteRender sr, Entity target, Vector2 position, int hp = 3)
            : base(cm, sr, target, position, hp: hp, spriteSheet: "ice_minion_sprite_sheet.png")
        {
           // nothing
        }
        public override float TouchFreezeTimer
        {
            get
            {
                return 0.5f;
            }
        }

        public override Color Tint
        {
            get
            {
                if (Health > 0)
                {
                    return Color.Lerp(Color.Cyan, base.Tint, 0.5f);
                }
                else
                {
                    return base.Tint;
                }
            }
        }
    }
}
