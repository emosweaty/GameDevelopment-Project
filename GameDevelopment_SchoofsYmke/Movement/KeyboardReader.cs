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

        private const float accelerate = 0.5f;
        private const float friction = accelerate *0.7f;
        private const float tolerance = friction * 0.9f;
        private const float gravity = 25.0f / 60;
        private float verticalVelocity = 0f;
        private float jumpForce = -10.5f;

        private float maxSpeed = 10f;

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

            if (state.IsKeyDown(Keys.Right))
            {
                direction.X = direction.X + accelerate;
                direction.X = MathHelper.Clamp(direction.X, -maxSpeed, maxSpeed);
                if (onGround && verticalVelocity >= 0)
                {
                    hero.IsMoving = true;
                }
            }
            else if (state.IsKeyDown(Keys.Left))
            {
                direction.X = direction.X - accelerate;
                direction.X = MathHelper.Clamp(direction.X, -maxSpeed, maxSpeed);
                if (onGround && verticalVelocity >= 0)
                {
                    hero.IsMoving = true;
                }
            }
        
            else
            {
                if (onGround == true)
                {
                    direction.X += -Math.Sign(direction.X) * forces.X;
                }
                if (Math.Abs(direction.X) <= tolerance)
                {
                    direction.X = 0f;
                    hero.IsMoving = false;
                }

            }

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

            direction.Y = verticalVelocity;

            return direction;
        }


    }
}
