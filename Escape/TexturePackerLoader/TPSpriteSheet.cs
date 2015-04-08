namespace TexturePackerLoader
{
    using System.Collections.Generic;

    using Microsoft.Xna.Framework.Graphics;

    public class TPSpriteSheet
    {
        private readonly IDictionary<string, SpriteFrame> spriteList;

        public TPSpriteSheet()
        {
            spriteList = new Dictionary<string, SpriteFrame>();
        }

        public void Add(string name, SpriteFrame sprite)
        {
            spriteList.Add(name, sprite);
        }

        public void Add(TPSpriteSheet otherSheet)
        {
            foreach (var sprite in otherSheet.spriteList)
            {
                spriteList.Add(sprite);
            }
        }

        public SpriteFrame Sprite(string sprite)
        {
            return this.spriteList[sprite];
        }

    }
}