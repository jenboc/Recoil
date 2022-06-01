using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Recoil
{
    class SpriteClass
    {
        const float HITBOXSCALE = 0.5f;
        
        public Texture2D texture { get; set; }
        public float x { get; set; }
        public float y { get; set; }
        public float angle { get; set; }
        public float dX { get; set; }
        public float dY { get; set; }
        public float dA { get; set; }
        public float scale { get; set; }
        public Vector2 origin { get; set; }

        public SpriteClass(GraphicsDevice gDevice, ContentManager content, string textureName, float scale)
        {
            this.scale = scale;

            if (texture == null && textureName != "")
            {
                texture = content.Load<Texture2D>(textureName);
            }

            origin = new Vector2(texture.Width / 2, texture.Height / 2);
        }
        public SpriteClass(GraphicsDevice gDevice, Texture2D texture, float scale)
        {
            this.scale = scale;
            this.texture = texture;
            origin = new Vector2(texture.Width / 2, texture.Height / 2);
        }

        public void Update(float elapsedTime)
        {
            x += dX * elapsedTime;
            y += dY * elapsedTime;
            angle += dA * elapsedTime;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Vector2(x, y), null, Color.White, angle, origin, new Vector2(scale, scale), SpriteEffects.None, 0f);
        }

        public bool RectangleCollision(SpriteClass otherSprite)
        {
            if (x + texture.Width * scale * HITBOXSCALE / 2 < otherSprite.x - otherSprite.texture.Width * otherSprite.scale / 2) return false;
            if (y + texture.Height * scale * HITBOXSCALE / 2 < otherSprite.y - otherSprite.texture.Height * otherSprite.scale / 2) return false;
            if (x - texture.Width * scale * HITBOXSCALE / 2 > otherSprite.x + otherSprite.texture.Width * otherSprite.scale / 2) return false;
            if (y - texture.Height * scale * HITBOXSCALE / 2 > otherSprite.y + otherSprite.texture.Height * otherSprite.scale / 2) return false;
            return true;
        }
    }
}
