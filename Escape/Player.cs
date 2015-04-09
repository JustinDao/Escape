using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using TexturePackerLoader;

namespace Escape
{
    class Player : AnimatedSpriteEntity
    {
        // constants
        public static readonly float SPEED = 300f;
        // power-ups
        public bool HasFire = false;
        public bool HasIce = false;
        // controls
        public readonly Controls Ctrls;

        // Value of the Player's current Submission
        public int Submission { get; set; }
        // Flag if the player has control of the character
        public bool PlayerControl { get; protected set; }

        // Collision Box (bottom half of the HitBox)
        public Rectangle CollisionBox
        {
            get
            {
                var OFFSET = 10; // pixel offset
                return new Rectangle(HitBox.X + OFFSET, HitBox.Y + HitBox.Height / 2, HitBox.Width - OFFSET * 2, HitBox.Height / 2);
            }
        }

        // The maximum submission value
        private int MAX_SUBMISSION = 100;
        // Variables to hold Submission Time and Interval
        private float SubmissionInterval = 100; // Time in milliseconds
        private float SubmissionTime = 0;

        // Check for switching rooms through doors
        private bool StandingOnDoor = false;

        // The angle of movement in radians
        public float MoveAngle
        {
            get
            {
                if (PlayerControl)
                {
                    return (float)Math.Atan2(Velocity.Y, Velocity.X);
                }
                else
                {
                    return (float)Math.Atan2(AIVelocity.Y, AIVelocity.X);
                }

            }
        }

        // the last direction moved
        Vector2 lastDir = new Vector2();

        // Velocity (direction AND magnitude) of the player
        Vector2 Velocity
        {
            get
            {
                if (PlayerControl)
                {
                    var stick = Ctrls.gp.ThumbSticks.Left;
                    stick.Y *= -1;

                    // If not using controller check keyboard input
                    if (stick.LengthSquared() == 0)
                    {
                        if (Ctrls.isPressed(Keys.Right, Buttons.DPadRight))
                        {
                            stick.X = 1;
                        }
                        else if (Ctrls.isPressed(Keys.Left, Buttons.DPadLeft))
                        {
                            stick.X = -1;
                        }

                        if (Ctrls.isPressed(Keys.Up, Buttons.DPadUp))
                        {
                            stick.Y = -1;
                        }
                        else if (Ctrls.isPressed(Keys.Down, Buttons.DPadDown))
                        {
                            stick.Y = 1;
                        }
                    }

                    return stick * SPEED;
                }
                else
                {
                    return AIVelocity * SPEED * 2;
                }
            }
        }

        // Variables for AI Movement
        // The current Velocity of the AI
        private Vector2 AIVelocity;
        // Variables to track when to update the AI Velocity
        private int AITime;
        private int AIInterval;
        // The Maximum interval to update the AI
        private int AI_SWITCH_TIME = 3000;
        // Random number generator
        private Random rand = new Random();

        // sprite info
        int currentSpriteIndex = 0;
        float currentSpriteInterval = 0;
        int MAX_SPRITE_INDEX = 8;
        int SPRITE_INTERVAL = 50;

        // Sprites
        public string[] UpSprites = {
            TexturePackerMonoGameDefinitions.SoldierSprite.Backward_Layer_1,
            TexturePackerMonoGameDefinitions.SoldierSprite.Backward_Layer_2,
            TexturePackerMonoGameDefinitions.SoldierSprite.Backward_Layer_3,
            TexturePackerMonoGameDefinitions.SoldierSprite.Backward_Layer_4,
            TexturePackerMonoGameDefinitions.SoldierSprite.Backward_Layer_5,
            TexturePackerMonoGameDefinitions.SoldierSprite.Backward_Layer_6,
            TexturePackerMonoGameDefinitions.SoldierSprite.Backward_Layer_7,
            TexturePackerMonoGameDefinitions.SoldierSprite.Backward_Layer_8,
            TexturePackerMonoGameDefinitions.SoldierSprite.Backward_Layer_9,
        };

        public string[] DownSprites = {
            TexturePackerMonoGameDefinitions.SoldierSprite.Forward_Layer_19,
            TexturePackerMonoGameDefinitions.SoldierSprite.Forward_Layer_20,
            TexturePackerMonoGameDefinitions.SoldierSprite.Forward_Layer_21,
            TexturePackerMonoGameDefinitions.SoldierSprite.Forward_Layer_22,
            TexturePackerMonoGameDefinitions.SoldierSprite.Forward_Layer_23,
            TexturePackerMonoGameDefinitions.SoldierSprite.Forward_Layer_24,
            TexturePackerMonoGameDefinitions.SoldierSprite.Forward_Layer_25,
            TexturePackerMonoGameDefinitions.SoldierSprite.Forward_Layer_26,
            TexturePackerMonoGameDefinitions.SoldierSprite.Forward_Layer_27,
        };

        public string[] LeftSprites = {
            TexturePackerMonoGameDefinitions.SoldierSprite.Left_Layer_10,
            TexturePackerMonoGameDefinitions.SoldierSprite.Left_Layer_11,
            TexturePackerMonoGameDefinitions.SoldierSprite.Left_Layer_12,
            TexturePackerMonoGameDefinitions.SoldierSprite.Left_Layer_13,
            TexturePackerMonoGameDefinitions.SoldierSprite.Left_Layer_14,
            TexturePackerMonoGameDefinitions.SoldierSprite.Left_Layer_15,
            TexturePackerMonoGameDefinitions.SoldierSprite.Left_Layer_16,
            TexturePackerMonoGameDefinitions.SoldierSprite.Left_Layer_17,
            TexturePackerMonoGameDefinitions.SoldierSprite.Left_Layer_18,
        };

        public string[] RightSprites = {
            TexturePackerMonoGameDefinitions.SoldierSprite.Right_Layer_28,
            TexturePackerMonoGameDefinitions.SoldierSprite.Right_Layer_29,
            TexturePackerMonoGameDefinitions.SoldierSprite.Right_Layer_30,
            TexturePackerMonoGameDefinitions.SoldierSprite.Right_Layer_31,
            TexturePackerMonoGameDefinitions.SoldierSprite.Right_Layer_32,
            TexturePackerMonoGameDefinitions.SoldierSprite.Right_Layer_33,
            TexturePackerMonoGameDefinitions.SoldierSprite.Right_Layer_34,
            TexturePackerMonoGameDefinitions.SoldierSprite.Right_Layer_35,
            TexturePackerMonoGameDefinitions.SoldierSprite.Right_Layer_36,
        };

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
        public Player(ContentManager cm, SpriteRender sr, Controls ctrls)
            : base(cm, sr, "soldier_sprite_sheet.png")
        {
            Ctrls = ctrls;
            Origin = new Vector2(0.5f, 0.5f); // TODO why does this not work quite right
            PlayerControl = true; // start with the player in control
            Submission = MAX_SUBMISSION; // Initial Submission
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
            Position += Velocity * delta;

            // submission
            UpdateSubmission(gt);

            // AI
            if (!PlayerControl)
            {
                UpdateAI(gt);
            }

            // stuff that involves the castle
            var castle = s as Castle;
            if (castle == null) return;
            var room = castle.CurrentRoom;

            // Check the boundaries
            CheckBoundaries(castle);

            // doors
            CheckDoors(castle);

            // Check holes
            CheckHoles(room);

            // collision
            CollideWalls(room);

            // powerups
            CheckPowerUps(room);

            // Controller Actions
            Action(room);

        }

        private void Action(Room room)
        {
            if (!PlayerControl) return;

            // fire
            if (Ctrls.onPress(Keys.D1, Buttons.A) && HasFire && lastDir.LengthSquared() > 0)
            {
                var fbp = new Vector2(lastDir.X, lastDir.Y);
                room.AddFireBall(Position, fbp);
            }

            // ice
            if (Ctrls.onPress(Keys.D2, Buttons.X) && HasIce)
            {
                room.AddSnowflakes(Position);
            }

        }

        private void CollideWalls(Room room)
        {
            foreach (Wall wall in room.Walls)
            {
                var pBox = CollisionBox;
                var wBox = wall.HitBox;
                var overlap = Rectangle.Intersect(pBox, wBox);

                // If there is no overlap, skip
                if (overlap.Width == 0 && overlap.Height == 0)
                {
                    continue;
                }

                if (overlap.Width > overlap.Height) // move vertically
                {
                    var mov = overlap.Height;
                    if (pBox.Center.Y < wBox.Center.Y)
                    {
                        mov *= -1;
                    }
                    Position.Y += mov;
                }
                else if (overlap.Width < overlap.Height) // move horizontally
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

        private void UpdateSubmission(GameTime gt)
        {
            if (Submission > 0)
            {
                if (SubmissionTime > SubmissionInterval)
                {
                    Submission--;
                    SubmissionTime = 0;
                }
                else
                {
                    SubmissionTime += (float)gt.ElapsedGameTime.TotalMilliseconds;
                }
            }
            else if (Submission <= 0)
            {
                PlayerControl = false;
            }
        }

        private void CheckPowerUps(Room room)
        {
            List<Entity> toRemove = new List<Entity>();

            foreach (Entity o in room.Obstacles)
            {
                if (o is PowerUp)
                {
                    PowerUp p = o as PowerUp;
                    if (p.HitBox.Intersects(this.HitBox))
                    {
                        if (p.IsFire)
                        {
                            this.HasFire = true;
                            toRemove.Add(o);
                        }

                        if (p.IsIce)
                        {
                            this.HasIce = true;
                            toRemove.Add(o);
                        }
                    }
                }
            }

            room.Obstacles = room.Obstacles.Except(toRemove).ToList();
        }

        private void UpdateAI(GameTime gt)
        {
            this.AITime += (int)gt.ElapsedGameTime.TotalMilliseconds;

            if (this.AITime > this.AIInterval)
            {
                AIVelocity = GetRandomVelocity();
                AITime = 0;
                AIInterval = rand.Next(AI_SWITCH_TIME);
            }
        }

        private void CheckDoors(Castle castle)
        {
            var room = castle.CurrentRoom;

            Door door = GetIntersectingDoor(room);

            if (door != null)
            {
                if (!StandingOnDoor)
                {
                    if (door.Equals(room.LeftDoor()))
                    {
                        castle.MoveLeft();
                        this.FlipPosition(room);
                    }
                    else if (door.Equals(room.RightDoor()))
                    {
                        castle.MoveRight();
                        this.FlipPosition(room);
                    }
                    else if (door.Equals(room.UpDoor()))
                    {
                        castle.MoveUp();
                        this.FlipPosition(room);
                    }
                    else if (door.Equals(room.DownDoor()))
                    {
                        castle.MoveDown();
                        this.FlipPosition(room);
                    }
                }

                StandingOnDoor = true;
            }
            else
            {
                StandingOnDoor = false;
            }
        }

        private void CheckBoundaries(Castle castle)
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

        private void CheckHoles(Room room)
        {
            foreach (Entity o in room.Obstacles)
            {
                if (o is Hole)
                {
                    Hole h = (Hole)o;

                    if (h.HitBox.Intersects(CollisionBox))
                    {
                        Respawn();
                        return;
                    }
                }
            }
        }

        private void UpdateSprites(GameTime gt)
        {
            // If the velocity is > 0 in any direction
            // update the last direction
            // update sprites
            if (Velocity.LengthSquared() > 0)
            {
                lastDir = new Vector2(Velocity.X, Velocity.Y); // copy?
                lastDir.Normalize();

                currentSpriteInterval += (float)gt.ElapsedGameTime.TotalMilliseconds;

                if (currentSpriteInterval > SPRITE_INTERVAL)
                {
                    currentSpriteIndex += 1;
                    currentSpriteInterval = 0;

                    if (currentSpriteIndex > MAX_SPRITE_INDEX)
                    {
                        currentSpriteIndex = 0;
                    }
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

        // Helper Methods

        public void RegainControl()
        {
            this.PlayerControl = true;
            this.Submission = MAX_SUBMISSION;
        }

        private void Respawn()
        {
            // TODO: What happens when you die
            this.Position = new Vector2(200, 200);
        }

        private Door GetIntersectingDoor(Room room)
        {
            foreach (Door d in room.Doors.Values)
            {
                if (d == null)
                {
                    return null;
                }

                if (d.HitBox.Intersects(CollisionBox))
                {
                    return d;
                }
            }

            return null;
        }

        private Vector2 GetRandomVelocity()
        {
            return new Vector2((float)rand.NextDouble() * 2 - 1, (float)rand.NextDouble() * 2 - 1);
        }

        private void ChangeAIDirection()
        {
            // Flip Direction of AI if intersecting with wall
            if (!PlayerControl)
            {
                AIVelocity = GetRandomVelocity();
            }
        }

        private void FlipPosition(Room room)
        {
            int x = (int)this.Position.X;
            int y = (int)this.Position.Y;

            int wid = room.Width;
            int hei = room.Height;

            if (x > (wid / 2) - 50 && x < (wid / 2) + 50)
            {
                if (y < hei / 2)
                {
                    y = hei;
                }
                else
                {
                    y = 0;
                }
            }
            if (y > (hei / 2) - 50 && y < (hei / 2) + 50)
            {
                if (x < wid / 2)
                {
                    x = wid;
                }
                else
                {
                    x = 0;
                }
            }

            this.Position = new Vector2(x, y);
        }


    }
}
