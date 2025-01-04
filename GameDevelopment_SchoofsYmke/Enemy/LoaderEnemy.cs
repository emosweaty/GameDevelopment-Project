using GameDevelopment_SchoofsYmke.Animation;
using GameDevelopment_SchoofsYmke.Blocks;
using GameDevelopment_SchoofsYmke.Enemy;
using GameDevelopment_SchoofsYmke.Interfaces;
using GameDevelopment_SchoofsYmke.Map;
using GameDevelopment_SchoofsYmke.Movement;
using GameDevelopment_SchoofsYmke.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pong.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;


namespace GameDevelopment_SchoofsYmke.Characters
{
    internal class LoaderEnemy : BaseEnemy
    {
        private double attackCooldown;
        private double attackAnimationTimer;

        private float shootingCooldown;
        private float timeSinceLastShot;
        protected override Vector2 Direction
        {
            get { return direction; }
        }

        public LoaderEnemy(Texture2D texture, Vector2 initialPosition, float speed, float viewRange)
           : base(texture, initialPosition, speed, viewRange)
        {
            attackCooldown = 0.4f;
            shootingCooldown = 2.0f;
        }

        protected override void InitializeAnimation()
        {
            animation.GetFramesFromTexture(texture.Width, texture.Height, 6, 9, "loader");
            animation.SetAnimationState(AnimationState.Idle);
        }

        public void Shoot(Vector2 heroPosition, ProjectileManager projectile)
        {
            Vector2 spawnPosition = Bounds.Center.ToVector2();

            if (lastDirectionX > 0)
            {
                spawnPosition.X += Bounds.Width / 2;
            }
            else if (lastDirectionX < 0)
            {
                spawnPosition.X -= Bounds.Width / 2;
            }

            Vector2 direction = Vector2.Normalize(heroPosition - spawnPosition);
            Vector2 initialVelocity = new Vector2(heroPosition.X - spawnPosition.X, (heroPosition.Y - spawnPosition.Y) * 2.5f);
            projectile.AddProjectile(spawnPosition, initialVelocity);
        }

        protected override void UpdateBehaviour(GameTime gameTime, Hero hero, ProjectileManager projectile)
        {
            float distanceToHero = Vector2.Distance(hero.location, location);
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            timeSinceLastShot += (float)gameTime.ElapsedGameTime.TotalSeconds;
            attackAnimationTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (distanceToHero <= viewRange)
            {
                direction.X = Math.Sign(hero.location.X - location.X);

                if (distanceToHero < 180) direction.X = 0;
                else location.X += direction.X * speed * deltaTime;

                if (attackAnimationTimer > 0) animation.SetAnimationState(AnimationState.Attack1);
                else if (distanceToHero > 180) animation.SetAnimationState(AnimationState.Moving);
                else animation.SetAnimationState(AnimationState.Idle);

                if (timeSinceLastShot >= shootingCooldown)
                {
                    attackAnimationTimer = attackCooldown;
                    Shoot(hero.location, projectile);
                    timeSinceLastShot = 0f;
                }
            }
            else animation.SetAnimationState(AnimationState.Idle);
        }
       
    }

}
