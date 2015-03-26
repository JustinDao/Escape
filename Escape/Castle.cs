using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Escape
{
    class Castle : Screen
    {
        public Vector2 CastlePosition { get; set; }
        public int CastleHeight { get; set; }
        public int CastleWidth { get; set; }
        public List<Room> Rooms { get; set; }
        public Room CurrentRoom { get; set; }
        public Player Player { get; set; }
        public Enemy Enemy { get; set; }

        public Castle(MainGame mg)
        {
            Player = new Player(mg, 50, 50);

            Enemy = new Enemy(mg, 200, 200);
            

            Rooms = new List<Room>();

            Rooms.Add(new Room());
            CurrentRoom = Rooms.First<Room>();
        }

        public void Update(Controls controls, GameTime gameTime)
        {
            Player.Update(controls, gameTime, CurrentRoom);
            Enemy.Update(gameTime, CurrentRoom);

            foreach (Room r in Rooms)
            {
                r.Update(gameTime);
            }
        }

        override public void Draw(SpriteBatch sb)
        {
            foreach (Room r in Rooms)
            {
                r.Draw(sb);
            }

            Enemy.Draw(sb);
            Player.Draw(sb);
        }

        public override void LoadContent(ContentManager cm)
        {
            Player.LoadContent(cm);
            Enemy.LoadContent(cm);

            foreach (Room r in Rooms)
            {
                r.LoadContent(cm);
            }

        }

        public override void Update(GameTime gt)
        {
            throw new NotImplementedException();
        }

    }
}
