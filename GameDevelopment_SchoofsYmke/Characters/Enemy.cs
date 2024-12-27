using GameDevelopment_SchoofsYmke.Animation;
using GameDevelopment_SchoofsYmke.Blocks;
using GameDevelopment_SchoofsYmke.Interfaces;
using GameDevelopment_SchoofsYmke.Map;
using GameDevelopment_SchoofsYmke.Movement;
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
    internal class Enemy : ICollidable
    {
        private Animatie animation;
        private Texture2D texture;
        public Vector2 location;
        private Vector2 direction;
        private float viewRange;
        private float speed;
        private float lastDirectionX = 1;
        private double elapsedTimeCounter = 0; 
        private double attackDuration = 50;

        private float verticalVelocity = 0f;
        private const float gravity = 0.5f;
        private const float maxFallSpeed = 5f;
        public Rectangle Bounds => new Rectangle((int)location.X + 60, (int)location.Y+80, 120, 135);
        public bool IsOnGround { get; private set; }
        public bool IsSolid => false;

        public Enemy(Texture2D texture, Vector2 initialPosition, float speed, float viewRange)
        {
            this.texture = texture;
            location = initialPosition;
            this.speed = speed;
            this.viewRange = viewRange;

            animation = new Animatie();
            animation.GetFramesFromTexture(texture.Width, texture.Height, 6, 9, "loader");
            animation.SetAnimationState(AnimationState.Idle);
        }

        public void Update(GameTime gameTime, Vector2 heroPosition, Hero hero, CollisionManager collision)
        {
            float distanceToHero = Vector2.Distance(heroPosition, location);

            elapsedTimeCounter--;

            Vector2 movement = Vector2.Zero;

            if (distanceToHero <= viewRange)
            {
                direction.X = Math.Sign(heroPosition.X - location.X);

                if (Bounds.Intersects(hero.Bounds))
                {
                    direction.X = 0;

                    if (elapsedTimeCounter <= 0) elapsedTimeCounter = attackDuration;

                    else if (elapsedTimeCounter <= 20) animation.SetAnimationState(AnimationState.Idle);

                    else if (elapsedTimeCounter > 20) animation.SetAnimationState(AnimationState.Attack1);
                }
                else
                {
                    animation.SetAnimationState(AnimationState.Moving);
                    movement.X = direction.X * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
            }
            else animation.SetAnimationState(AnimationState.Idle);

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

            animation.Update(gameTime);

            if (direction.X != 0) lastDirectionX = direction.X;
          
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
