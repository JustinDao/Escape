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
        public bool Removed = false;

        public const float SCALE = 0.75f;

        public Boulder(ContentManager cm, Vector2 position, float scale = SCALE)
            : base(cm, "boulder.png")
        {
            Origin = new Vector2(0.5f);
            Center = position;
            Scale = SCALE;
        }

        public override void Update(GameTime gt, Screen s)
        {

            // stuff that involves the Castle
            var castle = s as Castle;
            if (castle == null) return;
            var room = castle.CurrentRoom;
            var player = castle.Player;

            CollideCharacter(player, player.UsingStrength);

            foreach (Enemy e in room.Enemies)
            {
                CollideCharacter(e, false);
            }
            CollideObstacles(room);

        }

        public void CollideCharacter(Character c, bool strength)
        {
            if (c.CollisionBox.Intersects(this.HitBox))
            {
                Rectangle overlap = Rectangle.Intersect(c.CollisionBox, this.HitBox);

                if (overlap.Width == 0 && overlap.Height == 0)
                {
                    return;
                }

                if (overlap.Width > overlap.Height)
                {
                    var mov = overlap.Height;

                    if (c.CollisionBox.Center.Y < this.HitBox.Center.Y)
                    {
                        mov *= -1;
                    }

                    if (strength)
                    {
                        this.Position -= new Vector2(0, mov);
                    }
                    else
                    {
                        c.Position.Y += mov;
                    }                   
                }

                if (overlap.Width < overlap.Height)
                {
                    var mov = overlap.Width;
                    if (c.CollisionBox.Center.X < this.HitBox.Center.X)
                    {
                        mov *= -1;
                    }

                    if (strength)
                    {
                        this.Position -= new Vector2(mov, 0);
                    }
                    else
                    {
                        c.Position.X += mov;
                    }  
                }

            }
        }

        public virtual void CollideObstacles(Room r)
        {
            foreach (Entity w in r.Walls)
            {
                CollideObstacle(w, r, false);
            }
            foreach (Entity e in r.Obstacles)
            {
                bool h = (e is Hole);
                CollideObstacle(e, r, h);
            }
        }

        protected void CollideObstacle(Entity e, Room r, bool h)
        {
            Rectangle overlap = Rectangle.Intersect(this.HitBox, e.HitBox);

            if (overlap.Width == 0 && overlap.Height == 0)
            {
                return;
            }
            if (InAHole(r))
            {
                r.Boulders.Remove(this);
                Removed = true;
            }
            if (overlap.Width > overlap.Height && !h)
            {
                var mov = overlap.Height;
                if (this.HitBox.Center.Y < e.HitBox.Center.Y)
                {
                    mov *= -1;
                }
                this.Position.Y += mov;
            }
            if (overlap.Width < overlap.Height && !h)
            {
                var mov = overlap.Width;
                if (this.HitBox.Center.X < e.HitBox.Center.X)
                {
                    mov *= -1;
                }
                this.Position.X += mov;
            }
        }

        public bool InAHole(Room r)
        {
            int xl = this.HitBox.Left;
            int xr = this.HitBox.Right;
            int yt = this.HitBox.Top;
            int yb = this.HitBox.Bottom;
            Point c = this.HitBox.Center;

            int tl = 0;
            int tr = 0;
            int bl = 0;
            int br = 0;
            int cc = 0;

            foreach (var e in r.Obstacles)
            {
                if (e is Hole)
                {
                    if (e.HitBox.Contains(new Point(xl, yt)))
                    {
                        tl += 1;
                    }
                    if (e.HitBox.Contains(new Point(xr, yt)))
                    {
                        tr += 1;
                    }
                    if (e.HitBox.Contains(new Point(xl, yb)))
                    {
                        tl += 1;
                    }
                    if (e.HitBox.Contains(new Point(xr, yb)))
                    {
                        tl += 1;
                    }
                    if (e.HitBox.Contains(c))
                    {
                        cc += 1;
                    }
                }
            }
            return (tl + tr + bl + br + cc) >= 5;
        }
    }
}
