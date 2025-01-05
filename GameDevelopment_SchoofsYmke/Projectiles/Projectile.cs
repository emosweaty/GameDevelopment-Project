using GameDevelopment_SchoofsYmke.Animation;
using GameDevelopment_SchoofsYmke.Interfaces;
using GameDevelopment_SchoofsYmke.Movement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDevelopment_SchoofsYmke.Projectiles
{
    internal class Projectile  : ICollidable
    {
        private Vector2 position;
        private Vector2 velocity;
        private Texture2D texture;
        private Animatie animation;
        private float gravity;
        public string OwnerType { get; private set; } 
        public bool IsActive { get; set; }
        public bool IsSolid { get; set; } = false;

        private float elapsedtime;
        public Rectangle Bounds => new Rectangle((int)position.X, (int)position.Y, texture.Width / 6, texture.Height);


        public Projectile(Vector2 position, Vector2 velocity, Texture2D texture, string ownerType)
        {
            this.position = position;
            this.velocity = velocity;
            this.texture = texture;

            animation = new Animatie();
            animation.GetFramesFromTexture(texture.Width, texture.Height, 6, 1, "projectile");
            animation.SetAnimationState(AnimationState.Idle);

            IsActive = true;
            gravity = 50f;
            OwnerType = ownerType;
        }

        public void Update(GameTime gameTime, Point mapSize, int screenHeigth, CollisionManager collision)
        {
            if (!IsActive) return;

            velocity.Y += gravity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            position += velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

            animation.Update(gameTime);

            if (position.X < 0 || position.X > mapSize.X || position.Y < 0 || position.Y > screenHeigth)
            {
                Debug.WriteLine("Projectile went out of bounds");
                IsActive = false;
                return;
            }

            if (collision.IsOnGround(this))
            {
                Debug.WriteLine("Projectile collided with an object");
                IsActive = false;
            }
        }

        public void Draw(SpriteBatch sprite)
        {
            if (IsActive)
            {
                sprite.Draw(texture, position, animation.CurrentFrame.SourceRectangle,Color.White);
                Debug.WriteLine("Projectile created at: " + position);

            }
        }

        public bool CollidesWith(ICollidable other)
        {
            return Bounds.Intersects(other.Bounds);
        }
    }
}
