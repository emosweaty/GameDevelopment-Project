using GameDevelopment_SchoofsYmke.Animation;
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
    internal class Projectile 
    {
        private Vector2 position;
        private Vector2 velocity;
        private Texture2D texture;
        private Animatie animation;
        public bool IsActive { get; set; } = true;
        public Rectangle Bounds => new Rectangle((int)position.X, (int)position.Y, texture.Width / 6, texture.Height);


        public Projectile(Vector2 position, Vector2 velocity, Texture2D texture)
        {
            this.position = position;
            this.velocity = velocity;
            this.texture = texture;

            animation = new Animatie();
            animation.GetFramesFromTexture(texture.Width, texture.Height, 6, 1, "projectile");
            animation.SetAnimationState(AnimationState.Idle);
        }

        public void Update(GameTime gameTime, Point mapSize, int screenHeigth)
        {
            if (!IsActive) return;

            position += velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

            animation.Update(gameTime);

            if (position.X < 0 || position.X > mapSize.X || position.Y < 0 || position.Y > screenHeigth)
            {
                IsActive = false;
            }

        }

        public void Draw(SpriteBatch sprite)
        {
            if (IsActive)
            {
                sprite.Draw(texture, position, animation.CurrentFrame.SourceRectangle,Color.White);
            }
        }
    }
}
