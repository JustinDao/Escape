﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TexturePackerLoader;

namespace Escape
{
    abstract class Character : AnimatedSpriteEntity
    {

        // Collision Box (bottom half of the HitBox)
        public Rectangle CollisionBox
        {
            get
            {
                var OFFSET = 10; // pixel offset
                return new Rectangle(HitBox.X + OFFSET, HitBox.Y + HitBox.Height / 2, HitBox.Width - OFFSET * 2, HitBox.Height / 2);
            }
        }

        // The angle of movement in radians
        public float MoveAngle
        {
            get
            {
                return (float)Math.Atan2(CurrentVelocity.Y, CurrentVelocity.X);
            }
        }

        public abstract float MaxSpeed
        {
            get;
        }

        // Velocity (direction AND magnitude) of the character
        public abstract Vector2 CurrentVelocity
        {
            get;
        }

        // sprite info
        int currentSpriteIndex = 0;
        float currentSpriteInterval = 0;

        public int MaxSpriteIndex {
            get
            {
                return CurrentSprites.Length-1; // derp :)
            }
        }

        public abstract int SpriteInterval {
            get;
        }

        // Sprites
        public abstract string[] UpSprites {
            get;
        }

        public abstract string[] DownSprites {
            get;
        }

        public abstract string[] LeftSprites {
            get;
        }

        public abstract string[] RightSprites {
            get;
        }

        // The Current Array of Sprites
        public string[] CurrentSprites;

        // The current Sprite referenced by a string
        public override string SpriteString
        {
            get
            {
                return CurrentSprites[currentSpriteIndex];
            }
        }

        // Constructor
        public Character(ContentManager cm, SpriteRender sr, string spriteSheetName)
            : base(cm, sr, spriteSheetName)
        {
            Origin = new Vector2(0.5f, 0.5f); // TODO why does this not work quite right
            CurrentSprites = DownSprites;
        }

        // Update Methods

        public override void Update(GameTime gt, Screen s)
        {
            // Get Time since last frame
            var delta = (float)gt.ElapsedGameTime.TotalSeconds;

            // Update the SpriteSheet
            UpdateSprites(gt);

            // Move the player
            Position += CurrentVelocity * delta;

            // stuff that involves the castle
            var castle = s as Castle;
            if (castle == null) return;
            var room = castle.CurrentRoom;

            // Check the boundaries
            CheckBoundaries(castle);

            // collision
            CollideObstacles(room);

        }

        protected void collideObstacle(Entity e)
        {
            var pBox = CollisionBox;
            var eBox = e.HitBox;
            var overlap = Rectangle.Intersect(pBox, eBox);

            // If there is no overlap, skip
            if (overlap.Width == 0 && overlap.Height == 0)
            {
                return;
            }

            if (overlap.Width > overlap.Height) // move vertically
            {
                var mov = overlap.Height;
                if (pBox.Center.Y < eBox.Center.Y)
                {
                    mov *= -1;
                }
                Position.Y += mov;
            }
            else if (overlap.Width < overlap.Height) // move horizontally
            {
                var mov = overlap.Width;
                if (pBox.Center.X < eBox.Center.X)
                {
                    mov *= -1;
                }
                Position.X += mov;
            }
        }

        public virtual void CollideObstacles(Room room)
        {
            foreach (var wall in room.Walls)
            {
                collideObstacle(wall);
            }
            foreach (var e in room.Obstacles)
            {
                collideObstacle(e);
            }
        }

        protected void CheckBoundaries(Castle castle)
        {
            var game = castle.mg;

            if (this.Position.X < 0)
            {
                Position = new Vector2(0, Position.Y);
            }

            else if (this.Position.X > game.GAME_WIDTH - this.CollisionBox.Width)
            {
                Position = new Vector2(game.GAME_WIDTH - this.CollisionBox.Width, Position.Y);
            }

            if (this.Position.Y < -25)
            {
                Position = new Vector2(Position.X, 0);
            }

            else if (this.Position.Y > game.GAME_HEIGHT - this.CollisionBox.Height)
            {
                Position = new Vector2(Position.X, game.GAME_HEIGHT - this.CollisionBox.Height);
            }
        }

        protected void UpdateSprites(GameTime gt)
        {
            // If the velocity is > 0 in any direction
            // update the last direction
            // update sprites
            if (CurrentVelocity.LengthSquared() > 0)
            {

                var percentSpeed = CurrentVelocity.Length() / MaxSpeed;
                currentSpriteInterval += (float)gt.ElapsedGameTime.TotalMilliseconds * percentSpeed;

                if (currentSpriteInterval > SpriteInterval)
                {
                    currentSpriteIndex += 1;
                    currentSpriteInterval = 0;

                    if (currentSpriteIndex > MaxSpriteIndex)
                    {
                        currentSpriteIndex = 0;
                    }
                }

                // Check Direction, and update Sprites accordingly

                var pi = Math.PI;

                float angle = MoveAngle;

                if (angle < 0)
                {
                    angle += (float)(2 * pi);
                }

                if (angle < pi / 4 || angle > 7 * pi / 4)
                {
                    CurrentSprites = RightSprites;
                }
                else if (angle < 3 * pi / 4)
                {
                    CurrentSprites = DownSprites;
                }
                else if (angle < 5 * pi / 4)
                {
                    CurrentSprites = LeftSprites;
                }
                else
                {
                    CurrentSprites = UpSprites;
                }
            }

        }

    }
}
