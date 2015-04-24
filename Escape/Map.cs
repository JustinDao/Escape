using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Escape
{
    class Map : Entity
    {
        const int ROOM_W = 40;
        const int ROOM_H = 24;
        const int ROOM_PADDING = 10;
        const int MAP_PADDING = ROOM_PADDING;
        private Room[,] rooms
        {
            get
            {
                return reader.Rooms;
            }
        }
        public override Rectangle HitBox
        {
            get
            {
                int w = reader.NumCols;
                int h = reader.NumRows;
                return new Rectangle(
                    (int) Position.X,
                    (int) Position.Y,
                    (ROOM_W * w) + (ROOM_PADDING * (w-1)) + MAP_PADDING*2,
                    (ROOM_H * h) + (ROOM_PADDING * (h-1)) + MAP_PADDING*2
                );
            }
        }
        private RoomReader reader;
        public Texture2D RoomSprite;
        private Castle castle;
        public Map(ContentManager cm, Castle c, RoomReader reader)
        {
            this.reader = reader;
            RoomSprite = cm.Load<Texture2D>("pixel.png");
            castle = c;
        }
        public override void Draw(SpriteBatch sb)
        {
            var bgColor = new Color(Color.Black, 0.4f);
            sb.Draw(RoomSprite, HitBox, bgColor);
            for (int x = 0; x < reader.NumCols; x++)
            {
                for (int y = 0; y < reader.NumRows; y++)
                {
                    var room = rooms[x, y];
                    if (room == null) continue;
                    if (!room.Visited) continue;
                    var color = (castle.CurrentRoom == room) ? Color.Blue : Color.Black;
                    var rect = new Rectangle(
                        x * (ROOM_W + ROOM_PADDING) + MAP_PADDING,
                        y * (ROOM_H + ROOM_PADDING) + MAP_PADDING,
                        ROOM_W, ROOM_H);
                    rect.X += (int)Position.X;
                    rect.Y += (int)Position.Y;
                    color = new Color(color, 0.85f);
                    sb.Draw(RoomSprite, rect, color);
                    // left/right doors!
                    if (x != 0)
                    {
                        var neighbor = rooms[x - 1, y];
                        if (neighbor != null)
                        {
                            var r2 = new Rectangle(rect.X, rect.Y, rect.Width, rect.Height);
                            r2.X -= ROOM_PADDING / 2;
                            r2.Width = ROOM_PADDING / 2;
                            r2.Y += ROOM_H / 2 - ROOM_PADDING / 2;
                            r2.Height = ROOM_PADDING;
                            sb.Draw(RoomSprite, r2, color);
                        }
                    }
                    if (x < reader.NumCols - 1)
                    {
                        var neighbor = rooms[x + 1, y];
                        if (neighbor != null)
                        {
                            var r2 = new Rectangle(rect.X, rect.Y, rect.Width, rect.Height);
                            r2.X += ROOM_W;
                            r2.Width = ROOM_PADDING / 2;
                            r2.Y += ROOM_H / 2 - ROOM_PADDING / 2;
                            r2.Height = ROOM_PADDING;
                            sb.Draw(RoomSprite, r2, color);
                        }
                    }
                    // up/down doors!
                    if (y != 0)
                    {
                        var neighbor = rooms[x, y - 1];
                        if (neighbor != null)
                        {
                            var r2 = new Rectangle(rect.X, rect.Y, rect.Width, rect.Height);
                            r2.Y -= ROOM_PADDING / 2;
                            r2.Height = ROOM_PADDING / 2;
                            r2.X += ROOM_W / 2 - ROOM_PADDING / 2;
                            r2.Width = ROOM_PADDING;
                            sb.Draw(RoomSprite, r2, color);
                        }
                    }
                    if (y < reader.NumRows - 1)
                    {
                        var neighbor = rooms[x, y + 1];
                        if (neighbor != null)
                        {
                            var r2 = new Rectangle(rect.X, rect.Y, rect.Width, rect.Height);
                            r2.Y += ROOM_H;
                            r2.Height = ROOM_PADDING / 2;
                            r2.X += ROOM_W / 2 - ROOM_PADDING / 2;
                            r2.Width = ROOM_PADDING;
                            sb.Draw(RoomSprite, r2, color);
                        }
                    }
                }
            }
        }
        public override void Update(GameTime gt, Screen s)
        {

        }
    }
}
