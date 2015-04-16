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

            CurrentRoom.AddText("Left Stick to move.", new Vector2(100, 500));
            CurrentRoom.AddText("Right Stick to attack.", new Vector2(750, 500));

            Room MainRoom = new Room(mg, this);

            CurrentRoom.UpRoom = MainRoom;
            MainRoom.DownRoom = CurrentRoom;

            MainRoom.LeftRoom = new Room(mg, this, "RoomTemplateRight.csv");
            MainRoom.LeftRoom.RightRoom = MainRoom;
            MainRoom.LeftRoom.Enemies.Add(new Ghost(mg.Content, mg.SpriteRender, this.Player, new Vector2(450, 500)));
            MainRoom.LeftRoom.Enemies.Add(new Ghost(mg.Content, mg.SpriteRender, this.Player, new Vector2(150, 400)));
            MainRoom.LeftRoom.Enemies.Add(new Ghost(mg.Content, mg.SpriteRender, this.Player, new Vector2(500, 300)));

            //MainRoom.LeftRoom.UpRoom = new RoomEnd(mg, this);
            //MainRoom.LeftRoom.UpRoom.DownRoom = MainRoom.LeftRoom;

            MainRoom.RightRoom = new Room(mg, this, "RoomTemplateLeft.csv");
            MainRoom.RightRoom.LeftRoom = MainRoom;
            MainRoom.RightRoom.Enemies.Add(new Ghost(mg.Content, mg.SpriteRender, this.Player, new Vector2(450, 500)));
            MainRoom.RightRoom.Enemies.Add(new Ghost(mg.Content, mg.SpriteRender, this.Player, new Vector2(150, 400)));


            MainRoom.UpRoom = new Room(mg, this, "RoomTemplateUp.csv");
            MainRoom.UpRoom.DownRoom = MainRoom;
            MainRoom.UpRoom.Obstacles.Add(new Hole(mg.Content, 400, 400, 1));
            MainRoom.UpRoom.Obstacles.Add(new Hole(mg.Content, 425, 400, 2));
            MainRoom.UpRoom.Obstacles.Add(new Hole(mg.Content, 450, 400, 2));
            MainRoom.UpRoom.Obstacles.Add(new Hole(mg.Content, 475, 400, 3));
            MainRoom.UpRoom.Obstacles.Add(new Hole(mg.Content, 475, 425, 4));
            MainRoom.UpRoom.Obstacles.Add(new Hole(mg.Content, 475, 450, 5));
            MainRoom.UpRoom.Obstacles.Add(new Hole(mg.Content, 450, 450, 6));
            MainRoom.UpRoom.Obstacles.Add(new Hole(mg.Content, 425, 450, 6));
            MainRoom.UpRoom.Obstacles.Add(new Hole(mg.Content, 400, 450, 7));
            MainRoom.UpRoom.Obstacles.Add(new Hole(mg.Content, 400, 425, 8));
            MainRoom.UpRoom.Obstacles.Add(new Hole(mg.Content, 425, 425, 9));
            MainRoom.UpRoom.Obstacles.Add(new Hole(mg.Content, 450, 425, 9));


            MainRoom.UpRoom.UpRoom = new Room(mg, this, "RoomTemplateRightDown.csv");
            MainRoom.UpRoom.UpRoom.DownRoom = MainRoom.UpRoom;
            MainRoom.UpRoom.UpRoom.Obstacles.Add(new Hole(mg.Content, 400, 400, 0));
            MainRoom.UpRoom.UpRoom.Obstacles.Add(new Hole(mg.Content, 300, 300, 0));
            MainRoom.UpRoom.UpRoom.Obstacles.Add(new Hole(mg.Content, 450, 200, 0));
            MainRoom.UpRoom.UpRoom.Obstacles.Add(new Hole(mg.Content, 175, 100, 0));
            MainRoom.UpRoom.UpRoom.Obstacles.Add(new Hole(mg.Content, 500, 350, 0));
            MainRoom.UpRoom.UpRoom.Enemies.Add(new Ghost(mg.Content, mg.SpriteRender, this.Player, new Vector2(450, 500)));
            MainRoom.UpRoom.UpRoom.Enemies.Add(new Ghost(mg.Content, mg.SpriteRender, this.Player, new Vector2(150, 400)));
            MainRoom.UpRoom.UpRoom.Enemies.Add(new Ghost(mg.Content, mg.SpriteRender, this.Player, new Vector2(500, 300)));


            MainRoom.RightRoom.DownRoom = new Room(mg, this, "Boss2.csv");
            MainRoom.RightRoom.DownRoom.UpRoom = MainRoom.RightRoom;

            MainRoom.RightRoom.UpRoom = new Room(mg, this, "BoulderRoom.csv");
            MainRoom.RightRoom.UpRoom.DownRoom = MainRoom.RightRoom;

            MainRoom.LeftRoom.DownRoom = new Room(mg, this, "R4.csv");
            MainRoom.LeftRoom.DownRoom.UpRoom = MainRoom.LeftRoom;

            



            /*MainRoom.UpRoom.UpRoom = new Room(mg, this, "R5.csv");
            MainRoom.UpRoom.UpRoom.DownRoom = MainRoom.UpRoom;
            MainRoom.UpRoom.UpRoom.Enemies.Add(new EarthBoss(mg.Content, mg.SpriteRender, new Vector2[] 
                { 
                    new Vector2(100, 100), new Vector2(200, 100), new Vector2(300, 200), new Vector2(400, 300)
                }
            ));*/

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
            foreach (var textureName in preloadTextures)
            {
                cm.Load<Texture2D>(textureName);
            }
        }

    }
}
