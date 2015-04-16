using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TexturePackerLoader;

namespace Escape
{
    class Player : Character
    {
        // player stuff
        // attacking!
        const int ATTACK_SIZE = 20;
        const float ATTACK_REACH = 100 - ATTACK_SIZE;
        public Vector2 AttackVector
        {
            get
            {
                var rStick = Ctrls.gp.ThumbSticks.Right;
                rStick.Y *= -1;

                if (rStick.LengthSquared() == 0)
                {
                    if(this.Ctrls.isPressed(Keys.W))
                    {
                        rStick.Y = -1;
                    }

                    if (this.Ctrls.isPressed(Keys.A))
                    {
                        rStick.X = -1;
                    }

                    if (this.Ctrls.isPressed(Keys.S))
                    {
                        rStick.Y = 1;
                    }

                    if (this.Ctrls.isPressed(Keys.D))
                    {
                        rStick.X = 1;
                    }
                }

               return rStick;
            }
        }
        public Rectangle? AttackArea = null;
        Texture2D weaponTexture;
        // getting hit
        const float HIT_TIME = 1;
        public float HitTimer = 0;
        // power-ups
        public bool HasFire = false;
        public bool HasIce = false;
        public bool HasSpeed = false;
        public bool HasStrength = false;
        public bool UsingStrength
        {
            get
            {
                return (Ctrls.isPressed(Keys.D3, Buttons.A) && HasStrength);
            }
        }

        public bool BeatTheGame = false;

        // controls
        public readonly Controls Ctrls;
        // Value of the Player's current Submission
        public int Submission { get; set; }
        // Flag if the player has control of the character
        public bool PlayerControl { get; protected set; }

        // The maximum submission value
        public const int MAX_SUBMISSION = 10000;
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

        // Questions
        public List<Question> Questions;

        // overrides
        public override Color Tint
        {
            get
            {
                return HitTimer > 0 ? Color.Red : base.Tint;
            }
        }

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

        // Player Sprites
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
                    if (AttackArea.HasValue)
                    {
                        return Vector2.Zero;
                    }
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

                    if (HasSpeed && Ctrls.isPressed(Keys.D4, Buttons.Y))
                    {
                        return stick * MaxSpeed * 2;
                    }
                    else
                    {
                        return stick * MaxSpeed;
                    }
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

            weaponTexture = cm.Load<Texture2D>("spear.png");

            Questions = new List<Question>();

            // Create Question Set
            var path = @"Content\\questions.txt";

            using (var stream = TitleContainer.OpenStream(path))
            using (var reader = new StreamReader(stream))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();

                    string[] cells = line.Split(';');

                    var question = cells[0];

                    var answers = new List<string>();

                    for (int i = 1; i < cells.Length; i++)
                    {
                        answers.Add(cells[i]);
                    }

                    Questions.Add(new Question(question, answers, rand.Next(answers.Count)));
                }
            }
        }

        public override void Update(GameTime gt, Screen s)
        {
            float delta = (float)gt.ElapsedGameTime.TotalSeconds;
            if (HitTimer > 0)
            {
                HitTimer -= delta;
            }

            // Check If the Player should beat the game
            CheckEndGame(s);

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

            // stuff that involves the Castle
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

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch sb)
        {
            base.Draw(sb);
            if (AttackArea.HasValue)
            {
                var area = AttackArea.Value;
                // sb.Draw(weaponTexture, AttackArea.Value, Tint);
                sb.Draw(
                    weaponTexture,
                    new Vector2(area.Center.X, area.Center.Y),
                    null,
                    Tint,
                    (float)Math.Atan2(AttackVector.Y,AttackVector.X),
                    new Vector2(weaponTexture.Width, weaponTexture.Height/2f),
                    1,
                    SpriteEffects.FlipHorizontally,
                    Depth);
            }
        }
        private void CheckEndGame(Screen s)
        {
            if (!(s is Castle)) return;

            var castle = s as Castle;

            if (castle.CurrentRoom is RoomEnd)
            {
                if (this.HasFire && this.HasIce && this.HasSpeed && this.HasStrength)
                {
                    this.BeatTheGame = true;
                }
            }
            
        }

        private void Action(Room room)
        {
            if (!PlayerControl)
            {
                AttackArea = null;
                return;
            }

            // lance
            AttackArea = null;
            
            // 0.15 buffer for control stick
            if (AttackVector.LengthSquared() > 0.15)
            {
                Console.WriteLine(AttackVector.LengthSquared());
                var pCenter = HitBox.Center;
                var area = new Rectangle(pCenter.X - ATTACK_SIZE / 2,
                    pCenter.Y - ATTACK_SIZE / 2,
                    ATTACK_SIZE,
                    ATTACK_SIZE
                );
                area.X += (int)(ATTACK_REACH * AttackVector.X);
                area.Y += (int)(ATTACK_REACH * AttackVector.Y);
                AttackArea = area;
            }

            // fire
            if (Ctrls.onPress(Keys.D1, Buttons.B) && HasFire && lastDir.LengthSquared() > 0)
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
            List<PowerUp> toRemove = new List<PowerUp>();

            foreach (PowerUp p in room.PowerUps)
                {
                    if (p.HitBox.Intersects(this.HitBox))
                    {
                        var o = p;
                        Submission = MAX_SUBMISSION;
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

                        if (p.IsStrength)
                        {
                            this.HasStrength = true;
                            room.AddText("You feel stronger! Press A to activate strength power!", new Vector2(300, 300));
                            toRemove.Add(o);
                        }

                        if (p.IsSpeed)
                        {
                            this.HasSpeed = true;
                            toRemove.Add(o);
                        }
                    
                }
            }

            room.PowerUps = room.PowerUps.Except(toRemove).ToList();
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

        public override void OnEnemyCollision(Enemy e)
        {
            if (HitTimer > 0) return;
            HitTimer = HIT_TIME;
            Submission -= e.Damage;
        }

        public void OnProjectileCollision(Projectile p)
        {
            if (!p.Evil) return;
            OnEnemyCollision(p.Owner); // hacky :P
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
