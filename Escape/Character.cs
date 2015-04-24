using Microsoft.Xna.Framework;
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
        public bool DetectEnemyCollision = false;
        public Character Parent = null;
        public float FreezeTimer = 0;
        public bool Frozen
        {
            get
            {
                return FreezeTimer > 0;
            }
        }
        public virtual float SpeedMult
        {
            get
            {
                return Frozen ? FreezeMult : 1f;
            }
        }
        public virtual float FreezeMult
        {
            get
            {
                return 0.1f;
            }
        }

        protected bool ignoreHoles = false;
        protected bool ignoreWater = false;
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

        public virtual float AnimationSpeed
        {
            get
            {
                return SpeedMult * CurrentVelocity.Length() / MaxSpeed;
            }
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
            Position += CurrentVelocity * delta * SpeedMult;

            // stuff that involves the Castle
            var castle = s as Castle;
            if (castle == null) return;
            var room = castle.CurrentRoom;

            if (FreezeTimer > 0)
            {
                FreezeTimer -= delta;
            }

            // Check the boundaries
            CheckBoundaries(castle);

            // collision
            CollideObstacles(room);

        }

        public bool CollideObstacle(Rectangle eBox)
        {
            var pBox = CollisionBox;
            var overlap = Rectangle.Intersect(pBox, eBox);

            // If there is no overlap, skip
            if (overlap.Width == 0 && overlap.Height == 0)
            {
                return false;
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
            return true;
        }

        public virtual void OnEnemyCollision(Enemy e)
        {
            // do nothing by default
        }

        public virtual void CollideObstacles(Room room)
        {
            foreach (var wall in room.Walls)
            {
                CollideObstacle(wall.HitBox);
            }
            foreach (var e in room.Obstacles)
            {
                if (e is Hole && ignoreHoles) continue;
                if (e is Water && ignoreWater) continue;
                if (e is Water && (e as Water).IsFrozen) continue;
                CollideObstacle(e.HitBox);
            }
            if (DetectEnemyCollision)
            {
                foreach (var e in room.Enemies)
                {
                    if (e == this || e == Parent || e.Parent == this) continue;
                    if (CollideObstacle(e.CollisionBox))
                    {
                        OnEnemyCollision(e);
                    }
                }
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
            // update sprites

            var percentSpeed = AnimationSpeed;
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

            // If the velocity is > 0 in any direction
            // update the last direction
            if (CurrentVelocity.LengthSquared() > 0)
            {

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
