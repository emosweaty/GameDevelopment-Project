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
        public bool IsActive { get; set; } = true;

        private Point screenSize;

        public Projectile(Vector2 position, Vector2 velocity, Texture2D texture)
        {
            this.position = position;
            this.velocity = velocity;
            this.texture = texture;

            this.screenSize = new Point(2550, 1500);
        }

        public void Update(GameTime gameTime, Point mapSize)
        {
            if (!IsActive) return;

            position += velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (position.X < 0 || position.X > mapSize.X || position.Y < 0 || position.Y > screenSize.Y)
            {
                IsActive = false;
            }

        }

        public void Draw(SpriteBatch sprite)
        {
            if (IsActive)
            {
                sprite.Draw(texture, position, Color.White);
            }
        }
    }
}
