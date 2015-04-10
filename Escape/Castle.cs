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
            Player.Position += new Vector2(mg.GAME_WIDTH / 2, mg.GAME_HEIGHT - 100);

            InitializeRooms();
        }

        public void InitializeRooms()
        {
            CurrentRoom = new Room(mg, this, "StartRoom.csv");

            Room MainRoom = new Room(mg, this);

            CurrentRoom.UpRoom = MainRoom;
            MainRoom.DownRoom = CurrentRoom;

            MainRoom.LeftRoom = new Room(mg, this, "RoomTemplate.csv");

            MainRoom.LeftRoom.Enemies.Add(new FireBoss(mg.Content, mg.SpriteRender, new Vector2[] 
                { 
                    new Vector2(300, 100), new Vector2(500, 100), new Vector2(300, 300), new Vector2(500, 300)
                }
            ));

            MainRoom.LeftRoom.RightRoom = MainRoom;

            MainRoom.LeftRoom.UpRoom = new RoomEnd(mg, this);
            MainRoom.LeftRoom.UpRoom.DownRoom = MainRoom.LeftRoom;

            MainRoom.RightRoom = new Room(mg, this, "R2.csv");
            MainRoom.RightRoom.LeftRoom = MainRoom;

            MainRoom.RightRoom.DownRoom = new Room(mg, this, "Boss2.csv");
            MainRoom.RightRoom.DownRoom.UpRoom = MainRoom.RightRoom;

            MainRoom.RightRoom.UpRoom = new Room(mg, this, "BoulderRoom.csv");
            MainRoom.RightRoom.UpRoom.DownRoom = MainRoom.RightRoom;

            MainRoom.LeftRoom.DownRoom = new Room(mg, this, "R4.csv");
            MainRoom.LeftRoom.DownRoom.UpRoom = MainRoom.LeftRoom;

            MainRoom.UpRoom = new Room(mg, this, "R3.csv");
            MainRoom.UpRoom.DownRoom = MainRoom;
            MainRoom.UpRoom.Enemies.Add(new Ghost(mg.Content, mg.SpriteRender, this.Player, new Vector2(400, 300)));

            MainRoom.UpRoom.Enemies.Add(new Ghost(mg.Content, mg.SpriteRender, this.Player, new Vector2(400, 300)));
            MainRoom.UpRoom.Enemies.Add(new Ghost(mg.Content, mg.SpriteRender, this.Player, new Vector2(450, 500)));
            MainRoom.UpRoom.Enemies.Add(new Ghost(mg.Content, mg.SpriteRender, this.Player, new Vector2(150, 400)));

            CurrentRoom.UpRoom.UpRoom.DownRoom = CurrentRoom.UpRoom;
            CurrentRoom.DownRoom = new Room(mg, this, "R5.csv");
            CurrentRoom.DownRoom.Enemies.Add(new EarthBoss(mg.Content, mg.SpriteRender, new Vector2[] 
                { 
                    new Vector2(100, 100), new Vector2(200, 100), new Vector2(300, 200), new Vector2(400, 300)
                }
            ));
            CurrentRoom.DownRoom.UpRoom = CurrentRoom;

            // Infinite Room Loop!
            MainRoom.LeftRoom.LeftRoom = MainRoom;
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
