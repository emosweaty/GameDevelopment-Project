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


namespace GameDevelopment_SchoofsYmke
{
    internal class Hero : IGameObject, ICollidable
    {
        private Texture2D texture;
        public Vector2 location;
        private Vector2 speed;
        private Rectangle bounds;
        private SpriteEffects spriteEffects;

        Animatie animation;

        private IInputReader inputReader;
        private CollisionManager collision;

        public bool IsOnGround { get; private set; }
        public bool IsMoving { get; set; }
        public bool IsJumping { get; set; }
        public AnimationState CurrentState { get; set; }

        public Hero(Texture2D texture)
        {
            this.texture = texture;
            this.inputReader = new KeyboardReader(this);
            CurrentState = Animation.AnimationState.Idle;

            animation = new Animatie();
            animation.GetFramesFromTexture(texture.Width, texture.Height, 13, 8);

            location = new Vector2(10, 870);
            speed = new Vector2(10, 1);
            bounds = new Rectangle((int)location.X, (int)location.Y, texture.Width / 13, texture.Height / 8);

            IsOnGround = location.Y >= 900f;

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
            sprite.Draw(texture, location, animation.CurrentFrame.SourceRectangle, Color.White,
                0f, Vector2.Zero, 1.0f, spriteEffects, 0f);
        }



        public void Update(GameTime gameTime)
        {
            Move(gameTime);
            animation.Update(gameTime);
            HandleState();
        }

        private void Move(GameTime gameTime)
        {
            Debug.WriteLine(IsOnGround);
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Vector2 direction = inputReader.ReadInput(gameTime) * speed;
            Vector2 adjustedMovement = collision?.CalculateNewPosition(this, direction) ?? direction;

            location.X += adjustedMovement.X * deltaTime;
            location.Y += adjustedMovement.Y * deltaTime;

            bounds.Location = location.ToPoint();

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

        public Rectangle Bounds => bounds;

        public bool IsSolid => true;

        public bool IsGround => false;

        public bool CollidesWith(ICollidable other)
        {
            return Bounds.Intersects(other.Bounds);
        }
    }
}
