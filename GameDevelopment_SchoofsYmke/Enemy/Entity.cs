using GameDevelopment_SchoofsYmke.Animation;
using GameDevelopment_SchoofsYmke.Interfaces;
using GameDevelopment_SchoofsYmke.Movement;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Pong.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDevelopment_SchoofsYmke.Enemy
{
    abstract class Entity : ICollidable, IGameObject
    {
        protected Vector2 location;
        protected float speed;

        protected Sprite sprite;
        protected Animatie animation;
        protected CollisionManager collisionManager;

        public int Health { get; protected set; }
        public bool IsOnGround { get; protected set; }
        public bool IsSolid => true;
        public Vector2 Location => location;
        public Entity(Texture2D texture, int animationHeight, int animationWidth, string name)
        {
            animation = new Animatie();
            animation.GetFramesFromTexture(texture.Width, texture.Height, animationHeight, animationWidth, name);

            sprite = new Sprite(texture, animation);
            Health = 100;
        }

        public void SetCollisionManager(CollisionManager collisionManager)
        {
            this.collisionManager = collisionManager;
        }

        public virtual void Move(Vector2 movement, GameTime gameTime)
        {
            Vector2 resolvedMovement = collisionManager?.CalculateNewPosition(this, movement) ?? movement;
            location += resolvedMovement;

            IsOnGround = collisionManager?.IsOnGround(this) ?? false;
        }

        public virtual void TakeDamage(int damage)
        {
            Health -= damage;
            if (Health <= 0) IsDead();
        }

        protected virtual void IsDead()
        {
            Debug.WriteLine("DEAD");
            animation.SetAnimationState(AnimationState.Dead);
        }

        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch);

        public Rectangle Bounds => new Rectangle((int)location.X, (int)location.Y, 50, 50);
        public bool CollidesWith(ICollidable other) => Bounds.Intersects(other.Bounds);
    }
}

