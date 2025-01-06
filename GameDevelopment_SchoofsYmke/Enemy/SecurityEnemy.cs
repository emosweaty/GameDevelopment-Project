using GameDevelopment_SchoofsYmke.Animation;
using GameDevelopment_SchoofsYmke.Character;
using GameDevelopment_SchoofsYmke.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDevelopment_SchoofsYmke.Enemy
{
    internal class SecurityEnemy : BaseEnemy
    {
        private double attackCooldown;
        private double attackAnimationTimer;
        private float shootingCooldown;
        private float timeSinceLastShot;
        public override Rectangle Bounds => new Rectangle((int)location.X + 70, (int)location.Y + 120, 100, 120);

        public SecurityEnemy(Texture2D texture, Texture2D projectileTexture, Vector2 initialPosition, float speed, float viewRange, ProjectileManager projectileManager)
       : base(texture, projectileTexture, initialPosition, speed, viewRange, projectileManager)
        {
            attackCooldown = 0.5f;
            shootingCooldown = 1.5f;
        }

        protected override void InitializeAnimation()
        {
            animation.GetFramesFromTexture(texture.Width, texture.Height, 8, 9, "security");
            animation.SetAnimationState(AnimationState.Idle);
        }

        protected override void EnemyBehaviour(GameTime gameTime, Hero hero, ProjectileManager projectile)
        {
            float distanceToHero = Vector2.Distance(hero.location, location);
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            timeSinceLastShot += deltaTime;
            attackAnimationTimer -= deltaTime;

            if (distanceToHero <= viewRange)
            {
                direction.X = Math.Sign(hero.location.X - location.X);

                if (Bounds.Intersects(hero.Bounds))
                {
                    direction.X = 0;
                }
                else location.X += direction.X * speed * deltaTime;

                if (attackAnimationTimer > 0)
                {
                    animation.SetAnimationState(AnimationState.Attack1);

                    var attackBounds = direction.X >= 0
                        ? new Rectangle(
                            (int)location.X + 170,
                            (int)location.Y + 120,
                            40,
                            Bounds.Height
                        )
                        : new Rectangle(
                            (int)location.X + 40,
                            (int)location.Y + 120,
                            40,
                            Bounds.Height
                        );

                    if (attackBounds.Intersects(hero.Bounds))
                    {
                        hero.TakeDamage(10);
                    }
                }
                else if (distanceToHero > 10) animation.SetAnimationState(AnimationState.Moving);
                else animation.SetAnimationState(AnimationState.Idle);

                if (timeSinceLastShot >= shootingCooldown && distanceToHero <= 200)
                {
                    attackAnimationTimer = attackCooldown;
                    timeSinceLastShot = 0f;
                }
            }
            else animation.SetAnimationState(AnimationState.Idle);
        }
    }
}

