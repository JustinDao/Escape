using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Escape
{
    class SubmissionRoom : Room
    {
        PlayerCutScene playerCutScene;
        ContentManager cm;
        Controls controls;
        Castle castle;


        public bool DrawPlayer = true;

        private Rectangle BackgroundBox;
        private SpriteFont Font;
        private Texture2D BackgroundTexture;
        public Player Player { get; set; }
        public Room CurrentRoom { get; set; }
        public Boolean visited = false;


        Vector2[] playerMovePoints;

        public SubmissionRoom(MainGame mg, Castle castle, String csvName)
            : base(mg, castle, csvName)
        {
            this.mg = mg;
            this.controls = mg.Control;
            this.cm = mg.Content;
            this.castle = castle;

            
            CurrentRoom = new RoomSub(mg, castle);

            BackgroundBox = new Rectangle(0, 0, mg.GAME_WIDTH, mg.GAME_HEIGHT);
            Font = cm.Load<SpriteFont>("QuestionFont");
            BackgroundTexture = new Texture2D(mg.GraphicsDevice, 1, 1);
            BackgroundTexture.SetData(new Color[] { Color.White });
        }

        public void Draw(SpriteBatch sb)
        {
            this.Draw(sb);

        }

        public void Update(GameTime gt, Screen s)
        {

            if (!visited)
            {
                castle.Player.Submission = 10;
                visited = true;
            }
        }
    }
}
