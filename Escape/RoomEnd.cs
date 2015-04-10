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

        public void Update(GameTime gameTime, Screen s)
        {
            base.Update(gameTime, s);
        }

        public void Draw(SpriteBatch sb)
        {
            base.Draw(sb);
        }
    }
}
