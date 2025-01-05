using GameDevelopment_SchoofsYmke.Character;
using GameDevelopment_SchoofsYmke.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;

using System.Text;
using System.Threading.Tasks;

namespace GameDevelopment_SchoofsYmke.Movement
{
    internal class KeyboardReader : IInputReader
    {
        private Hero hero;
        private bool onGround;

        private const float accelerate = 0.2f;
        private const float friction = accelerate *0.9f;
        private const float tolerance = friction * 0.9f;
        private const float gravity = 25.0f / 60;
        private float verticalVelocity = 0f;
        private float velocity = 0f;
        private float jumpForce = -10.5f;
        private float maxSpeed = 5;

        private Vector2 forces = new Vector2(friction, gravity);


        public KeyboardReader(Hero hero) 
        {
            this.hero = hero;
        }

        public Vector2 ReadInput(GameTime gameTime)
        {
            onGround = hero.IsOnGround;

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            KeyboardState state = Keyboard.GetState();
            Vector2 direction = Vector2.Zero;

            if (state.IsKeyDown(Keys.Right) || state.IsKeyDown(Keys.D))
            {
                //direction.X = direction.X + accelerate;
                //direction.X = MathHelper.Clamp(direction.X, -maxSpeed, maxSpeed);
                velocity += accelerate;
                if (onGround && verticalVelocity >= 0)
                {
                    hero.IsMoving = true;
                }
            }
            else if (state.IsKeyDown(Keys.Left) || state.IsKeyDown(Keys.A))
            {
                //direction.X = direction.X - accelerate;
                //direction.X = MathHelper.Clamp(direction.X, -maxSpeed, maxSpeed);
                velocity -= accelerate;
                if (onGround && verticalVelocity >= 0)
                {
                    hero.IsMoving = true;
                }
            }
            else
            {
                if (onGround == true)
                {
                    velocity += -Math.Sign(velocity) * friction;
                }
                if (Math.Abs(velocity) < tolerance)
                {
                    velocity = 0f;
                    hero.IsMoving = false;
                }

            }
            velocity = MathHelper.Clamp(velocity, -maxSpeed, maxSpeed);

            if (state.IsKeyDown(Keys.Space) && onGround == true)
            {
                verticalVelocity = jumpForce;
                hero.IsJumping = true;
                hero.IsMoving = false;
            }

            if (onGround && verticalVelocity >= 0)
            {
                hero.IsJumping = false; 
            }

            if (!onGround)
            {
                verticalVelocity += gravity;
            }
            direction.X = velocity;
            direction.Y = verticalVelocity;

            return direction;
        }


    }
}
