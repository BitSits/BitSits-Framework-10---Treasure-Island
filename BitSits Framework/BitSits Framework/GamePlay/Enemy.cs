using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Box2D.XNA;

namespace BitSits_Framework
{
    class Enemy
    {
        GameContent gameContent;
        Body body;
        Animation walk;
        AnimationPlayer animationPlayer;

        float direction;
        const float MaxMoveTime = 2;
        float moveTime;

        float linearImpulse;

        public Enemy(World world, GameContent gameContent, int index, Vector2 position)
        {
            this.gameContent = gameContent;

            walk = new Animation(gameContent.enemy[index], 2, 0.15f, true, new Vector2(0.5f));
            animationPlayer.PlayAnimation(walk);

            if (index == 0) linearImpulse = 1f / 2;
            else linearImpulse = 0.75f / 2;

            BodyDef bd = new BodyDef();
            bd.position = position / gameContent.b2Scale;
            bd.type = BodyType.Dynamic;
            bd.linearDamping = 10;
            body = world.CreateBody(bd);

            CircleShape cs = new CircleShape();
            cs._radius = (float)(walk.FrameWidth - 1) / gameContent.b2Scale / 2;
            FixtureDef fd = new FixtureDef();
            fd.shape = cs;
            fd.filter.groupIndex = -1;

            body.CreateFixture(fd);
            body.SetUserData(this);
        }

        public Rectangle Bounds
        {
            get
            {
                int halfSize = walk.FrameWidth / 3;
                return new Rectangle((int)(body.Position.X * gameContent.b2Scale) - halfSize,
                    (int)(body.Position.Y * gameContent.b2Scale) - halfSize, halfSize * 2, halfSize * 2);
            }
        }

        public void Update(GameTime gameTime, Vector2 playerPos)
        {
            Vector2 v = body.Position * gameContent.b2Scale;

            if (Vector2.Distance(v, playerPos) < 2 * Tile.Width)
            {
                direction = (float)Math.Atan2(playerPos.Y - v.Y, playerPos.X - v.X);
            }
            else
            {
                moveTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (moveTime > MaxMoveTime)
                {
                    moveTime = 0;
                    direction = (float)gameContent.random.Next(360) / 180 * (float)Math.PI;
                }
            }

            body.ApplyLinearImpulse(new Vector2((float)Math.Cos(direction), (float)Math.Sin(direction)) * linearImpulse
                * 60 * (float)gameTime.ElapsedGameTime.TotalSeconds, body.Position);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            animationPlayer.Draw(gameTime, spriteBatch, body.Position * gameContent.b2Scale, Color.White, 0,
                Math.Abs(direction) > Math.PI / 2? SpriteEffects.FlipHorizontally : SpriteEffects.None);
        }
    }
}
