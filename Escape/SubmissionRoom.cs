using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Escape
{
    class SubmissionRoom : Room
    {
        ContentManager cm;
        Controls controls;
        Castle castle;

        public Room CurrentRoom { get; set; }
        public Boolean done = false;



        public SubmissionRoom(MainGame mg, Castle castle)
            : base(mg, castle, "SubmissionRoom.csv")
        {
            this.mg = mg;
            this.controls = mg.Control;
            this.cm = mg.Content;
            this.castle = castle;
        }


        public override void Update(GameTime gt, Screen s)
        {
            if (!done && castle.Player.Position.Y < 200)
            {
                castle.Player.Submission = 0;
                done = true;
            }

            base.Update(gt, s);
        }
    }
}
