using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;

namespace Escape
{
    // http://lpc.opengameart.org/static/lpc-style-guide/assets.html

    class Player : SpriteSheet
    {
        // Reference to Castle
        public Castle Castle { get; set; }

        // Powerup information
        public bool HasFire = false;
        public bool HasIce = false;

        // Position of the Player
        public Vector2 Position { get; set; }
        // The Width and Height of the Player
        public int PlayerWidth { get; set; }
        public int PlayerHeight { get; set; }

        // The Speed the player moves at
        private float speed;
        // X and Y acceleration of the Player
        private float xAccel;
        private float yAccel;
        // Friction determines how fast the player moves/stops moving
        private double friction;

        // Variables to hold Submission Time and Interval
        private int SubmissionInterval = 100; // Time in milliseconds
        private int SubmissionTime = 0;

        // Variable for Sprite Rendering
        private int spriteTime = 0;
        private int spriteInterval = 50;
        private int spriteRightStart = 19;
        private int spriteRightHeight = 207;
        private int spriteLeftStart = 21;
        private int spriteLeftHeight = 78;
        private int spriteSpace = 64;

        // Variables for AI Movement
        private Vector2 AIDirection;
        private int AITime;
        private int AIInterval;
        private int AI_SWITCH_TIME = 3000;
        private Random rand = new Random();

        // Variables to Hold Speed and final movement amount
        public double XVelocity;
        public double YVelocity;
        public int MovedX;
        public int MovedY;

        // HitBox for the Player
        public Rectangle HitBox
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, PlayerWidth, PlayerHeight);
            }
        }

        // Reference to the MainGame
        public MainGame Game;

        // Value of the Player's current Submission
        public int Submission;
        // The maximum submission value
        private int MAX_SUBMISSION = 100;

        // Bool is hold if the player is in control
        public bool PlayerControl = true;

        // The Direction the Player is currently moving in or looking towards
        public Vector2 Dir
        {
            get
            {
                float length = (float)Math.Sqrt(MovedX * MovedX + MovedY * MovedY);
                return new Vector2(MovedX / length, MovedY / length);
            }
        }

        // Bool to hold if player is standing on an obstacle?
        public bool StandingOnDoor = false;

        public Player(MainGame game, Castle castle, int x, int y)
        {
            this.Game = game;
            this.Castle = castle;
            this.Submission = MAX_SUBMISSION;

            this.Position = new Vector2(50, 50);

            this.spriteWidth = 30;
            this.spriteHeight = 48;

            this.PlayerWidth = spriteWidth;
            this.PlayerHeight = spriteHeight;

            this.spriteX = spriteRightStart;
            this.spriteY = spriteRightHeight;

            this.AITime = 0;
            this.AIInterval = rand.Next(AI_SWITCH_TIME);

            // Movement
            speed = 10;
            friction = .15;
            xAccel = 0;
            XVelocity = 0;
            YVelocity = 0;
            MovedX = 0;
        }

        public void LoadContent(ContentManager content)
        {
            spriteSheet = content.Load<Texture2D>("soldier.png");
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(spriteSheet,
                    new Rectangle((int)Position.X, (int)Position.Y, PlayerWidth, PlayerHeight),
                    new Rectangle(spriteX, spriteY, spriteWidth, spriteHeight),
                    Color.White);
        }

        public void Update(Controls controls, GameTime gameTime, Room currentRoom)
        {
            UpdateSubmission(gameTime);
            Move(controls, currentRoom, gameTime);
            Action(controls, gameTime, currentRoom);
            UpdateSprite(gameTime);
        }

        public void UpdateSubmission(GameTime gameTime)
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
                    SubmissionTime += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
                }

                // Randomly lose control of character based on submission
                //Random rand = new Random();
                //int num = rand.Next(Submission*2);
                //int num2 = rand.Next(Submission*2);
                //if (num == num2)
                //{
                //    PlayerControl = false;
                //}
                //else
                //{
                //    if (!PlayerControl)
                //    {
                //        PlayerControl = true;
                //        xAccel = 0;
                //        movingLeft = false;
                //        movingRight = false;
                //    }
                //}

            }
            else if (Submission <= 0)
            {
                PlayerControl = false;
            }


        }

        public void UpdateSprite(GameTime gameTime)
        {
            if (spriteTime > spriteInterval)
            {
                if (IsMoving())
                {
                    if (Dir.X < 0)
                    {
                        spriteY = spriteLeftHeight;
                        if (spriteX < spriteLeftStart + spriteSpace * 7)
                        {
                            spriteX += spriteSpace;
                        }
                        else
                        {
                            spriteX = spriteLeftStart;
                        }
                    }
                    else if (Dir.X > 0)
                    {
                        spriteY = spriteRightHeight;
                        if (spriteX < spriteRightStart + spriteSpace * 8)
                        {
                            spriteX += spriteSpace;
                        }
                        else
                        {
                            spriteX = spriteRightStart;
                        }
                    }
                }

                spriteTime = 0;
            }
            else
            {
                spriteTime += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
            }
        }

        public void Move(Controls controls, Room currentRoom, GameTime gt)
        {
            if (PlayerControl)
            {
                xAccel = speed * controls.gp.ThumbSticks.Left.X;
                yAccel = -speed * controls.gp.ThumbSticks.Left.Y;
            }
            else
            {
                AIMove(gt);
            }

            XVelocity = XVelocity * (1 - friction) + xAccel * .10;
            MovedX = Convert.ToInt32(XVelocity);

            YVelocity = YVelocity * (1 - friction) + yAccel * .10;
            MovedY = Convert.ToInt32(YVelocity);

            if (!CheckCollision(currentRoom))
            {
                Position += new Vector2(MovedX, 0);
                Position += new Vector2(0, MovedY);
            }

            Obstacle chkG = CheckGround(currentRoom);
            if (chkG is Hole)
            {
                Position = new Vector2(200, 200);
                ChangeAIDirection();
            }

            Door door = CheckDoors(currentRoom);

            if (door != null)
            {
                if(!StandingOnDoor)
                {
                    if (door.Equals(currentRoom.LeftDoor()))
                    {
                        Castle.MoveLeft();
                        this.FlipPosition(currentRoom);
                    }
                    else if (door.Equals(currentRoom.RightDoor()))
                    {
                        Castle.MoveRight();
                        this.FlipPosition(currentRoom);
                    }
                    else if (door.Equals(currentRoom.UpDoor()))
                    {
                        Castle.MoveUp();
                        this.FlipPosition(currentRoom);
                    }
                    else if (door.Equals(currentRoom.DownDoor()))
                    {
                        Castle.MoveDown();
                        this.FlipPosition(currentRoom);
                    }
                }

                StandingOnDoor = true;                
            }
            else
            {
                StandingOnDoor = false;
            }

            CheckBoundaries();
            CheckPowerUps(currentRoom);
        }

        public void RegainControl()
        {
            this.PlayerControl = true;
            this.Submission = MAX_SUBMISSION;
            this.xAccel = 0;
            this.yAccel = 0;
        }

        private void AIMove(GameTime gt)
        {
            this.AITime += (int)gt.ElapsedGameTime.TotalMilliseconds;

            if (this.AITime > this.AIInterval)
            {
                AIDirection = GetRandomDirection();
                AITime = 0;
                AIInterval = rand.Next(AI_SWITCH_TIME);
            }

            xAccel = speed * 2 * AIDirection.X;
            yAccel = speed * 2 * AIDirection.Y;

        }

        private void CheckBoundaries()
        {
            if (this.Position.X < 0)
            {
                Position = new Vector2(0, Position.Y);
            }

            else if (this.Position.X > Game.GAME_WIDTH - this.PlayerWidth)
            {
                Position = new Vector2(Game.GAME_WIDTH - this.PlayerWidth, Position.Y);
            }

            if (this.Position.Y < -25)
            {
                Position = new Vector2(Position.X, 0);
            }

            else if (this.Position.Y > Game.GAME_HEIGHT - this.PlayerHeight)
            {
                Position = new Vector2(Position.X, Game.GAME_HEIGHT - this.PlayerHeight);
            }
        }

        private bool CheckCollision(Room currentRoom)
        {
            Vector2 tempPos = new Vector2(this.Position.X, this.Position.Y);
            tempPos += new Vector2(MovedX, 0);
            tempPos += new Vector2(0, MovedY);
            Rectangle tempBox = new Rectangle((int)tempPos.X, (int)tempPos.Y + (this.PlayerHeight / 2), this.PlayerWidth, this.PlayerHeight / 2);

            foreach (Wall w in currentRoom.Walls)
            {
                if (w.HitBox.Intersects(tempBox))
                {
                    ChangeAIDirection();
                    return true;
                }
            }

            return false;
        }

        private Obstacle CheckGround(Room currentRoom)
        {
            int tempX = (int)this.Position.X + (this.PlayerWidth / 4);
            int tempY = (int)this.Position.Y + 3 * (this.PlayerHeight / 4);
            int tempW = this.PlayerWidth / 2;
            int tempH = this.PlayerHeight / 4;
            Rectangle tempBox = new Rectangle(tempX, tempY, tempW, tempH);

            foreach (Obstacle o in currentRoom.Obstacles)
            {
                if (o is Hole)
                {
                    Hole h = (Hole)o;

                    if (h.HitBox.Intersects(tempBox))
                    {
                        return h;
                    }
                }
            }

            return null;
        }

        private Door CheckDoors(Room currentRoom)
        {
            int tempX = (int)this.Position.X + (this.PlayerWidth / 4);
            int tempY = (int)this.Position.Y + 3 * (this.PlayerHeight / 4);
            int tempW = this.PlayerWidth / 2;
            int tempH = this.PlayerHeight / 4;
            Rectangle tempBox = new Rectangle(tempX, tempY, tempW, tempH);

            foreach(Door d in currentRoom.Doors.Values)
            {
                if (d == null)
                {
                    return null;
                }

                if (d.HitBox.Intersects(tempBox))
                {
                    return d;
                }  
            }             

            return null;
        }

        private void FlipPosition(Room currentRoom)
        {
            int x = (int)this.Position.X;
            int y = (int)this.Position.Y;

            int wid = currentRoom.Width;
            int hei = currentRoom.Height;

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

        private void Action(Controls controls, GameTime gameTime, Room currentRoom)
        {
            if (!PlayerControl) return;

            if (controls.onPress(Keys.Space, Buttons.A) && HasFire)
            {
                shootFireBall(currentRoom);
            }

            if (controls.onPress(Keys.None, Buttons.X) && HasIce)
            {
                letItGo(currentRoom);
            }

        }

        private void letItGo(Room room)
        {
            room.AddSnowflakes(Position);
        }

        private bool IsMoving()
        {
            if (xAccel == 0 && yAccel == 0)
            {
                return false;
            }

            return true;
        }

        private void shootFireBall(Room currentRoom)
        {
            currentRoom.AddFireBall(Position, Dir);
        }

        private Vector2 GetRandomDirection()
        {
            return new Vector2((float)rand.NextDouble() * 2 - 1, (float)rand.NextDouble() * 2 - 1);
        }

        private void ChangeAIDirection()
        {
            // Flip Direction of AI if intersecting with wall
            if (!PlayerControl)
            {
                AIDirection = GetRandomDirection();
            }
        }

        private void CheckPowerUps(Room currentRoom)
        {
            List<Obstacle> toRemove = new List<Obstacle>();

            foreach (Obstacle o in currentRoom.Obstacles)
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

            currentRoom.Obstacles = currentRoom.Obstacles.Except(toRemove).ToList();
        }

    }
}
