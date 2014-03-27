using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BitSits_Framework
{
    enum TileType
    {
        land, sea,
    }

    struct Tile
    {
        public const int Width = 64;
        public const int Height = 64;

        public static Rectangle GetBounds(Vector2 position)
        {
            int x = (int)Math.Floor(position.X / Width);
            int y = (int)Math.Floor(position.Y / Height);

            return new Rectangle(x * Width, y * Height, Width, Height);
        }

        public static Vector2 TileCenter(Vector2 position)
        {
            Point p = GetBounds(position).Center; return new Vector2(p.X, p.Y);
        }

        public Texture2D texture;
        public TileType tileType;

        public Tile(Texture2D texture, TileType tileType)
        {
            this.texture = texture; this.tileType = tileType;
        }
    }
}
