using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TexturePackerLoader;

namespace Escape
{
    class PlayerClip : AnimatedSpriteEntity
    {
        // sprite info
        int currentSpriteIndex = 0;
        float currentSpriteInterval = 0;

        // The Current Array of Sprites
        public string[] CurrentSprites;

        public int MaxSpriteIndex
        {
            get
            {
                return CurrentSprites.Length - 1; // derp :)
            }
        }

        public float SpriteInterval
        {
            get
            {
                return 0.05f;
            }
        }

        // The current Sprite referenced by a string
        public override string SpriteString
        {
            get
            {
                return CurrentSprites[currentSpriteIndex];
            }
        }

        // Player Sprites
        public string[] UpSprites
        {
            get
            {
                return LeftSprites;
            }
        }

        public string[] DownSprites
        {
            get
            {
                return LeftSprites;
            }
        }

        public string[] LeftSprites
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

        public string[] RightSprites
        {
            get
            {
                return LeftSprites;
            }
        }

        // Constructor
        public PlayerClip(ContentManager cm, SpriteRender sr)
            : base(cm, sr, "soldier_sprite_sheet.png")
        {
            CurrentSprites = LeftSprites;
        }

        public override void Update(GameTime gt, Screen s)
        {
            UpdateSprites(gt);
        }

        public void UpdateSprites(GameTime gt)
        {
            var delta = (float)gt.ElapsedGameTime.TotalSeconds;
            currentSpriteInterval += delta;

            if(currentSpriteInterval > SpriteInterval)
            {
                currentSpriteInterval = 0;
                currentSpriteIndex++;
                if (currentSpriteIndex > MaxSpriteIndex)
                {
                    currentSpriteIndex = 0;
                }                
            }

        }
    }
}
