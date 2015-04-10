using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Escape
{
    class EndScreen : Castle
    {
        PlayerCutScene playerCutScene;
        MacGuffin Amulet;
        Portal Portal;

        public bool DrawAmulet = false;
        public bool BackAway = false;
        public bool DrawPortal = false;


        Vector2[] playerMovePoints;

        public EndScreen(MainGame mg) : base(mg)
        {
            this.mg = mg;

            playerMovePoints = new Vector2[] 
            {
                new Vector2(mg.GAME_WIDTH / 2, 200),
            };

            playerCutScene = new PlayerCutScene(mg.Content, mg.SpriteRender, playerMovePoints[0]);
            Amulet = new MacGuffin(mg, mg.Content);
            Portal = new Portal(mg, mg.Content, mg.SpriteRender);
            playerCutScene.Position = new Vector2(mg.GAME_WIDTH / 2, mg.GAME_HEIGHT);
            CurrentRoom = new RoomEnd(mg, this);
        }

        public override void Draw(SpriteBatch sb)
        {
            CurrentRoom.Draw(sb);
            playerCutScene.Draw(sb);

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
            else if (BackAway)
            {
                playerCutScene.Target = new Vector2(playerCutScene.Target.X, 350);
            }
        }

        public void Update(GameTime gt)
        {
            playerCutScene.Update(gt, this);
            Portal.Update(gt, this);
        }        

        public override void LoadContent(ContentManager cm)
        {
            // nothing to load!
        }
    }
}
