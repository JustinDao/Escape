using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace Escape
{
    class NewPlayer : SpriteEntity
    {
        // constants
        public static readonly float SPEED = 150f;
        // power-ups
        public bool HasFire = true;
        public bool HasIce = true;
        // controls
        public readonly Controls Ctrls;
        // other info
        Vector2 lastDir = new Vector2();
        public NewPlayer(ContentManager cm, Controls ctrls) : base(cm,"soldier.png",new Rectangle(13, 137, 38, 58))
        {
            Ctrls = ctrls;
            Origin = new Vector2(0.5f, 0.5f); // TODO why does this not work quite right
        }
        public override void Update(Microsoft.Xna.Framework.GameTime gt, Screen s)
        {
            var delta = (float) gt.ElapsedGameTime.TotalSeconds;
            var stick = Ctrls.gp.ThumbSticks.Left;
            stick.Y *= -1;
            var velocity = stick * SPEED;
            Position += velocity * delta;

            if (velocity.LengthSquared() > 0)
            {
                lastDir = new Vector2(velocity.X, velocity.Y); // copy?
                lastDir.Normalize();
            }
            
            // stuff that involves the castle
            var castle = s as Castle;
            if (castle == null) return;
            var room = castle.CurrentRoom;

            // collision
            CollideWalls(room);

            // fire
            if (Ctrls.onPress(Keys.None, Buttons.A) && HasFire && lastDir.LengthSquared() > 0)
            {
                var fbp = new Vector2(lastDir.X, lastDir.Y);
                room.AddFireBall(Position, fbp);
            }

            // ice
            if (Ctrls.onPress(Keys.None, Buttons.X) && HasIce)
            {
                room.AddSnowflakes(Position);
            }
        }

        private void CollideWalls(Room room)
        {
            foreach(Wall wall in room.Walls)
            {
                var pBox = HitBox;
                var wBox = wall.HitBox;
                var overlap = Rectangle.Intersect(pBox, wBox);
                if (overlap.Width == 0 && overlap.Height == 0)
                {
                    continue;
                } else if (overlap.Width > overlap.Height) // move vertically
                {
                    var mov = overlap.Height;
                    if (pBox.Center.Y < wBox.Center.Y)
                    {
                        mov *= -1;
                    }
                    Position.Y += mov;
                }
                else // move horizontally
                {
                    var mov = overlap.Width;
                    if (pBox.Center.X < wBox.Center.X)
                    {
                        mov *= -1;
                    }
                    Position.X += mov;
                }
            }
        }
    }
}
