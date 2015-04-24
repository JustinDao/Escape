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
    class CreditsScreen : Screen
    {
        MainGame mg;
        ContentManager cm;
        SpriteFont Font;
        PlayerClip PlayerClip;

        List<Text> TextList = new List<Text>();

        Rectangle BackgroundBox;

        string fontString = "QuestionFont";

        float SCROLL_SPEED = 75;
        bool Finished = false;

        public CreditsScreen(ContentManager cm, MainGame mg)
        {
            this.mg = mg;
            this.cm = cm;
            this.BackgroundTexture = new Texture2D(mg.GraphicsDevice, 1, 1);
            this.BackgroundTexture.SetData(new Color[] { Color.White });
            this.BackgroundBox = new Rectangle(0, 0, mg.GAME_WIDTH, mg.GAME_HEIGHT);
            this.PlayerClip = new PlayerClip(cm, mg.SpriteRender);
            PlayerClip.Position = new Vector2(50, 550);

            this.Font = cm.Load<SpriteFont>(fontString);

            var height = 700;

            using (var stream = TitleContainer.OpenStream(@"Content/Credits.txt"))
            using (var reader = new StreamReader(stream))
            {
                while(!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var stringLength = Font.MeasureString(line);
                    TextList.Add(new Text(cm, line, new Vector2(mg.GAME_WIDTH / 2 - stringLength.X / 2, height), fontString));
                    height += 30;
                }
                
            }
            
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(BackgroundTexture, BackgroundBox, Color.Black);

            foreach(Text t in TextList)
            {
                t.Draw(sb);
            }

            if(Finished)
            {
                var s = "Start to Play Again, Back to Quit.";
                sb.DrawString(Font, s, new Vector2(mg.GAME_WIDTH / 2 - (Font.MeasureString(s).X / 2), TextList.Last().Position.Y + 30), Color.White);
            }

            PlayerClip.Draw(sb);
        }

        public void Update(GameTime gt)
        {
            if (!Finished)
            {
                foreach (Text t in TextList)
                {
                    t.Position.Y -= SCROLL_SPEED * (float)gt.ElapsedGameTime.TotalSeconds;
                }

                var last = TextList.Last();
                if (last.Position.Y <= mg.GAME_HEIGHT / 2) Finished = true;
            }
            else
            {
                if(mg.Control.onPress(Keys.Space, Buttons.Start))
                {
                    mg.ReInitialize();
                }
                else if(mg.Control.onPress(Keys.Escape, Buttons.Back))
                {
                    mg.Exit();
                }
            }
           

            PlayerClip.Update(gt, this);
        }

        public override void LoadContent(ContentManager cm)
        {
            // nothing!
        }
    }

    

}
