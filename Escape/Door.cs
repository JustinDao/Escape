using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;

namespace Escape
{
	class Door : Obstacle
	{
		public Texture2D sprite;
		public int XPosition;
		public int YPosition;
		public int Width;
		public int Height;
		public Rectangle HitBox;

		public Door(int x, int y, bool side)
		{
            if (side)
            {
                this.Width = 25;
                this.Height = 50;
            }
            else
            {
                this.Width = 50;
                this.Height = 25;
            }

			
			this.XPosition = x;
			this.YPosition = y;
			HitBox = new Rectangle(XPosition, YPosition, Width, Height);
		}

		override public void LoadContent(ContentManager content)
		{
			sprite = content.Load<Texture2D>("null.png");
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
