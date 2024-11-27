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
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;


namespace GameDevelopment_SchoofsYmke
{
    internal class Hero : IGameObject, ICollidable
    {
        private Texture2D texture;
        Animatie animation;
        private Vector2 location;
        private Vector2 speed;
        private Rectangle bounds;
        private SpriteEffects spriteEffects;

        private IInputReader inputReader;
        private CollisionManager collision;

        public Hero(Texture2D texture, IInputReader inputReader)
        {
            this.texture = texture;
            this.inputReader = inputReader;          

            animation = new Animatie();
            animation.GetFramesFromTexture(texture.Width, texture.Height,8,2);

            location = new Vector2(0, 700);
            speed = new Vector2(10, 1);
            bounds = new Rectangle((int)location.X, (int)location.Y, texture.Width/8, texture.Height/2);
        }

        public void SetMovementManager(CollisionManager collision)
        {
            this.collision = collision;
        }

        public void Draw(SpriteBatch sprite)
        {
            sprite.Draw(texture, location, animation.CurrentFrame.SourceRectangle, Color.White,
                0f, Vector2.Zero, 1.3f, spriteEffects, 0f);
        }

        public void Update(GameTime gameTime)
        {
            Move();
            animation.Update(gameTime);
        }

        private Vector2 Limit(Vector2 v, float max)
        {
            if (v.Length() > max)
            {
                var ratio = max / v.Length();
                v.X *= ratio;
                v.Y *= ratio;
            }
            return v;
        }

        private void Move()
        {
            Vector2 direction = inputReader.ReadInput()* speed;
            Vector2 adjustedMovement = collision?.CalculateNewPosition(location, direction, this) ?? direction;

            location += adjustedMovement;
            bounds.Location = location.ToPoint();

            if(direction.X > 0) { spriteEffects = SpriteEffects.None; }
            if(direction.X < 0) { spriteEffects = SpriteEffects.FlipHorizontally; }
        }

        public Rectangle Bounds => bounds;

        public bool IsSolid => true;

        public bool CollidesWith(ICollidable other)
        {
            return Bounds.Intersects(other.Bounds);
        }
    }
}
