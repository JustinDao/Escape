using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;

namespace Escape
{
    class SubmissionBar : Bar
    {

        public SubmissionBar(Rectangle bounds, GraphicsDeviceManager graphics)
            : base(graphics, bounds, new Color(Color.Red, 0.5f), new Color(Color.Black, 0.5f))
        {

        }

        public void Update(Player player, GraphicsDeviceManager graphics)
        {
            Percent = (float)player.Submission / (float)Player.MAX_SUBMISSION;
        }
    }
}
