using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TexturePackerLoader;

namespace Escape
{       
    class TestThing
    {
        TPSpriteSheet spriteSheet;
        ContentManager Content;
        float xAccel = 0;
        float yAccel = 0;
        float speed = 200;

        float xPosition = 200;
        float yPosition = 200;

        Vector2 lastMove;

        int currentSpriteIndex = 0;
        float currentSpriteInterval = 0;
        int MAX_SPRITE_INDEX = 8;
        int SPRITE_INTERVAL = 50;

        public string[] UpSprites = {
            TexturePackerMonoGameDefinitions.SoldierSprite.Backward_Layer_1,
            TexturePackerMonoGameDefinitions.SoldierSprite.Backward_Layer_2,
            TexturePackerMonoGameDefinitions.SoldierSprite.Backward_Layer_3,
            TexturePackerMonoGameDefinitions.SoldierSprite.Backward_Layer_4,
            TexturePackerMonoGameDefinitions.SoldierSprite.Backward_Layer_5,
            TexturePackerMonoGameDefinitions.SoldierSprite.Backward_Layer_6,
            TexturePackerMonoGameDefinitions.SoldierSprite.Backward_Layer_7,
            TexturePackerMonoGameDefinitions.SoldierSprite.Backward_Layer_8,
            TexturePackerMonoGameDefinitions.SoldierSprite.Backward_Layer_9,
        };

        public string[] DownSprites = {
            TexturePackerMonoGameDefinitions.SoldierSprite.Forward_Layer_19,
            TexturePackerMonoGameDefinitions.SoldierSprite.Forward_Layer_20,
            TexturePackerMonoGameDefinitions.SoldierSprite.Forward_Layer_21,
            TexturePackerMonoGameDefinitions.SoldierSprite.Forward_Layer_22,
            TexturePackerMonoGameDefinitions.SoldierSprite.Forward_Layer_23,
            TexturePackerMonoGameDefinitions.SoldierSprite.Forward_Layer_24,
            TexturePackerMonoGameDefinitions.SoldierSprite.Forward_Layer_25,
            TexturePackerMonoGameDefinitions.SoldierSprite.Forward_Layer_26,
            TexturePackerMonoGameDefinitions.SoldierSprite.Forward_Layer_27,
        };

        public string[] LeftSprites = {
            TexturePackerMonoGameDefinitions.SoldierSprite.Left_Layer_10,
            TexturePackerMonoGameDefinitions.SoldierSprite.Left_Layer_11,
            TexturePackerMonoGameDefinitions.SoldierSprite.Left_Layer_12,
            TexturePackerMonoGameDefinitions.SoldierSprite.Left_Layer_13,
            TexturePackerMonoGameDefinitions.SoldierSprite.Left_Layer_14,
            TexturePackerMonoGameDefinitions.SoldierSprite.Left_Layer_15,
            TexturePackerMonoGameDefinitions.SoldierSprite.Left_Layer_16,
            TexturePackerMonoGameDefinitions.SoldierSprite.Left_Layer_17,
            TexturePackerMonoGameDefinitions.SoldierSprite.Left_Layer_18,
        };

        public string[] RightSprites = {
            TexturePackerMonoGameDefinitions.SoldierSprite.Right_Layer_28,
            TexturePackerMonoGameDefinitions.SoldierSprite.Right_Layer_29,
            TexturePackerMonoGameDefinitions.SoldierSprite.Right_Layer_30,
            TexturePackerMonoGameDefinitions.SoldierSprite.Right_Layer_31,
            TexturePackerMonoGameDefinitions.SoldierSprite.Right_Layer_32,
            TexturePackerMonoGameDefinitions.SoldierSprite.Right_Layer_33,
            TexturePackerMonoGameDefinitions.SoldierSprite.Right_Layer_34,
            TexturePackerMonoGameDefinitions.SoldierSprite.Right_Layer_35,
            TexturePackerMonoGameDefinitions.SoldierSprite.Right_Layer_36,
        };

        public string[] CurrentSprites;        

        public float MoveAngle
        {
            get
            {
                return (float)Math.Atan2(lastMove.Y, lastMove.X);
            }
        }

        public TestThing(MainGame mg)
        {
            this.Content = mg.Content;
            var spriteSheetLoader = new SpriteSheetLoader(this.Content);
            this.spriteSheet = spriteSheetLoader.Load("soldier_sprite_sheet.png");
            this.CurrentSprites = DownSprites;
            lastMove = new Vector2(0, 0);
        }

        public void Draw(SpriteBatch sb, SpriteRender sr)
        {
            sr.Draw(
                this.spriteSheet.Sprite(
                    CurrentSprites[currentSpriteIndex]
                ),
                new Vector2(xPosition, yPosition)
            );
        }

        public void Update(GameTime gt, Controls controls)
        {
            if (controls.onPress(Keys.Right, Buttons.DPadRight))
            {
                xAccel = speed;
            }
            else if (controls.onRelease(Keys.Right, Buttons.DPadRight))
            {
                xAccel = 0;
            }

            if (controls.onPress(Keys.Left, Buttons.DPadLeft))
            {
                xAccel = -speed;
            }
            else if (controls.onRelease(Keys.Left, Buttons.DPadLeft))
            {
                xAccel = 0;
            }

            // Y movement
            if (controls.onPress(Keys.Down, Buttons.DPadDown))
            {
                yAccel = speed;
            }
            else if (controls.onRelease(Keys.Down, Buttons.DPadDown))
            {
                yAccel = 0;
            }

            if (controls.onPress(Keys.Up, Buttons.DPadUp))
            {
                yAccel = -speed;
            }
            else if (controls.onRelease(Keys.Up, Buttons.DPadUp))
            {
                yAccel = 0;
            }

            if (xAccel != 0 || yAccel != 0)
            {
                lastMove = new Vector2(xAccel, yAccel);

                currentSpriteInterval += (float)gt.ElapsedGameTime.TotalMilliseconds;

                if (currentSpriteInterval > SPRITE_INTERVAL)
                {
                    currentSpriteIndex += 1;
                    currentSpriteInterval = 0;

                    if (currentSpriteIndex > MAX_SPRITE_INDEX)
                    {
                        currentSpriteIndex = 0;
                    }
                }    

            }

            var pi = Math.PI;

            float angle = MoveAngle;

            if (angle < 0)
            {
                angle += (float)(2 * pi);
            }

            if (angle < pi / 4 || angle > 7 * pi / 4)
            {
                CurrentSprites = RightSprites;
            }
            else if (angle < 3 * pi / 4)
            {
                CurrentSprites = DownSprites;
            }
            else if (angle < 5 * pi / 4)
            {
                CurrentSprites = LeftSprites;
            }
            else
            {
                CurrentSprites = UpSprites;
            }            

            xPosition += (float)(xAccel * gt.ElapsedGameTime.TotalSeconds);
            yPosition += (float)(yAccel * gt.ElapsedGameTime.TotalSeconds);
        }
    }
}
