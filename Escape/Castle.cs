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
        public Player Player { get; set; }

        public Castle(MainGame mg)
        {
            this.mg = mg;

            Player = new Player(mg.Content, mg.SpriteRender, mg.Control);
            Player.Position += new Vector2(200, 200);

            InitializeRooms();
        }

        public void InitializeRooms()
        {
            CurrentRoom = new Room(mg, this);
            CurrentRoom.LeftRoom = new Room(mg, this, "RoomTemplate.csv");

            CurrentRoom.LeftRoom.Enemies.Add(new FireBoss(mg.Content, mg.SpriteRender, new Vector2[] 
                { 
                    new Vector2(300, 100), new Vector2(500, 100), new Vector2(300, 300), new Vector2(500, 300)
                }
            ));

            CurrentRoom.LeftRoom.RightRoom = CurrentRoom;

            CurrentRoom.LeftRoom.UpRoom = new RoomEnd(mg, this);
            CurrentRoom.LeftRoom.UpRoom.DownRoom = CurrentRoom.LeftRoom;

            CurrentRoom.RightRoom = new Room(mg, this, "R2.csv");
            CurrentRoom.RightRoom.LeftRoom = CurrentRoom;

            CurrentRoom.UpRoom = new Room(mg, this, "R3.csv");
            CurrentRoom.UpRoom.Enemies.Add(new Ghost(mg.Content, mg.SpriteRender, this.Player, new Vector2(400, 300)));
            CurrentRoom.UpRoom.Enemies.Add(new Ghost(mg.Content, mg.SpriteRender, this.Player, new Vector2(450, 500)));
            CurrentRoom.UpRoom.Enemies.Add(new Ghost(mg.Content, mg.SpriteRender, this.Player, new Vector2(150, 400)));

            CurrentRoom.UpRoom.DownRoom = CurrentRoom;
            CurrentRoom.UpRoom.UpRoom = new Room(mg, this, "R4.csv");
            CurrentRoom.UpRoom.UpRoom.Enemies.Add(new Ghost(mg.Content, mg.SpriteRender, this.Player, new Vector2(400, 300)));

            CurrentRoom.UpRoom.UpRoom.DownRoom = CurrentRoom.UpRoom;
            CurrentRoom.DownRoom = new Room(mg, this, "R5.csv");
            CurrentRoom.DownRoom.UpRoom = CurrentRoom;

            // Infinite Room Loop!
            CurrentRoom.LeftRoom.LeftRoom = CurrentRoom;
        }

        public void Update(Controls controls, GameTime gameTime)
        {
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
                                       };
            foreach (var textureName in preloadTextures) {
                cm.Load<Texture2D>(textureName);
            }
        }

    }
}
