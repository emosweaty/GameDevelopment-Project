﻿using GameDevelopment_SchoofsYmke.Animation;
using GameDevelopment_SchoofsYmke.Display;
using GameDevelopment_SchoofsYmke.Interfaces;
using GameDevelopment_SchoofsYmke.Movement;
using GameDevelopment_SchoofsYmke.Projectiles;
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
        private Texture2D textureArrow;
        public Vector2 location;
        private Vector2 speed;
        private Rectangle bounds;
        private Vector2 positionOffset;
        private SpriteEffects spriteEffects;
        Animatie animation;
        private ProjectileManager projectileManager;
        private IInputReader inputReader;
        private CollisionManager collision;
        private HealthBar healthBar;
        public bool IsOnGround { get; private set; }
        public bool IsMoving { get; set; }
        public bool IsJumping { get; set; }
        public bool IsHit { get; set; }
        public Vector2 Velocity { get; private set; }
        public AnimationState CurrentState { get; set; }
        public int Health { get; private set; }

        private float damageAnimationTimer;
        private float damageAnimationDuration;
        private bool isTakingDamage;

        private float shootCooldown;
        private float maxShootCooldown;
        private float shootingAnimationTimer;
        private float shootingAnimationDuration;
        private bool isShooting;

        public Hero(Texture2D texture, Texture2D arrowtexture, ProjectileManager projectileManager, HealthBar healthBar)
        {
            this.texture = texture;
            textureArrow = arrowtexture;
            this.projectileManager = projectileManager;
            this.healthBar = healthBar;
            inputReader = new KeyboardReader(this);
            CurrentState = AnimationState.Idle;

            animation = new Animatie();
            animation.GetFramesFromTexture(texture.Width, texture.Height, 13, 8, "hero");

            location = new Vector2(40, 1200);
            positionOffset = new Vector2(-100, -60);

            speed = new Vector2(10, 1);

            IsOnGround = location.Y >= 900f;

            Health = 50;
            damageAnimationDuration = 0.5f;
            isTakingDamage = false;

            maxShootCooldown = 0.8f;
            shootingAnimationDuration = 0.6f;
        }

        public void TakeDamage(int damage)
        {
            if (!isTakingDamage)
            {
                Health -= damage;
                isTakingDamage = true;
                damageAnimationTimer = damageAnimationDuration;

                healthBar.TakeDamage(damage);
                if (Health <= 0)
                {
                    Health = 0;
                }
            }
            IsHit = true;
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

            projectileManager.Draw(sprite, textureArrow);
        }

        public void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (shootCooldown > 0)
            {
                shootCooldown -= deltaTime;
            }

            if (isTakingDamage)
            {
                damageAnimationTimer -= deltaTime;

                if (damageAnimationTimer <= 0)
                {
                    isTakingDamage = false;
                }
            }

            if (isShooting)
            {
                shootingAnimationTimer -= deltaTime;
                if (shootingAnimationTimer <= 0)
                {
                    isShooting = false;
                }
            }

            Shoot();

            projectileManager.Update(gameTime, this, new Point(1920, 1080), 1080, collision);

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
            if (isShooting)
            {
                animation.SetAnimationState(AnimationState.Attack1);
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

        private void Shoot()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.F) && shootCooldown <= 0)
            {
                isShooting = true;
                shootingAnimationTimer = shootingAnimationDuration;

                Vector2 spawnPosition = location;
                Vector2 initialVelocity = new Vector2(0, -500);
                projectileManager.AddProjectile(spawnPosition, initialVelocity);

                shootCooldown = maxShootCooldown;
                Debug.WriteLine("Hero fired a projectile!");
            }
        }

        public void Reset()
        {
            location = new Vector2(40, 1200);
            speed = new Vector2(10, 1);
            IsOnGround = location.Y >= 900f;
            Health = 50;
            isTakingDamage = false;
            damageAnimationTimer = 0f;
            shootCooldown = 0f;
            shootingAnimationTimer = 0f;
            isShooting = false;
            IsMoving = false;
            IsJumping = false;
            spriteEffects = SpriteEffects.None;
            animation.SetAnimationState(AnimationState.Idle);
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
