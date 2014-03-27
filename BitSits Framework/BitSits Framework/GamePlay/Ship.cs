using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BitSits_Framework
{
    class Ship
    {
        Texture2D texture;
        public Vector2 position;
        public Vector2 direction;

        SpriteEffects spriteEffects = SpriteEffects.None;

        public Ship(Texture2D texture, Vector2 position)
        {
            this.texture = texture;
            this.position = position;
        }

        public Rectangle BoundingRectangle
        {
            get
            {
                int halfSize = texture.Width / 2;
                return new Rectangle((int)position.X - halfSize, (int)position.Y - halfSize,
                    halfSize * 2, halfSize * 2);
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (direction.X == -1) spriteEffects = SpriteEffects.FlipHorizontally;
            if (direction.X == 1) spriteEffects = SpriteEffects.None;

            spriteBatch.Draw(texture, position - new Vector2(texture.Width, texture.Height) / 2, null,
                Color.White, 0, Vector2.Zero, 1, spriteEffects, 1);
        }
    }
}
