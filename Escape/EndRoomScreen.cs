using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace Escape
{
    class EndRoomScreen : Screen
    {
        public MainGame mg { get; set; }
        public GraphicsDevice gd { get; set; }
        public bool Active { get; set; }
        private Rectangle BackgroundBox { get; set; }
        private SpriteFont Font { get; set; }

        public EndRoomScreen(MainGame mg, GraphicsDevice gd)
        {
            this.mg = mg;
            this.gd = gd;
            this.BackgroundBox = new Rectangle(0, 0, mg.GAME_WIDTH, mg.GAME_HEIGHT);
            this.Active = false;
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(BackgroundTexture, BackgroundBox, Color.White * 0.6f);
            sb.DrawString(Font, "Choose who to escape:", new Vector2(mg.GAME_WIDTH / 2, 200), Color.White);
            sb.DrawString(Font, "You or Max", new Vector2(mg.GAME_WIDTH / 2, 300), Color.White);

        }


        public override void LoadContent(ContentManager cm)
        {
            this.BackgroundTexture = new Texture2D(this.gd, 1, 1);
            this.BackgroundTexture.SetData(new Color[] { Color.White });
            this.Font = cm.Load<SpriteFont>("QuestionFont");
        }

        public void Update(Controls controls)
        {

            /*if (controls.onPress(Keys.LeftControl, Buttons.LeftShoulder))
            {
                sb.DrawString(Font, "You have escaped", new Vector2(mg.GAME_WIDTH / 2, 500), Color.White);
            }

            if (controls.onPress(Keys.RightControl, Buttons.RightShoulder))
            {
                sb.DrawString(Font, "Max has escaped", new Vector2(mg.GAME_WIDTH / 2, 500), Color.White);
            }*/
        }
    }
}

