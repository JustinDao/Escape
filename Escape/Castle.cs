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
    class Castle : Screen
    {
        public MainGame mg { get; set; }
        public Vector2 CastlePosition { get; set; }
        public int CastleHeight { get; set; }
        public int CastleWidth { get; set; }
        public Room CurrentRoom { get; set; }
        public Player Player { get; set; }
        public NewPlayer Hero;

        public Castle(MainGame mg)
        {
            this.mg = mg;

            Player = new Player(mg, this, 50, 50);
            Hero = new NewPlayer(mg.Content, mg.Control);
            Hero.Position += new Vector2(200, 200);

            InitializeRooms();
        }

        public void InitializeRooms()
        {
            CurrentRoom = new Room(mg);
            CurrentRoom.LeftRoom = new Room(mg, "RoomTemplate.csv");
            CurrentRoom.LeftRoom.RightRoom = CurrentRoom;
            CurrentRoom.RightRoom = new Room(mg, "R1.csv");
            CurrentRoom.RightRoom.LeftRoom = CurrentRoom;
            CurrentRoom.UpRoom = new Room(mg, "R3.csv");
            CurrentRoom.UpRoom.DownRoom = CurrentRoom;
            CurrentRoom.DownRoom = new Room(mg, "R5.csv");
            CurrentRoom.DownRoom.UpRoom = CurrentRoom;

            // Infinite Room Loop!
            CurrentRoom.LeftRoom.LeftRoom = CurrentRoom;
        }

        public void Update(Controls controls, GameTime gameTime)
        {
            if (controls.onPress(Keys.Space, Buttons.Start))
            {
                mg.SwitchToPause();
            }

            Player.Update(controls, gameTime, CurrentRoom);
            Hero.Update(gameTime, this);
            CurrentRoom.Update(gameTime, this);
        }

        public void MoveLeft()
        {
            this.CurrentRoom = this.CurrentRoom.LeftRoom;
        }

        public void MoveRight()
        {
            this.CurrentRoom = this.CurrentRoom.RightRoom;
        }

        public void MoveUp()
        {
            this.CurrentRoom = this.CurrentRoom.UpRoom;
        }

        public void MoveDown()
        {
            this.CurrentRoom = this.CurrentRoom.DownRoom;
        }

        public override void Draw(SpriteBatch sb)
        {
            CurrentRoom.Draw(sb);
            Player.Draw(sb);
            Hero.Draw(sb);
        }

        public override void LoadContent(ContentManager cm)
        {
            // preload assets
            string[] preloadTextures = {
                                           "fireball.png",
                                           "snowflake.png",
                                       };
            foreach (var textureName in preloadTextures) {
                cm.Load<Texture2D>(textureName);
            }
            Player.LoadContent(cm);

        }

    }
}
