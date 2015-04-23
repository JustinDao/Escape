using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace Escape
{
    class StartMenu : Screen
    {
        public MainGame mg { get; set; }
        public GraphicsDevice gd { get; set; }
        public bool Active { get; set; }
        private Rectangle BackgroundBox { get; set; }
        private SpriteFont Font { get; set; }

        public StartMenu(MainGame mg, GraphicsDevice gd)
        {
            this.mg = mg;
            this.gd = gd;
            this.BackgroundBox = new Rectangle(0, 0, mg.GAME_WIDTH, mg.GAME_HEIGHT);
            this.Active = false;

            var song = mg.Content.Load<SoundEffect>("Songs\\StartMenu");
            mg.CurrentSong = song.CreateInstance();
            mg.CurrentSong.IsLooped = true;
            mg.CurrentSong.Play();
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(BackgroundTexture, BackgroundBox, Color.White);
        }

        public override void LoadContent(ContentManager cm)
        {
			this.BackgroundTexture = cm.Load<Texture2D>("Cover.png");
            this.Font = cm.Load<SpriteFont>("QuestionFont");
        }

        public void Update(Controls controls)
        {
            if (controls.onPress(Keys.Space, Buttons.Start))
            {
                mg.CurrentSong.Stop();
                mg.SwitchToCastle();
                var song = mg.Content.Load<SoundEffect>("Songs\\Prelude");
                mg.CurrentSong = song.CreateInstance();
                mg.PlayingPrelude = true;
                mg.CurrentSong.Play();
            }
        }
    }
}
