using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Escape
{
    class Boulder : SpriteEntity
    {
		Player Player;

		public Boulder(ContentManager cm, Vector2 position, Player player)
			: base(cm, "boulder.png")
        {
            Origin = new Vector2(0.5f);
            Position = position;
			Player = player;
        }

        public override void Update(GameTime gt, Screen s)
        {
			if (Player.UsingStrength && Player.HitBox.Intersects(this.HitBox)) 
			{
//				Rectangle intersecting = Rectangle.Intersect(Player.HitBox, this.HitBox);
//				int x = intersecting.Width/25;
//				int y = intersecting.Height/25;
				int x = (int)(Position.X - Player.Position.X)/25;
				int y = (int)(Position.Y - Player.Position.Y)/25;

				Position = new Vector2(Position.X + x, Position.Y + y);
			}
        }
    }
}
