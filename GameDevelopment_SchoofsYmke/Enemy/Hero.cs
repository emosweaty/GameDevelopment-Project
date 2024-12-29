using GameDevelopment_SchoofsYmke.Animation;
using GameDevelopment_SchoofsYmke.Interfaces;
using GameDevelopment_SchoofsYmke.Movement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pong.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;


namespace GameDevelopment_SchoofsYmke.Characters
{
    internal class Hero : IGameObject, ICollidable
    {
        private Texture2D texture;
        public Vector2 location;
        private Vector2 speed;
        private Rectangle bounds;
        private Vector2 positionOffset;
        private SpriteEffects spriteEffects;

        Animatie animation;

        private IInputReader inputReader;
        private CollisionManager collision;

        public bool IsOnGround { get; private set; }
        public bool IsMoving { get; set; }
        public bool IsJumping { get; set; }
        public Vector2 Velocity { get; private set; }
        public AnimationState CurrentState { get; set; }
        public int Health { get; private set; }

        private float damageAnimationTimer;
        private float damageAnimationDuration;
        private bool isTakingDamage = false;
        public Hero(Texture2D texture)
        {
            this.texture = texture;
            inputReader = new KeyboardReader(this);
            CurrentState = AnimationState.Idle;

            animation = new Animatie();
            animation.GetFramesFromTexture(texture.Width, texture.Height, 13, 8, "hero");

            location = new Vector2(40, 700);
            positionOffset = new Vector2(-100, -60);

            speed = new Vector2(10, 1);

            IsOnGround = location.Y >= 900f;

            Health = 50;
            damageAnimationDuration = 0.5f;
        }

        public void TakeDamage(int damage)
        {
            if (!isTakingDamage)
            {
                Health -= damage;
                isTakingDamage = true;
                damageAnimationTimer = damageAnimationDuration;

                if (Health <= 0)
                {
                    Health = 0;
                    IsDeath();
                }
            }
        }

        public void IsDeath()
        {
            Debug.WriteLine("OOPS YOU DIED");
        }

        public void SetMovementManager(CollisionManager collision)
        {
            this.collision = collision;
        }

        public void SetState(AnimationState state)
        {
            CurrentState = state;
            animation.SetAnimationState(state);
        }

        public void Draw(SpriteBatch sprite)
        {
            sprite.Draw(texture, location + positionOffset, animation.CurrentFrame.SourceRectangle, Color.White,
                0f, Vector2.Zero, 1.0f, spriteEffects, 0f);
        }



        public void Update(GameTime gameTime)
        {
            if (isTakingDamage)
            {
                damageAnimationTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (damageAnimationTimer <= 0)
                {
                    isTakingDamage = false;
                }
            }

            Move(gameTime);
            animation.Update(gameTime);
            HandleState();
        }

        private void Move(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Vector2 direction = inputReader.ReadInput(gameTime);
            Vector2 adjustedMovement = collision?.CalculateNewPosition(this, direction) ?? direction;

            Velocity = direction * speed;

            location.X += adjustedMovement.X * deltaTime;
            location.Y += adjustedMovement.Y * deltaTime;

            // bounds.Location = (location + offsetBounds).ToPoint();

            if (collision != null)
            {
                IsOnGround = collision.IsOnGround(this);
            }
            else
            {
                IsOnGround = false;
            }

            if (direction.X > 0) { spriteEffects = SpriteEffects.None; }
            if (direction.X < 0) { spriteEffects = SpriteEffects.FlipHorizontally; }

        }

        private void HandleState()
        {
            if (isTakingDamage)
            {
                animation.SetAnimationState(AnimationState.Hit);
                return;
            }

            if (IsMoving)
            {
                animation.SetAnimationState(AnimationState.Moving);
            }
            else if (IsJumping)
            {
                animation.SetAnimationState(AnimationState.Jumping);
            }
            else
            {
                animation.SetAnimationState(AnimationState.Idle);
            }

        }

        public Rectangle Bounds
        {
            get
            {
                return new Rectangle(
                    (int)location.X,
                    (int)location.Y,
                    65,
                    110);
            }
        }

        public bool IsSolid => true;

        public bool IsGround => false;

        public bool CollidesWith(ICollidable other)
        {
            return Bounds.Intersects(other.Bounds);
        }
    }
}
