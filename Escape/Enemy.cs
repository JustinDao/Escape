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

    class Enemy : SpriteSheet
    {

        public bool Frozen = false;

        // Position of the Enemy
        public Vector2 Position { get; set; }
        // The Width and Height of the Enemy
        public int EnemyWidth { get; set; }
        public int EnemyHeight { get; set; }

        // The Speed the enemy moves at
        private int speed;
        // X and Y acceleration of the Enemy
        private int xAccel;
        private int yAccel;
        // Friction determines how fast the enemy moves/stops moving
        private double friction;

        // Variable for Sprite Rendering
        private int spriteTime = 0;
        private int spriteInterval = 50;
        private int spriteRightStart = 19;
        private int spriteRightHeight = 207;
        private int spriteLeftStart = 533;
        private int spriteLeftHeight = 78;
        private int spriteSpace = 64;

        // Variables for AI Movement
        // false = right
        // true = left
        private bool XDirection = false;
        private bool YDirection = false;

        // Variables to Hold Speed and final movement amount
        public double XVelocity;
        public double YVelocity;
        public int MovedX;
        public int MovedY;
        private int counter = 0;

        // HitBox for the Enemy
        public Rectangle HitBox;

        // Reference to the MainGame
        public MainGame Game;

        // Bool is hold if the enemy is in control
        public bool EnemyControl = true;

        // The Direction the Enemy is currently moving in or looking towards
        public Direction Dir { get; set; }

        public Enemy(MainGame game, int x, int y)
        {
            LoadContent(game.Content); // TODO eeuadhsahdfkjsahgdf :(
            this.Game = game;

            this.Position = new Vector2(200, 200);

            this.spriteWidth = 30;
            this.spriteHeight = 48;

            this.EnemyWidth = spriteWidth;
            this.EnemyHeight = spriteHeight;

            this.spriteX = spriteRightStart;
            this.spriteY = spriteRightHeight;

            HitBox = new Rectangle((int)Position.X, (int)Position.Y, EnemyWidth, EnemyHeight);

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
            var color = Frozen ? Color.Cyan : Color.White;
            sb.Draw(spriteSheet,
                    new Rectangle((int)Position.X, (int)Position.Y, EnemyWidth, EnemyHeight),
                    new Rectangle(spriteX, spriteY, spriteWidth, spriteHeight),
                    color);
        }

        public void Update(GameTime gameTime, Room currentRoom)
        {
            Move(currentRoom, 50);
            UpdateSprite(gameTime);
            UpdateHitBox();
        }

        public void UpdateSprite(GameTime gameTime)
        {
            if (Frozen) return;
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

        public void Move(Room currentRoom, int distance)
        {
            if (Frozen) return;
            if (XDirection && counter != distance)
            {
                xAccel = -speed;
                counter++;
            }
            else if (!XDirection && counter != distance)
            {
                xAccel = speed;
                counter++;
            }
            else
            {
                XDirection = !XDirection;
                counter = 0;
            }

            XVelocity = XVelocity * (1 - friction) + xAccel * .10;
            MovedX = Convert.ToInt32(XVelocity);

            YVelocity = YVelocity * (1 - friction) + yAccel * .10;
            MovedY = Convert.ToInt32(YVelocity);

            if (!CheckCollision(currentRoom))
            {
                UpdateDirection();
                var movement = new Vector2(MovedX, MovedY);
                //if (Frozen)
                //{
                //    movement *= 0.0f;
                //}
                Position += movement;
            }

            if (CheckGround(currentRoom))
            {
                Position = new Vector2(200, 200);
            }

            CheckBoundaries();
            UpdateHitBox();
        }

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

            if (this.Position.Y < -25)
            {
                XDirection = false;
                Position = new Vector2(Position.X, 0);
            }

            else if (this.Position.Y > Game.GAME_HEIGHT - this.EnemyHeight)
            {
                XDirection = true;
                Position = new Vector2(Position.X, Game.GAME_HEIGHT - this.EnemyHeight);
            }
        }

        private bool CheckCollision(Room currentRoom)
        {
            Vector2 tempPos = new Vector2(this.Position.X, this.Position.Y);
            tempPos += new Vector2(MovedX, 0);
            tempPos += new Vector2(0, MovedY);
            //      (int) Position.X, (int) Position.Y + (this.EnemyHeight / 4), this.EnemyWidth, 3*(this.EnemyHeight / 4)
            Rectangle tempBox = new Rectangle((int)tempPos.X, (int)tempPos.Y + (this.EnemyHeight / 2), this.EnemyWidth, this.EnemyHeight / 2);

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
            int tempX = (int)this.Position.X + (this.EnemyWidth / 4);
            int tempY = (int)this.Position.Y + 3 * (this.EnemyHeight / 4);
            int tempW = this.EnemyWidth / 2;
            int tempH = this.EnemyHeight / 4;
            Rectangle tempBox = new Rectangle(tempX, tempY, tempW, tempH);

            foreach (Entity o in currentRoom.Obstacles)
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

        private void UpdateHitBox()
        {
            HitBox.X = (int)Position.X;
            HitBox.Y = (int)Position.Y;
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
    }
}
