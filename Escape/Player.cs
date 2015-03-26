﻿using System;
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
        public Vector2 Position { get; set; }
        public int PlayerWidth { get; set; }
        public int PlayerHeight { get; set; }

        private bool grounded;
        private int speed;
        private int xAccel;
        private int yAccel;
        private double friction;
        private int jumpPoint = 0;
        private bool pushing;

        private int SubmissionInterval = 1000; // Time in milliseconds
        private int SubmissionTime = 0;

        private int spriteTime = 0;
        private int spriteInterval = 50;
        private int spriteRightStart = 19;
        private int spriteRightHeight = 207;
        private int spriteLeftStart = 533;
        private int spriteLeftHeight = 78;
        private int spriteSpace = 64;

        // false = right
        // true = left
        private bool XDirection = false;
        private bool YDirection = false;

        public double XVelocity;
        public double YVelocity;
        public int MovedX;
        public int MovedY;

        public int JumpSpeed = 12;
        public double Gravity = .5;
        public int MaxFallSpeed = 10;
        public Rectangle HitBox;
        public MainGame Game;
        public int Submission;
        public bool PlayerControl = true;

        public Direction Dir { get; set; }

        public Player(MainGame game, int x, int y)
        {
            this.Game = game;
            this.Submission = 10;

            this.Position = new Vector2(50, 50);

            this.spriteWidth = 30;
            this.spriteHeight = 48;

            this.PlayerWidth = spriteWidth;
            this.PlayerHeight = spriteHeight;

            this.spriteX = spriteRightStart;
            this.spriteY = spriteRightHeight;

            HitBox = new Rectangle((int) Position.X, (int) Position.Y, PlayerWidth, PlayerHeight);

            grounded = false;
            pushing = false;

            // Movement
            speed = 7;
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
                    new Rectangle((int) Position.X, (int) Position.Y, PlayerWidth, PlayerHeight),
                    new Rectangle(spriteX, spriteY, spriteWidth, spriteHeight),
                    Color.White);
        }

        public void Update(Controls controls, GameTime gameTime, Room currentRoom)
        {
            UpdateSubmission(gameTime);
            Move(controls, currentRoom);
            Action(controls, gameTime, currentRoom);
            UpdateSprite(gameTime);
            UpdateHitBox();
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

                if (!PlayerControl && Submission > 100)
                {
                    Submission = 100;
                    PlayerControl = true;
                    xAccel = 0;
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
                    if (Dir == Direction.W || Dir == Direction.SW || Dir == Direction.NW)
                    {
                    spriteY = spriteLeftHeight;
                    if (spriteX > spriteLeftStart - spriteSpace * 7)
                    {
                        spriteX -= spriteSpace;
                    }
                    else
                    {
                        spriteX = spriteLeftStart;
                    }
                }
                    else if (Dir == Direction.E || Dir == Direction.SE || Dir == Direction.NE)
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

        public void Move(Controls controls, Room currentRoom)
        {
            if (PlayerControl)
            {
                // Sideways Acceleration
                if (controls.onPress(Keys.Right, Buttons.DPadRight))
                {
                    xAccel += speed;
                }
                else if (controls.onRelease(Keys.Right, Buttons.DPadRight))
                {
                    xAccel -= speed;
                }

                if (controls.onPress(Keys.Left, Buttons.DPadLeft))
                {
                    xAccel -= speed;
                }
                else if (controls.onRelease(Keys.Left, Buttons.DPadLeft))
                {
                    xAccel += speed;
                }

                // Y movement
                if (controls.onPress(Keys.Down, Buttons.DPadDown))
                {
                    yAccel += speed;
                }
                else if (controls.onRelease(Keys.Down, Buttons.DPadDown))
                {
                    yAccel -= speed;
                }

                if (controls.onPress(Keys.Up, Buttons.DPadUp))
                {
                    yAccel -= speed;
                }
                else if (controls.onRelease(Keys.Up, Buttons.DPadUp))
                {
                    yAccel += speed;
                }
            }
            else
            {
                AIMove();
            }

            double playerFriction = pushing ? (friction * 3) : friction;

            XVelocity = XVelocity * (1 - playerFriction) + xAccel * .10;
            MovedX = Convert.ToInt32(XVelocity);

            YVelocity = YVelocity * (1 - playerFriction) + yAccel * .10;
            MovedY = Convert.ToInt32(YVelocity);

            if (!CheckCollision(currentRoom))
            {
                UpdateDirection();
                Position += new Vector2(MovedX, 0);
                Position += new Vector2(0, MovedY);
            }

			if (CheckGround(currentRoom)) 
			{
				Position = new Vector2(200,200);
			}

            CheckBoundaries();
            UpdateHitBox();
        }

        private void AIMove()
        {
            Random rand = new Random();
            int num = rand.Next(75);
            int num2 = rand.Next(75);
            int num3 = rand.Next(75);
            int num4 = rand.Next(75);

            if (num == num2)
            {
                XDirection = !XDirection;
            }

            if (num3 == num4)
            {
                YDirection = !YDirection;
            }

            if (XDirection)
            {
                xAccel = -speed * 2;
            }
            else
            {
                xAccel = speed * 2;
                yAccel = speed * 2;
            }

            if (YDirection)
            {
                yAccel = -speed * 2;
            }
            else
            {
                yAccel = speed * 2;
            }

            num = rand.Next(75);
            num2 = rand.Next(75);
        }

        private void CheckBoundaries()
        {
            if (this.Position.X < 0)
            {
                XDirection = false;
                Position = new Vector2(0, Position.Y);
            }

            else if (this.Position.X > Game.GAME_WIDTH - this.PlayerWidth)
            {
                XDirection = true;
                Position = new Vector2(Game.GAME_WIDTH - this.PlayerWidth, Position.Y);
            }

            if (this.Position.Y < -25)
            {
                XDirection = false;
                Position = new Vector2(Position.X, 0);
            }

            else if (this.Position.Y > Game.GAME_HEIGHT - this.PlayerHeight)
            {
                XDirection = true;
                Position = new Vector2(Position.X, Game.GAME_HEIGHT - this.PlayerHeight);
            }
        }

        private bool CheckCollision(Room currentRoom)
        {
            Vector2 tempPos = new Vector2(this.Position.X, this.Position.Y);
            tempPos += new Vector2(MovedX, 0);
            tempPos += new Vector2(0, MovedY);
//			(int) Position.X, (int) Position.Y + (this.PlayerHeight / 4), this.PlayerWidth, 3*(this.PlayerHeight / 4)
			Rectangle tempBox = new Rectangle((int)tempPos.X, (int)tempPos.Y + (this.PlayerHeight / 2), this.PlayerWidth, this.PlayerHeight / 2);

            foreach (Wall w in currentRoom.Walls)
            {
                if (w.HitBox.Intersects(tempBox))
                {
                    return true;
                }
            }

            return false;
        }

		private bool CheckGround(Room currentRoom)
		{
			int tempX = (int)this.Position.X + (this.PlayerWidth / 4);
			int tempY = (int)this.Position.Y + 3*(this.PlayerHeight / 4);
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
                        return true;
                    }
                }
				
			}

			return false;
		}

        private void Action(Controls controls, GameTime gameTime, Room currentRoom)
        {
            // Jump on button press
            if (controls.onPress(Keys.Space, Buttons.A))
            {
                shootFireBall(currentRoom);
            }

            // Cut jump short on button release
            else if (controls.onRelease(Keys.Space, Buttons.A) && YVelocity < 0)
            {
                YVelocity /= 2;
            }
            else if (controls.onPress(Keys.X, Buttons.X))
            {
                if (!PlayerControl)
                {
                    Submission += 10;
                }
            }
        }

        private void UpdateHitBox()
        {
            HitBox.X = (int) Position.X;
            HitBox.Y = (int) Position.Y;
        }

        private void UpdateDirection()
        {
            if (MovedX > 0)
            {
                if (MovedY > 0)
                {
                    Dir = Direction.SE;
                }
                else if (MovedY < 0)
                {
                    Dir = Direction.NE;
                }
                else
                {
                    Dir = Direction.E;
                }
            }
            else if (MovedX < 0)
            {
                if (MovedY > 0)
                {
                    Dir = Direction.SW;
                }
                else if (MovedY < 0)
                {
                    Dir = Direction.NW;
                }
                else
                {
                    Dir = Direction.W;
                }
            }
            else
            {
                if (MovedY < 0)
                {
                    Dir = Direction.N;
                }
                else if (MovedY > 0)
                {
                    Dir = Direction.S;
                }
            }
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
    }
}
