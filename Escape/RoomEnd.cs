using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Escape
{
    class RoomEnd : Room
    {
        public RoomEnd(MainGame mg, Castle castle) : base(mg, castle, "EndRoom.csv")
        {

        }

        public override void DrawText(SpriteBatch sb)
        {
            if (!this.Castle.Player.BeatTheGame)
            {
                foreach (Text t in OnScreenText)
                {
                    t.Draw(sb);
                }
            }            
        }
    }
}
