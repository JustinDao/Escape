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
    class Pause : Screen
    {
        public MainGame mg { get; set; }
        public GraphicsDevice gd { get; set; }
        public bool Active { get; set; }
        private Rectangle BackgroundBox { get; set; }
        private SpriteFont Font { get; set; }

        public Pause(MainGame mg, GraphicsDevice gd)
        {
            this.mg = mg;
            this.gd = gd;
            this.BackgroundBox = new Rectangle(0, 0, mg.GAME_WIDTH, mg.GAME_HEIGHT);
            this.Active = false;
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(BackgroundTexture, BackgroundBox, Color.White * 0.6f);
            sb.DrawString(Font, "PAUSED", new Vector2(mg.GAME_WIDTH / 2, 300), Color.White);
            sb.DrawString(Font, "Press start to resume", new Vector2(mg.GAME_WIDTH / 2, 500), Color.White);

        }


        public override void LoadContent(ContentManager cm)
        {
            this.BackgroundTexture = new Texture2D(this.gd, 1, 1);
            this.BackgroundTexture.SetData(new Color[] { Color.Black });
            this.Font = cm.Load<SpriteFont>("QuestionFont");
        }

        public void Update(Controls controls)
        {

            if (controls.onPress(Keys.Space, Buttons.Start))
            {
                mg.SwitchToCastle();
            }
        }
    }
}

