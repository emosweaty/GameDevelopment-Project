using GameDevelopment_SchoofsYmke.Animation;
using GameDevelopment_SchoofsYmke.Characters;
using GameDevelopment_SchoofsYmke.Interfaces;
using GameDevelopment_SchoofsYmke.Movement;
using GameDevelopment_SchoofsYmke.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDevelopment_SchoofsYmke.Enemy
{
    internal abstract class BaseEnemy : ICollidable
    {
        protected Texture2D texture;
        protected Animatie animation;
        protected Vector2 location;
        protected Vector2 direction;
        protected float speed;
        protected float viewRange;

        protected float lastDirectionX;
        protected float verticalVelocity;
        protected float gravity;
        protected float maxFallSpeed;
        protected Vector2 movement;

        protected virtual Vector2 Direction { get; }
        public Rectangle Bounds => new Rectangle((int)location.X + 60, (int)location.Y + 80, 120, 135);
        public bool IsOnGround { get; protected set; }
        public bool IsSolid => false;

        protected BaseEnemy(Texture2D texture, Vector2 initialPosition, float speed, float viewRange)
        {
            this.texture = texture;
            this.location = initialPosition;
            this.speed = speed;
            this.viewRange = viewRange;

            animation = new Animatie();
            InitializeAnimation();

            lastDirectionX = 1;
            gravity = 0.5f;
            maxFallSpeed = 5f;
        }

        protected abstract void InitializeAnimation();

        public virtual void Update(GameTime gameTime, Hero hero, CollisionManager collision, ProjectileManager projectile)
        {
            animation.Update(gameTime);
            Movement(gameTime, hero, collision);
            UpdateBehaviour(gameTime, hero, projectile);

        }

        protected virtual void Movement(GameTime gameTime, Hero hero, CollisionManager collision)
        {
            Vector2 movement = Vector2.Zero;

            if (!collision.IsOnGround(this))
            {
                verticalVelocity += gravity * (float)gameTime.ElapsedGameTime.TotalSeconds;
                verticalVelocity = MathHelper.Clamp(verticalVelocity, -maxFallSpeed, maxFallSpeed);
            }

            if (collision.IsOnGround(this)) verticalVelocity = 0f;

            movement.Y += verticalVelocity;

            Vector2 resolvedMovement = collision.CalculateNewPosition(this, movement);

            location.X += resolvedMovement.X;
            location.Y += resolvedMovement.Y;

            IsOnGround = collision.IsOnGround(this);

            if (direction.X != 0) lastDirectionX = direction.X;

            if (lastDirectionX > 0) animation.CurrentFrame.SpriteEffect = SpriteEffects.None;

            else if (lastDirectionX < 0) animation.CurrentFrame.SpriteEffect = SpriteEffects.FlipHorizontally;
        }

        protected abstract void UpdateBehaviour(GameTime gameTime, Hero hero, ProjectileManager projectile);

        protected void UpdateFlipping(Vector2 currenDirection)
        {
            if (currenDirection.X != 0) lastDirectionX = currenDirection.X;
            if (lastDirectionX > 0) animation.CurrentFrame.SpriteEffect = SpriteEffects.None;
            else if (lastDirectionX < 0) animation.CurrentFrame.SpriteEffect = SpriteEffects.FlipHorizontally;
        }

        public void Draw(SpriteBatch sprite)
        {
            sprite.Draw(texture, location, animation.CurrentFrame.SourceRectangle, Color.White, 0f, Vector2.Zero, 1f, animation.CurrentFrame.SpriteEffect, 0f);
        }

        public bool CollidesWith(ICollidable other)
        {
            return Bounds.Intersects(other.Bounds);
        }
    }
}
