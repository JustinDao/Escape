using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Escape
{
    class RoomSub : Room
    {
        public RoomSub(MainGame mg, Castle castle)
            : base(mg, castle, "SubmissionRoom.csv")
        {

        }

    }
}
