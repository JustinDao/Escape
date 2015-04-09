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
    class NewPlayer : AnimatedSpriteEntity
    {
        // constants
        public static readonly float SPEED = 300f;
        // power-ups
        public bool HasFire = true;
        public bool HasIce = true;
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

        public override string SpriteString 
        {
            get
            {
                return CurrentSprites[currentSpriteIndex];
            }
        }        

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

        // other info
        Vector2 lastDir = new Vector2();
        // Stores Velocity
        Vector2 Velocity
        {
            get
            {
                if (PlayerControl)
                {
                    var stick = Ctrls.gp.ThumbSticks.Left;
                    stick.Y *= -1;
                    return stick * SPEED;
                }
                else
                {
                    return AIVelocity * SPEED * 2;
                }
            }
        }
        
        public NewPlayer(ContentManager cm, SpriteRender sr, Controls ctrls)
            : base(cm, sr, "soldier_sprite_sheet.png")
        {
            Ctrls = ctrls;
            Origin = new Vector2(0.5f, 0.5f); // TODO why does this not work quite right
            PlayerControl = true; // start with the player in control
            Submission = 1000; // Initial Submission
            CurrentSprites = DownSprites;
        }

        public override void Update(GameTime gt, Screen s)
        {
            // Get Time since last frame
            var delta = (float)gt.ElapsedGameTime.TotalSeconds;
            // Get current velocity
            var velocity = Velocity;
               
            // If the velocity is > 0 in any direction
            // update the last direction
            // update sprites
            if (velocity.LengthSquared() > 0)
            {
                lastDir = new Vector2(velocity.X, velocity.Y); // copy?
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
            
            // Move the player
            Position += velocity * delta;

            // stuff that involves the castle
            var castle = s as Castle;
            if (castle == null) return;
            var room = castle.CurrentRoom;

            // collision
            CollideWalls(room);

            // submission
            UpdateSubmission(gt);

            // AI
            if (!PlayerControl)
            {
                UpdateAI(gt);
            }

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
            foreach (Wall wall in room.Walls)
            {
                var pBox = HitBox;
                var wBox = wall.HitBox;
                var overlap = Rectangle.Intersect(pBox, wBox);
                if (overlap.Width == 0 && overlap.Height == 0)
                {
                    continue;
                }
                else if (overlap.Width > overlap.Height) // move vertically
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

        public void RegainControl()
        {
            this.PlayerControl = true;
            this.Submission = MAX_SUBMISSION;
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
    }
}
