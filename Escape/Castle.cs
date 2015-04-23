using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TexturePackerLoader;

namespace Escape
{
    class Castle : Screen
    {
        public MainGame mg { get; set; }
        public Vector2 CastlePosition { get; set; }
        public int CastleHeight { get; set; }
        public int CastleWidth { get; set; }
        public Room CurrentRoom { get; set; }
        public Room DebugRoom = null;
        public Player Player { get; set; }
        public RoomReader RR;
        public SubmissionRoom Subroom { get; set; }

        public Castle(MainGame mg)
        {
            this.mg = mg;

            Player = new Player(mg.Content, mg.SpriteRender, mg.Control, this);
            Player.Position += new Vector2(mg.GAME_WIDTH / 2, mg.GAME_HEIGHT - 100);

            InitializeRooms();
        }

        public void InitializeRooms()
        {
            this.RR = new RoomReader(mg, this, "Master.csv");
            CurrentRoom = this.RR.StartRoom;
            Player.VisitedRooms.Add(CurrentRoom);
        }

        public void Update(Controls controls, GameTime gameTime)
        {
            if (DebugRoom != null && CurrentRoom != DebugRoom)
            {
                CurrentRoom = DebugRoom;
                Player.VisitedRooms.Clear();
                Player.VisitedRooms.Add(CurrentRoom);
                DebugRoom = null;
            }

            if (controls.onPress(Keys.Space, Buttons.Start))
            {
                mg.Pause();
            }

            Player.Update(gameTime, this);
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
        }

        public override void LoadContent(ContentManager cm)
        {
            // preload assets
            string[] preloadTextures = {
                                           "fireball.png",
                                           "snowflake.png",
                                           "castle_door.png",
                                       };
            foreach (var textureName in preloadTextures)
            {
                cm.Load<Texture2D>(textureName);
            }
        }

    }
}
