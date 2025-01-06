using GameDevelopment_SchoofsYmke.Animation;
using GameDevelopment_SchoofsYmke.Blocks;
using GameDevelopment_SchoofsYmke.Character;
using GameDevelopment_SchoofsYmke.Character;
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
        protected ProjectileManager projectileManager;
        protected Animatie animation;
        protected Texture2D texture;
        protected Texture2D projectileTexture;

        protected Vector2 location;
        private Vector2 initialPosition;
        protected Vector2 direction;
        protected float viewRange;
        protected float lastDirectionX;

        protected float speed;
        protected float verticalVelocity;
        protected float gravity;
        protected float maxFallSpeed;
        protected Vector2 movement;

        protected int health;
        public bool isAlive { get; set; }

        protected virtual Vector2 Direction { get; }
        public virtual Rectangle Bounds => new Rectangle((int)location.X + 60, (int)location.Y + 80, 120, 135);
        public bool IsOnGround { get; protected set; }
        public bool IsSolid { get; set; }

        protected float hitAnimationTimer;
        protected float hitAnimationDuration = 0.5f;
        protected bool isTakingDamage;

        protected BaseEnemy(Texture2D texture, Texture2D projectileTexture, Vector2 initialPosition, float speed, float viewRange, ProjectileManager projectileManager)
        {
            this.projectileManager = projectileManager;
            this.texture = texture;
            this.projectileTexture = projectileTexture;

            this.initialPosition = initialPosition;
            this.location = initialPosition;
            this.speed = speed;
            this.viewRange = viewRange;

            animation = new Animatie();
            InitializeAnimation();

            lastDirectionX = 1;
            gravity = 0.5f;
            maxFallSpeed = 5f;

            health = 50;
            isAlive = true;
            IsSolid = true;
        }

        protected abstract void InitializeAnimation();

        public virtual void Update(GameTime gameTime, Hero hero, CollisionManager collision, ProjectileManager projectile)
        {
            if (!isAlive)
            {
                Die();
                return;
            }
            if (isAlive)
            {
                animation.Update(gameTime);
                Movement(gameTime, hero, collision);
                UpdateBehaviour(gameTime, hero, projectile);
            }
        }

        public virtual void Reset()
        {
            location = initialPosition;
            health = 100;
            isAlive = true;
            IsSolid = true;
        }

        public void TakeDamage(int damage)
        {
            health -= damage;

            if (health <= 0)
            {
                isAlive = false;
            }
            else
            {
                isTakingDamage = true;
                hitAnimationTimer = hitAnimationDuration;
            }
        }

        public void Die()
        {
            IsSolid = false;
        }

        protected virtual void Movement(GameTime gameTime, Hero hero, CollisionManager collision)
        {
            Vector2 movement = Vector2.Zero;

            verticalVelocity += gravity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            verticalVelocity = MathHelper.Clamp(verticalVelocity, -maxFallSpeed, maxFallSpeed);

            if (collision.IsOnGround(this)) verticalVelocity = 0f;

            movement.Y += verticalVelocity;
            movement.X += direction.X;

            Vector2 adjustedMovement = collision.CalculateNewPosition(this, movement);
            location += adjustedMovement;

            IsOnGround = collision.IsOnGround(this);

            if (direction.X != 0) lastDirectionX = direction.X;

            if (lastDirectionX > 0) animation.CurrentFrame.SpriteEffect = SpriteEffects.None;
            else if (lastDirectionX < 0) animation.CurrentFrame.SpriteEffect = SpriteEffects.FlipHorizontally;
        }

        protected virtual void UpdateBehaviour(GameTime gameTime, Hero hero, ProjectileManager projectile)
        {
            if (isTakingDamage)
            {
                hitAnimationTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (hitAnimationTimer <= 0)
                {
                    isTakingDamage = false;
                }

                animation.SetAnimationState(AnimationState.Hit);
            }
            else
            {
                EnemyBehaviour(gameTime, hero, projectile);
            }
        }

        protected abstract void EnemyBehaviour(GameTime gameTime, Hero hero, ProjectileManager projectile);

        public virtual void Draw(SpriteBatch sprite)
        {
            if (isAlive)
            {
                sprite.Draw(texture, location, animation.CurrentFrame.SourceRectangle, Color.White, 0f, Vector2.Zero, 1f, animation.CurrentFrame.SpriteEffect, 0f);
            }
        }

        public bool CollidesWith(ICollidable other)
        {
            return Bounds.Intersects(other.Bounds);
        }
    }
}
