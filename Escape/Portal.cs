using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TexturePackerLoader;

namespace Escape
{
    class Portal : AnimatedSpriteEntity
    {
        public string[] Sprites = new string[] 
        {
            TexturePackerMonoGameDefinitions.PortalSprites.First,
            TexturePackerMonoGameDefinitions.PortalSprites.Second,
            TexturePackerMonoGameDefinitions.PortalSprites.Third,
            TexturePackerMonoGameDefinitions.PortalSprites.Fourth,
        };

        private int currentSpriteIndex = 0;
        private float currentSpriteInterval = 0;
        private float SpriteInterval = 100;

        public override string SpriteString
        {
            get
            {
                return Sprites[currentSpriteIndex];
            }
        }

        public Portal(MainGame mg, ContentManager cm, SpriteRender sr)
            : base(cm, sr, "portal_sprite_sheet.png")
        {
            Position = new Vector2(mg.GAME_WIDTH / 2 - this.HitBox.Width / 2 + 50, 100);
        }

        public override void Update(GameTime gt, Screen s)
        {
            currentSpriteInterval += (float)gt.ElapsedGameTime.TotalMilliseconds;

            if (currentSpriteInterval > SpriteInterval)
            {
                currentSpriteIndex += 1;

                if (currentSpriteIndex >= Sprites.Count())
                {
                    currentSpriteIndex = 0;
                }

                currentSpriteInterval = 0;
            }

            
        }
    }
}
