﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TexturePackerLoader;

namespace Escape
{
    class IceBoss : EnemyStationary
    {
        public override int MaxHealth
        {
            get
            {
                return 10;
            }
        }
        public override string[] DownSprites
        {
            get
            {
                return new string[] {

                    TexturePackerMonoGameDefinitions.IceBossSprites.Layer_1,
                    TexturePackerMonoGameDefinitions.IceBossSprites.Layer_2,
                    TexturePackerMonoGameDefinitions.IceBossSprites.Layer_3,
                    TexturePackerMonoGameDefinitions.IceBossSprites.Layer_4,
                    TexturePackerMonoGameDefinitions.IceBossSprites.Layer_5,
                    TexturePackerMonoGameDefinitions.IceBossSprites.Layer_6,
                    TexturePackerMonoGameDefinitions.IceBossSprites.Layer_5,
                };
            }
        }
        public override string[] UpSprites
        {
            get
            {
                return DownSprites;
            }
        }
        public override string[] LeftSprites
        {
            get
            {
                return DownSprites;
            }
        }
        public override string[] RightSprites
        {
            get
            {
                return DownSprites;
            }
        }

        public override int SpriteInterval
        {
            get { return 100; }
        }

        public override float MaxSpeed
        {
            get { return 200; }
        }
        public override float AnimationSpeed
        {
            get
            {
                return base.AnimationSpeed * (float)Health / (float)MaxHealth;
            }
        }

        public override int Damage
        {
            get
            {
                return base.Damage * 3;
            }
        }

        public override float TouchFreezeTimer
        {
            get
            {
                return 1f;
            }
        }

        private float minionTimer = 0f;
        const float MINION_INTERVAL = 2f;
        const uint MAX_MINIONS = 3;
        const float MINION_DIST = 50;

        public IceBoss(ContentManager cm, SpriteRender sr, Room room, Vector2 location)
            : base(cm, sr, "ice_boss_sprite_sheet.png", location)
        {
            ignoreHoles = true;
            ignoreWater = true;
            Scale = 2f;
            Drop = new PowerUp(cm, Vector2.Zero, "naryu.png", isIce: true);

            var castle = room.Castle;
            var player = castle.Player;
            var dir = player.Center - Center;
            dir.Normalize();

            // Three initial IceMinions
            var e1 = new IceMinion(castle.mg.Content, castle.mg.SpriteRender, player, Center + dir);
            e1.Position = new Vector2(400, 300);
            e1.Parent = this;

            var e2 = new IceMinion(castle.mg.Content, castle.mg.SpriteRender, player, Center + dir);
            e2.Position = new Vector2(600, 300);
            e2.Parent = this;

            var e3 = new IceMinion(castle.mg.Content, castle.mg.SpriteRender, player, Center + dir);
            e3.Position = new Vector2(500, 400);
            e3.Parent = this;

            room.Enemies.Add(e1);
            room.Enemies.Add(e2);
            room.Enemies.Add(e3);
        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);
            if (Health <= 0) return;
        }

        public override void Update(GameTime gt, Screen s)
        {

            var delta = (float)gt.ElapsedGameTime.TotalSeconds;
            var castle = s as Castle;
            if (castle == null)
            {
                return;
            }

            var room = castle.CurrentRoom;
            var player = castle.Player;
            var nMinions = room.Enemies.Count() - 1;
            if (nMinions >= MAX_MINIONS)
            {
                minionTimer = 0;
            }
            else
            {
                minionTimer += delta;
                if (minionTimer >= MINION_INTERVAL)
                {
                    minionTimer -= MINION_INTERVAL;
                    var dir = player.Center - Center;
                    dir.Normalize();
                    dir *= MINION_DIST;
                    Spawn = new IceMinion(castle.mg.Content, castle.mg.SpriteRender, player, Center + dir);
                    Spawn.Parent = this;
                }
            }

            base.Update(gt, s);
        }

        public override void OnDeath(Room r)
        {
            
            base.OnDeath(r);
        }

    }
}
