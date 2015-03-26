using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;

namespace Escape
{
	class Hole : Obstacle
	{
		public Texture2D sprite;
		public int XPosition;
		public int YPosition;
		public int Width;
		public int Height;
		public int Type;
		public Rectangle HitBox;

		public Hole(int x, int y, int t)
		{
			this.Width = 25;
			this.Height = 25;
			this.XPosition = x;
			this.YPosition = y;
			this.Type = t;
			HitBox = new Rectangle(XPosition, YPosition, Width, Height);
		}

		override public void LoadContent(ContentManager content)
		{
			string file = string.Concat (string.Concat ("hole", this.Type.ToString()), ".png");
			sprite = content.Load<Texture2D>(file);
		}

		override public void Draw(SpriteBatch sb)
		{
			sb.Draw(sprite,
				new Rectangle(XPosition, YPosition, Width, Height),
				Color.White);
		}

        public override void Update(GameTime gt)
        {
            throw new NotImplementedException();
        }
	}
}
