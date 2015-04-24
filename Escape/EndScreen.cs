using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Escape
{
    class EndScreen : Castle
    {
        PlayerCutScene playerCutScene;
        ContentManager cm;
        MacGuffin Amulet;
        Portal Portal;
        Controls controls;

        public bool DrawPlayer = true;
        public bool DrawAmulet = false;
        public bool BackAway = false;
        public bool WalkBack = false;
        public bool DrawPortal = false;
        public bool SelectedChoice = false;
        public bool ChooseMax = false;
        public bool ChoosePlayer = false;
        public bool EndAll = false;

        private Rectangle BackgroundBox;
        private SpriteFont Font;

        Vector2[] playerMovePoints;

        public EndScreen(MainGame mg) : base(mg)
        {
            this.mg = mg;
            this.controls = mg.Control;
            this.cm = mg.Content;

            playerMovePoints = new Vector2[] 
            {
                new Vector2(mg.GAME_WIDTH / 2, 200),
            };

            playerCutScene = new PlayerCutScene(mg.Content, mg.SpriteRender, playerMovePoints[0]);
            Amulet = new MacGuffin(mg, mg.Content);
            Portal = new Portal(mg, mg.Content, mg.SpriteRender);
            playerCutScene.Position = new Vector2(mg.GAME_WIDTH / 2, mg.GAME_HEIGHT);
            CurrentRoom = new RoomEnd(mg, this);

            BackgroundBox = new Rectangle(0, 0, mg.GAME_WIDTH, mg.GAME_HEIGHT);
            Font = cm.Load<SpriteFont>("QuestionFont");
            BackgroundTexture = new Texture2D(mg.GraphicsDevice, 1, 1);
            BackgroundTexture.SetData(new Color[] { Color.White });
        }

        public override void Draw(SpriteBatch sb)
        {
            CurrentRoom.Draw(sb);

            if(DrawPlayer)
            {
                playerCutScene.Draw(sb);
            }
            
            if(DrawAmulet)
            {
                Amulet.Draw(sb);
            }

            if(DrawPortal)
            {
                Portal.Draw(sb);
            }

            if(DrawAmulet && !BackAway)
            {
                playerCutScene.Target = new Vector2(playerCutScene.Target.X, 500);
            }
            else if (BackAway && !WalkBack)
            {
                playerCutScene.Target = new Vector2(playerCutScene.Target.X, 350);
            }
            else if (WalkBack && !SelectedChoice)
            {
                sb.Draw(BackgroundTexture, BackgroundBox, Color.White * 0.6f);
                sb.DrawString(Font, "Choose who to escape:", new Vector2(mg.GAME_WIDTH / 2 - 150, 200), Color.Black);
                sb.DrawString(Font, "X: Max", new Vector2(mg.GAME_WIDTH / 2 - 300, 300), Color.Black);
                sb.DrawString(Font, "B: You", new Vector2(mg.GAME_WIDTH / 2 + 300, 300), Color.Black);
            }
            else if (EndAll)
            {
                if(ChoosePlayer)
                {
                    DrawPortal = false;
                }

                DrawPlayer = false;
            }
            

        }

        public void Update(GameTime gt)
        {
            playerCutScene.Update(gt, this);
            Portal.Update(gt, this);
            //endRoomScreen.Update(controls);

            if (WalkBack && !SelectedChoice)
            {
                if (controls.onPress(Keys.D2, Buttons.X))
                {
                    this.ChooseMax = true;
                    playerCutScene.Target = new Vector2(playerCutScene.Target.X, mg.GAME_HEIGHT - 25);
                    SelectedChoice = true;
                }
                else if (controls.onPress(Keys.D3, Buttons.B))
                {
                    this.ChoosePlayer = true;
                    playerCutScene.Target = new Vector2(playerCutScene.Target.X, Portal.HitBox.Y);
                    SelectedChoice = true;
                }
            }
        }        

        public override void LoadContent(ContentManager cm)
        {
            // nothing to load!
        }
    }
}
