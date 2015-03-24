﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Escape
{
    class Castle : Screen
    {
        public Vector2 CastlePosition { get; set; }
        public int CastleHeight { get; set; }
        public int CastleWidth { get; set; }
        public List<Room> Rooms { get; set; }
        public Player Player { get; set; }

        public Castle(MainGame mg)
        {
            Player = new Player(mg, 50, 50);
        }

        override public void Initialize(ContentManager cm) { }

        public void Update(Controls controls, GameTime gameTime, List<Wall> walls) 
        {
            Player.Update(controls, gameTime, walls);
        }

        override public void Draw(SpriteBatch sb) 
        {
            Player.Draw(sb);
        }

        public override void LoadContent(ContentManager cm)
        {
            Player.LoadContent(cm);
        }

        public override void Update(GameTime gt)
        {
            throw new NotImplementedException();
        }

    }
}
