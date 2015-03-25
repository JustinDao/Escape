using System;
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
        private bool movingLeft;
        private bool movingRight;

        private bool grounded;
        private int speed;
        private int xAccel;
        private double friction;
        private int jumpPoint = 0;
        private bool pushing;

        private int SubmissionInterval = 100; // Time in milliseconds
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
        private bool direction = false;

        public int PlayerWidth;
        public int PlayerHeight;

        public int XPosition;
        public int YPosition;
        public double XVelocity;
        public double YVelocity;
        public int MovedX;

        public int JumpSpeed = 12;
        public double Gravity = .5;
        public int MaxFallSpeed = 10;
        public Rectangle HitBox;
        public MainGame Game;
        public int Submission;
        public bool PlayerControl = true;

        public Player(MainGame game, int x, int y)
        {
            this.Game = game;
            this.Submission = 100;

            this.XPosition = x;
            this.YPosition = y;

            this.spriteWidth = 30;
            this.spriteHeight = 48;

            this.PlayerWidth = spriteWidth;
            this.PlayerHeight = spriteHeight;

            this.spriteX = spriteRightStart;
            this.spriteY = spriteRightHeight;

            HitBox = new Rectangle(XPosition, YPosition, PlayerWidth, PlayerHeight);

            grounded = false;
            pushing = false;

            movingLeft = false;
            movingRight = false;

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
                    new Rectangle(XPosition, YPosition, PlayerWidth, PlayerHeight),
                    new Rectangle(spriteX, spriteY, spriteWidth, spriteHeight),
                    Color.White);
        }

        public void Update(Controls controls, GameTime gameTime, List<Wall> walls)
        {
            UpdateSubmission(gameTime);
            Move(controls, walls);
            Action(controls, gameTime);
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
                    movingLeft = false;
                    movingRight = false;
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
                if (movingLeft)
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
                else if (movingRight)
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
                spriteTime = 0;
            }
            else
            {
                spriteTime += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
            }
        }

        public void Move(Controls controls, List<Wall> walls)
        {
            if (PlayerControl)
            {
                // Sideways Acceleration
                if (controls.onPress(Keys.Right, Buttons.DPadRight))
                {
                    xAccel += speed;
                    movingRight = true;
                }
                else if (controls.onRelease(Keys.Right, Buttons.DPadRight))
                {
                    xAccel -= speed;
                    movingRight = false;
                }

                if (controls.onPress(Keys.Left, Buttons.DPadLeft))
                {
                    xAccel -= speed;
                    movingLeft = true;
                }
                else if (controls.onRelease(Keys.Left, Buttons.DPadLeft))
                {
                    xAccel += speed;
                    movingLeft = false;
                }
            }
            else
            {
                AIMove();
            }

            double playerFriction = pushing ? (friction * 3) : friction;
            XVelocity = XVelocity * (1 - playerFriction) + xAccel * .10;
            MovedX = Convert.ToInt32(XVelocity);
            XPosition += MovedX;

            // Check Walls
            HitBox.X = XPosition;
            Wall wall = this.IntersectingWall(walls);

            if (wall != null)
            {
                if (wall.XPosition < this.XPosition)
                {
                    XPosition = wall.XPosition + wall.Width;
                }
                else if (wall.XPosition >= this.XPosition)
                {
                    XPosition = wall.XPosition - this.PlayerWidth;
                }

            }

            // Check Side Walls
            int sidePosition = checkXCollisions();
            if (sidePosition >= 0)
            {
                XPosition = sidePosition;
            }

            // Gravity
            if (!grounded)
            {
                YVelocity += Gravity;
                if (YVelocity > MaxFallSpeed)
                {
                    YVelocity = MaxFallSpeed;
                }
            }
            else
            {
                YVelocity += Gravity;
                if (YVelocity > MaxFallSpeed)
                {
                    YVelocity = MaxFallSpeed;
                }
            }

            YPosition += Convert.ToInt32(YVelocity);

            HitBox.Y = YPosition;

            Wall Ywall = this.IntersectingWall(walls);

            if (Ywall != null)
            {
                YPosition = Ywall.YPosition - this.PlayerHeight;
                grounded = true;
            }

            // Check up/down collisions, then left/right
            checkYCollisions(walls);
            checkXCollisions();
        }

        private void AIMove()
        {
            Random rand = new Random();
            int num = rand.Next(75);
            int num2 = rand.Next(75);

            if (num == num2)
            {
                direction = !direction;
            }

            if (direction)
            {
                xAccel = -speed * 2;
                movingLeft = true;
                movingRight = false;
            }
            else
            {
                xAccel = speed * 2;
                movingLeft = false;
                movingRight = true;
            }

            num = rand.Next(75);
            num2 = rand.Next(75);


            if (num == num2 && grounded)
            {
                // Jump
                YVelocity = -JumpSpeed;
                grounded = false;
            }


        }

        private void checkYCollisions(List<Wall> walls)
        {
            foreach (Wall wall in walls)
            {
                if (this.IntersectsWall(wall))
                {
                    grounded = true;
                    break;
                }
            }
        }

        private int checkXCollisions()
        {
            if (this.XPosition < 0)
            {
                direction = false;
                return 0;
            }

            if (this.XPosition > Game.GAME_WIDTH - this.PlayerWidth)
            {
                direction = true;
                return Game.GAME_WIDTH - this.PlayerWidth;
            }

            return -1;
        }

        private Boolean IntersectsWall(Wall wall)
        {
            return this.HitBox.Intersects(wall.HitBox);
        }

        private Wall IntersectingWall(List<Wall> walls)
        {
            foreach (Wall wall in walls)
            {
                if (this.IntersectsWall(wall))
                {
                    return wall;
                }
            }

            return null;
        }

        private void Action(Controls controls, GameTime gameTime)
        {
            // Jump on button press
            if (controls.onPress(Keys.Space, Buttons.A))
            {
                if (grounded && PlayerControl)
                {
                    YVelocity = -JumpSpeed;
                    jumpPoint = (int)(gameTime.TotalGameTime.TotalMilliseconds);
                    grounded = false;
                }
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
            HitBox.X = XPosition;
            HitBox.Y = YPosition;
        }
    }
}
