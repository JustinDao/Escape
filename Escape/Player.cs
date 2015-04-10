using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TexturePackerLoader;

namespace Escape
{
    class Player : Character
    {
        // player stuff
        // power-ups
        public bool HasFire = false;
        public bool HasIce = false;
        // controls
        public readonly Controls Ctrls;
        // Value of the Player's current Submission
        public int Submission { get; set; }
        // Flag if the player has control of the character
        public bool PlayerControl { get; protected set; }

        // The maximum submission value
        private int MAX_SUBMISSION = 100;
        // Variables to hold Submission Time and Interval
        private float SubmissionInterval = 100; // Time in milliseconds
        private float SubmissionTime = 0;

        // Check for switching rooms through doors
        private bool StandingOnDoor = false;

        // the last direction moved
        Vector2 lastDir = new Vector2();

        // AI direction

        private Vector2 aiVelocity;

        private int aiTime;
        private int aiInterval;
        // The Maximum interval to update the AI
        const int AI_SWITCH_TIME = 3000;
        // Random number generator
        private Random rand = new Random();



        // overrides
        public override float MaxSpeed
        {
            get { return 300; }
        }

        public override int SpriteInterval
        {
            get
            {
                return 50;
            }
        }

        // eeewww :(
        public override string[] UpSprites
        {
            get
            {
                return new string[] {
                    TexturePackerMonoGameDefinitions.SoldierSprites.Backward_Layer_1,
                    TexturePackerMonoGameDefinitions.SoldierSprites.Backward_Layer_2,
                    TexturePackerMonoGameDefinitions.SoldierSprites.Backward_Layer_3,
                    TexturePackerMonoGameDefinitions.SoldierSprites.Backward_Layer_4,
                    TexturePackerMonoGameDefinitions.SoldierSprites.Backward_Layer_5,
                    TexturePackerMonoGameDefinitions.SoldierSprites.Backward_Layer_6,
                    TexturePackerMonoGameDefinitions.SoldierSprites.Backward_Layer_7,
                    TexturePackerMonoGameDefinitions.SoldierSprites.Backward_Layer_8,
                    TexturePackerMonoGameDefinitions.SoldierSprites.Backward_Layer_9,
                };
            }
        }

        public override string[] DownSprites
        {
            get
            {
                return new string[] {
                    TexturePackerMonoGameDefinitions.SoldierSprites.Forward_Layer_19,
                    TexturePackerMonoGameDefinitions.SoldierSprites.Forward_Layer_20,
                    TexturePackerMonoGameDefinitions.SoldierSprites.Forward_Layer_21,
                    TexturePackerMonoGameDefinitions.SoldierSprites.Forward_Layer_22,
                    TexturePackerMonoGameDefinitions.SoldierSprites.Forward_Layer_23,
                    TexturePackerMonoGameDefinitions.SoldierSprites.Forward_Layer_24,
                    TexturePackerMonoGameDefinitions.SoldierSprites.Forward_Layer_25,
                    TexturePackerMonoGameDefinitions.SoldierSprites.Forward_Layer_26,
                    TexturePackerMonoGameDefinitions.SoldierSprites.Forward_Layer_27,
                };
            }
        }

        public override string[] LeftSprites
        {
            get
            {
                return new string[] {
                    TexturePackerMonoGameDefinitions.SoldierSprites.Left_Layer_10,
                    TexturePackerMonoGameDefinitions.SoldierSprites.Left_Layer_11,
                    TexturePackerMonoGameDefinitions.SoldierSprites.Left_Layer_12,
                    TexturePackerMonoGameDefinitions.SoldierSprites.Left_Layer_13,
                    TexturePackerMonoGameDefinitions.SoldierSprites.Left_Layer_14,
                    TexturePackerMonoGameDefinitions.SoldierSprites.Left_Layer_15,
                    TexturePackerMonoGameDefinitions.SoldierSprites.Left_Layer_16,
                    TexturePackerMonoGameDefinitions.SoldierSprites.Left_Layer_17,
                    TexturePackerMonoGameDefinitions.SoldierSprites.Left_Layer_18,
                };
            }
        }

        public override string[] RightSprites
        {
            get
            {
                return new string[] {
                    TexturePackerMonoGameDefinitions.SoldierSprites.Right_Layer_28,
                    TexturePackerMonoGameDefinitions.SoldierSprites.Right_Layer_29,
                    TexturePackerMonoGameDefinitions.SoldierSprites.Right_Layer_30,
                    TexturePackerMonoGameDefinitions.SoldierSprites.Right_Layer_31,
                    TexturePackerMonoGameDefinitions.SoldierSprites.Right_Layer_32,
                    TexturePackerMonoGameDefinitions.SoldierSprites.Right_Layer_33,
                    TexturePackerMonoGameDefinitions.SoldierSprites.Right_Layer_34,
                    TexturePackerMonoGameDefinitions.SoldierSprites.Right_Layer_35,
                    TexturePackerMonoGameDefinitions.SoldierSprites.Right_Layer_36,
                };
            }
        }

        // Velocity (direction AND magnitude) of the player
        public override Vector2 CurrentVelocity
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

                    return stick * MaxSpeed;
                }
                else
                {
                    return aiVelocity * MaxSpeed * 2;
                }
            }
        }

        public Player(ContentManager cm, SpriteRender sr, Controls ctrls)
            : base(cm, sr, "soldier_sprite_sheet.png")
        {
            Ctrls = ctrls;
            PlayerControl = true;
            Submission = MAX_SUBMISSION;
        }

        public override void Update(GameTime gt, Screen s)
        {
            // submission
            UpdateSubmission(gt);

            // AI
            if (!PlayerControl)
            {
                UpdateAI(gt);
            }

            // lastDir
            if (CurrentVelocity.LengthSquared() > 0)
            {
                lastDir = CurrentVelocity;
                lastDir.Normalize();
            }

            // stuff that involves the castle
            var castle = s as Castle;
            if (castle != null)
            {
                var room = castle.CurrentRoom;
                // doors
                CheckDoors(castle);

                // powerups
                CheckPowerUps(room);

                // Controller Actions
                Action(room);
            }

            // base update!
            base.Update(gt, s);
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
            this.aiTime += (int)gt.ElapsedGameTime.TotalMilliseconds;

            if (this.aiTime > this.aiInterval)
            {
                aiVelocity = GetRandomVelocity();
                aiTime = 0;
                aiInterval = rand.Next(AI_SWITCH_TIME);
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
            // Flip Direction of AI if intersecting with c
            if (!PlayerControl)
            {
                aiVelocity = GetRandomVelocity();
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
