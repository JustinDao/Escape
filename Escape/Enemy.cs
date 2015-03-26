using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;


namespace Escape
{
    class Enemy : SpriteSheet
    {
        public Vector2 Position { get; set; }
        public int EnemyWidth { get; set; }
        public int EnemyHeight { get; set; }

        private bool movingLeft;
        private bool movingRight;

        private bool grounded;
        private int speed;
        private int xAccel;
        private int yAccel;
        private double friction;

        private int spriteTime = 0;
        private int spriteInterval = 50;
        private int spriteRightStart = 19;
        private int spriteRightHeight = 207;
        private int spriteLeftStart = 533;
        private int spriteLeftHeight = 78;
        private int spriteSpace = 64;
        private int counter = 0;

        // false = right
        // true = left
        private bool XDirection = false;
        private bool YDirection = false;

        public double XVelocity;
        public double YVelocity;
        public int MovedX;
        public int MovedY;

        public double Gravity = .5;
        public int MaxFallSpeed = 10;
        public Rectangle HitBox;
        public MainGame Game;

        public Enemy(MainGame game, int x, int y)
        {
            this.Game = game;

            this.Position = new Vector2(x, y);

            this.spriteWidth = 30;
            this.spriteHeight = 48;

            this.EnemyWidth = spriteWidth;
            this.EnemyHeight = spriteHeight;

            this.spriteX = spriteRightStart;
            this.spriteY = spriteRightHeight;

            HitBox = new Rectangle((int) Position.X, (int) Position.Y, EnemyWidth, EnemyHeight);

            grounded = false;

            movingLeft = false;
            movingRight = false;

            // Movement
            speed = 7;
            friction = .15;
            xAccel = 0;
            XVelocity = 1;
            YVelocity = 1;
            MovedX = 0;
        }

        public void LoadContent(ContentManager content)
        {
            spriteSheet = content.Load<Texture2D>("soldier.png");
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(spriteSheet,
                    new Rectangle((int) Position.X, (int) Position.Y, EnemyWidth, EnemyHeight),
                    new Rectangle(spriteX, spriteY, spriteWidth, spriteHeight),
                    Color.White);
        }

        public void Update(GameTime gameTime, Room currentRoom)
        {
            Move(currentRoom, );
            UpdateSprite(gameTime);
            UpdateHitBox();
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

        public void Move(Room currentRoom, int distance)
        {
 
            //YVelocity = YVelocity + yAccel * .10;
            //MovedY = Convert.ToInt32(YVelocity);

            if (XDirection && counter != distance)
            {
                xAccel = -speed;
                movingLeft = true;
                movingRight = false;
                counter++;
            }
            else if (!XDirection && counter != distance)
            {
                xAccel = speed;
                movingLeft = false;
                movingRight = true;
                counter++;
            }
            else
            {
                XDirection = !XDirection;
                counter = 0;
            }

            XVelocity = XVelocity + xAccel * .10;
            MovedX = Convert.ToInt32(XVelocity);

            if (!CheckCollision(currentRoom))
            {
                Position += new Vector2(MovedX, 0);
                Position += new Vector2(0, MovedY);
               
            }


            CheckBoundaries();
            UpdateHitBox();
        }

       /* private void AIMove(int num)
        {

            if (num == num2)
            {
                XDirection = !XDirection;
            }

            if (XDirection)
            {
                xAccel = -speed;
                movingLeft = true;
                movingRight = false;
            }
            else
            {
                xAccel = speed;
                yAccel = speed;
                movingLeft = false;
                movingRight = true;
            }

            num = rand.Next(75);
            num2 = rand.Next(75);
        }*/

        private void CheckBoundaries()
        {
            if (this.Position.X < 0)
            {
                XDirection = false;
                Position = new Vector2(0, Position.Y);
            }

            else if (this.Position.X > Game.GAME_WIDTH - this.EnemyWidth)
            {
                XDirection = true;
                Position = new Vector2(Game.GAME_WIDTH - this.EnemyWidth, Position.Y);
            }

        }

        private bool CheckCollision(Room currentRoom)
        {
            Vector2 tempPos = new Vector2(this.Position.X, this.Position.Y);
            tempPos += new Vector2(MovedX, 0);
            tempPos += new Vector2(0, MovedY);

            Rectangle tempBox = new Rectangle((int)tempPos.X, (int)tempPos.Y, this.EnemyWidth, this.EnemyHeight);

            foreach (Wall w in currentRoom.Walls)
            {
                if (w.HitBox.Intersects(tempBox))
                {
                    return true;
                }
            }

            return false;
        }


        private void UpdateHitBox()
        {
            HitBox.X = (int) Position.X;
            HitBox.Y = (int) Position.Y;
        }
    }
}
