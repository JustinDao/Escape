using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TexturePackerLoader;

namespace Escape
{
    class Ghost : EnemyFollow
    {
        public override string[] UpSprites
        {
            get
            {
                return new string[] {
                    TexturePackerMonoGameDefinitions.GhostSprites.Up1,
                    TexturePackerMonoGameDefinitions.GhostSprites.Up2,
                    TexturePackerMonoGameDefinitions.GhostSprites.Up3,
                    TexturePackerMonoGameDefinitions.GhostSprites.Up2,
                };
            }      
        }

        public override string[] DownSprites
        {
            get
            {
                return new string[] {
                    TexturePackerMonoGameDefinitions.GhostSprites.Down1,
                    TexturePackerMonoGameDefinitions.GhostSprites.Down2,
                    TexturePackerMonoGameDefinitions.GhostSprites.Down3,
                    TexturePackerMonoGameDefinitions.GhostSprites.Down2,
                };
            }  
        }

        public override string[] LeftSprites
        {
            get
            {
                return new string[] {
                    TexturePackerMonoGameDefinitions.GhostSprites.Left1,
                    TexturePackerMonoGameDefinitions.GhostSprites.Left2,
                    TexturePackerMonoGameDefinitions.GhostSprites.Left3,
                    TexturePackerMonoGameDefinitions.GhostSprites.Left2,
                };
            }
        }

        public override string[] RightSprites
        {
            get
            {
                return new string[] {
                    TexturePackerMonoGameDefinitions.GhostSprites.Right1,
                    TexturePackerMonoGameDefinitions.GhostSprites.Right2,
                    TexturePackerMonoGameDefinitions.GhostSprites.Right3,
                    TexturePackerMonoGameDefinitions.GhostSprites.Right2,
                };
            }
        }

        public override int SpriteInterval
        {
            get { return 150; }
        }

        public override float MaxSpeed
        {
            get { return 100; }
        }

        // Constuctor
        public Ghost(ContentManager cm, SpriteRender sr, AnimatedSpriteEntity target, Vector2 position)
            : base(cm, sr, target, "ghost_sprite_sheet.png")
        {
            ignoreHoles = true;
            ignoreWater = true;
            Target = target;
            Position = position;
        }

        public override void Update(GameTime gt, Screen s)
        {   
            // base update!
            base.Update(gt, s);
        }


    }
}
