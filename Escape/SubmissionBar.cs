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


        public SubmissionBar(int x, int y, GraphicsDeviceManager graphics)
        {
            this.XPosition = x;
            this.YPosition = y;
            this.Box = new Rectangle(XPosition, YPosition, Width, Height);

            this.BoxColor = Color.Red;
            this.Texture = new Texture2D(graphics.GraphicsDevice, Width, Height);
            Color[] data = new Color[80 * 30];
            for (int i = 0; i < data.Length; ++i) data[i] = this.BoxColor;
            Texture.SetData(data);
            this.Coor = new Vector2(20, 20);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Coor, Color.White);
        }

        public void Update(Player player, GraphicsDeviceManager graphics)
        {
            this.Width = player.Submission;
            this.Texture = new Texture2D(graphics.GraphicsDevice, Width, Height);
            Color[] data = new Color[80 * 30];
            for (int i = 0; i < data.Length; ++i) data[i] = this.BoxColor;
            Texture.SetData(data);
        }
    }
}
