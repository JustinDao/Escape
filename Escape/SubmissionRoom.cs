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
        public Boolean visited = false;



        public SubmissionRoom(MainGame mg, Castle castle)
            : base(mg, castle, "SubmissionRoom.csv")
        {
            this.mg = mg;
            this.controls = mg.Control;
            this.cm = mg.Content;
            this.castle = castle;


            //castle.Player.Submission = 10;

        }


        public override void Update(GameTime gt, Screen s)
        {
            if (!visited)
            {
                castle.Player.Submission = 10;
                visited = true;
            }

            base.Update(gt, s);
        }
    }
}
