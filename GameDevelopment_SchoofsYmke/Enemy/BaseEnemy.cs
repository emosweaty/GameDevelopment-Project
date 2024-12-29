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

        public Rectangle Bounds;
        public bool IsOnGround { get; protected set; }
        public bool IsSolid;

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

            Bounds = new Rectangle((int)location.X + 60, (int)location.Y + 80, 120, 135);
            IsSolid = false;
        }

        protected abstract void InitializeAnimation();

        public void Update(GameTime gameTime, Hero hero, CollisionManager collision, ProjectileManager projectile)
        {
            Movement(gameTime, hero, collision);
            UpdateBehaviour(gameTime, hero, projectile);
            animation.Update(gameTime);
        }

        protected virtual void Movement(GameTime gameTime, Hero hero, CollisionManager collision) 
        {
            Vector2 movement = Vector2.Zero;

            if (!collision.IsOnGround(this))
            {
                verticalVelocity += gravity * (float)gameTime.ElapsedGameTime.TotalSeconds;
                verticalVelocity = Math.Clamp(verticalVelocity, -maxFallSpeed, maxFallSpeed);
            }

            if (collision.IsOnGround(this)) verticalVelocity = 0f;

            movement.Y += verticalVelocity;

            Vector2 resolvedMovement = collision.CalculateNewPosition(this, movement);
            location.X += resolvedMovement.X;
            location.Y += resolvedMovement.Y;

            IsOnGround = collision.IsOnGround(this);

            if (direction.X != 0) lastDirectionX = direction.X;
        }

        protected abstract void UpdateBehaviour(GameTime gameTime, Hero hero, ProjectileManager projectile);

        public void Draw(SpriteBatch sprite)
        {
            sprite.Draw(texture, location, animation.CurrentFrame.SourceRectangle, Color.White);
        }

        public bool CollidesWith(ICollidable other)
        {
            return Bounds.Intersects(other.Bounds);
        }
    }
}
