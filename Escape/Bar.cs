using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;

namespace Escape
{
    abstract class Bar
    {
        public static Texture2D tex = null;
        public readonly Color FGColor;
        public readonly Color BGColor;
        public Rectangle Bounds;
        public float Percent;

        public Bar(GraphicsDeviceManager gdm, Rectangle bounds, Color fg, Color bg)
        {
            Bounds = bounds;
            FGColor = fg;
            BGColor = bg;
            if (tex == null)
            {
                tex = new Texture2D(gdm.GraphicsDevice, 1, 1);
                Color[] data = new Color[1] {
                    Color.White
                };
                tex.SetData(data);
            }
        }

        public void Draw(SpriteBatch batch)
        {
            batch.Draw(tex, Bounds, BGColor);
            var fg = new Rectangle(Bounds.X, Bounds.Y, (int)(Percent * Bounds.Width), Bounds.Height);
            batch.Draw(tex, fg, FGColor);
        }
    }
}
