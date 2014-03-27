using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Box2D.XNA;

namespace BitSits_Framework
{
    class Player
    {
        GameContent gameContent;
        World world;
        public Body body;

        public Vector2 direction;
        SpriteEffects spriteEffects = SpriteEffects.None;

        const float MaxHelth = 100;
        public float health = MaxHelth;

        public bool inSea = false;

        Animation idle, walk, die;
        AnimationPlayer animationPlayer = new AnimationPlayer();

        public Player(GameContent gameContent, World world, Vector2 position)
        {
            this.gameContent = gameContent;
            this.world = world;

            idle = new Animation(gameContent.playerIdle, 2, 2f, true, new Vector2(0.5f));
            walk = new Animation(gameContent.playerWalk, 2, 0.2f, true, new Vector2(0.5f));
            die = new Animation(gameContent.playerDie, 2, 0.2f, false, new Vector2(0.5f));

            animationPlayer.PlayAnimation(idle);

            BodyDef bd = new BodyDef();
            bd.position = position / gameContent.b2Scale;
            bd.type = BodyType.Dynamic;
            bd.linearDamping = 10;
            body = world.CreateBody(bd);

            CircleShape cs = new CircleShape();
            cs._radius = (float)(idle.FrameWidth - 1) / gameContent.b2Scale / 2;
            FixtureDef fd = new FixtureDef();
            fd.shape = cs;
            fd.filter.groupIndex = -1;

            body.CreateFixture(fd);
        }

        public Rectangle HealthBounds
        {
            get
            {
                int halfSize = idle.FrameWidth / 3;
                return new Rectangle((int)(body.Position.X * gameContent.b2Scale) - halfSize,
                    (int)(body.Position.Y * gameContent.b2Scale) - halfSize, halfSize * 2, halfSize * 2);
            }
        }

        private Rectangle Bounds
        {
            get
            {
                int halfSize = idle.FrameWidth / 2;
                return new Rectangle((int)(body.Position.X * gameContent.b2Scale) - halfSize,
                    (int)(body.Position.Y * gameContent.b2Scale) - halfSize, halfSize * 2, halfSize * 2);
            }
        }

        public void Update(GameTime gameTime)
        {
            health = MathHelper.Clamp(health, 0, MaxHelth);

            float e = (float)gameTime.ElapsedGameTime.TotalSeconds;
            body.ApplyLinearImpulse(direction * (inSea ? 0.75f : 1f) * 60 * e, body.Position);

            if (health == 0) animationPlayer.PlayAnimation(die);
            else
            {
                if (direction != Vector2.Zero) animationPlayer.PlayAnimation(walk);
                else animationPlayer.PlayAnimation(idle);
            }
        }

        public void CheckCollision(Rectangle shipBounds)
        {
            for (ContactEdge ce = body.GetContactList(); ce != null; ce = ce.Next)
            {
                bool isTouching = false;
                float theta = 0;

                if (ce.Contact.IsTouching() && ((string)ce.Contact.GetFixtureB().GetUserData() == "string"
                    || (string)ce.Contact.GetFixtureA().GetUserData() == "string"))
                {
                    WorldManifold wm; ce.Contact.GetWorldManifold(out wm);
                    theta = (float)Math.Atan2(-wm._normal.Y, -wm._normal.X);

                    for (int i = -2; i <= 2; i++) if ((float)Math.PI / 2 * i == theta) isTouching = true;
                }

                if (isTouching)
                {
                    if (!inSea)
                    {
                        if (shipBounds.Intersects(Bounds))
                        {
                            Point p = shipBounds.Center;
                            Vector2 v = new Vector2(p.X, p.Y), b = body.Position * gameContent.b2Scale;

                            float delta = (float)Math.Atan2(v.Y - b.Y, v.X - b.X);

                            if ((theta - .75f < delta && delta < theta + .75f)
                                || ((theta == (float)Math.PI || theta == -(float)Math.PI) 
                                && (-theta - .75f < delta && delta < -theta + .75f)))
                            {
                                inSea = true;
                                body.SetLinearVelocity(Vector2.Zero);
                                gameContent.jump.Play();

                                body.Position = Tile.TileCenter(v) / gameContent.b2Scale;

                                break;
                            }
                        }
                    }
                    else
                    {
                        inSea = false;
                        body.Position = Tile.TileCenter(new Vector2((float)Math.Cos(theta),
                            (float)Math.Sin(theta)) * Tile.Width + body.Position * gameContent.b2Scale)
                            / gameContent.b2Scale;

                        break;
                    }
                }
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (direction.X < 0) spriteEffects = SpriteEffects.FlipHorizontally;
            if (direction.X > 0) spriteEffects = SpriteEffects.None;

            animationPlayer.Draw(gameTime, spriteBatch, body.Position * gameContent.b2Scale,
                Color.White, 0, spriteEffects);
        }

        public void DrawHealthBar(SpriteBatch spriteBatch)
        {
            Vector2 size = new Vector2(8 * 30, 8 * 4);
            Vector2 v = new Vector2(400, 32) - size / 2;

            spriteBatch.Draw(gameContent.healthBar, v, Color.White * 0.8f);
            spriteBatch.Draw(gameContent.healthBar, v, new Rectangle(0, 0, (int)(health / MaxHelth * size.X),
                (int)size.Y), Color.Crimson * 0.8f);
        }
    }
}
