using GameDevelopment_SchoofsYmke.Animation;
using GameDevelopment_SchoofsYmke.Character;
using GameDevelopment_SchoofsYmke.Movement;
using GameDevelopment_SchoofsYmke.Projectiles;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDevelopment_SchoofsYmke.Enemy
{
    internal class OperatorEnemy : BaseEnemy
    {
        private double attackCooldown;
        private double attackAnimationTimer;
        private float shootingCooldown;
        private float timeSinceLastShot;
        private Rectangle attackBounds;
        public override Rectangle Bounds => new Rectangle((int)location.X, (int)location.Y, 192, 192);

        public OperatorEnemy(Texture2D texture, Texture2D projectileTexture, Vector2 initialPosition, float speed, float viewRange, ProjectileManager projectileManager)
            : base(texture, projectileTexture, initialPosition, speed, viewRange, projectileManager)
        {
            attackCooldown = 0.5f;
            shootingCooldown = 1.5f;

            health = 40;
        }

        protected override void InitializeAnimation()
        {
            animation.GetFramesFromTexture(texture.Width, texture.Height, 8, 11, "operator");
            animation.SetAnimationState(AnimationState.Idle);
        }

        public override void Update(GameTime gameTime, Hero hero, CollisionManager collision, ProjectileManager projectile)
        {
            if (!isAlive)
            {
                Die();
                return;
            }
            if (isAlive)
            {
                animation.Update(gameTime);
                UpdateBehaviour(gameTime, hero, projectile);
            }
        }
        public override void Reset()
        {
            health = 20;
            base.Reset();
        }


        protected override void EnemyBehaviour(GameTime gameTime, Hero hero, ProjectileManager projectile)
        {
            float distanceToHero = Vector2.Distance(hero.location, location);
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            timeSinceLastShot += deltaTime;
            attackAnimationTimer -= deltaTime;

            location.Y += (float)Math.Sin(gameTime.TotalGameTime.TotalSeconds * 1f) * 0.5f;

            if (distanceToHero <= viewRange)
            {
                if (attackAnimationTimer > 0)
                {
                    animation.SetAnimationState(AnimationState.Attack1);

                    attackBounds = new Rectangle(
                         (int)hero.location.X + 50,
                         (int)hero.location.Y + 60,
                         50,
                         50 
                    );

                    if (attackBounds.Intersects(hero.Bounds))
                    {
                        hero.TakeDamage(10);
                    }
                }
                else
                {
                    animation.SetAnimationState(AnimationState.Idle);
                }

                if (timeSinceLastShot >= shootingCooldown)
                {
                    attackAnimationTimer = attackCooldown;
                    timeSinceLastShot = 0f;
                }
            }
            else
            {
                animation.SetAnimationState(AnimationState.Idle);
            }
        }

        public override void Draw(SpriteBatch sprite)
        {
            base.Draw(sprite);

            if (attackAnimationTimer > 0)
            {
                sprite.Draw(texture, attackBounds, Color.Red * 0.5f); 
            }
        }
    }
}

