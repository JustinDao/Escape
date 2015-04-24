using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
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
        public Map Minimap;
        public bool ShowMap = false;

        public Castle(MainGame mg)
        {
            this.mg = mg;

            Player = new Player(mg.Content, mg.SpriteRender, mg.Control, mg, this);
            Player.Position += new Vector2(mg.GAME_WIDTH / 2, mg.GAME_HEIGHT - 100);

            InitializeRooms();
        }

        public void InitializeRooms()
        {
            this.RR = new RoomReader(mg, this, "Master.csv");
            Minimap = new Map(mg.Content, this, RR);
            var mapArea = Minimap.HitBox;
            Minimap.Center = new Vector2(500, 300);
            CurrentRoom = this.RR.StartRoom;
        }

        public void Update(Controls controls, GameTime gameTime)
        {
            if (DebugRoom != null && CurrentRoom != DebugRoom)
            {
                CurrentRoom = DebugRoom;
                Player.VisitedRooms.Clear();
                DebugRoom = null;
            }

            if (controls.onPress(Keys.Space, Buttons.Start))
            {
                mg.Pause();
            }

            if (controls.onPress(Keys.Enter, Buttons.Back))
            {
                ShowMap = !ShowMap;
            }

            Player.Update(gameTime, this);
            if (!CurrentRoom.Visited) CurrentRoom.Visited = true;
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
            if (ShowMap) Minimap.Draw(sb);
        }

        public override void LoadContent(ContentManager cm)
        {
            // preload assets
            string[] preloadTextures = {
                                           "fireball.png",
                                           "snowflake.png",
                                           "castle_door.png",
                                       };
            string[] preloadSounds = {
                                         "swish",
                                         "ow",
                                         "slash",
                                         "fireball",
                                         "ice",
                                         "grunt",
                                         "zoom",
                                     };
            foreach (var textureName in preloadTextures)
            {
                cm.Load<Texture2D>(textureName);
            }
            foreach (var soundName in preloadSounds)
            {
                cm.Load<SoundEffect>("Sounds/" + soundName);
            }
        }

    }
}
