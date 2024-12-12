using GameDevelopment_SchoofsYmke.Animation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pong.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDevelopment_SchoofsYmke.Characters
{
    internal class Enemy :IGameObject
    {
        private Animatie animation;
        private Vector2 position;
        private Texture2D texture;
        private Vector2 velocity;
        private float speed;

        public Rectangle Bounds => new Rectangle((int)position.X, (int)position.Y, 64, 64);

        public Enemy(Texture2D texture, Vector2 initialPosition, float speed)
        {
            animation = new Animatie();
            animation.GetFramesFromTexture(texture.Width, texture.Height, 11, 6);
            animation.SetAnimationState(AnimationState.Idle);

            this.texture = texture;
            position = initialPosition;
            this.speed = speed;
            velocity = new Vector2(speed, 0);
        }

        public void Update(GameTime gameTime)
        {
            Move(gameTime);
        }

        public void Draw(SpriteBatch sprite)
        {
            var frame = animation.CurrentFrame;
            sprite.Draw(texture, position,frame.SourceRectangle, Color.White);
        }

        public void Move(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            position += velocity * deltaTime;

            if (position.X < 0 || position.X  + 64 > 800)
            {
                velocity.X -= velocity.X;
            }
            animation.Update(gameTime);
        }
    }
}
