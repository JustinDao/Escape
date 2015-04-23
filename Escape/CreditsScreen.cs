using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
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
                    height += 50;
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

            PlayerClip.Draw(sb);
        }

        public void Update(GameTime gt)
        {
            foreach (Text t in TextList)
            {
                t.Position.Y -= 100 * (float)gt.ElapsedGameTime.TotalSeconds;
            }

            PlayerClip.Update(gt, this);
        }

        public override void LoadContent(ContentManager cm)
        {
            // nothing!
        }
    }

    

}
