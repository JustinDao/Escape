using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Escape
{
    class GameOverScreen : Screen
    {
        MainGame mg;
        ContentManager cm;
        SpriteFont Font;
       
        Rectangle BackgroundBox;

        string fontString = "QuestionFont";

        public GameOverScreen(ContentManager cm, MainGame mg)
        {
            this.mg = mg;
            this.cm = cm;
            this.BackgroundTexture = new Texture2D(mg.GraphicsDevice, 1, 1);
            this.BackgroundTexture.SetData(new Color[] { Color.White });
            this.BackgroundBox = new Rectangle(0, 0, mg.GAME_WIDTH, mg.GAME_HEIGHT);
      
            this.Font = cm.Load<SpriteFont>(fontString);
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(BackgroundTexture, BackgroundBox, Color.Black);

            var s = "Game Over!";
            var sPositionX = mg.GAME_WIDTH / 2 - (Font.MeasureString(s).X / 2);
            sb.DrawString(Font, s, new Vector2(sPositionX, mg.GAME_HEIGHT / 2), Color.White);

            s = "Press Start to restart, or Back to quit.";
            sPositionX = mg.GAME_WIDTH / 2 - (Font.MeasureString(s).X / 2);
            sb.DrawString(Font, s, new Vector2(sPositionX, mg.GAME_HEIGHT / 2 + 30), Color.White);
        }
          

        public void Update(GameTime gt)
        {

            if (mg.Control.onPress(Keys.Space, Buttons.Start))
            {
                mg.ReInitialize();
            }
            else if (mg.Control.onPress(Keys.Escape, Buttons.Back))
            {
                mg.Exit();
            }
            
        }

        public override void LoadContent(ContentManager cm)
        {
            // nothing!
        }
    }



}
